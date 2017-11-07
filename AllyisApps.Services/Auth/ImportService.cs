using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Lib;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Services
{
    /// <summary>
    /// Business logic for importing directly from spreadsheets.
    /// </summary>
    public partial class AppService : BaseService
    {
        private const string HourMinutePattern = @"^(\d+):(\d+)$";
        private const string DecimalPattern = @"^\d*\.?\d*$";
        private const float MinutesInHour = 60.0f;

        /// <summary>
        /// Import data from a workbook. Imports customers, projects, users, project/user relationships, and/or time entry data.
        /// </summary>
        /// <param name="subscriptionId">SubscriptionId.</param>
        /// <param name="importData">Workbook with data to import.</param>
        /// <param name="organizationId">The organization's Id.</param>
        /// <param name="inviteUrl">Used for userImport when adding users via AddMemberPage</param>
        public async Task<ImportActionResult> Import(DataSet importData, int subscriptionId = 0, int organizationId = 0, string inviteUrl = null)
        {
            int orgId;
            if (subscriptionId > 0 && UserContext.SubscriptionsAndRoles[subscriptionId] != null)
            {
                orgId = UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
            }
            else if (organizationId > 0 && UserContext.OrganizationsAndRoles[organizationId] != null)
            {
                orgId = organizationId;
            }
            else
            {
                return null; // subsciptionId and/or organization id are invalid
            }

            // For some reason, linq won't work directly with DataSets, so we start by just moving the tables over to a linq-able List
            // The tables are ranked and sorted in order to get customers to import first, before projects, avoiding some very complicated look-up logic.
            var sortableTables = new List<Tuple<DataTable, int>>();
            foreach (DataTable table in importData.Tables)
            {
                int rank;
                if (table.Columns.Contains(ColumnHeaders.CustomerName) || table.Columns.Contains(ColumnHeaders.CustomerId))
                {
                    if (table.Columns.Contains(ColumnHeaders.ProjectName) || table.Columns.Contains(ColumnHeaders.ProjectId))
                    {
                        rank = 2;
                    }
                    else
                    {
                        rank = 3;
                    }
                }
                else
                {
                    if (table.Columns.Contains(ColumnHeaders.ProjectName) || table.Columns.Contains(ColumnHeaders.ProjectId))
                    {
                        rank = 1;
                    }
                    else
                    {
                        rank = 0;
                    }
                }

                sortableTables.Add(new Tuple<DataTable, int>(table, rank));
            }
            var tables = sortableTables.OrderBy(tup => tup.Item2 * -1).Select(tup => tup.Item1).ToList();

            // Retrieval of existing customer and project data
            var customersProjects = new List<Tuple<Customer, List<Project.Project>>>();
            foreach (Customer customer in GetCustomerList(orgId))
            {
                customersProjects.Add(new Tuple<Customer, List<Project.Project>>(
                    customer,
                    (await GetProjectsByCustomer(customer.CustomerId)).ToList()
                ));
            }

            // Retrieval of existing user data
            User userGet = await GetUserAsync(UserContext.UserId);
            var users = GetOrganizationMemberList(orgId).Select(o => new Tuple<string, User>(o.EmployeeId, userGet)).ToList();

            // Retrieval of existing user product subscription data
            const int ttProductId = (int)ProductIdEnum.TimeTracker;
            SubscriptionDisplayDBEntity ttSub = (await DBHelper.GetSubscriptionsDisplayByOrg(orgId)).SingleOrDefault(s => s.ProductId == ttProductId);
            var userSubs = (await GetUsersWithSubscriptionToProductInOrganization(orgId, ttProductId)).ToList();

            // Retrieval of existing pay class data
            var classGet = await DBHelper.GetPayClasses(orgId);
            var payClasses = classGet.Select(InitializePayClassInfo).ToList();

            // Result object
            var result = new ImportActionResult();

            // Do we even have tables to import?
            if (tables.Count == 0)
            {
                result.GeneralFailures.Add("No spreadsheets to import.");
                return result;
            }

            // Loop through and see what can be imported from each table in turn. Order doesn't matter, since missing information
            // will be sought from other tables as needed.
            foreach (DataTable table in tables)
            {
                #region Column Header Checks

                // Customer importing: requires both customer name and customer id. Other information is optional, and can be filled in later.
                bool hasCustomerName = table.Columns.Contains(ColumnHeaders.CustomerName);
                bool hasCustomerId = table.Columns.Contains(ColumnHeaders.CustomerId);
                bool canCreateCustomers = hasCustomerName && hasCustomerId;
                var customerImportLinks = new List<DataTable>();
                if (hasCustomerName ^ hasCustomerId)
                {
                    // If only one thing is on this sheet, we see if both exist together on another sheet
                    customerImportLinks = tables.Where(t => t.Columns.Contains(ColumnHeaders.CustomerName) && t.Columns.Contains(ColumnHeaders.CustomerId)).ToList();
                    if (customerImportLinks.Any())
                    {
                        canCreateCustomers = true;
                    }
                }

                // Non-required customer columns. This is checked ahead of time to eliminate useless column lookups on each row and save a lot of time.
                bool hasCustomerStreetAddress = table.Columns.Contains(ColumnHeaders.CustomerStreetAddress);
                bool hasCustomerCity = table.Columns.Contains(ColumnHeaders.CustomerCity);
                bool hasCustomerCountry = table.Columns.Contains(ColumnHeaders.CustomerCountry);
                bool hasCustomerState = table.Columns.Contains(ColumnHeaders.CustomerState);
                bool hasCustomerPostalCode = table.Columns.Contains(ColumnHeaders.CustomerPostalCode);
                bool hasCustomerEmail = table.Columns.Contains(ColumnHeaders.CustomerEmail);
                bool hasCustomerPhoneNumber = table.Columns.Contains(ColumnHeaders.CustomerPhoneNumber);
                bool hasCustomerFaxNumber = table.Columns.Contains(ColumnHeaders.CustomerFaxNumber);
                bool hasCustomerEin = table.Columns.Contains(ColumnHeaders.CustomerEIN);
                bool hasNonRequiredCustomerInfo = hasCustomerStreetAddress || hasCustomerCity || hasCustomerCountry || hasCustomerState ||
                         hasCustomerPostalCode || hasCustomerEmail || hasCustomerPhoneNumber || hasCustomerFaxNumber || hasCustomerEin;

                // Project importing: requires both project name and project id, as well as one identifying field for a customer (name or id)
                bool hasProjectName = table.Columns.Contains(ColumnHeaders.ProjectName);
                bool hasProjectId = table.Columns.Contains(ColumnHeaders.ProjectId);
                List<DataTable>[,] projectLinks = new List<DataTable>[3, 3];
                projectLinks[0, 1] = projectLinks[1, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.ProjectName) && t.Columns.Contains(ColumnHeaders.ProjectId)).ToList();
                projectLinks[0, 2] = projectLinks[2, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.ProjectName) && (t.Columns.Contains(ColumnHeaders.CustomerName) || t.Columns.Contains(ColumnHeaders.CustomerId))).ToList();
                projectLinks[1, 2] = projectLinks[2, 1] = tables.Where(t => t.Columns.Contains(ColumnHeaders.ProjectId) && (t.Columns.Contains(ColumnHeaders.CustomerName) || t.Columns.Contains(ColumnHeaders.CustomerId))).ToList();
                bool canImportProjects = (hasProjectName || projectLinks[0, 1].Count > 0 || projectLinks[0, 2].Count > 0) &&
                    (hasProjectId || projectLinks[1, 0].Count > 0 || projectLinks[1, 2].Count > 0) &&
                    (hasCustomerName || hasCustomerId || projectLinks[2, 0].Count > 0 || projectLinks[2, 1].Count > 0);

                // Non-required project columns

                // TODO use this line once project isHourly property is supported.  Currently disabled
                // bool hasProjectType = table.Columns.Contains(ColumnHeaders.ProjectType);
                bool hasProjectStartDate = table.Columns.Contains(ColumnHeaders.ProjectStartDate);
                bool hasProjectEndDate = table.Columns.Contains(ColumnHeaders.ProjectEndDate);
                bool hasNonRequiredProjectInfo = /*hasProjectType ||*/ hasProjectStartDate || hasProjectEndDate;

                // User importing: requires email, id, first and last name
                bool hasUserEmail = table.Columns.Contains(ColumnHeaders.UserEmail);
                bool hasEmployeeId = table.Columns.Contains(ColumnHeaders.EmployeeId);
                bool hasUserName = table.Columns.Contains(ColumnHeaders.UserFirstName) && table.Columns.Contains(ColumnHeaders.UserLastName);
                var userLinks = new List<DataTable>[3, 3];
                userLinks[0, 1] = userLinks[1, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.UserEmail) && t.Columns.Contains(ColumnHeaders.EmployeeId)).ToList();
                userLinks[0, 2] = userLinks[2, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.UserEmail) && t.Columns.Contains(ColumnHeaders.UserFirstName) && t.Columns.Contains(ColumnHeaders.UserLastName)).ToList();
                userLinks[1, 2] = userLinks[2, 1] = tables.Where(t => t.Columns.Contains(ColumnHeaders.EmployeeId) && t.Columns.Contains(ColumnHeaders.UserFirstName) && t.Columns.Contains(ColumnHeaders.UserLastName)).ToList();
                bool canImportUsers =
                    (hasUserEmail || userLinks[0, 1].Count > 0 || userLinks[0, 2].Count > 0) &&
                    (hasEmployeeId || userLinks[1, 0].Count > 0 || userLinks[1, 2].Count > 0) &&
                    (hasUserName || userLinks[2, 0].Count > 0 || userLinks[2, 1].Count > 0);

                // Non-required user columns
                bool hasUserAddress = table.Columns.Contains(ColumnHeaders.UserAddress);
                bool hasUserCity = table.Columns.Contains(ColumnHeaders.UserCity);
                bool hasUserCountry = table.Columns.Contains(ColumnHeaders.UserCountry);
                bool hasUserDateOfBirth = table.Columns.Contains(ColumnHeaders.UserDateOfBirth);
                bool hasUserUsername = table.Columns.Contains(ColumnHeaders.UserName);
                bool hasUserPhoneExtension = table.Columns.Contains(ColumnHeaders.UserPhoneExtension);
                bool hasUserPhoneNumber = table.Columns.Contains(ColumnHeaders.UserPhoneNumber);
                bool hasUserPostalCode = table.Columns.Contains(ColumnHeaders.UserPostalCode);
                bool hasUserState = table.Columns.Contains(ColumnHeaders.UserState);
                bool hasNonRequiredUserInfo = hasUserAddress || hasUserCity || hasUserCountry || hasUserDateOfBirth || hasUserUsername ||
                    hasUserPhoneExtension || hasUserPhoneNumber || hasUserPostalCode || hasUserState;

                // Project-user importing: perfomed when identifying information for both project and user are present
                bool canImportProjectUser = (hasProjectName || hasProjectId) && (hasUserEmail || hasEmployeeId || hasUserName);

                // Time Entry importing: unlike customers, projects, and users, time entry data must have all time entry information on the same sheet
                // Requires indentifying data for user and project, as well as date, duration, and pay class
                bool hasTTDate = table.Columns.Contains(ColumnHeaders.Date);
                bool hasTTDuration = table.Columns.Contains(ColumnHeaders.Duration);
                bool hasTTPayClass = table.Columns.Contains(ColumnHeaders.PayClass);
                bool canImportTimeEntry = canImportProjectUser && hasTTDate && hasTTDuration && hasTTPayClass;
                if (canImportTimeEntry && ttSub == null)
                {
                    // No Time Tracker subscription
                    result.TimeEntryFailures.Add("Cannot import time entries: no subscription to Time Tracker.");
                    canImportTimeEntry = false;
                }

                // Non-required time entry column
                bool hasTTDescription = table.Columns.Contains(ColumnHeaders.Description);

                #endregion Column Header Checks

                // Do we have rows/data to import?
                if (table.Rows.Count == 0
                    || !hasCustomerName
                    && !hasCustomerId
                    && !hasProjectName
                    && !hasProjectId
                    && !hasUserEmail
                    && !hasEmployeeId
                    && !hasUserName
                    && !canImportProjectUser
                    && !canImportTimeEntry)
                {
                    result.GeneralFailures.Add($"There is no readable data to import from spreadsheet \"{table.TableName}\".");
                }

                // After all checks are complete, we go through row by row and import the information
                foreach (DataRow row in table.Rows)
                {
                    if (row.ItemArray.All(i => string.IsNullOrEmpty(i?.ToString()))) break; // Avoid iterating through empty rows

                    #region Customer Import

                    Customer customer = null;

                    // If there is no identifying information for customers, all customer related importing is skipped.
                    if (hasCustomerName || hasCustomerId)
                    {
                        // Find the existing customer using name, or id if name isn't on this sheet.
                        customer = customersProjects.Select(tup => tup.Item1).FirstOrDefault(c => hasCustomerName ? c.CustomerName.Equals(row[ColumnHeaders.CustomerName].ToString()) : c.CustomerOrgId.Equals(row[ColumnHeaders.CustomerId].ToString()));
                        if (customer == null)
                        {
                            if (canCreateCustomers)
                            {
                                // No customer was found, so a new one is created.
                                Customer newCustomer;
                                if (customerImportLinks.Count == 0)
                                {
                                    // If customerImportLinks is empty, it's because all the information is on this sheet.
                                    string name = null;
                                    string custOrgId = null;
                                    ReadColumn(row, ColumnHeaders.CustomerName, n => name = n);
                                    ReadColumn(row, ColumnHeaders.CustomerId, n => custOrgId = n);
                                    if (name == null && custOrgId == null)
                                    {
                                        result.CustomerFailures.Add(string.Format("Error importing customer on sheet {0}, row {1}: both {2} and {3} cannot be read.", table.TableName, table.Rows.IndexOf(row) + 2, ColumnHeaders.CustomerName, ColumnHeaders.CustomerId));
                                        continue;
                                    }

                                    if (name == null || custOrgId == null)
                                    {
                                        result.CustomerFailures.Add(string.Format("Could not create customer {0}: no matching {1}.", name == null ? custOrgId : name, name == null ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId));
                                        continue;
                                    }

                                    newCustomer = new Customer
                                    {
                                        CustomerName = name,
                                        CustomerOrgId = custOrgId,
                                        OrganizationId = orgId
                                    };
                                }
                                else
                                {
                                    // If customerImportLinks has been set, we have to grab some information from another sheet.
                                    string knownValue = null;
                                    string readValue = null;
                                    ReadColumn(row, hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId, n => knownValue = n);
                                    if (knownValue == null)
                                    {
                                        result.CustomerFailures.Add(string.Format("Error importing customer on sheet {0}, row {1}: {2} cannot be read.", table.TableName, table.Rows.IndexOf(row) + 2, hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId));
                                        continue;
                                    }

                                    foreach (DataTable link in customerImportLinks)
                                    {
                                        try
                                        {
                                            readValue = link.Select(string.Format("[{0}] = '{1}'", hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId, knownValue))[0][hasCustomerName ? ColumnHeaders.CustomerId : ColumnHeaders.CustomerName].ToString();
                                            if (readValue != null) break;
                                        }
                                        catch (IndexOutOfRangeException) { }
                                    }

                                    if (readValue == null)
                                    {
                                        result.CustomerFailures.Add(string.Format("Could not create customer {0}: no matching {1}.", knownValue, hasCustomerName ? ColumnHeaders.CustomerId : ColumnHeaders.CustomerName));
                                        continue;
                                    }

                                    newCustomer = new Customer
                                    {
                                        CustomerName = hasCustomerName ? knownValue : readValue,
                                        CustomerOrgId = hasCustomerName ? readValue : knownValue,
                                        OrganizationId = orgId
                                    };
                                }

                                if (newCustomer != null)
                                {
                                    int? newCustomerId = await CreateCustomer(newCustomer, subscriptionId);
                                    if (newCustomerId == null)
                                    {
                                        result.CustomerFailures.Add(string.Format("Could not create customer {0}: permission failure.", newCustomer.CustomerName));
                                        continue;
                                    }

                                    newCustomer.CustomerId = newCustomerId.Value;
                                    if (newCustomer.CustomerId == -1)
                                    {
                                        result.CustomerFailures.Add(string.Format("Database error while creating customer {0}.", newCustomer.CustomerName));
                                        continue;
                                    }

                                    customersProjects.Add(new Tuple<Customer, List<Project.Project>>(
                                        newCustomer,
                                        new List<Project.Project>()
                                    ));
                                    customer = newCustomer;
                                    result.CustomersImported += 1;
                                }
                            }
                            else
                            {
                                // Not enough information to create customer
                                result.CustomerFailures.Add(string.Format("Could not create customer {0}: no matching {1}.", row[hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId], hasCustomerName ? ColumnHeaders.CustomerId : ColumnHeaders.CustomerName));
                            }
                        }

                        // Importing non-required customer data
                        if (customer != null && hasNonRequiredCustomerInfo)
                        {
                            bool updated = false;

                            if (hasCustomerStreetAddress) updated = ReadColumn(row, ColumnHeaders.CustomerStreetAddress, val => customer.Address.Address1 = val) || updated;
                            if (hasCustomerCity) updated = ReadColumn(row, ColumnHeaders.CustomerCity, val => customer.Address.City = val) || updated;
                            if (hasCustomerCountry) updated = ReadColumn(row, ColumnHeaders.CustomerCountry, val => customer.Address.CountryName = val) || updated;
                            if (hasCustomerState) updated = ReadColumn(row, ColumnHeaders.CustomerState, val => customer.Address.StateName = val) || updated;
                            if (hasCustomerPostalCode) updated = ReadColumn(row, ColumnHeaders.CustomerPostalCode, val => customer.Address.PostalCode = val) || updated;
                            if (hasCustomerEmail) updated = ReadColumn(row, ColumnHeaders.CustomerEmail, val => customer.ContactEmail = val) || updated;
                            if (hasCustomerPhoneNumber) updated = ReadColumn(row, ColumnHeaders.CustomerPhoneNumber, val => customer.ContactPhoneNumber = val) || updated;
                            if (hasCustomerFaxNumber) updated = ReadColumn(row, ColumnHeaders.CustomerFaxNumber, val => customer.FaxNumber = val) || updated;
                            if (hasCustomerEin) updated = ReadColumn(row, ColumnHeaders.CustomerEIN, val => customer.EIN = val) || updated;

                            if (updated)
                            {
                                await UpdateCustomer(customer, subscriptionId);
                            }
                        }
                    }

                    #endregion Customer Import

                    #region Project Import

                    DateTime? defaultProjectStartDate = null;
                    DateTime? defaultProjectEndDate = null;

                    Project.Project project = null;

                    // If there is no identifying information for projects, all project related importing is skipped.
                    if (hasProjectName || hasProjectId)
                    {
                        bool thisRowHasProjectName = hasProjectName;
                        bool thisRowHasProjectId = hasProjectId;

                        // Start with getting the project information that is known from this sheet
                        string knownValue = null;
                        string readValue = null;
                        ReadColumn(row, hasProjectName ? ColumnHeaders.ProjectName : ColumnHeaders.ProjectId, p => knownValue = p);
                        if (hasProjectName && hasProjectId)
                        {
                            // If both columns exist, knownValue is Name and readValue will be Id
                            if (!ReadColumn(row, ColumnHeaders.ProjectId, p => readValue = p))
                            {
                                if (knownValue == null)
                                {
                                    // Failed to read both values
                                    result.ProjectFailures.Add(string.Format("Error importing project on sheet {0}, row {1}: both {2} and {3} cannot be read.", table.TableName, table.Rows.IndexOf(row) + 2, ColumnHeaders.ProjectName, ColumnHeaders.ProjectId)); //'all', line 5
                                    continue;
                                }

                                // Failed to read Id, but read Name successfully.
                                thisRowHasProjectId = false;
                            }

                            if (knownValue == null)
                            {
                                // Failed to read Name. If reading the Id also failed, the continue above would have been hit, so it must have succeeded.
                                // This means that we should change knownValue to Id.
                                thisRowHasProjectName = false;
                                knownValue = readValue;
                                readValue = null;
                            }
                        }

                        if (customer != null)
                        {
                            // We now have the customer and at least one piece of identifying project information. That's enough to tell if the project already exists.
                            project = customersProjects
                                .FirstOrDefault(tup => tup.Item1.CustomerId == customer.CustomerId).Item2
                                .FirstOrDefault(p => thisRowHasProjectName ? p.ProjectName.Equals(knownValue) : p.ProjectOrgId.Equals(knownValue));
                            if (project == null)
                            {
                                // Project does not exist, so we should create it
                                if (!canImportProjects)
                                {
                                    result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
                                    continue;
                                }

                                if (thisRowHasProjectName ^ thisRowHasProjectId)
                                {
                                    // We still need the other bit of project info
                                    foreach (DataTable link in projectLinks[0, 1])
                                    {
                                        try
                                        {
                                            readValue = link.Select(string.Format("[{0}] = '{1}'", thisRowHasProjectName ? ColumnHeaders.ProjectName : ColumnHeaders.ProjectId, knownValue))[0][thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName].ToString();
                                            if (!string.IsNullOrEmpty(readValue))
                                            {
                                                break; // Match found.
                                            }
                                        }
                                        catch (IndexOutOfRangeException) { }
                                    }

                                    if (string.IsNullOrEmpty(readValue))
                                    {
                                        result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
                                        continue;
                                    }
                                }

                                // All required information is known: time to create the project
                                project = new Project.Project
                                {
                                    owningCustomer = new Customer
                                    {
                                        CustomerId = customer.CustomerId,
                                        OrganizationId = orgId,
                                    },
                                    ProjectName = thisRowHasProjectName ? knownValue : readValue,
                                    IsHourly = false, // TODO un-hardcode once project isHourly property is supported.  Currently disabled
                                    OrganizationId = orgId,
                                    ProjectOrgId = thisRowHasProjectName ? readValue : knownValue,
                                    StartingDate = defaultProjectStartDate,
                                    EndingDate = defaultProjectEndDate
                                };
                                project.ProjectId = await CreateProject(project);
                                if (project.ProjectId == -1)
                                {
                                    result.ProjectFailures.Add(string.Format("Database error while creating project {0}", project.ProjectName));
                                    project = null;
                                }
                                else
                                {
                                    customersProjects.FirstOrDefault(tup => tup.Item1 == customer).Item2.Add(project);
                                    result.ProjectsImported += 1;
                                }
                            }
                        }
                        else
                        {
                            // if(!canImportProjects)
                            //{
                            //    result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
                            //    continue;
                            //}

                            // No customer yet specified. Now, we have to use all the links to try and get customer and the complete project info
                            string[] fields =
                                {
                                    thisRowHasProjectName ? knownValue : null,
                                    thisRowHasProjectId ? thisRowHasProjectName ? readValue : knownValue : null,
                                    null
                                };
                            bool customerFieldIsName = true;

                            /*
								There are 3 required fields, and we may need to traverse at most 2 links to get them all, with no knowledge of which links will succeed or fail in providing
								the needed information. To solve this, we do 2 passes (i), each time checking for the missing information (j) using the links we've found to the information
								we already have (k). On each pass, any known information is skipped, so time won't be wasted if the first pass succeeds. This way, any combination of paths
								to acquire the missing information from the known information is covered.
							*/
                            for (int i = 0; i < 2; i++)
                            {
                                // i = pass, out of 2
                                for (int j = 0; j < fields.Length; j++)
                                {
                                    // j = field we are currently trying to find
                                    for (int k = 0; k < fields.Length; k++)
                                    {
                                        // k = field we are trying to find j from, using a link
                                        if (fields[j] != null || j == k || fields[k] == null) continue;

                                        foreach (DataTable link in projectLinks[j, k])
                                        {
                                            try
                                            {
                                                bool thisLinkCustomerFieldIsName = (k == 2 || j == 2) && link.Columns.Contains(ColumnHeaders.CustomerName);
                                                const int rowNum = 0;
                                                string col =
                                                    j == 0
                                                        ? ColumnHeaders.ProjectName
                                                        : j == 1
                                                            ? ColumnHeaders.ProjectId
                                                            : thisLinkCustomerFieldIsName
                                                                ? ColumnHeaders.CustomerName
                                                                : ColumnHeaders.CustomerId;

                                                fields[j] = link
                                                    .Select(
                                                        string.Format(
                                                            "[{0}] = '{1}'",
                                                            k == 0
                                                                ? ColumnHeaders.ProjectName
                                                                : k == 1
                                                                    ? ColumnHeaders.ProjectId
                                                                    : thisLinkCustomerFieldIsName
                                                                        ? ColumnHeaders.CustomerName
                                                                        : ColumnHeaders.CustomerId,
                                                            fields[k].Replace("'", "''")
                                                        )
                                                    )[rowNum][col].ToString();

                                                if (fields[j] == null) continue;

                                                if (j == 2)
                                                {
                                                    customerFieldIsName = thisLinkCustomerFieldIsName;
                                                }

                                                break;
                                            }
                                            catch (IndexOutOfRangeException) { }
                                        }
                                    }
                                }
                            }

                            // After that, if we don't have all the information, it's safe to say it can't be found
                            if (!string.IsNullOrEmpty(fields[2]))
                            {
                                customer = customersProjects.Select(tup => tup.Item1).FirstOrDefault(c => customerFieldIsName ? c.CustomerName.Equals(fields[2]) : c.CustomerOrgId.Equals(fields[2]));

                                if (customer == null)
                                {
                                    result.ProjectFailures.Add(string.Format("Could not create project {0}: No customer to create it under.", knownValue));
                                    continue;
                                }

                                project = customersProjects
                                    .FirstOrDefault(tup => tup.Item1.CustomerId == customer.CustomerId).Item2
                                    .FirstOrDefault(p => p.ProjectName.Equals(fields[0]));
                                if (project == null)
                                {
                                    // Project does not exist, so we should create it
                                    if (string.IsNullOrEmpty(fields[0]) || string.IsNullOrEmpty(fields[1]))
                                    {
                                        result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
                                        continue;
                                    }

                                    // All required information is known: time to create the project
                                    project = new Project.Project
                                    {
                                        owningCustomer = new Customer
                                        {
                                            CustomerId = customer.CustomerId
                                        },
                                        ProjectName = fields[0],
                                        IsHourly = false,  // TODO un-hardocode once project isHourly property is supported.  Currently disabled
                                        OrganizationId = orgId,
                                        ProjectOrgId = fields[1],
                                        StartingDate = defaultProjectStartDate,
                                        EndingDate = defaultProjectEndDate
                                    };
                                    project.ProjectId = await CreateProject(project);
                                    if (project.ProjectId == -1)
                                    {
                                        result.ProjectFailures.Add(string.Format("Database error while creating project {0}", project.ProjectName));
                                        project = null;
                                    }
                                    else
                                    {
                                        customersProjects.FirstOrDefault(tup => tup.Item1 == customer).Item2.Add(project);
                                        result.ProjectsImported += 1;
                                    }
                                }
                            }
                            else
                            {
                                // No customer could be found for this project, so we try to find a matching project under any existing customer
                                project = customersProjects
                                    .Select(tup => tup.Item2)
                                    .Select(plst => plst
                                        .FirstOrDefault(p =>
                                            thisRowHasProjectName
                                                && p.ProjectName.Equals(knownValue)
                                                && (string.IsNullOrEmpty(fields[1]) || p.ProjectOrgId.Equals(fields[1]))
                                            || p.ProjectOrgId.Equals(knownValue)
                                                && (string.IsNullOrEmpty(fields[0]) || p.ProjectName.Equals(fields[0]))))
                                    .FirstOrDefault(p => p != null);

                                if (project == null)
                                {
                                    result.ProjectFailures.Add(string.Format("Could not find any project {0}, and no customer was specified to create project under.", string.IsNullOrEmpty(fields[0]) ? fields[1] : fields[0]));
                                    continue;
                                }
                            }
                        }

                        // Importing non-required project data
                        if (project != null && hasNonRequiredProjectInfo)
                        {
                            bool updated = false;
                            string startDate = null;
                            string endDate = null;

                            // TODO use this line once project isHourly property is supported.  Currently disabled
                            // if (hasProjectType) updated = this.readColumn(row, ColumnHeaders.ProjectType, val => project.isHourly = val) || updated;
                            if (hasProjectStartDate) updated = ReadColumn(row, ColumnHeaders.ProjectStartDate, val => startDate = val) || updated;
                            if (hasProjectEndDate) updated = ReadColumn(row, ColumnHeaders.ProjectEndDate, val => endDate = val) || updated;
                            if (startDate != null) project.StartingDate = DateTime.Parse(startDate);
                            if (endDate != null) project.EndingDate = DateTime.Parse(endDate);

                            if (updated)
                            {
                                UpdateProject(subscriptionId, project);
                            }
                        }
                    }

                    #endregion Project Import

                    #region User Import

                    User userInOrg = null;
                    if (hasUserEmail || hasEmployeeId || hasUserName)
                    {
                        Tuple<string, User> userTuple = null;

                        // Find existing user by whatever information we have
                        string readValue = null;
                        if (hasUserEmail)
                        {
                            if (ReadColumn(row, ColumnHeaders.UserEmail, e => readValue = e))
                            {
                                userTuple = users.FirstOrDefault(tup => tup.Item2.Email.Equals(readValue));
                            }
                        }
                        if (userTuple == null)
                        {
                            if (hasEmployeeId)
                            {
                                if (ReadColumn(row, ColumnHeaders.EmployeeId, e => readValue = e))
                                {
                                    userTuple = users.FirstOrDefault(tup => tup.Item1.Equals(readValue));
                                }
                            }//Remove logic only email matters.
                            if (userTuple == null)
                            {
                                string readLastName = null;
                                if (ReadColumn(row, ColumnHeaders.UserFirstName, e => readValue = e) && ReadColumn(row, ColumnHeaders.UserLastName, e => readLastName = e))
                                {
                                    userTuple = users.FirstOrDefault(tup => tup.Item2.FirstName.Equals(readValue) && tup.Item2.LastName.Equals(readLastName));
                                }
                            }
                        }
                        userInOrg = userTuple?.Item2;

                        if (userInOrg == null)
                        {
                            if (canImportUsers)
                            {
                                // No user found, create one if possible
                                // Find all required fields, if they exist
                                string[] fields =
                                {
                                    hasUserEmail ? row[ColumnHeaders.UserEmail].ToString() : null,
                                    hasEmployeeId ? row[ColumnHeaders.EmployeeId].ToString() : null,
									// Since first and last name must be together and treated as one piece of information, they are joined in this datastructure. Hopefully, we'll never
									// have a user who's name includes the text __IMPORT__
									hasUserName ? (row[ColumnHeaders.UserFirstName].ToString() == "" || row[ColumnHeaders.UserLastName].ToString() == "" ? null : row[ColumnHeaders.UserFirstName] + "__IMPORT__" + row[ColumnHeaders.UserLastName]) : null
                                };
                                // if (fields[2] == "__IMPORT__") fields[2] = null;

                                /*
									There are 3 required fields, and we may need to traverse at most 2 links to get them all, with no knowledge of which links will succeed or fail in providing
									the needed information. To solve this, we do 2 passes (i), each time checking for the missing information (j) using the links we've found to the information
									we already have (k). On each pass, any known information is skipped, so time won't be wasted if the first pass succeeds. This way, any combination of paths
									to acquire the missing information from the known information is covered.
								*/
                                for (int i = 0; i < 2; i++)
                                {
                                    // i = pass, out of 2
                                    for (int j = 0; j < fields.Length; j++)
                                    {
                                        // j = field we are currently trying to find
                                        for (int k = 0; k < fields.Length; k++)
                                        {
                                            // k = field we are trying to find j from, using a link
                                            if (fields[j] != null || j == k || fields[k] == null) continue;

                                            foreach (DataTable link in userLinks[j, k])
                                            {
                                                fields[j] = ReadUserDataColumn(k, j, link, fields[k]); // A private method that can handle reading one column or the case of both name columns, with no difference in usage here.
                                                if (fields[j] != null)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (fields.Any(string.IsNullOrEmpty))
                                {
                                    // Couldn't get all the information
                                    bool[] fieldStatuses = fields.Select(string.IsNullOrEmpty).ToArray();
                                    result.UserFailures.Add(string.Format(
                                        "Could not create user {0}: missing {1}{2}.",
                                        fieldStatuses[0]
                                            ? fieldStatuses[1]
                                                ? fields[2] != null
                                                    ? string.Join(" ", fields[2].Split(new[] { "__IMPORT__" }, StringSplitOptions.None))
                                                    : null
                                                : fields[1]
                                            : fields[0],
                                        fieldStatuses[0]
                                            ? ColumnHeaders.UserEmail
                                            : fieldStatuses[1]
                                                ? ColumnHeaders.EmployeeId
                                                : string.Format("{0}/{1}", ColumnHeaders.UserFirstName, ColumnHeaders.UserLastName),
                                        fieldStatuses
                                            .Where(s => s)
                                            .Count() == 2
                                                ? string.Format(
                                                    " and {0}",
                                                    !fieldStatuses[2]
                                                        ? ColumnHeaders.EmployeeId
                                                        : string.Format("{0}/{1}", ColumnHeaders.UserFirstName, ColumnHeaders.UserLastName))
                                                : ""
                                        )
                                    );
                                    continue;
                                }

                                // All required info was found successfully
                                string[] names = fields[2].Split(new[] { "__IMPORT__" }, StringSplitOptions.None);

                                if (!Utility.IsValidEmail(fields[0]))
                                {
                                    result.UserFailures.Add(string.Format("Could not create user {0}, {1}: invalid email format ({2}).", names[0], names[1], fields[0]));
                                    continue;
                                }

                                try
                                {
                                    await InviteUser(inviteUrl, fields[0].Trim(), names[0], names[1], orgId, UserContext.OrganizationsAndRoles[organizationId].OrganizationName, OrganizationRoleEnum.Member, fields[1], "{ \"Time\" : 0, \"Expense\" : 0, \"Staffing\" : 0 }"); //We need to update this with values from the excel sheet.
                                    result.UsersImported += 1;
                                }
                                catch (DuplicateNameException)
                                {
                                    result.UserFailures.Add(string.Format("Employee Id: {0} is already taken. Please assign a different Id.", fields[1]));
                                }
                                catch (InvalidOperationException)
                                {
                                    result.UserFailures.Add(string.Format("{0} {1} already exists in the organization", names[0], names[1]));
                                }
                                catch (System.Data.SqlClient.SqlException)
                                {
                                    result.UserFailures.Add(string.Format("Database error creating user {0} {1}.", names[0], names[1]));
                                    continue;
                                }
                            }
                        }

                        // Importing non-required user data
                        if (userInOrg != null && hasNonRequiredUserInfo)//If user exists then allow org to change user.
                        {
                            bool updated = false;
                            /* This allows any org to change thier users infomation Org are items of users, users are not properties of orgs */
                            if (hasUserAddress) updated = ReadColumn(row, ColumnHeaders.UserAddress, val => userInOrg.Address.Address1 = val) || updated;
                            if (hasUserCity) updated = ReadColumn(row, ColumnHeaders.UserCity, val => userInOrg.Address.City = val) || updated;
                            if (hasUserCountry) updated = ReadColumn(row, ColumnHeaders.UserCountry, val => userInOrg.Address.CountryName = val) || updated;
                            string dateOfBirth = null;
                            if (hasUserDateOfBirth) updated = ReadColumn(row, ColumnHeaders.UserDateOfBirth, val => dateOfBirth = val) || updated;
                            if (!string.IsNullOrEmpty(dateOfBirth))
                            {
                                DateTime.TryParse(dateOfBirth, out DateTime dob);
                                if (dob <= DateTime.MinValue)
                                {
                                    result.UserFailures.Add(string.Format("The birthdate entered for {0} {1} was invalid. Please check to make sure it's in date format: dd/mm/yyyy and preferably after 1900 ", userInOrg.FirstName, userInOrg.LastName));
                                }
                                else
                                {
                                    userInOrg.DateOfBirth = dob;
                                }
                            }

                            if (hasUserPhoneExtension) updated = ReadColumn(row, ColumnHeaders.UserPhoneExtension, val => userInOrg.PhoneExtension = val) || updated;
                            if (hasUserPhoneNumber) updated = ReadColumn(row, ColumnHeaders.UserPhoneNumber, val => userInOrg.PhoneNumber = val) || updated;
                            if (hasUserPostalCode) updated = ReadColumn(row, ColumnHeaders.UserPostalCode, val => userInOrg.Address.PostalCode = val) || updated;
                            if (hasUserState) updated = ReadColumn(row, ColumnHeaders.UserState, val => userInOrg.Address.StateName = val) || updated;

                            if (updated)
                            {
                                await UpdateUserProfile(userInOrg.UserId, Utility.GetDaysFromDateTime(userInOrg.DateOfBirth), userInOrg.FirstName, userInOrg.LastName, userInOrg.PhoneNumber, userInOrg.Address?.AddressId, userInOrg.Address?.Address1, userInOrg.Address?.City, userInOrg.Address?.StateId, userInOrg.Address?.PostalCode, userInOrg.Address?.CountryCode);
                            }
                        }
                    }

                    #endregion User Import

                    #region Project-user and Time Entry Import

                    // Double-check that previous adding/finding of project and user didn't fail
                    if (!canImportProjectUser || project == null || userInOrg == null) continue;

                    // Find existing project user
                    var proj = await GetProjectsByUserAndOrganization(userInOrg.UserId);
                    if (proj.All(p => p.ProjectId != project.ProjectId))
                    {
                        // If no project user entry exists for this user and project, we create one.
                        CreateProjectUser(project.ProjectId, userInOrg.UserId);
                    }

                    // Time Entry Import
                    if (!canImportTimeEntry) continue;

                    // Check for subscription role
                    bool canImportThisEntry;
                    if (userSubs.All(u => u.UserId != userInOrg.UserId))
                    {
                        // No existing subscription for this user, so we create one.
                        await DBHelper.UpdateSubscriptionUserProductRole((int)TimeTrackerRole.User, ttSub.SubscriptionId, userInOrg.UserId);
                        userSubs.Add(userInOrg);
                        result.UsersAddedToSubscription += 1;
                        canImportThisEntry = true; // Successfully created.
                    }
                    else
                    {
                        // Found existing subscription user.
                        canImportThisEntry = true;
                    }

                    // Import entry
                    if (!canImportThisEntry) continue;

                    string date = null;
                    string duration = null;
                    string description = "";
                    string payclass = "Regular";

                    ReadColumn(row, ColumnHeaders.Date, val => date = val);
                    ReadColumn(row, ColumnHeaders.Duration, val => duration = val);
                    if (hasTTDescription) ReadColumn(row, ColumnHeaders.Description, val => description = val);
                    ReadColumn(row, ColumnHeaders.PayClass, val => payclass = val);

                    PayClass payClass = payClasses.SingleOrDefault(p => string.Equals(p.PayClassName, payclass, StringComparison.OrdinalIgnoreCase));
                    DateTime theDate;
                    float? theDuration;

                    if (payClass == null)
                    {
                        result.TimeEntryFailures.Add($"Error importing time entry on sheet {table.TableName}, row {table.Rows.IndexOf(row) + 2}: unknown {ColumnHeaders.PayClass} ({payclass}).");
                        continue;
                    }

                    try
                    {
                        theDate = DateTime.Parse(date);
                        if (theDate.Year < 1753) throw new FormatException();
                    }
                    catch (Exception)
                    {
                        result.TimeEntryFailures.Add($"Error importing time entry on sheet {table.TableName}, row {table.Rows.IndexOf(row) + 2}: bad date format ({date}).");
                        continue;
                    }

                    if (!(theDuration = ParseDuration(duration)).HasValue)
                    {
                        result.TimeEntryFailures.Add($"You must enter the duration as HH:MM or H.HH format for the date {theDate}");
                        continue;
                    }
                    if (ParseDuration(duration) == 0)
                    {
                        result.TimeEntryFailures.Add($"You must enter a time larger than 00:00 for the date {theDate}");
                        continue;
                    }

                    // Find existing entry. If none, create new one     TODO: See if there's a good way to populate this by sheet rather than by row, or once at the top
                    var entryGet = await DBHelper.GetTimeEntriesByUserOverDateRange(new List<int> { userInOrg.UserId }, orgId, theDate, theDate);
                    var entries = entryGet.ToList();
                    if (entries.Any(e => (e.Description == null && description.Equals("") || description.Equals(e.Description))
                        && e.Duration == theDuration
                        && e.PayClassId == payClass.PayClassId
                        && e.ProjectId == project.ProjectId)) continue;

                    if (entries.Select(e => e.Duration).Sum() + theDuration > 24)
                    {
                        result.TimeEntryFailures.Add($"Error importing time entry on sheet {table.TableName}, row {table.Rows.IndexOf(row) + 2}: cannot have more than 24 hours of work in one day.");
                        continue;
                    }

                    // All required information is present and valid
                    if (await DBHelper.CreateTimeEntry(new TimeEntryDBEntity
                    {
                        Date = theDate,
                        Description = description,
                        Duration = theDuration.Value, // value is verified earlier
                        FirstName = userInOrg.FirstName,
                        LastName = userInOrg.LastName,
                        PayClassId = payClass.PayClassId,
                        ProjectId = project.ProjectId,
                        UserId = userInOrg.UserId,
                        TimeEntryStatusId = (int)TimeEntryStatus.Pending //all time entries are submitted as pending and must go through the approval process
                    }) == -1)
                    {
                        result.TimeEntryFailures.Add($"Database error importing time entry on sheet {table.TableName}, row {table.Rows.IndexOf(row) + 2}.");
                    }
                    else
                    {
                        result.TimeEntriesImported += 1;
                    }

                    #endregion Project-user and Time Entry Import
                }
            }

            return result;
        }

        /// <summary>
        /// Helper method for reading column data from a spreadsheet. It will try to read data in the given column name from the given
        /// DataRow. If it exists and there is data there (it's not blank), it will then execute the lambda function using the discovered
        /// data, and return true. If either the column does not exist or the row has nothing in that column, the lambda will never be
        /// executed and the function will return false.
        /// </summary>
        /// <param name="row">DataRow.</param>
        /// <param name="columnName">Column name to read.</param>
        /// <param name="useValue">Function to execute using data from column, if present.</param>
        /// <returns>True is data found and function executed, false otherwise.</returns>
        private static bool ReadColumn(DataRow row, string columnName, Func<string, string> useValue)
        {
            try
            {
                string value = row[columnName].ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    useValue(value);
                    return true;
                }
            }
            catch (ArgumentException) { }
            return false;
        }

        /// <summary>
        /// Reads user required fields from matches in a linking data table.
        /// </summary>
        /// <param name="fieldIdFrom">Field index for value linking from (0 = email, 1 = employee id, 2 = name).</param>
        /// <param name="fieldIdTo">Field index for value linking to.</param>
        /// <param name="link">DataTable linking fields.</param>
        /// <param name="fromValue">Value of field linking from.</param>
        /// <returns>Matching value of field linking to, or null.</returns>
        private static string ReadUserDataColumn(int fieldIdFrom, int fieldIdTo, DataTable link, string fromValue)
        {
            try
            {
                fromValue = fromValue.Replace("'", "''"); // Escape any 's in the names
                string selectText;
                if (fieldIdFrom == 2)
                {
                    string[] names = fromValue.Split(new[] { "__IMPORT__" }, StringSplitOptions.None);
                    selectText = string.Format("[{0}] = '{1}' AND [{2}] = '{3}'", ColumnHeaders.UserFirstName, names[0], ColumnHeaders.UserLastName, names[1]);
                }
                else
                {
                    selectText = string.Format("[{0}] = '{1}'", fieldIdFrom == 0 ? ColumnHeaders.UserEmail : ColumnHeaders.EmployeeId, fromValue);
                }

                DataRow row = link.Select(selectText)[0];
                if (fieldIdTo != 2)
                {
                    return row[fieldIdTo == 0 ? ColumnHeaders.UserEmail : ColumnHeaders.EmployeeId].ToString();
                }

                if (row[ColumnHeaders.UserFirstName].ToString().Length == 0 || row[ColumnHeaders.UserLastName].ToString().Length == 0)
                {
                    return null;
                }

                return row[ColumnHeaders.UserFirstName] + "__IMPORT__" + row[ColumnHeaders.UserLastName];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Parses the input duration for either HH.HH or HH:MM format.
        /// </summary>
        /// <param name="duration">Duration in either format.</param>
        /// <returns>Parsed duration or null.</returns>
        public float? ParseDuration(string duration)
        {
            float? durationOut = null;
            if (string.IsNullOrWhiteSpace(duration)) return null;

            Match theMatch;
            if ((theMatch = Regex.Match(duration, HourMinutePattern)).Success)
            {
                float minutes = int.Parse(theMatch.Groups[2].Value) / MinutesInHour;
                durationOut = float.Parse(theMatch.Groups[1].Value) + minutes;
            }
            else if (Regex.Match(duration, DecimalPattern).Success)
            {
                durationOut = float.Parse(duration);
            }

            return durationOut;
        }
    }

    /// <summary>
    /// An object to hold the accumulated results of an import action.
    /// </summary>
    public class ImportActionResult
    {
        /// <summary>
        /// Number of customers successfully imported.
        /// </summary>
        public int CustomersImported { get; set; }

        /// <summary>
        /// Number of projects successfully imported.
        /// </summary>
        public int ProjectsImported { get; set; }

        /// <summary>
        /// Number of users successfully imported.
        /// </summary>
        public int UsersImported { get; set; }

        /// <summary>
        /// Number of users added to the organization.
        /// </summary>
        public int UsersAddedToOrganization { get; set; }

        /// <summary>
        /// Number of users added to the subscription.
        /// </summary>
        public int UsersAddedToSubscription { get; set; }

        /// <summary>
        /// Number of time entries successfully imported.
        /// </summary>
        public int TimeEntriesImported { get; set; }

        /// <summary>
        /// Returns the total number of imported things.
        /// </summary>
        public int TotalImports() => CustomersImported + ProjectsImported + UsersImported + TimeEntriesImported;

        /// <summary>
        /// A list of error messages related to customer imports.
        /// </summary>
        public List<string> CustomerFailures;

        /// <summary>
        /// A list of error messages related to project imports.
        /// </summary>
        public List<string> ProjectFailures;

        /// <summary>
        /// A list of error messages related to user imports.
        /// </summary>
        public List<string> UserFailures;

        /// <summary>
        /// A list of error messages related to adding users to the organization.
        /// </summary>
        public List<string> OrgUserFailures;

        /// <summary>
        /// A list of error messages related to adding users to the subscription.
        /// </summary>
        public List<string> UserSubscriptionFailures;

        /// <summary>
        /// A list of error messages related to time entry imports.
        /// </summary>
        public List<string> TimeEntryFailures;

        /// <summary>
        /// A list of error messages related to time entry imports.
        /// </summary>
        public List<string> GeneralFailures;

        public ImportActionResult()
        {
            CustomerFailures = new List<string>();
            ProjectFailures = new List<string>();
            UserFailures = new List<string>();
            OrgUserFailures = new List<string>();
            UserSubscriptionFailures = new List<string>();
            TimeEntryFailures = new List<string>();
            GeneralFailures = new List<string>();
        }
    }
}