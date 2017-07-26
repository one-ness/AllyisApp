using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Lib;
using AllyisApps.Services.TimeTracker;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;

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
		/// <param name="subscriptionId">subscriptionId</param>
		/// <param name="importData">Workbook with data to import.</param>
		/// <param name="organizationId">The organization's Id</param>
		public ImportActionResult Import(DataSet importData, int subscriptionId = 0, int organizationId = 0)
		{
			int orgId;
			if (subscriptionId > 0 && UserContext.UserSubscriptions[subscriptionId] != null)
			{
				orgId = UserContext.UserSubscriptions[subscriptionId].OrganizationId;
			}
			else if (organizationId > 0 && UserContext.UserOrganizations[organizationId] != null)
			{
				orgId = organizationId;
			}
			else
			{
				return null; //subsciptionId and/or organization id are invalid
			}

			// For some reason, linq won't work directly with DataSets, so we start by just moving the tables over to a linq-able List
			// The tables are ranked and sorted in order to get customers to import first, before projects, avoiding some very complicated look-up logic.
			List<DataTable> tables = new List<DataTable>();
			List<Tuple<DataTable, int>> sortableTables = new List<Tuple<DataTable, int>>();
			foreach (DataTable table in importData.Tables)
			{
				int rank = (table.Columns.Contains(ColumnHeaders.CustomerName) || table.Columns.Contains(ColumnHeaders.CustomerId) ? 3 : 0);
				rank = table.Columns.Contains(ColumnHeaders.ProjectName) || table.Columns.Contains(ColumnHeaders.ProjectId) ? rank == 3 ? 2 : 1 : rank;
				sortableTables.Add(new Tuple<DataTable, int>(table, rank));
			}
			tables = sortableTables.OrderBy(tup => tup.Item2 * -1).Select(tup => tup.Item1).ToList();

			// Retrieval of existing customer and project data
			List<Tuple<Customer, List<Project>>> customersProjects = new List<Tuple<Customer, List<Project>>>();
			foreach (Customer customer in this.GetCustomerList(orgId))
			{
				customersProjects.Add(new Tuple<Customer, List<Project>>(
					customer,
					this.GetProjectsByCustomer(customer.CustomerId).ToList()
				));
			}

			// Retrieval of existing user data
			List<Tuple<string, User>> users = this.GetOrganizationMemberList(orgId).Select(o => new Tuple<string, User>(o.EmployeeId, this.GetUser(o.UserId))).ToList();

			// Retrieval of existing user product subscription data
			int ttProductId = GetProductIdByName("Time Tracker");
			SubscriptionDisplayDBEntity ttSub = DBHelper.GetSubscriptionsDisplayByOrg(orgId).Where(s => s.ProductId == ttProductId).SingleOrDefault();
			List<User> userSubs = this.GetUsersWithSubscriptionToProductInOrganization(orgId, ttProductId).ToList();

			// Retrieval of existing pay class data
			List<PayClass> payClasses = DBHelper.GetPayClasses(orgId).Select(pc => InitializePayClassInfo(pc)).ToList();

			//Result object
			ImportActionResult result = new ImportActionResult();

			// Loop through and see what can be imported from each table in turn. Order doesn't matter, since missing information
			// will be sought from other tables as needed.
			foreach (DataTable table in tables)
			{
				#region Column Header Checks

				// Customer importing: requires both customer name and customer id. Other information is optional, and can be filled in later.
				bool hasCustomerName = table.Columns.Contains(ColumnHeaders.CustomerName);
				bool hasCustomerId = table.Columns.Contains(ColumnHeaders.CustomerId);
				bool canCreateCustomers = hasCustomerName && hasCustomerId;
				List<DataTable> customerImportLinks = new List<DataTable>();
				if (hasCustomerName ^ hasCustomerId)
				{
					// If only one thing is on this sheet, we see if both exist together on another sheet
					customerImportLinks = tables.Where(t => t.Columns.Contains(ColumnHeaders.CustomerName) && t.Columns.Contains(ColumnHeaders.CustomerId)).ToList();
					if (customerImportLinks.Count() > 0)
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
				bool hasCustomerEIN = table.Columns.Contains(ColumnHeaders.CustomerEIN);
				bool hasNonRequiredCustomerInfo = hasCustomerStreetAddress || hasCustomerCity || hasCustomerCountry || hasCustomerState ||
						 hasCustomerPostalCode || hasCustomerEmail || hasCustomerPhoneNumber || hasCustomerFaxNumber || hasCustomerEIN;

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
				bool hasProjectType = table.Columns.Contains(ColumnHeaders.ProjectType);
				bool hasProjectStartDate = table.Columns.Contains(ColumnHeaders.ProjectStartDate);
				bool hasProjectEndDate = table.Columns.Contains(ColumnHeaders.ProjectEndDate);
				bool hasNonRequiredProjectInfo = hasProjectType || hasProjectStartDate || hasProjectEndDate;

				// User importing: requires email, id, first and last name
				bool hasUserEmail = table.Columns.Contains(ColumnHeaders.UserEmail);
				bool hasEmployeeId = table.Columns.Contains(ColumnHeaders.EmployeeId);
				bool hasUserName = table.Columns.Contains(ColumnHeaders.UserFirstName) && table.Columns.Contains(ColumnHeaders.UserLastName);
				List<DataTable>[,] userLinks = new List<DataTable>[3, 3];
				userLinks[0, 1] = userLinks[1, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.UserEmail) && t.Columns.Contains(ColumnHeaders.EmployeeId)).ToList();
				userLinks[0, 2] = userLinks[2, 0] = tables.Where(t => t.Columns.Contains(ColumnHeaders.UserEmail) && t.Columns.Contains(ColumnHeaders.UserFirstName) && t.Columns.Contains(ColumnHeaders.UserLastName)).ToList();
				userLinks[1, 2] = userLinks[2, 1] = tables.Where(t => t.Columns.Contains(ColumnHeaders.EmployeeId) && t.Columns.Contains(ColumnHeaders.UserFirstName) && t.Columns.Contains(ColumnHeaders.UserLastName)).ToList();
				bool canImportUsers =
					(hasUserEmail ? true : userLinks[0, 1].Count > 0 || userLinks[0, 2].Count > 0) &&
					(hasEmployeeId ? true : userLinks[1, 0].Count > 0 || userLinks[1, 2].Count > 0) &&
					(hasUserName ? true : userLinks[2, 0].Count > 0 || userLinks[2, 1].Count > 0);

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
				bool hasEmployeeType = table.Columns.Contains(ColumnHeaders.EmployeeType);  //if not provided, set to Salaried as default
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
						customer = customersProjects.Select(tup => tup.Item1).Where(c => hasCustomerName ? c.Name.Equals(row[ColumnHeaders.CustomerName].ToString()) : c.CustomerOrgId.Equals(row[ColumnHeaders.CustomerId].ToString())).FirstOrDefault();
						if (customer == null)
						{
							if (canCreateCustomers)
							{
								// No customer was found, so a new one is created.
								Customer newCustomer = null;
								if (customerImportLinks.Count == 0)
								{
									// If customerImportLinks is empty, it's because all the information is on this sheet.
									string name = null;
									string custOrgId = null;
									this.readColumn(row, ColumnHeaders.CustomerName, n => name = n);
									this.readColumn(row, ColumnHeaders.CustomerId, n => custOrgId = n);
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
										Name = name,
										CustomerOrgId = custOrgId,
										OrganizationId = orgId
									};
								}
								else
								{
									// If customerImportLinks has been set, we have to grab some information from another sheet.
									string knownValue = null;
									string readValue = null;
									this.readColumn(row, hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId, n => knownValue = n);
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
										Name = hasCustomerName ? knownValue : readValue,
										CustomerOrgId = hasCustomerName ? readValue : knownValue,
										OrganizationId = orgId
									};
								}

								if (newCustomer != null)
								{
									int? newCustomerId = this.CreateCustomer(newCustomer, subscriptionId);
									if (newCustomerId == null)
									{
										result.CustomerFailures.Add(string.Format("Could not create customer {0}: permission failure.", newCustomer.Name));
										continue;
									}

									newCustomer.CustomerId = newCustomerId.Value;
									if (newCustomer.CustomerId == -1)
									{
										result.CustomerFailures.Add(string.Format("Database error while creating customer {0}.", newCustomer.Name));
										continue;
									}

									customersProjects.Add(new Tuple<Customer, List<Project>>(
										newCustomer,
										new List<Project>()
									));
									customer = newCustomer;
									result.CustomersImported += 1;
								}
							}
							else
							{
								// Not enough information to create customer
								result.CustomerFailures.Add(string.Format("Could not create customer {0}: no matching {1}.", row[hasCustomerName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId].ToString(), hasCustomerName ? ColumnHeaders.CustomerId : ColumnHeaders.CustomerName));
							}
						}

						// Importing non-required customer data
						if (customer != null && hasNonRequiredCustomerInfo)
						{
							bool updated = false;

							if (hasCustomerStreetAddress) updated = this.readColumn(row, ColumnHeaders.CustomerStreetAddress, val => customer.Address = val) || updated;
							if (hasCustomerCity) updated = this.readColumn(row, ColumnHeaders.CustomerCity, val => customer.City = val) || updated;
							if (hasCustomerCountry) updated = this.readColumn(row, ColumnHeaders.CustomerCountry, val => customer.Country = val) || updated;
							if (hasCustomerState) updated = this.readColumn(row, ColumnHeaders.CustomerState, val => customer.State = val) || updated;
							if (hasCustomerPostalCode) updated = this.readColumn(row, ColumnHeaders.CustomerPostalCode, val => customer.PostalCode = val) || updated;
							if (hasCustomerEmail) updated = this.readColumn(row, ColumnHeaders.CustomerEmail, val => customer.ContactEmail = val) || updated;
							if (hasCustomerPhoneNumber) updated = this.readColumn(row, ColumnHeaders.CustomerPhoneNumber, val => customer.ContactPhoneNumber = val) || updated;
							if (hasCustomerFaxNumber) updated = this.readColumn(row, ColumnHeaders.CustomerFaxNumber, val => customer.FaxNumber = val) || updated;
							if (hasCustomerEIN) updated = this.readColumn(row, ColumnHeaders.CustomerEIN, val => customer.EIN = val) || updated;

							if (updated)
							{
								this.UpdateCustomer(customer, subscriptionId);
							}
						}
					}

					#endregion Customer Import

					#region Project Import

					DateTime? defaultProjectStartDate = null;
					DateTime? defaultProjectEndDate = null;

					Project project = null;

					// If there is no identifying information for projects, all project related importing is skipped.
					if (hasProjectName || hasProjectId)
					{
						bool thisRowHasProjectName = hasProjectName;
						bool thisRowHasProjectId = hasProjectId;

						// Start with getting the project information that is known from this sheet
						string knownValue = null;
						string readValue = null;
						this.readColumn(row, hasProjectName ? ColumnHeaders.ProjectName : ColumnHeaders.ProjectId, p => knownValue = p);
						if (hasProjectName && hasProjectId)
						{
							// If both columns exist, knownValue is Name and readValue will be Id
							if (!this.readColumn(row, ColumnHeaders.ProjectId, p => readValue = p))
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
							project = customersProjects.Where(tup => tup.Item1.CustomerId == customer.CustomerId).FirstOrDefault().Item2.Where(
								p => thisRowHasProjectName ? p.Name.Equals(knownValue) : p.ProjectOrgId.Equals(knownValue)).FirstOrDefault();
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
								project = new Project
								{
									CustomerId = customer.CustomerId,
									Name = thisRowHasProjectName ? knownValue : readValue,
									Type = "Hourly",
									OrganizationId = orgId,
									ProjectOrgId = thisRowHasProjectName ? readValue : knownValue,
									StartingDate = defaultProjectStartDate,
									EndingDate = defaultProjectEndDate
								};
								project.ProjectId = this.CreateProject(project);
								if (project.ProjectId == -1)
								{
									result.ProjectFailures.Add(string.Format("Database error while creating project {0}", project.Name));
									project = null;
								}
								else
								{
									customersProjects.Where(tup => tup.Item1 == customer).FirstOrDefault().Item2.Add(project);
									result.ProjectsImported += 1;
								}
							}
						}
						else
						{
							//if(!canImportProjects)
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
								for (int j = 0; j < 3; j++)
								{
									// j = field we are currently trying to find
									for (int k = 0; k < 3; k++)
									{
										// k = field we are trying to find j from, using a link
										if (fields[j] == null)
										{
											if (j == k) continue;
											if (fields[k] != null)
											{
												foreach (DataTable link in projectLinks[j, k])
												{
													try
													{
														bool thisLinkCustomerFieldIsName = k == 2 || j == 2 ? link.Columns.Contains(ColumnHeaders.CustomerName) : false;
														fields[j] = link.Select(string.Format("[{0}] = '{1}'",
															k == 0 ? ColumnHeaders.ProjectName : k == 1 ? ColumnHeaders.ProjectId : thisLinkCustomerFieldIsName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId,
															fields[k].Replace("'", "''")
														))[0][j == 0 ? ColumnHeaders.ProjectName : j == 1 ? ColumnHeaders.ProjectId : thisLinkCustomerFieldIsName ? ColumnHeaders.CustomerName : ColumnHeaders.CustomerId].ToString();
														if (fields[j] != null)
														{
															customerFieldIsName = j == 2 ? thisLinkCustomerFieldIsName : customerFieldIsName;
															break;
														}
													}
													catch (IndexOutOfRangeException) { }
												}
											}
										}
									}
								}
							}

							// After that, if we don't have all the information, it's safe to say it can't be found
							if (!string.IsNullOrEmpty(fields[2]))
							{
								customer = customersProjects.Select(tup => tup.Item1).Where(c => customerFieldIsName ? c.Name.Equals(fields[2]) : c.CustomerOrgId.Equals(fields[2])).FirstOrDefault();

								if (customer == null)
								{
									result.ProjectFailures.Add(string.Format("Could not create project {0}: No customer to create it under.", knownValue));
									continue;
								}

								project = customersProjects.Where(tup => tup.Item1.CustomerId == customer.CustomerId).FirstOrDefault().Item2.Where(p => p.Name.Equals(fields[0])).FirstOrDefault();
								if (project == null)
								{
									// Project does not exist, so we should create it
									if (string.IsNullOrEmpty(fields[0]) || string.IsNullOrEmpty(fields[1]))
									{
										result.ProjectFailures.Add(string.Format("Could not create project {0}: no corresponding {1}.", knownValue, thisRowHasProjectName ? ColumnHeaders.ProjectId : ColumnHeaders.ProjectName));
										continue;
									}

									// All required information is known: time to create the project
									project = new Project
									{
										CustomerId = customer.CustomerId,
										Name = fields[0],
										Type = "Hourly",
										OrganizationId = orgId,
										ProjectOrgId = fields[1],
										StartingDate = defaultProjectStartDate,
										EndingDate = defaultProjectEndDate
									};
									project.ProjectId = this.CreateProject(project);
									if (project.ProjectId == -1)
									{
										result.ProjectFailures.Add(string.Format("Database error while creating project {0}", project.Name));
										project = null;
									}
									else
									{
										customersProjects.Where(tup => tup.Item1 == customer).FirstOrDefault().Item2.Add(project);
										result.ProjectsImported += 1;
									}
								}
							}
							else
							{
								// No customer could be found for this project, so we try to find a matching project under any existing customer
								project = customersProjects.Select(
									tup => tup.Item2).Select(
										plst => plst.Where(
											p => thisRowHasProjectName ? p.Name.Equals(knownValue) && (!string.IsNullOrEmpty(fields[1]) ? p.ProjectOrgId.Equals(fields[1]) : true) :
												p.ProjectOrgId.Equals(knownValue) && (!string.IsNullOrEmpty(fields[0]) ? p.Name.Equals(fields[0]) : true)
										).FirstOrDefault()
									).Where(p => p != null).FirstOrDefault();

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

							if (hasProjectType) updated = this.readColumn(row, ColumnHeaders.ProjectType, val => project.Type = val) || updated;
							if (hasProjectStartDate) updated = this.readColumn(row, ColumnHeaders.ProjectStartDate, val => startDate = val) || updated;
							if (hasProjectEndDate) updated = this.readColumn(row, ColumnHeaders.ProjectEndDate, val => endDate = val) || updated;
							if (startDate != null) project.StartingDate = DateTime.Parse(startDate);
							if (endDate != null) project.EndingDate = DateTime.Parse(endDate);

							if (updated)
							{
								this.UpdateProject(subscriptionId, project);
							}
						}
					}

					#endregion Project Import

					#region User Import

					User user = null;
					if (hasUserEmail || hasEmployeeId || hasUserName)
					{
						Tuple<string, User> userTuple = null;

						// Find existing user by whatever information we have
						string readValue = null;
						if (hasUserEmail)
						{
							if (this.readColumn(row, ColumnHeaders.UserEmail, e => readValue = e))
							{
								userTuple = users.Where(tup => tup.Item2.Email.Equals(readValue)).FirstOrDefault();
							}
						}
						if (userTuple == null)
						{
							if (hasEmployeeId)
							{
								if (this.readColumn(row, ColumnHeaders.EmployeeId, e => readValue = e))
								{
									userTuple = users.Where(tup => tup.Item1.Equals(readValue)).FirstOrDefault();
								}
							}
							if (userTuple == null)
							{
								string readLastName = null;
								if (this.readColumn(row, ColumnHeaders.UserFirstName, e => readValue = e) && this.readColumn(row, ColumnHeaders.UserLastName, e => readLastName = e))
								{
									userTuple = users.Where(tup => tup.Item2.FirstName.Equals(readValue) && tup.Item2.LastName.Equals(readLastName)).FirstOrDefault();
								}
							}
						}
						user = userTuple == null ? null : userTuple.Item2;

						if (user == null)
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
                                    hasUserName ? (row[ColumnHeaders.UserFirstName].ToString() == "" || row[ColumnHeaders.UserLastName].ToString() == "" ? null : row[ColumnHeaders.UserFirstName].ToString() + "__IMPORT__" + row[ColumnHeaders.UserLastName].ToString()) : null
								};
								//if (fields[2] == "__IMPORT__") fields[2] = null;

								/*
                                    There are 3 required fields, and we may need to traverse at most 2 links to get them all, with no knowledge of which links will succeed or fail in providing
                                    the needed information. To solve this, we do 2 passes (i), each time checking for the missing information (j) using the links we've found to the information
                                    we already have (k). On each pass, any known information is skipped, so time won't be wasted if the first pass succeeds. This way, any combination of paths
                                    to acquire the missing information from the known information is covered.
                                */
								for (int i = 0; i < 2; i++)
								{
									// i = pass, out of 2
									for (int j = 0; j < 3; j++)
									{
										// j = field we are currently trying to find
										for (int k = 0; k < 3; k++)
										{
											// k = field we are trying to find j from, using a link
											if (fields[j] == null)
											{
												if (j == k) continue;
												if (fields[k] != null)
												{
													foreach (DataTable link in userLinks[j, k])
													{
														try
														{
															fields[j] = this.readUserDataColumn(k, j, link, fields[k]); // A private method that can handle reading one column or the case of both name columns, with no difference in usage here.
															if (fields[j] != null)
															{
																break;
															}
														}
														catch (IndexOutOfRangeException) { }
													}
												}
											}
										}
									}
								}

								if (fields.Any(s => string.IsNullOrEmpty(s)))
								{
									// Couldn't get all the information
									bool[] fieldStatuses = fields.Select(f => string.IsNullOrEmpty(f)).ToArray();
									result.UserFailures.Add(string.Format("Could not create user {0}: missing {1}{2}.", (fieldStatuses[0] ? fieldStatuses[1] ?
										fields[2] != null ? string.Join(" ", fields[2].Split(new string[] { "__IMPORT__" }, StringSplitOptions.None)) : null : fields[1] : fields[0]),
										fieldStatuses[0] ? ColumnHeaders.UserEmail : fieldStatuses[1] ? ColumnHeaders.EmployeeId : string.Format("{0}/{1}", ColumnHeaders.UserFirstName, ColumnHeaders.UserLastName),
										fieldStatuses.Where(s => s).Count() == 2 ? string.Format(" and {0}", !fieldStatuses[2] ? ColumnHeaders.EmployeeId : string.Format("{0}/{1}", ColumnHeaders.UserFirstName, ColumnHeaders.UserLastName)) : ""));
									continue;
								}

								// All required info was found successfully
								string[] names = fields[2].Split(new string[] { "__IMPORT__" }, StringSplitOptions.None);

								if (!Utility.IsValidEmail(fields[0]))
								{
									result.UserFailures.Add(string.Format("Could not create user {0}, {1}: invalid email format ({2}).", names[0], names[1], fields[0]));
									continue;
								}

								try
								{
									user = this.GetUserByEmail(fields[0]); // User may already exist, but not be a member of this organization
									if (user == null)
									{
										user = new User()
										{
											Email = fields[0],
											FirstName = names[0],
											LastName = names[1],
											EmailConfirmationCode = Guid.NewGuid(),
											// TODO: Figure out a better default password generation system
											PasswordHash = Crypto.GetPasswordHash("password")
										};
										try
										{
											var task = DBHelper.CreateUserAsync(GetDBEntityFromUser(user));
											task.RunSynchronously();
											result.UsersImported += 1;
											user.UserId = task.Result;
										}
										catch
										{
											result.UserFailures.Add(string.Format("Could not create user {0}, {1}: error adding user to database.", names[0], names[1]));
										}
									}
									if (user.UserId != -1)
									{
										try
										{
											string employeeType = hasEmployeeType ? row[ColumnHeaders.EmployeeType].ToString() : "";
											//get the id of employeeType, if not found default to Salaried
											int employeeTypeId = DBHelper.GetEmployeeTypeIdByTypeName(employeeType);
											if (employeeTypeId == 0) { employeeTypeId = 1; }
											DBHelper.CreateOrganizationUser(new OrganizationUserDBEntity()
											{
												EmployeeId = fields[1],
												OrganizationId = orgId,
												OrganizationRoleId = (int)(OrganizationRole.Member),
												UserId = user.UserId,
												EmployeeTypeId = employeeTypeId
											});
											result.UsersAddedToOrganization += 1;
										}
										catch (System.Data.SqlClient.SqlException)
										{
											result.OrgUserFailures.Add(string.Format("Database error assigning user {0} {1} to organization. Could be a duplicate employee id ({2}).", names[0], names[1], fields[1]));
											continue;
										}
										users.Add(new Tuple<string, User>(fields[1], user));
									}
									else
									{
										result.UserFailures.Add(string.Format("Database error creating user {0} {1}.", names[0], names[1]));
										continue;
									}
								}
								catch (System.Data.SqlClient.SqlException)
								{
									result.UserFailures.Add(string.Format("Database error creating user {0} {1}.", names[0], names[1]));
									continue;
								}
							}
						}

						// Importing non-required user data
						if (user != null && hasNonRequiredUserInfo)
						{
							bool updated = false;

							if (hasUserAddress) updated = this.readColumn(row, ColumnHeaders.UserAddress, val => user.Address = val) || updated;
							if (hasUserCity) updated = this.readColumn(row, ColumnHeaders.UserCity, val => user.City = val) || updated;
							if (hasUserCountry) updated = this.readColumn(row, ColumnHeaders.UserCountry, val => user.Country = val) || updated;
							string dateOfBirth = null;
							if (hasUserDateOfBirth) updated = this.readColumn(row, ColumnHeaders.UserDateOfBirth, val => dateOfBirth = val) || updated;
							if (!string.IsNullOrEmpty(dateOfBirth))
							{
								DateTime dob;
								DateTime.TryParse(dateOfBirth, out dob);
								if (DateTime.Compare(dob, DateTime.MinValue) <= 0)
								{
									result.UserFailures.Add(string.Format("The birthdate entered for {0} {1} was invalid. Please check to make sure it's in date format: dd/mm/yyyy and preferably after 1900 ", user.FirstName, user.LastName));
								}
								else
								{
									user.DateOfBirth = dob;
								}
							}

							if (hasUserPhoneExtension) updated = this.readColumn(row, ColumnHeaders.UserPhoneExtension, val => user.PhoneExtension = val) || updated;
							if (hasUserPhoneNumber) updated = this.readColumn(row, ColumnHeaders.UserPhoneNumber, val => user.PhoneNumber = val) || updated;
							if (hasUserPostalCode) updated = this.readColumn(row, ColumnHeaders.UserPostalCode, val => user.PostalCode = val) || updated;
							if (hasUserState) updated = this.readColumn(row, ColumnHeaders.UserState, val => user.State = val) || updated;

							if (updated)
							{
								this.SaveUserInfo(user);
							}
						}
					}

					#endregion User Import

					#region Project-user and Time Entry Import

					if (canImportProjectUser)
					{
						// Double-check that previous adding/finding of project and user didn't fail
						if (project != null && user != null)
						{
							// Find existing project user
							if (!this.GetProjectsByUserAndOrganization(user.UserId).Where(p => p.ProjectId == project.ProjectId).Any())
							{
								// If no project user entry exists for this user and project, we create one.
								this.CreateProjectUser(project.ProjectId, user.UserId);
							}

							// Time Entry Import
							if (canImportTimeEntry)
							{
								// Check for subscription role
								bool canImportThisEntry = false;
								if (!userSubs.Where(u => u.UserId == user.UserId).Any())
								{
									// No existing subscription for this user, so we create one.
									if (ttSub.SubscriptionsUsed < ttSub.NumberOfUsers)
									{
										this.DBHelper.UpdateSubscriptionUserProductRole((int)(TimeTrackerRole.User), ttSub.SubscriptionId, user.UserId);
										userSubs.Add(user);
										result.UsersAddedToSubscription += 1;
										canImportThisEntry = true; // Successfully created.
									}
									else
									{
										result.UserSubscriptionFailures.Add(string.Format("Cannot add user {0} {1} to Time Tracker subscription: number of users for subscription is at maximum ({2}).", user.FirstName, user.LastName, ttSub.SubscriptionsUsed));
										continue;
									}
								}
								else
								{
									// Found existing subscription user.
									canImportThisEntry = true;
								}

								// Import entry
								if (canImportThisEntry)
								{
									string date = null;
									string duration = null;
									string description = "";
									string payclass = "Regular";

									this.readColumn(row, ColumnHeaders.Date, val => date = val);
									this.readColumn(row, ColumnHeaders.Duration, val => duration = val);
									if (hasTTDescription) this.readColumn(row, ColumnHeaders.Description, val => description = val);
									this.readColumn(row, ColumnHeaders.PayClass, val => payclass = val);

									PayClass payClass = payClasses.Where(p => p.Name.ToUpper().Equals(payclass.ToUpper())).SingleOrDefault();
									DateTime theDate;
									float? theDuration;

									if (payClass == null)
									{
										result.TimeEntryFailures.Add(string.Format("Error importing time entry on sheet {0}, row {1}: unknown {2} ({3}).", table.TableName, table.Rows.IndexOf(row) + 2, ColumnHeaders.PayClass, payclass));
										continue;
									}

									try
									{
										theDate = DateTime.Parse(date);
										if (theDate.Year < 1753) throw new FormatException();
									}
									catch (Exception)
									{
										result.TimeEntryFailures.Add(string.Format("Error importing time entry on sheet {0}, row {1}: bad date format ({2}).", table.TableName, table.Rows.IndexOf(row) + 2, date));
										continue;
									}

									if (!(theDuration = this.ParseDuration(duration)).HasValue)
									{
										result.TimeEntryFailures.Add(string.Format("You must enter the duration as HH:MM or H.HH format for the date {0}", theDate));
										continue;
									}
									if(this.ParseDuration(duration) == 0)
									{
										result.TimeEntryFailures.Add(string.Format("You must enter a time larger than 00:00 for the date {0}", theDate));
										continue;
									}

									// Find existing entry. If none, create new one     TODO: See if there's a good way to populate this by sheet rather than by row, or once at the top
									List<TimeEntryDBEntity> entries = DBHelper.GetTimeEntriesByUserOverDateRange(new List<int> { user.UserId }, orgId, theDate, theDate).ToList();
									if (!entries.Where(e => e.Description.Equals(description) && e.Duration == theDuration && e.PayClassId == payClass.PayClassId && e.ProjectId == project.ProjectId).Any())
									{
										if (entries.Select(e => e.Duration).Sum() + theDuration > 24)
										{
											result.TimeEntryFailures.Add(string.Format("Error importing time entry on sheet {0}, row {1}: cannot have more than 24 hours of work in one day.", table.TableName, table.Rows.IndexOf(row) + 2));
											continue;
										}

										// All required information is present and valid
										if (DBHelper.CreateTimeEntry(new DBModel.TimeTracker.TimeEntryDBEntity
										{
											Date = theDate,
											Description = description,
											Duration = theDuration.Value, //value is verified earlier
											FirstName = user.FirstName,
											LastName = user.LastName,
											PayClassId = payClass.PayClassId,
											ProjectId = project.ProjectId,
											UserId = user.UserId
										}) == -1)
										{
											result.TimeEntryFailures.Add(string.Format("Database error importing time entry on sheet {0}, row {1}.", table.TableName, table.Rows.IndexOf(row) + 2));
										}
										else
										{
											result.TimeEntriesImported += 1;
										}
									}
								}
							}
						}
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
		private bool readColumn(DataRow row, string columnName, Func<string, string> useValue)
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
		private string readUserDataColumn(int fieldIdFrom, int fieldIdTo, DataTable link, string fromValue)
		{
			try
			{
				fromValue = fromValue.Replace("'", "''"); //Escape any 's in the names
				string selectText = null;
				if (fieldIdFrom == 2)
				{
					string[] names = fromValue.Split(new string[] { "__IMPORT__" }, StringSplitOptions.None);
					selectText = string.Format("[{0}] = '{1}' AND [{2}] = '{3}'", ColumnHeaders.UserFirstName, names[0], ColumnHeaders.UserLastName, names[1]);
				}
				else
				{
					selectText = string.Format("[{0}] = '{1}'", fieldIdFrom == 0 ? ColumnHeaders.UserEmail : ColumnHeaders.EmployeeId, fromValue);
				}
				DataRow row = link.Select(selectText)[0];
				if (fieldIdTo == 2)
				{
					if (row[ColumnHeaders.UserFirstName].ToString() == "" || row[ColumnHeaders.UserLastName].ToString() == "") return null;
					else return row[ColumnHeaders.UserFirstName].ToString() + "__IMPORT__" + row[ColumnHeaders.UserLastName].ToString();
				}
				else
				{
					return row[fieldIdTo == 0 ? ColumnHeaders.UserEmail : ColumnHeaders.EmployeeId].ToString();
				}
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
			Match theMatch;
			if (!string.IsNullOrWhiteSpace(duration))
			{
				if ((theMatch = Regex.Match(duration, HourMinutePattern)).Success)
				{
					float minutes = int.Parse(theMatch.Groups[2].Value) / MinutesInHour;
					durationOut = float.Parse(theMatch.Groups[1].Value) + minutes;
				}
				else if ((theMatch = Regex.Match(duration, DecimalPattern)).Success)
				{
					durationOut = float.Parse(duration);
				}
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

		public ImportActionResult()
		{
			CustomerFailures = new List<string>();
			ProjectFailures = new List<string>();
			UserFailures = new List<string>();
			OrgUserFailures = new List<string>();
			UserSubscriptionFailures = new List<string>();
			TimeEntryFailures = new List<string>();
		}
	}
}
