using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using System.Collections;
using AllyisApps.Lib;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Configuration;
using static AllyisApps.Services.Actions;
using AllyisApps.DBModel;
using AllyisApps.Services.Billing;

namespace AllyisApps.Services.Tests
{

    [TestClass]
    public class AuthTest
    {
        public static string connectionStr = "Data Source=(local);Initial Catalog=AllyisAppsDB;User Id=aaUser;Password=BlueSky23#;";

        /**********************************************
        ** Set up test data
        **********************************************/

        //create an user for testing purpose and return that user's UserId
        //precondition: a user with this email address does not exist in the database
        public static int createTestUser(string email)
        {
            var service = new Service(connectionStr);
            string insertStmt = "INSERT INTO [Auth].[User]([FirstName], [LastName],[Email],[UserName],[PasswordHash] ,[LanguagePreference]) " +
                                "VALUES(@fname, @lname, @email, @email, @pwHash, @langpref)";
            string selectStmt = "SELECT [UserId] FROM [Auth].[User] WHERE [Email] = @email";
            SqlDataReader reader;
            int userId = -1;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(insertStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@fname", SqlDbType.VarChar, 100).Value = "Test";
                    cmd.Parameters.Add("@lname", SqlDbType.VarChar, 100).Value = "User";
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@pwHash", SqlDbType.VarChar).Value = Crypto.GetPasswordHash("AllyisApps.123");
                    cmd.Parameters.Add("@langpref", SqlDbType.Bit).Value = 1;

                    int result = cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;

                    // execute cmd
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        userId = (int)reader["UserId"];
                    }
                }
                connection.Close();
            }
            return userId;
        }

        //check whether a test user is already inserted into the db
        public static bool testUserExists(string email)
        {
            var service = new Service(connectionStr);
            string selectStmt = "SELECT [UserId] FROM [Auth].[User] WHERE [Email] = @email";
            SqlDataReader reader;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows) return true; //found
                    connection.Close();
                }
            }
            return false;
        }

        //delete test user from the User table after the test
        public static void deleteTestUser(string email)
        {
            var service = new Service(connectionStr);
            string stmt1 = "DELETE FROM [Auth].[User] WHERE [Email] = @email";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                //open connection
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(stmt1, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

                    //execute command
                    int result = cmd.ExecuteNonQuery();
                }

                //close connection
                connection.Close();
            }
        }

        //create a test Org and return the OrganizationId
        public static int createTestOrg(string orgName)
        {
            var service = new Service(connectionStr);
            string insertStmt = "INSERT INTO [Auth].[Organization]([Name], [Subdomain], [IsActive]) " +
                                "VALUES(@name, @subdomain, @isActive)";
            string selectStmt = "SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Name] = @name";
            SqlDataReader reader;
            int orgId = -1;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(insertStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = orgName;
                    cmd.Parameters.Add("@subdomain", SqlDbType.VarChar, 100).Value = orgName.ToLower();
                    cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = 1;

                    // execute cmd
                    int result = cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = orgName;

                    // execute cmd
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orgId = (int)reader["OrganizationId"];
                    }
                }
                connection.Close();
            }
            return orgId;
        }

        //delete test org from the Organization table after the test
        public static void deleteTestOrg(int orgId)
        {
            var service = new Service(connectionStr);
            string stmt1 = "DELETE FROM [Auth].[Organization] WHERE [OrganizationId] = @orgId";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                //open connection
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(stmt1, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;

                    //execute command
                    int result = cmd.ExecuteNonQuery();
                }

                //close connection
                connection.Close();
            }
        }

        //create a simple invitation (without subscription or role) and return the id
        public static int createUserInvitation(string email, int orgId, int orgRole, string employeeId)
        {
            var service = new Service(connectionStr);
            string insertStmt = "INSERT INTO [Auth].[Invitation]([OrganizationId], [FirstName], [LastName],[Email],[IsActive],[EmployeeId],[DateOfBirth],[AccessCode],[OrgRole]) " +
                                "VALUES(@orgId, @fname, @lname, @email, @isActive, @employeeId, @dob, @accessCode, @orgRole)";
            string selectStmt = "SELECT [InvitationId] FROM [Auth].[Invitation] WHERE [OrganizationId] = @orgId AND [Email] = @email";
            SqlDataReader reader;
            int inviId = -1;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(insertStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
                    cmd.Parameters.Add("@fname", SqlDbType.VarChar, 100).Value = "Test";
                    cmd.Parameters.Add("@lname", SqlDbType.VarChar, 100).Value = "User";
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;
                    cmd.Parameters.Add("@employeeId", SqlDbType.VarChar, 100).Value = employeeId;
                    cmd.Parameters.Add("@dob", SqlDbType.DateTime2).Value = "1980-01-01";
                    cmd.Parameters.Add("@accessCode", SqlDbType.VarChar, 100).Value = "fakecode";
                    cmd.Parameters.Add("@orgRole", SqlDbType.Int).Value = orgRole;

                    int result = cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;

                    // execute cmd
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        inviId = (int)reader["InvitationId"];
                    }
                }
                connection.Close();
            }
            return inviId;
        }

        public static void deleteUserInvitation(string email)
        {
            var service = new Service(connectionStr);
            string stmt = "DELETE FROM [Auth].[Invitation] WHERE [Email] = @email";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(stmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

                    //execute command
                    int result = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void createOrgUser(int userId, int orgId, int roleId, string employeeId)
        {
            var service = new Service(connectionStr);
            string insertStmt = "INSERT INTO [Auth].[OrganizationUser]([UserId], [OrganizationId], [EmployeeId], [OrgRoleId]) " +
                                "VALUES(@userId, @orgId, @employeeId, @orgRoleId)";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(insertStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
                    cmd.Parameters.Add("@employeeId", SqlDbType.VarChar, 16).Value = employeeId;
                    cmd.Parameters.Add("@orgRoleId", SqlDbType.Int).Value = roleId;

                    // execute cmd
                    int result = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void deleteOrgUser(int userId, int orgId)
        {
            var service = new Service(connectionStr);
            string stmt = "DELETE FROM [Auth].[OrganizationUser] WHERE [UserId] = @userId AND [OrganizationId] = @orgId";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(stmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;

                    //execute command
                    int result = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static int createSubscription(int orgId, int skuId, int numOfUsers)
        {
            var service = new Service(connectionStr);
            string insertStmt = "INSERT INTO [Billing].[Subscription]([OrganizationId], [SkuId], [NumberOfUsers],[IsActive]) " +
                                "VALUES(@orgId, @skuId, @num, @isActive)";
            string selectStmt = "SELECT [SubscriptionId] FROM [Billing].[Subscription] WHERE [OrganizationId] = @orgId AND [SkuId] = @skuId";
            SqlDataReader reader;
            int subId = -1;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(insertStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
                    cmd.Parameters.Add("@skuId", SqlDbType.Int).Value = skuId;
                    cmd.Parameters.Add("@num", SqlDbType.Int).Value = numOfUsers;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;

                    int result = cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
                    cmd.Parameters.Add("@skuId", SqlDbType.Int).Value = skuId;

                    // execute cmd
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        subId = (int)reader["SubscriptionId"];
                    }
                }
                connection.Close();
            }
            return subId;
        }

        public static void deleteSubscription(int subId)
        {
            var service = new Service(connectionStr);
            string stmt = "DELETE FROM [Billing].[Subscription] WHERE [SubscriptionId] = @subId";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(stmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@subId", SqlDbType.Int).Value = subId;
                    
                    //execute command
                    int result = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void createSubUser(int subId, int userId, int productRoleId)
        {
            var service = new Service(connectionStr);
            string insertStmt = "INSERT INTO [Billing].[SubscriptionUser]([SubscriptionId], [UserId], [ProductRoleId]) " +
                                "VALUES(@subId, @userId, @pRoleId)";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(insertStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@subId", SqlDbType.Int).Value = subId;
                    cmd.Parameters.Add("@pRoleId", SqlDbType.Int).Value = productRoleId;

                    // execute cmd
                    int result = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void deleteSubUser(int userId, int subId)
        {
            var service = new Service(connectionStr);
            string stmt = "DELETE FROM [Billing].[SubscriptionUser] WHERE [UserId] = @userId AND [SubscriptionId] = @subId";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(stmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@subId", SqlDbType.Int).Value = subId;

                    //execute command
                    int result = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        /**************************************************************************************************
        *                              Unit tests for methods in AccountService.cs
        ***************************************************************************************************/

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Country name must have a value.")]
        public void ValidStates_Should_Throw_Exception_For_Empty_Country_Name()
        {
            //Arrange
            string countryName = "";

            var service = new Service(connectionStr);

            //Act
            IEnumerable validStates = service.ValidStates(countryName);
        }

        [TestMethod]
        public void ValidStates_Should_Return_All_States_For_Valid_Country_Name()
        {
            //Arrange
            string countryName = "United Kingdom";
            var service = new Service(connectionStr);
            IEnumerable<string> expected = new List<string>() { "Scotland", "Wales", "Northern Ireland", "England" };

            //Act
            IEnumerable returnedStates = service.ValidStates(countryName);
            IEnumerable<string> returned = returnedStates.Cast<string>();

            //Assert
            bool isCorrect = (returned.Count() == expected.Count() && returned.Intersect(expected).Count() == expected.Count());
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void ValidCountries_Should_Return_All_Countries()
        {
            //Arrange
            var service = new Service(connectionStr);

            //Act
            IEnumerable returnedCountries = service.ValidCountries();
            IEnumerable<string> returned = returnedCountries.Cast<string>();

            //Assert
            Assert.IsTrue(returned.Count() == 242);
        }

        [TestMethod]
        public void ValidLanguages_Should_Return_All_Languages()
        {
            //Arrange
            var service = new Service(connectionStr);

            //Act
            IEnumerable<Language> returnedLang = service.ValidLanguages();

            //Assert
            Assert.IsTrue(returnedLang.Count() == 2);
        }

        [TestMethod]
        public void GetInvitationsByUser_Should_Retrieve_All_Invitations_For_A_User()
        {
            //Arrange
            string userEmail = "testuser@test.com";
            string orgName = "UnitTestOrg";

            createTestUser(userEmail);
            int orgId = createTestOrg(orgName);
            createUserInvitation(userEmail, orgId, 1, "111");

            var service = new Service(connectionStr);

            //Act
            List<InvitationInfo> invi = service.GetInvitationsByUser(userEmail);

            //Clean up
            deleteUserInvitation(userEmail);
            deleteTestOrg(orgId);
            deleteTestUser(userEmail);

            //Assert 
            Assert.IsTrue(invi.Count() == 1);
        }

        [TestMethod]
        //it should add the user to the invitation's organization, subscriptions, and projects, then deletes the invitations
        public void AcceptUserInvitation_Should_Add_User_To_Corresponding_Tables_And_Delete_Invitation()
        {
            //Arange
            string userEmail = "testuser@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(userEmail);
            int orgId = createTestOrg(orgName);
            int inviId = createUserInvitation(userEmail, orgId, 1, "111");

            UserContext userContext = new UserContext(userId, userEmail, userEmail, 0, 0, null, 0);
            var service = new Service(connectionStr, userContext);

            //Act
            string result = service.AcceptUserInvitation(inviId);

            //user should be added to the OrganizationUser table
            string selectStmt = "SELECT [UserId] FROM [Auth].[OrganizationUser] WHERE [UserId] = @userId AND [OrganizationId] = @orgId AND [EmployeeId] = @employeeId AND [OrgRoleId] = @orgRoleId";
            SqlDataReader reader;
            bool addedToOrgUserTable = false;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
                    cmd.Parameters.Add("@employeeId", SqlDbType.VarChar, 16).Value = "111";
                    cmd.Parameters.Add("@orgRoleId", SqlDbType.Int).Value = 1;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows) { addedToOrgUserTable = true; }
                    connection.Close();
                }
            }
            //invitation is removed from Invitation table
            string selectStmt1 = "SELECT [InvitationId] FROM [Auth].[Invitation] WHERE [InvitationId] = @inviId";
            SqlDataReader reader1;
            bool inviRemoved = false;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt1, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@inviId", SqlDbType.Int).Value = inviId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader1 = cmd.ExecuteReader();
                    if (!reader1.HasRows) { inviRemoved = true; }
                    connection.Close();
                }
            }
            //returned string includes org name and role
            string expectedStr = "You have successfully joined " + orgName + " in the role of Member.";

            //Clean up
            if (inviRemoved == false) { deleteUserInvitation(userEmail); }
            deleteOrgUser(userId, orgId);
            deleteTestUser(userEmail);
            deleteTestOrg(orgId);

            //Assert
            Assert.IsTrue(addedToOrgUserTable && inviRemoved && result.Equals(expectedStr));
        }

        [TestMethod]
        public void RejectUserInvitation_Should_Delete_Invitation()
        {
            //Arange
            string userEmail = "testuser@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(userEmail);
            int orgId = createTestOrg(orgName);
            int inviId = createUserInvitation(userEmail, orgId, 1, "111");

            UserContext userContext = new UserContext(userId, userEmail, userEmail, 0, 0, null, 0);
            var service = new Service(connectionStr, userContext);

            //Act
            string result = service.RejectUserInvitation(inviId);

            //user should NOT be added to the OrganizationUser table
            string selectStmt = "SELECT [UserId] FROM [Auth].[OrganizationUser] WHERE [UserId] = @userId AND [OrganizationId] = @orgId AND [EmployeeId] = @employeeId AND [OrgRoleId] = @orgRoleId";
            SqlDataReader reader;
            bool notAddedToOrgUserTable = false;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
                    cmd.Parameters.Add("@employeeId", SqlDbType.VarChar,16).Value = "111";
                    cmd.Parameters.Add("@orgRoleId", SqlDbType.Int).Value = 1;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    if (!reader.HasRows) { notAddedToOrgUserTable = true; }
                    connection.Close();
                }
            }
            //invitation is removed from Invitation table
            string selectStmt1 = "SELECT [InvitationId] FROM [Auth].[Invitation] WHERE [InvitationId] = @inviId";
            SqlDataReader reader1;
            bool inviRemoved = false;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt1, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@inviId", SqlDbType.Int).Value = inviId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader1 = cmd.ExecuteReader();
                    if (!reader1.HasRows) { inviRemoved = true; }
                    connection.Close();
                }
            }
            //returned string
            string expectedStr = "The invitation has been rejected.";

            //Clean up
            if (inviRemoved == false) { deleteUserInvitation(userEmail); }
            //deleteOrgUser(userId, orgId);
            deleteTestUser(userEmail);
            deleteTestOrg(orgId);

            //Assert
            Assert.IsTrue(notAddedToOrgUserTable && inviRemoved && result.Equals(expectedStr));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "User name must have a value.")]
        public async Task SetUpNewUser_Should_Throw_Exception_For_Empty_First_Name()
        {
            //Arrange
            string email = "testuser@test.com";
            string fname = "";
            string lname = "User";
            DateTime dob = new DateTime();
            string address = "";
            string city = "";
            string state = "";
            string country = "";
            string postalCode = "";
            string phone = "";
            string password = "fakepassword";
            int lang = 1;
            string confirmUrl = "";
            bool twoFactorEnabled = false;
            bool lockOutEnabled = false;
            DateTime lockOutEndDateUtc = new DateTime();

            var service = new Service(connectionStr);

            //Act
            await service.SetupNewUser(email, fname, lname, dob, address, city, state, country, postalCode, phone, password, lang, confirmUrl, twoFactorEnabled, lockOutEnabled, lockOutEndDateUtc);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "User name must have a value.")]
        public async Task SetUpNewUser_Should_Throw_Exception_For_Empty_Last_Name()
        {
            //Arrange
            string email = "testuser@test.com";
            string fname = "Test";
            string lname = "";
            DateTime dob = new DateTime();
            string address = "";
            string city = "";
            string state = "";
            string country = "";
            string postalCode = "";
            string phone = "";
            string password = "fakepassword";
            int lang = 1;
            string confirmUrl = "";
            bool twoFactorEnabled = false;
            bool lockOutEnabled = false;
            DateTime lockOutEndDateUtc = new DateTime();

            var service = new Service(connectionStr);

            //Act
            await service.SetupNewUser(email, fname, lname, dob, address, city, state, country, postalCode, phone, password, lang, confirmUrl, twoFactorEnabled, lockOutEnabled, lockOutEndDateUtc);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email address must have a value.")]
        public async Task SetUpNewUser_Should_Throw_Exception_For_Empty_Email()
        {
            //Arrange
            string email = "";
            string fname = "Test";
            string lname = "User";
            DateTime dob = new DateTime();
            string address = "";
            string city = "";
            string state = "";
            string country = "";
            string postalCode = "";
            string phone = "";
            string password = "fakepassword";
            int lang = 1;
            string confirmUrl = "";
            bool twoFactorEnabled = false;
            bool lockOutEnabled = false;
            DateTime lockOutEndDateUtc = new DateTime();

            var service = new Service(connectionStr);

            //Act
            await service.SetupNewUser(email, fname, lname, dob, address, city, state, country, postalCode, phone, password, lang, confirmUrl, twoFactorEnabled, lockOutEnabled, lockOutEndDateUtc);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Email address must be in a valid format.")]
        public async Task SetUpNewUser_Should_Throw_Exception_For_Invalid_Email()
        {
            //Arrange
            string email = "invalidemail";
            string fname = "Test";
            string lname = "User";
            DateTime dob = new DateTime();
            string address = "";
            string city = "";
            string state = "";
            string country = "";
            string postalCode = "";
            string phone = "";
            string password = "fakepassword";
            int lang = 1;
            string confirmUrl = "";
            bool twoFactorEnabled = false;
            bool lockOutEnabled = false;
            DateTime lockOutEndDateUtc = new DateTime();

            var service = new Service(connectionStr);

            //Act
            await service.SetupNewUser(email, fname, lname, dob, address, city, state, country, postalCode, phone, password, lang, confirmUrl, twoFactorEnabled, lockOutEnabled, lockOutEndDateUtc);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Password must have a value.")]
        public async Task SetUpNewUser_Should_Throw_Exception_For_Empty_Password()
        {
            //Arrange
            string email = "testuser@test.com";
            string fname = "Test";
            string lname = "User";
            DateTime dob = new DateTime();
            string address = "";
            string city = "";
            string state = "";
            string country = "";
            string postalCode = "";
            string phone = "";
            string password = "";
            int lang = 1;
            string confirmUrl = "";
            bool twoFactorEnabled = false;
            bool lockOutEnabled = false;
            DateTime lockOutEndDateUtc = new DateTime();

            var service = new Service(connectionStr);

            //Act
            await service.SetupNewUser(email, fname, lname, dob, address, city, state, country, postalCode, phone, password, lang, confirmUrl, twoFactorEnabled, lockOutEnabled, lockOutEndDateUtc);
        }

        [TestMethod]
        public async Task SetUpNewUser_Should_Throw_Exception_For_Duplicate_Email()
        {
            //Arrange
            string email = "testuser@test.com";
            string fname = "Test";
            string lname = "User";
            DateTime dob = DateTime.Today;
            string address = "";
            string city = "";
            string state = "";
            string country = "";
            string postalCode = "";
            string phone = "";
            string password = "AllyisApps.123";
            int lang = 1;
            string confirmUrl = "";
            bool twoFactorEnabled = false;
            bool lockOutEnabled = false;
            DateTime lockOutEndDateUtc = DateTime.Today;

            var service = new Service(connectionStr);
            createTestUser(email);

            //Act
            try
            {
                var result = await service.SetupNewUser(email, fname, lname, dob, address, city, state, country, postalCode, phone, password, lang, confirmUrl, twoFactorEnabled, lockOutEnabled, lockOutEndDateUtc);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is SqlException);
            }

            //Clean up
            deleteTestUser(email);
        }

        [TestMethod]
        public async Task SetUpNewUser_Should_Create_New_User_With_Valid_Arguments()
        {
            //Arrange
            string email = "testuser@test.com";
            string fname = "Test";
            string lname = "User";
            DateTime dob = DateTime.Today;
            string address = "";
            string city = "";
            string state = "";
            string country = "";
            string postalCode = "";
            string phone = "";
            string password = "AllyisApps.123";
            int lang = 1;
            string confirmUrl = "http://allyisapps.com/Account/ConfirmEmail?userId=%7BuserId%7D&code=%7Bcode%7D";
            bool twoFactorEnabled = false;
            bool lockOutEnabled = false;
            DateTime lockOutEndDateUtc = DateTime.Today;

            var service = new Service(connectionStr);

            //Act
            await service.SetupNewUser(email, fname, lname, dob, address, city, state, country, postalCode, phone, password, lang, confirmUrl, twoFactorEnabled, lockOutEnabled, lockOutEndDateUtc);
            bool userCreated = testUserExists(email);

            //Clean up
            if (userCreated) { deleteTestUser(email); } 

            //Assert
            Assert.IsTrue(userCreated);
        }

        [TestMethod]
        public void ValidateLogin_Should_Return_Null_If_User_Is_Not_Found()
        {
            //Arrange
            string email = "testuser@test.com";
            string password = "AllyisApps.123";
            var service = new Service(connectionStr);

            //Act
            UserContext output = service.ValidateLogin(email, password);

            //Assert
            Assert.IsTrue(output == null);
        }

        [TestMethod]
        public void ValidateLogIn_Should_Return_Correct_User_For_Valid_Login_Credentials()
        {
            //Arrange
            string email = "testuser@test.com";
            string password = "AllyisApps.123";
            int userId = createTestUser(email);
            var service = new Service(connectionStr);
            UserContext expected = new UserContext(userId, email, email);

            //Act
            UserContext output = service.ValidateLogin(email, password);

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(expected.UserId == output.UserId && expected.Email == output.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "User ID cannot be 0 or negative.")]
        public void PopulateUserContext_Should_Throw_Exception_If_UserId_Is_Invalid()
        {
            var service = new Service(connectionStr);

            //Act
            service.PopulateUserContext(-1);
        }

        //Case 1: User is not part of any organization
        [TestMethod]
        public void PopulateUserContext_Should_Return_Fully_Populated_UserContext_With_Zero_Org()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            var service = new Service(connectionStr);

            //Act
            UserContext populated = service.PopulateUserContext(userId);

            //Cleanup
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(populated.UserId == userId
                && populated.UserName == email
                && populated.UserOrganizationInfoList.Count() == 0
                && populated.Email == email
                && populated.ChosenSubscriptionId == 0
                && populated.ChosenOrganizationId == 0
                && populated.ChosenLanguageID == 1);
        }

        //Case 2: User is part of one organization
        //TODO: update test to check subscription as well
        [TestMethod]
        public void PopulateUserContext_Should_Return_Fully_Populated_UserContext_With_One_Org()
        {
            //Arrange
            string email = "testuser@test.com";
            string orgName = "UnitTestOrg";

            int userId = createTestUser(email);
            int orgId = createTestOrg(orgName);
            createOrgUser(userId, orgId, 1, "111");    //roleId = 1 (member)

            var service = new Service(connectionStr);

            //Act
            UserContext populated = service.PopulateUserContext(userId);

            //Cleanup
            deleteOrgUser(userId, orgId);
            deleteTestOrg(orgId);
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(populated.UserId == userId
                && populated.UserName == email
                && populated.UserOrganizationInfoList.Count() == 1
                && populated.Email == email
                && populated.ChosenSubscriptionId == 0
                && populated.ChosenOrganizationId == orgId
                && populated.ChosenLanguageID == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "User ID cannot be 0 or negative.")]
        public void GetUser_Should_Throw_Exception_For_Invalid_UserId()
        {
            //Arrange
            var service = new Service(connectionStr);

            //Act
            service.GetUser(0);
        }

        [TestMethod]
        public void GetUser_Should_Return_User_Info_For_Valid_UserId()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            var service = new Service(connectionStr);

            //Act
            User user = service.GetUser(userId);

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(user.AccessFailedCount == 0
                && user.ActiveOrganizationId == 0
                && user.Address == null
                && user.City == null
                && user.Country == null
                && user.DateOfBirth == null
                && user.Email == email
                && user.EmailConfirmed == false
                && user.FirstName == "Test"
                && user.LastName == "User"
                && user.LastSubscriptionId == 0
                && user.LockoutEnabled == false
                && user.LockoutEndDateUtc == null
                && user.PasswordHash == null
                && user.PasswordResetCode == null
                && user.PhoneExtension == null
                && user.PhoneNumber == null
                && user.PhoneNumberConfirmed == false
                && user.State == null
                && user.TwoFactorEnabled == false
                && user.UserId == userId
                && user.UserName == email
                && user.PostalCode == null);
        }

        [TestMethod]
        public void GetUserOrgsAndInvitationInfo_Should_Return_Info()
        {
            //Arrange
            string email = "testuser@test.com";
            string orgName1 = "UnitTestOrg1";
            string orgName2 = "UnitTestOrg2";

            int userId = createTestUser(email);
            int org1Id = createTestOrg(orgName1);
            int org2Id = createTestOrg(orgName2);
            createOrgUser(userId, org1Id, 1, "111");  //test user is member of Org1
            int inviId = createUserInvitation(email, org2Id, 1, "111");    //test user is invited to Org2

            Organization org2 = new Organization();
            org2.Name = orgName2;

            UserContext userContext = new UserContext(userId, email, email);
            var service = new Service(connectionStr, userContext);

            //Act
            Tuple<User, List<Organization>, List<InvitationInfo>> userInfo = service.GetUserOrgsAndInvitationInfo();
            var returnedUser = userInfo.Item1;
            bool returnedCorrectUser = (returnedUser.UserId == userId && returnedUser.Email == email);

            var returnedOrg = userInfo.Item2;
            bool returnedCorrectOrg = (returnedOrg.Count() == 1 && returnedOrg.ElementAt(0).OrganizationId == org1Id);

            var returnedInvi = userInfo.Item3;
            bool returnedCorrectInvi = (returnedInvi.Count() == 1 && returnedInvi.ElementAt(0).InvitationId == inviId);

            //Clean up
            deleteUserInvitation(email);
            deleteOrgUser(userId, org1Id);
            deleteTestOrg(org1Id);
            deleteTestOrg(org2Id);
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(returnedCorrectUser && returnedCorrectOrg && returnedCorrectInvi);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email address must have a value.")]
        public void GetUserByEmail_Should_Throw_Exception_For_Null_Email()
        {
            //Arrange
            string email = null;
            var service = new Service(connectionStr);

            //Act
            service.GetUserByEmail(email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email address must have a value.")]
        public void GetUserByEmail_Should_Throw_Exception_For_Empty_Email()
        {
            //Arrange
            string email = "";
            var service = new Service(connectionStr);

            //Act
            service.GetUserByEmail(email);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Email address must be in a valid format.")]
        public void GetUserByEmail_Should_Throw_Exception_For_Invalid_Email()
        {
            //Arrange
            string email = "invalidemail";
            var service = new Service(connectionStr);

            //Act
            service.GetUserByEmail(email);
        }

        [TestMethod]
        public void GetUserByEmail_Should_Return_User_Info_For_Valid_Email()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            var service = new Service(connectionStr);

            //Act
            User user = service.GetUserByEmail(email);

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(user.AccessFailedCount == 0
                && user.ActiveOrganizationId == 0
                && user.Address == null
                && user.City == null
                && user.Country == null
                && user.DateOfBirth == null
                && user.Email == email
                && user.EmailConfirmed == false
                && user.FirstName == "Test"
                && user.LastName == "User"
                && user.LastSubscriptionId == 0
                && user.LockoutEnabled == false
                && user.LockoutEndDateUtc == null
                && user.PasswordResetCode == null
                && user.PhoneExtension == null
                && user.PhoneNumber == null
                && user.PhoneNumberConfirmed == false
                && user.State == null
                && user.TwoFactorEnabled == false
                && user.UserId == userId
                && user.UserName == email
                && user.PostalCode == null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "UserInfo object must not be null.")]
        public void SaveUserInfo_Should_Throw_Exception_For_Null_User()
        {
            //Arrange
            User model = null;
            var service = new Service(connectionStr);

            //Act
            service.SaveUserInfo(model);
        }

        [TestMethod]
        public void SaveUserInfo_Should_Update_User_Info_In_Db()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email); //create original user
            User updated = new User
            {
                AccessFailedCount = 0,
                ActiveOrganizationId = 0,
                Address = null,
                City = null,
                Country = null,
                DateOfBirth = null,
                Email = email,
                EmailConfirmed = false,
                FirstName = "NewFirstName", //change user's fname and lname
                LastName = "NewLastName",
                LastSubscriptionId = 0,
                LockoutEnabled = false,
                LockoutEndDateUtc = null,
                PasswordHash = null,
                PasswordResetCode = null,
                PhoneExtension = null,
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                State = null,
                TwoFactorEnabled = false,
                UserId = userId,
                UserName = email,
                PostalCode = null
            };

            var service = new Service(connectionStr);

            //Act
            service.SaveUserInfo(updated);

            string selectStmt = "SELECT [FirstName], [LastName] FROM [Auth].[User] WHERE [UserId] = @userId";
            SqlDataReader reader;
            string newFname = "";
            string newLname = "";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        newFname = (string)reader["FirstName"];
                        newLname = (string)reader["LastName"];
                    }
                }
            }

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(newFname == "NewFirstName" && newLname == "NewLastName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Language ID cannot be negative.")]
        public void SetLanguage_Should_Throw_Exception_For_Invalid_LanguageID()
        {
            //Arrange           
            var service = new Service(connectionStr);

            //Act
            service.SetLanguage(-1);
        }

        [TestMethod]
        public void SetLanguage_Should_Update_Language_Preference_For_Current_User()
        {
            //Arange
            string userEmail = "testuser@test.com";
            int userId = createTestUser(userEmail);

            UserContext userContext = new UserContext(userId, userEmail, userEmail, 0, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            service.SetLanguage(2); //change to Spanish

            string selectStmt = "SELECT [LanguagePreference] FROM [Auth].[User] WHERE [UserId] = @userId";
            SqlDataReader reader;
            int langPref = 0;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        langPref = (int)reader["LanguagePreference"];
                    }
                }
            }

            //Clean up
            deleteTestUser(userEmail);

            //Assert
            Assert.IsTrue(langPref == 2);  
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Language ID cannot be negative.")]
        public void GetLanguage_Should_Throw_Exception_For_Invalid_LanguageID()
        {
            //Arrange           
            var service = new Service(connectionStr);

            //Act
            service.GetLanguage(-1);
        }

        [TestMethod]
        public void GetLanguage_Should_Return_Correct_Language_For_Valid_LanguageID()
        {
            //Arrange
            var service = new Service(connectionStr);

            //Act
            Language lang = service.GetLanguage(1); //English

            //Assert
            Assert.IsTrue(lang.LanguageID == 1 && lang.CultureName == "en-US" && lang.LanguageName == "English (United States)");
        }

        [TestMethod]
        public void GetLanguage_Should_Return_English_For_Zero_LanguageID()
        {
            //Arrange
            var service = new Service(connectionStr);

            //Act
            Language lang = service.GetLanguage(0); //English

            //Assert
            Assert.IsTrue(lang.LanguageID == 1 && lang.CultureName == "en-US" && lang.LanguageName == "English (United States)");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email address must have a value.")]
        public async Task SendPasswordResetMessage_Should_Throw_Exception_For_Empty_Email()
        {
            string emptyEmail = "";
            string callbackUrl = "";
            var service = new Service(connectionStr);
            await service.SendPasswordResetMessage(emptyEmail, callbackUrl);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email address must have a value.")]
        public async Task SendPasswordResetMessage_Should_Throw_Exception_For_Null_Email()
        {
            string nullEmail = null;
            string callbackUrl = "";
            var service = new Service(connectionStr);
            await service.SendPasswordResetMessage(nullEmail, callbackUrl);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Email address must be in a valid format.")]
        public async Task SendPasswordResetMessage_Should_Throw_Exception_For_Invalid_Email()
        {
            string invalidEmail = "invalidemail";
            string callbackUrl = "";
            var service = new Service(connectionStr);
            await service.SendPasswordResetMessage(invalidEmail, callbackUrl);
        }

        [TestMethod]
        public async Task SendPasswordResetMessage_Should_Return_False_For_Nonexistent_User()
        {
            string email = "testuser@test.com";
            string callbackUrl = "";
            var service = new Service(connectionStr);
            bool result = await service.SendPasswordResetMessage(email, callbackUrl);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task SendPasswordResetMessage_Should_Update_PasswordResetCode_For_Valid_User()
        {
            string email = "testuser@test.com";
            string callbackUrl = "";
            int userId = createTestUser(email);
            var service = new Service(connectionStr);

            bool result = await service.SendPasswordResetMessage(email, callbackUrl);

            string selectStmt = "SELECT [PasswordResetCode] FROM [Auth].[User] WHERE [UserId] = @userId";
            SqlDataReader reader;
            object pwResetCode = null;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        pwResetCode = (Guid)reader["PasswordResetCode"];
                    }
                }
            }

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(result && pwResetCode != null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "User ID cannot be 0 or negative.")]
        public async Task ResetPassword_Should_Throw_Exception_For_Invalid_UserId()
        {
            var service = new Service(connectionStr);
            await service.ResetPassword(0, "code", "pw");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Code must have a value.")]
        public async Task ResetPassword_Should_Throw_Exception_For_Null_Code()
        {
            var service = new Service(connectionStr);
            await service.ResetPassword(1, null, "pw");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Code must have a value.")]
        public async Task ResetPassword_Should_Throw_Exception_For_Empty_Code()
        {
            var service = new Service(connectionStr);
            await service.ResetPassword(1, "", "pw");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Code must be a valid Guid.")]
        public async Task ResetPassword_Should_Throw_Exception_For_Invalid_Code()
        {
            var service = new Service(connectionStr);
            await service.ResetPassword(1, "code", "pw");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Password must have a value.")]
        public async Task ResetPassword_Should_Throw_Exception_For_Null_Password()
        {
            var service = new Service(connectionStr);
            Guid code = Guid.NewGuid();
            await service.ResetPassword(1, code.ToString(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Password must have a value.")]
        public async Task ResetPassword_Should_Throw_Exception_For_Empty_Password()
        {
            var service = new Service(connectionStr);
            Guid code = Guid.NewGuid();
            await service.ResetPassword(1, code.ToString(), "");
        }

        [TestMethod]
        public async Task ResetPassword_Should_Return_False_For_Nonexistent_UserId()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            deleteTestUser(email);  //userId is now deleted from User table

            var service = new Service(connectionStr);
            Guid code = Guid.NewGuid();

            //Act
            bool result = await service.ResetPassword(userId, code.ToString(), "NewPassword");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ResetPassword_Should_Return_False_For_Not_Matched_Code()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);

            var service = new Service(connectionStr);
            Guid code = Guid.NewGuid(); //a random new code

            //Act
            bool result = await service.ResetPassword(userId, code.ToString(), "NewPassword.123");

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ResetPassword_Should_Return_True_And_Update_Password_For_Valid_UserId()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);

            var service = new Service(connectionStr);
            Guid code = Guid.NewGuid(); //a new valid code

            //put the code in the user's PasswordResetCode
            string updateStmt = "UPDATE [Auth].[User] SET [PasswordResetCode] = @code WHERE [Email] = @email";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@code", SqlDbType.UniqueIdentifier).Value = code;

                    // open connection, execute command, close connection
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            //get the old PasswordHash to compare later
            string selectStmt1 = "SELECT [PasswordHash] FROM [Auth].[User] WHERE [Email] = @email";
            SqlDataReader oldReader;
            string oldPwHash = "";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt1, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

                    // open connection, execute command, close connection
                    connection.Open();
                    oldReader = cmd.ExecuteReader();
                    while (oldReader.Read())
                    {
                        oldPwHash = (string)oldReader["PasswordHash"];
                    }
                    connection.Close();
                }
            }

            //Act
            bool result = await service.ResetPassword(userId, code.ToString(), "NewPassword.123");

            //get the new PasswordHash and PasswordResetCode to compare
            string selectStmt2 = "SELECT [PasswordHash], [PasswordResetCode] FROM [Auth].[User] WHERE [Email] = @email";
            SqlDataReader newReader;
            string newPwHash = "";
            string newPwResetCode = "";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt2, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;

                    // open connection, execute command, close connection
                    connection.Open();
                    newReader = cmd.ExecuteReader();
                    while (newReader.Read())
                    {
                        newPwHash = (string)newReader["PasswordHash"];
                        newPwResetCode = newReader["PasswordResetCode"].ToString();
                    }
                    connection.Close();
                }
            }

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(result && oldPwHash != newPwHash && newPwResetCode == "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Old password must have a value.")]
        public void ChangePassword_Should_Throw_Exception_For_Null_Old_Password()
        {
            var service = new Service(connectionStr);
            bool result = service.ChangePassword(null, "NewPassword");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Old password must have a value.")]
        public void ChangePassword_Should_Throw_Exception_For_Empty_Old_Password()
        {
            var service = new Service(connectionStr);
            bool result = service.ChangePassword("", "NewPassword");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "New password must have a value.")]
        public void ChangePassword_Should_Throw_Exception_For_Null_New_Password()
        {
            var service = new Service(connectionStr);
            bool result = service.ChangePassword("OldPassword", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "New password must have a value.")]
        public void ChangePassword_Should_Throw_Exception_For_Empty_New_Password()
        {
            var service = new Service(connectionStr);
            bool result = service.ChangePassword("OldPassword", "");
        }

        [TestMethod]
        public void ChangePassword_Should_Return_False_For_Incorrect_Old_Password()
        {
            //Arange
            string userEmail = "testuser@test.com";
            int userId = createTestUser(userEmail);

            UserContext userContext = new UserContext(userId, userEmail, userEmail, 0, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            bool result = service.ChangePassword("incorrectPw", "NewPassword.123");

            //Clean up
            deleteTestUser(userEmail);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ChangePassword_Should_Return_True_And_Update_Password_For_Valid_Passwords()
        {
            //Arange
            string userEmail = "testuser@test.com";
            int userId = createTestUser(userEmail);

            UserContext userContext = new UserContext(userId, userEmail, userEmail, 0, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            bool result = service.ChangePassword("AllyisApps.123", "NewPassword.123");

            //get the new PasswordHash to compare
            string selectStmt = "SELECT [PasswordHash] FROM [Auth].[User] WHERE [Email] = @email";
            SqlDataReader reader;
            string newPwHash = "";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = userEmail;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        newPwHash = (string)reader["PasswordHash"];
                    }
                    connection.Close();
                }
            }

            //Clean up
            deleteTestUser(userEmail);

            //Assert
            Assert.IsTrue(result && Crypto.ValidatePassword("NewPassword.123", newPwHash));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Password must have a value.")]
        public void GetPasswordHash_Should_Throw_Exception_For_Null_Password()
        {
            var service = new Service(connectionStr);
            service.GetPasswordHash(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Password must have a value.")]
        public void GetPasswordHash_Should_Throw_Exception_For_Empty_Password()
        {
            var service = new Service(connectionStr);
            service.GetPasswordHash("");
        }

        [TestMethod]
        public void GetPasswordHash_Should_Return_PasswordHash_For_Valid_Password()
        {
            //Arrange
            var service = new Service(connectionStr);
            string pw = "ValidPassword.123";

            //Act
            string pwHash = service.GetPasswordHash(pw);

            //Assert
            Assert.IsTrue(Crypto.ValidatePassword(pw, pwHash));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "User ID cannot be 0 or negative.")]
        public async Task GetConfirmEmailCode_Should_Throw_Exception_For_Invalid_UserId()
        {
            var service = new Service(connectionStr);
            await service.GetConfirmEmailCode(-1);
        }

        [TestMethod]
        public async Task GetConfirmEmailCode_Should_Return_Guid_Code_For_Valid_UserId()
        {
            var service = new Service(connectionStr);
            string code = await service.GetConfirmEmailCode(1);

            Assert.IsFalse(string.IsNullOrEmpty(code));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "From email address must have a value.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Null_From_Email()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail(null, "to@email.com", "http://allyisapps.com/Account/ConfirmEmail?userId=%7BuserId%7D&code=%7Bcode%7D");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "From email address must have a value.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Empty_From_Email()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail("", "to@email.com", "http://allyisapps.com/Account/ConfirmEmail?userId=%7BuserId%7D&code=%7Bcode%7D");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "From email address must be in a valid format.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Invalid_From_Email()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail("invalidemail", "to@email.com", "http://allyisapps.com/Account/ConfirmEmail?userId=%7BuserId%7D&code=%7Bcode%7D");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "To email address must have a value.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Null_To_Email()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail("from@email.com", null, "http://allyisapps.com/Account/ConfirmEmail?userId=%7BuserId%7D&code=%7Bcode%7D");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "To email address must have a value.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Empty_To_Email()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail("from@email.com", "", "http://allyisapps.com/Account/ConfirmEmail?userId=%7BuserId%7D&code=%7Bcode%7D");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "To email address must be in a valid format.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Invalid_To_Email()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail("from@email.com", "invalidemail", "http://allyisapps.com/Account/ConfirmEmail?userId=%7BuserId%7D&code=%7Bcode%7D");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Confirm email url must have a value.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Null_confirmEmailUrl()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail("from@email.com", "to@email.com", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Confirm email url must have a value.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Empty_confirmEmailUrl()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail("from@email.com", "to@email.com", "");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Confirm email url must be in a valid format.")]
        public async Task SendConfirmationEmail_Should_Throw_Exception_For_Invalid_confirmEmailUrl()
        {
            var service = new Service(connectionStr);
            await service.SendConfirmationEmail("from@email.com", "to@email.com", "invalidurl");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "User ID cannot be 0 or negative.")]
        public void ConfirmUserEmail_Should_Throw_Exception_For_Invalid_UserId()
        {
            var service = new Service(connectionStr);
            string code = Guid.NewGuid().ToString();
            service.ConfirmUserEmail(-1, code);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Code must have a value.")]
        public void ConfirmUserEmail_Should_Throw_Exception_For_Null_Code()
        {
            var service = new Service(connectionStr);
            service.ConfirmUserEmail(1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Code must have a value.")]
        public void ConfirmUserEmail_Should_Throw_Exception_For_Empty_Code()
        {
            var service = new Service(connectionStr);
            service.ConfirmUserEmail(1, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Code must be a valid Guid.")]
        public void ConfirmUserEmail_Should_Throw_Exception_For_Invalid_Code()
        {
            var service = new Service(connectionStr);
            service.ConfirmUserEmail(1, "invalidcode");
        }

        [TestMethod]
        public void ConfirmUserEmail_Should_Return_False_If_Email_Is_Already_Confirmed()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);

            var service = new Service(connectionStr);

            //set user's EmailConfirmed to true
            string updateStmt = "UPDATE [Auth].[User] SET [EmailConfirmed] = @confirmed WHERE [Email] = @email";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@confirmed", SqlDbType.Bit).Value = 1;

                    // open connection, execute command, close connection
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            string code = Guid.NewGuid().ToString();

            //Act
            bool result = service.ConfirmUserEmail(userId, code);

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ConfirmUserEmail_Should_Return_False_For_Not_Matched_Code()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            Guid code = Guid.NewGuid();

            var service = new Service(connectionStr);

            //Act
            bool result = service.ConfirmUserEmail(userId, code.ToString());

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ConfirmUserEmail_Should_Return_True_On_Success()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            Guid code = Guid.NewGuid();

            var service = new Service(connectionStr);

            //put the code in the user's EmailConfirmationCode
            string updateStmt = "UPDATE [Auth].[User] SET [EmailConfirmationCode] = @code WHERE [Email] = @email";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@code", SqlDbType.UniqueIdentifier).Value = code;

                    // open connection, execute command, close connection
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            //Act
            bool result = service.ConfirmUserEmail(userId, code.ToString());

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetOrganizationsByUserId_Should_Return_Correct_List_Of_Org()
        {
            //Arrange
            string userEmail = "testuser@test.com";
            string orgName = "UnitTestOrg";

            int userId = createTestUser(userEmail);
            int orgId = createTestOrg(orgName);
            createOrgUser(userId, orgId, 1, "111");    //roleId = 1 (member)

            UserContext userContext = new UserContext(userId, userEmail, userEmail, 0, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            IEnumerable<Organization> orgList = service.GetOrganizationsByUserId();

            //Clean up
            deleteOrgUser(userId, orgId);
            deleteTestOrg(orgId);
            deleteTestUser(userEmail);

            //Assert
            Assert.IsTrue(orgList.Count() == 1 && orgList.ElementAt(0).OrganizationId == orgId);

        }

        [TestMethod]
        public void InitializeUser_Should_Return_Null_For_Null_UserDBEntity()
        {
            UserDBEntity user = null;
            User initializedUser = Service.InitializeUser(user);
            Assert.IsTrue(initializedUser == null);
        }

        [TestMethod]
        public void InitializeUser_Should_Return_Correct_User_Instance_For_A_UserDBEntity_Instance()
        {
            //Arrange
            string userEmail = "testuser@test.com";
            string pwHash = Crypto.GetPasswordHash("AllyisApps.123");
            int userId = createTestUser(userEmail);

            UserDBEntity user = new UserDBEntity
            {
                UserId = userId,
                FirstName = "Test",
                LastName = "User",
                Email = userEmail,
                PasswordHash = pwHash,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                LockoutEnabled = false,
                UserName = userEmail,
                DateOfBirth = null,
                Address = null,
                City = null,
                State = null,
                Country = null,
                PostalCode = null,
                PhoneNumber = null,
                PhoneExtension = null,
                LastSubscriptionId = 0,
                ActiveOrganizationId = 0,
                LockoutEndDateUtc = null,
                PasswordResetCode = null,
                LanguagePreference = 1
            };

            //Act
            User initializedUser = Service.InitializeUser(user);

            //Clean up
            deleteTestUser(userEmail);

            //Assert
            Assert.IsTrue(initializedUser.AccessFailedCount == 0
                && initializedUser.ActiveOrganizationId == 0
                && initializedUser.Address == null
                && initializedUser.City == null
                && initializedUser.Country == null
                && initializedUser.DateOfBirth == null
                && initializedUser.Email == userEmail
                && initializedUser.EmailConfirmed == false
                && initializedUser.FirstName == "Test"
                && initializedUser.LastName == "User"
                && initializedUser.LastSubscriptionId == 0
                && initializedUser.LockoutEnabled == false
                && initializedUser.LockoutEndDateUtc == null
                && initializedUser.PasswordHash == pwHash
                && initializedUser.PasswordResetCode == null
                && initializedUser.PhoneExtension == null
                && initializedUser.PhoneNumber == null
                && initializedUser.PhoneNumberConfirmed == false
                && initializedUser.State == null
                && initializedUser.TwoFactorEnabled == false
                && initializedUser.UserId == userId
                && initializedUser.UserName == userEmail
                && initializedUser.PostalCode == null);
        }

        [TestMethod]
        public void GetDBEntityFromUser_Should_Return_Null_For_Null_User()
        {
            User user = null;
            UserDBEntity userDbEntity = Service.GetDBEntityFromUser(user);
            Assert.IsTrue(userDbEntity == null);
        }

        [TestMethod]
        public void GetDBEntityFromUser_Should_Return_Correct_UserDbEntity_Instance_For_A_User_Instance()
        {
            //Arrange
            string userEmail = "testuser@test.com";
            string pwHash = Crypto.GetPasswordHash("AllyisApps.123");
            int userId = createTestUser(userEmail);

            User user = new User
            {
                UserId = userId,
                FirstName = "Test",
                LastName = "User",
                Email = userEmail,
                PasswordHash = pwHash,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                LockoutEnabled = false,
                UserName = userEmail,
                DateOfBirth = null,
                Address = null,
                City = null,
                State = null,
                Country = null,
                PostalCode = null,
                PhoneNumber = null,
                PhoneExtension = null,
                LastSubscriptionId = 0,
                ActiveOrganizationId = 0,
                LockoutEndDateUtc = null,
                PasswordResetCode = null,
            };

            //Act
            UserDBEntity DbEntity = Service.GetDBEntityFromUser(user);

            //Clean up
            deleteTestUser(userEmail);

            //Assert
            Assert.IsTrue(DbEntity.AccessFailedCount == 0
                && DbEntity.ActiveOrganizationId == 0
                && DbEntity.Address == null
                && DbEntity.City == null
                && DbEntity.Country == null
                && DbEntity.DateOfBirth == null
                && DbEntity.Email == userEmail
                && DbEntity.EmailConfirmed == false
                && DbEntity.FirstName == "Test"
                && DbEntity.LastName == "User"
                && DbEntity.LastSubscriptionId == 0
                && DbEntity.LockoutEnabled == false
                && DbEntity.LockoutEndDateUtc == null
                && DbEntity.PasswordHash == pwHash
                && DbEntity.PasswordResetCode == null
                && DbEntity.PhoneExtension == null
                && DbEntity.PhoneNumber == null
                && DbEntity.PhoneNumberConfirmed == false
                && DbEntity.State == null
                && DbEntity.TwoFactorEnabled == false
                && DbEntity.UserId == userId
                && DbEntity.UserName == userEmail
                && DbEntity.PostalCode == null
                && DbEntity.LanguagePreference == 1);
        }

        [TestMethod]
        public void InitializeUserRolesInfo_Should_Return_Null_For_Null_UserRolesDBEntity()
        {
            UserRolesDBEntity dbEntity = null;
            UserRolesInfo info = Service.InitializeUserRolesInfo(dbEntity);
            Assert.IsTrue(info == null);
        }

        [TestMethod]
        public void InitializeUserRolesInfo_Should_Return_Correct_UserRolesInfo_For_A_UserRolesDBEntity_Instance()
        {
            string userEmail = "testuser@test.com";

            UserRolesDBEntity dbEntity = new UserRolesDBEntity
            {
                FirstName = "Test",
                LastName = "User",
                UserId = "007",
                OrgRoleId = 1,
                Name = "Member",
                Email = userEmail,
                ProductRoleId = 1,
                SubscriptionId = null
            };

            UserRolesInfo info = Service.InitializeUserRolesInfo(dbEntity);

            Assert.IsTrue(info.Email == userEmail
                && info.FirstName == "Test"
                && info.LastName == "User"
                && info.Name == "Member"
                && info.OrgRoleId == 1
                && info.ProductRoleId == 1
                && info.SubscriptionId == -1
                && info.UserId == "007");
        }

        [TestMethod]
        public void InitializeSubscriptionUserInfo_Should_Return_Null_For_Null_SubscriptionUserDBEntity()
        {
            SubscriptionUserDBEntity subDbEntity = null;
            SubscriptionUserInfo subUserInfo = Service.InitializeSubscriptionUserInfo(subDbEntity);
            Assert.IsTrue(subUserInfo == null);
        }

        [TestMethod]
        public void InitializeSubscriptionUserInfo_Should_Return_Correct_SubscriptionUserInfo_For_A_SubscriptionUserDBEntity()
        {
            DateTime time = DateTime.Today;
            SubscriptionUserDBEntity subDbEntity = new SubscriptionUserDBEntity
            {
                FirstName = "Test",
                LastName = "User",
                ProductId = 1,
                ProductRoleId = "001",
                ProductRoleName = "User",
                UserId = 1,
                CreatedUTC = time,
                SubscriptionId = 1,
                SkuId = 1
            };

            SubscriptionUserInfo subUserInfo = Service.InitializeSubscriptionUserInfo(subDbEntity);

            Assert.IsTrue(subUserInfo.CreatedUTC == time
                && subUserInfo.FirstName == "Test"
                && subUserInfo.LastName == "User"
                && subUserInfo.ProductRoleId == "001"
                && subUserInfo.ProductRoleName == "User"
                && subUserInfo.SkuId == 1
                && subUserInfo.SubscriptionId == 1
                && subUserInfo.UserId == 1);
        }

        [TestMethod]
        public void GetCompressedEmail_Should_Return_Full_Email_For_Short_Email_Address()
        {
            string shortEmail = "not_too_long_address@email.com";
            string returned = Service.GetCompressedEmail(shortEmail);
            Assert.IsTrue(returned == shortEmail);
        }

        [TestMethod]
        public void GetCompressedEmail_Should_Compress_Too_Long_Email_Address_Correctly()
        {
            string longEmail = "first_020_characters_this_part_should_be_compressed_last_015_chars";
            string expected = "first_020_characters..._last_015_chars";
            string returned = Service.GetCompressedEmail(longEmail);
            Assert.IsTrue(returned == expected);
        }

        [TestMethod]
        public void IsEmailAddressValid_Should_Return_False_For_Invalid_Address()
        {
            List<string> invalidAddresses = new List<string>(new string[]
            {
                "plainaddress",
                "#@%^%#$@#$@#.com",
                "@domain.com",
                "Joe Smith <email@domain.com>",
                "email.domain.com",
                "email@domain@domain.com",
                ".email@domain.com",
                "email.@domain.com",
                "email..email@domain.com",
                "あいうえお@domain.com",
                "email@domain.com (Joe Smith)",
                "email@domain",
                "email@-domain.com",
                "email@domain.web",
                "email@111.222.333.44444",
                "email@domain..com"
            });

            bool output = false;
            string msg = "Failed on the following test cases: ";
            int failedCases = 0;

            foreach (string email in invalidAddresses)
            {
                output = Service.IsEmailAddressValid(email);
                if (output)
                {
                    failedCases++;
                    msg += email;
                    msg += ", ";
                }
            };

            Assert.AreEqual(0, failedCases, msg);
        }

        [TestMethod]
        public void IsEmailAddressValid_Should_Return_True_For_Valid_Address()
        {
            List<string> validAddresses = new List<string>(new string[]
            {
                "email@domain.com",
                "firstname.lastname@domain.com",
                "email@subdomain.domain.com",
                "firstname+lastname@domain.com",
                "email@123.123.123.123",
                "email@[123.123.123.123]",
                "\"email\"@domain.com",
                "1234567890@domain.com",
                "email@domain-one.com",
                "_______@domain.com",
                "email@domain.name",
                "email@domain.co.jp",
                "firstname-lastname@domain.com"
            });

            bool output = true;
            string msg = "Failed on the following test cases: ";
            int failedCases = 0;

            foreach (string email in validAddresses)
            {
                output = Service.IsEmailAddressValid(email);
                if (!output)
                {
                    failedCases++;
                    msg += email;
                    msg += ", ";
                }
            };

            Assert.AreEqual(0, failedCases, msg);
        }

        [TestMethod]
        public void IsUrlValid_Should_Return_False_For_Invalid_Urls()
        {
            List<string> invalidUrls = new List<string>(new string[]
            {
                "http://",
                "http://.",
                "//",
                "//a",
                "///a",
                "///",
                ":// should fail",
                "http://?",
                "foo.com"
            });

            bool output = false;
            string msg = "Failed on the following test cases: ";
            int failedCases = 0;

            foreach (string url in invalidUrls)
            {
                output = Service.IsUrlValid(url);
                if (output)
                {
                    failedCases++;
                    msg += url;
                    msg += ", ";
                }
            };

            Assert.AreEqual(0, failedCases, msg);
        }

        [TestMethod]
        public void IsUrlValid_Should_Return_True_For_Valid_Urls()
        {
            List<string> validUrls = new List<string>(new string[]
            {
                "http://foo.com/blah_blah",
                "http://foo.com/blah_blah/",
                "http://foo.com/blah_blah_(wikipedia)",
                "http://foo.com/blah_blah_(wikipedia)_(again)",
                "http://www.example.com/wpstyle/?p=364",
                "https://www.example.com/foo/?bar=baz&inga=42&quux",
                "http://foo.com/blah_(wikipedia)#cite-1",
                "http://userid:password@example.com:8080",
                "http://foo.com/(something)?after=parens",
                "http://code.google.com/events/#&product=browser",
                "http://j.mp",
                "http://1337.net"
            });

            bool output = true;
            string msg = "Failed on the following test cases: ";
            int failedCases = 0;

            foreach (string url in validUrls)
            {
                output = Service.IsUrlValid(url);
                if (!output)
                {
                    failedCases++;
                    msg += url;
                    msg += ", ";
                }
            };

            Assert.AreEqual(0, failedCases, msg);
        }

       /**************************************************************************************************
       *                              Unit tests for methods in Actions.cs
       ***************************************************************************************************/
       [TestMethod]
       public void GetProductForAction_Should_Return_ProductIdEnum_TimeTracker_For_Matching_Actions()
       {
            //Arrange
            List<CoreAction> validActions = new List<CoreAction>
            {
                CoreAction.EditCustomer,
                CoreAction.ViewCustomer,
                CoreAction.EditProject,
                CoreAction.TimeTrackerEditSelf,
                CoreAction.TimeTrackerEditOthers
            };

            string msg = "Failed on the following test cases: ";
            int failedCases = 0;

            //Act
            foreach(CoreAction action in validActions)
            {
                ProductIdEnum id = GetProductForAction(action);
                if (id != ProductIdEnum.TimeTracker)
                {
                    failedCases++;
                    msg += action.ToString();
                    msg += ", ";
                }
            };

            //Assert
            Assert.AreEqual(0, failedCases, msg);
       }

        [TestMethod]
        public void GetProductForAction_Should_Return_ProductIdEnum_None_For_Not_Matched_Actions()
        {
            //Arrange
            List<CoreAction> validActions = new List<CoreAction>
            {
                CoreAction.EditOrganization,
                CoreAction.ViewOrganization
            };

            string msg = "Failed on the following test cases: ";
            int failedCases = 0;

            //Act
            foreach (CoreAction action in validActions)
            {
                ProductIdEnum id = GetProductForAction(action);
                if (id != ProductIdEnum.None)
                {
                    failedCases++;
                    msg += action.ToString();
                    msg += ", ";
                }
            };

            //Assert
            Assert.AreEqual(0, failedCases, msg);
        }

        /**************************************************************************************************
        *                        Unit tests for methods in AuthorizationService.cs
        * TODO: Add more unit tests for thrown exceptions once the Can() method is completed/modified.
        * These current tests take only valid inputs.
        ***************************************************************************************************/
        [TestMethod]
        public void Can_Should_Return_False_If_No_Org_Is_Chosen()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;

            UserContext userContext = new UserContext(userId, email, email, 0, 0, null, 1); //chosenOrganizationId = 0;
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.ViewCustomer;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_Org_Member_Can_View_Organization()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, null, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.ViewOrganization;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_Org_Owner_Can_View_Organization()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, null, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.ViewOrganization;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_False_For_Org_Member_Cannot_Edit_Organization()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, null, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.EditOrganization;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_Org_Owner_Can_Edit_Organization()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, null, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.EditOrganization;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_TimeTracker_User_Can_View_Customer()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.ViewCustomer;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_TimeTracker_Manager_Can_View_Customer()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.ViewCustomer;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_False_For_TimeTracker_User_Cannot_Edit_Customer()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.EditCustomer;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_TimeTracker_Manager_Can_Edit_Customer()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.EditCustomer;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_False_For_TimeTracker_User_Cannot_Edit_Project()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.EditProject;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_TimeTracker_Manager_Can_Edit_Project()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.EditProject;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_TimeTracker_User_Can_Edit_Own_Entries()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.TimeTrackerEditSelf;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_TimeTracker_Manager_Can_Edit_Own_Entries()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.TimeTrackerEditSelf;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_True_For_TimeTracker_Manager_Can_Edit_Others_Entries()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.TimeTrackerEditOthers;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Should_Return_False_For_TimeTracker_User_Cannot_Edit_Others_Entries()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(1, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> orgInfoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, orgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            CoreAction targetAction = CoreAction.TimeTrackerEditOthers;

            //Act
            bool result = service.Can(targetAction);

            //Assert
            Assert.IsFalse(result);
        }

        /**************************************************************************************************
        *                        Unit tests for methods in ImportService.cs
        ***************************************************************************************************/


        /**************************************************************************************************
        *                        Unit tests for methods in OrgService.cs
        ***************************************************************************************************/
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Organization Id cannot be negative.")]
        public void GetSubdomainById_Should_Throw_Exception_For_Negative_orgId()
        {
            Service.GetSubdomainById(-1);
        }

        [TestMethod]
        public void GetSubdomainById_Should_Return_Correct_Subdomain_For_Valid_orgId()
        {
            //Arrange
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);
            string key = "DefaultConnection";            
            DBHelper.Instance.Init(key);

            //Act
            string subdomain = Service.GetSubdomainById(orgId);

            //Clean up
            deleteTestOrg(orgId);

            //Assert
            Assert.IsTrue(subdomain == "unittestorg");
        }

        [TestMethod]
        public void GetIdBySubdomain_Should_Return_Null_For_Nonexistent_Subdomain() {
            //Arrange
            string key = "DefaultConnection";
            DBHelper.Instance.Init(key);

            //Act
            int orgId = Service.GetIdBySubdomain("fakesubdomain");

            //Assert
            Assert.IsTrue(orgId == 0);
        }

        [TestMethod]
        public void GetIdBySubdomain_Should_Return_Correct_OrgId_For_Existing_Subdomain()
        {
            //Arrange
            string key = "DefaultConnection";
            DBHelper.Instance.Init(key);
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);

            //Act
            int returnedId = Service.GetIdBySubdomain("unittestorg");

            //Clean up
            deleteTestOrg(orgId);

            //Assert
            Assert.IsTrue(orgId == returnedId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Organization must not be null.")]
        public void CreateOrganization_Should_Throw_Exception_For_Null_Organization_Instance()
        {
            Service service = new Service(connectionStr);
            service.CreateOrganization(null, 1, "111");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Organization owner's user id cannot be 0 or negative.")]
        public void CreateOrganization_Should_Throw_Exception_For_Invalid_OwnerId()
        {
            Service service = new Service(connectionStr);
            Organization newOrg = new Organization();
            service.CreateOrganization(newOrg, -1, "111");
        }

        [TestMethod]
        public void CreateOrganization_Should_Return_Negative_One_If_Subdomain_Is_Taken()
        {
            //Arrange
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName); //subdomain is taken
            string email = "testuser@test.com";
            int ownerId = createTestUser(email);    //create owner

            Organization newOrg = new Organization();   //initilize Organization object
            newOrg.Name = orgName.ToUpper();
            newOrg.Subdomain = orgName.ToLower();

            Service service = new Service(connectionStr);

            //Act
            int returnedOrgId = service.CreateOrganization(newOrg, ownerId, "111");

            //Clean up
            deleteTestOrg(orgId);
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(returnedOrgId == -1);
        }

        [TestMethod]
        public void CreateOrganization_Should_Return_OrgId_If_Succeed()
        {
            //Arrange
            string email = "testuser@test.com";
            int ownerId = createTestUser(email);    //create owner

            Organization newOrg = new Organization();   //initilize Organization object
            string orgName = "UnitTestOrg";
            newOrg.Name = orgName;
            newOrg.Subdomain = orgName.ToLower();

            Service service = new Service(connectionStr);

            //Act
            int returnedOrgId = service.CreateOrganization(newOrg, ownerId, "111");

            bool returnedCorrectOrgId = false;
            bool orgUserCreated = false;
            bool usersChosenOrgUpdated = false;

            string selectStmt1 = "SELECT [OrganizationId] FROM [Auth].[Organization] WHERE [Subdomain] = @subdomain";
            SqlDataReader reader1;
            string selectStmt2 = "SELECT [EmployeeId], [OrgRoleId] FROM [Auth].[OrganizationUser] WHERE [UserId] = @userId AND [OrganizationId] = @orgId";
            SqlDataReader reader2;
            string selectStmt3 = "SELECT [ActiveOrganizationId] FROM [Auth].[User] WHERE [UserId] = @userId";
            SqlDataReader reader3;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                using (SqlCommand cmd1 = new SqlCommand(selectStmt1, connection))
                {
                    // set up the command's parameters
                    cmd1.Parameters.Add("@subdomain", SqlDbType.VarChar, 100).Value = orgName.ToLower();

                    reader1 = cmd1.ExecuteReader();
                    while (reader1.Read())
                    {
                        returnedCorrectOrgId = ((int)reader1["OrganizationId"] == returnedOrgId);
                    }
                    reader1.Close();
                }
                using (SqlCommand cmd2 = new SqlCommand(selectStmt2, connection))
                {
                    // set up the command's parameters
                    cmd2.Parameters.Add("@userId", SqlDbType.Int).Value = ownerId;
                    cmd2.Parameters.Add("@orgId", SqlDbType.Int).Value = returnedOrgId;

                    // execute cmd
                    reader2 = cmd2.ExecuteReader();
                    while (reader2.Read())
                    {
                        orgUserCreated = ((string)reader2["EmployeeId"] == "111" && (int)reader2["OrgRoleId"] == 2);
                    }
                    reader2.Close();
                }
                using (SqlCommand cmd3 = new SqlCommand(selectStmt3, connection))
                {
                    // set up the command's parameters
                    cmd3.Parameters.Add("@userId", SqlDbType.Int).Value = ownerId;

                    reader3 = cmd3.ExecuteReader();
                    while (reader3.Read())
                    {
                        usersChosenOrgUpdated = ((int)reader3["ActiveOrganizationId"] == returnedOrgId);
                    }
                    reader3.Close();
                }
                connection.Close();
            }

            //Clean up
            deleteOrgUser(ownerId, returnedOrgId);
            deleteTestUser(email);
            deleteTestOrg(returnedOrgId);

            //Assert
            Assert.IsTrue(returnedCorrectOrgId && orgUserCreated && usersChosenOrgUpdated);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Organization Id cannot be negative.")]
        public void GetOrganization_Should_Throw_Exception_For_Negative_OrgId()
        {
            Service service = new Service(connectionStr);

            //Act
            service.GetOrganization(-1);
        }

        [TestMethod]
        public void GetOrganization_Should_Return_Organization_Instance_For_Valid_OrgId()
        {
            //Arrange
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);

            Service service = new Service(connectionStr);

            //Act
            Organization org = service.GetOrganization(orgId);

            //Clean up
            deleteTestOrg(orgId);

            //Assert
            Assert.IsTrue(org.Address == null
                && org.City == null
                && org.Country == null
                && org.FaxNumber == null
                && org.Name == orgName
                && org.OrganizationId == orgId
                && org.PhoneNumber == null
                && org.SiteUrl == null
                && org.State == null
                && org.Subdomain == orgName.ToLower()
                && org.PostalCode == null);
        }

        [TestMethod]
        public void GetOrganization_Should_Return_Null_For_Nonexistent_OrgId()
        {
            //Arrange
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);
            deleteTestOrg(orgId);

            Service service = new Service(connectionStr);

            //Act
            Organization org = service.GetOrganization(orgId);

            //Assert
            Assert.IsTrue(org == null);
        }

        [TestMethod]
        public void GetOrganizationManagementInfo_Should_Return_All_Org_Management_Info()
        {
            //Arrange: user1 is Member of Org, user2 is invited to Org
            string userEmail1 = "testuser1@test.com";
            int userId1 = createTestUser(userEmail1);
            string userEmail2 = "testuser2@test.com";
            int userId2 = createTestUser(userEmail2);

            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);
            
            createOrgUser(userId1, orgId, 1, "111");
            int inviId = createUserInvitation(userEmail2, orgId, 1, "111");

            UserContext userContext = new UserContext(userId1, userEmail1, userEmail1, orgId, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            Tuple<Organization, List<OrganizationUserInfo>, List<SubscriptionDisplayInfo>, List<InvitationInfo>, string, List<Product>> tuple = service.GetOrganizationManagementInfo();

            //Clean up
            deleteUserInvitation(userEmail2);
            deleteOrgUser(userId1, orgId);
            deleteTestUser(userEmail1);
            deleteTestUser(userEmail2);
            deleteTestOrg(orgId);

            //Assert
            Organization org = tuple.Item1;
            List<OrganizationUserInfo> orgUserInfoList = tuple.Item2;
            List<SubscriptionDisplayInfo> subInfoList = tuple.Item3;
            List<InvitationInfo> inviInfoList = tuple.Item4;
            string str = tuple.Item5;
            List<Product> productList = tuple.Item6;

            bool correctOrg = (org.OrganizationId == orgId 
                            && org.Name == orgName 
                            && org.Subdomain == orgName.ToLower());
            bool correctOrgUserInfo = (orgUserInfoList.Count == 1
                                    && orgUserInfoList.ElementAt(0).UserId == userId1
                                    && orgUserInfoList.ElementAt(0).OrgRoleId == 1
                                    && orgUserInfoList.ElementAt(0).OrganizationId == orgId
                                    && orgUserInfoList.ElementAt(0).FirstName == "Test"
                                    && orgUserInfoList.ElementAt(0).LastName == "User"
                                    && orgUserInfoList.ElementAt(0).EmployeeId == "111"
                                    && orgUserInfoList.ElementAt(0).Email == userEmail1);
            bool correctSubInfo = (subInfoList.Count == 0);
            bool correctInviInfo = (inviInfoList.Count == 1
                                && inviInfoList.ElementAt(0).Email == userEmail2
                                && inviInfoList.ElementAt(0).EmployeeId == "111"
                                && inviInfoList.ElementAt(0).FirstName == "Test"
                                && inviInfoList.ElementAt(0).LastName == "User"
                                && inviInfoList.ElementAt(0).InvitationId == inviId
                                && inviInfoList.ElementAt(0).OrganizationId == orgId
                                && inviInfoList.ElementAt(0).OrgRole == 1
                                && inviInfoList.ElementAt(0).OrgRoleName == "Member");
            bool correctProduct = (productList.Count == 1
                                && productList.ElementAt(0).ProductId == 1
                                && productList.ElementAt(0).ProductName == "TimeTracker");

            Assert.IsTrue(correctOrg && correctOrgUserInfo && correctSubInfo && correctInviInfo && correctProduct);
        }

        [TestMethod]
        public void GetOrgWithCountriesAndEmployeeId_Should_Return_Chosen_Org_And_All_Countries_And_EmployeeId()
        {
            //Arrange
            string userEmail = "testuser@test.com";
            int userId = createTestUser(userEmail);

            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);

            createOrgUser(userId, orgId, 1, "111");    //user is Org's member

            UserContext userContext = new UserContext(userId, userEmail, userEmail, orgId, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            Tuple<Organization, List<string>, string> tuple = service.GetOrgWithCountriesAndEmployeeId();

            //Clean up
            deleteOrgUser(userId, orgId);
            deleteTestUser(userEmail);
            deleteTestOrg(orgId);

            //Assert
            Organization org = tuple.Item1;
            List<string> countries = tuple.Item2;
            string employeeId = tuple.Item3;

            bool correctOrg = (org.OrganizationId == orgId
                            && org.Name == orgName
                            && org.Subdomain == orgName.ToLower());
            bool correctCountries = (countries.Count == 242);
            bool correctEmployeeId = (employeeId == "111");

            Assert.IsTrue(correctOrg && correctCountries && correctEmployeeId);
        }

        [TestMethod]
        public void GetAddMemberInfo_Should_Return_All_Correct_Info()
        {
            //Arrange
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);     //org
            string user1 = "testuser1@test.com";
            int userId1 = createTestUser(user1);    //user 1
            string user2 = "testuser2@test.com";
            int userId2 = createTestUser(user2);    //user 2
            string user3 = "testuser3@test.com";
            int userId3 = createTestUser(user3);    //user 3
            createOrgUser(userId1, orgId, 1, "111");
            createOrgUser(userId2, orgId, 1, "112");    //user 1 & 2 are members of org, user2 has higher employeeId
            int inviId = createUserInvitation(user3, orgId, 1, "113");      //user 3 is invited to Org as member
            int subId = createSubscription(orgId, 1, 100);  //org subscribes to TimeTracker
            createSubUser(subId, userId1, 1);
            createSubUser(subId, userId2, 1);       //user 1 & 2 are TimeTracker sub's users

            UserContext userContext = new UserContext(userId1, user1, user1, orgId, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            Tuple<string, List<SubscriptionDisplayInfo>, List<ProductRole>, List<CompleteProjectInfo>, string> tuple = service.GetAddMemberInfo();

            //Clean up
            deleteSubUser(userId2, subId);
            deleteSubUser(userId1, subId);
            deleteSubscription(subId);
            deleteUserInvitation(user3);
            deleteOrgUser(userId2, orgId);
            deleteOrgUser(userId1, orgId);
            deleteTestUser(user3);
            deleteTestUser(user2);
            deleteTestUser(user1);
            deleteTestOrg(orgId);

            //Assert
            string recEmployeeId = tuple.Item1;
            List<SubscriptionDisplayInfo> subInfoList = tuple.Item2;
            List<ProductRole> productRoleList = tuple.Item3;
            List<CompleteProjectInfo> projInfoList = tuple.Item4;
            string inviEmployeeId = tuple.Item5;

            bool correctRecEmployeeId = (recEmployeeId == "112");
            bool correctSubInfoList = (subInfoList.Count == 1
                                    && subInfoList.ElementAt(0).NumberOfUsers == 100
                                    && subInfoList.ElementAt(0).OrganizationId == orgId
                                    && subInfoList.ElementAt(0).ProductId == 1
                                    && subInfoList.ElementAt(0).SubscriptionId == subId
                                    && subInfoList.ElementAt(0).SubscriptionsUsed == 2);
            bool correctProductRoleList = (productRoleList.Count == 2); //User and Manager roles
            bool correctProjInfoList = (projInfoList.Count == 0);
            bool correctInviEmployeeId = (inviEmployeeId == "113");

            Assert.IsTrue(correctRecEmployeeId && correctSubInfoList && correctProductRoleList && correctProjInfoList && correctInviEmployeeId);
        }

        [TestMethod]
        public void GetOrgAndSubRoles_Should_Return_All_Correct_Info()
        {
            //Arrange
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);     //org
            string user1 = "testuser1@test.com";
            int userId1 = createTestUser(user1);    //user1
            createOrgUser(userId1, orgId, 1, "111");//user1 is members of org
            int subId = createSubscription(orgId, 1, 100);  //org subscribes to TimeTracker
            createSubUser(subId, userId1, 1);       //user1 is TimeTracker sub's users

            UserContext userContext = new UserContext(userId1, user1, user1, orgId, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            Tuple<List<UserRolesInfo>, List<SubscriptionDisplayInfo>> tuple = service.GetOrgAndSubRoles();

            //Clean up
            deleteSubUser(userId1, subId);
            deleteSubscription(subId);
            deleteOrgUser(userId1, orgId);
            deleteTestUser(user1);
            deleteTestOrg(orgId);

            //Assert
            List<UserRolesInfo> userRolesInfoList = tuple.Item1;
            List<SubscriptionDisplayInfo> subInfoList = tuple.Item2;

            bool correctUserRolesInfoList = (userRolesInfoList.Count == 1
                                            && userRolesInfoList.ElementAt(0).UserId == userId1.ToString()
                                            && userRolesInfoList.ElementAt(0).Name == "Member"
                                            && userRolesInfoList.ElementAt(0).OrgRoleId == 1
                                            && userRolesInfoList.ElementAt(0).ProductRoleId == 1
                                            && userRolesInfoList.ElementAt(0).SubscriptionId == subId);
            bool correctSubInfoList = (subInfoList.Count == 1
                                    && subInfoList.ElementAt(0).SubscriptionId == subId
                                    && subInfoList.ElementAt(0).ProductId == 1
                                    && subInfoList.ElementAt(0).ProductName == "TimeTracker");

            Assert.IsTrue(correctUserRolesInfoList && correctSubInfoList);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Organization must not be null.")]
        public void UpdateOrganization_Should_Throw_Exception_For_Null_Organization()
        {
            var service = new Service(connectionStr);
            service.UpdateOrganization(null);
        }

        [TestMethod]
        public void UpdateOrganization_Should_Return_False_If_User_Is_Not_Authorized_to_Perform_Action()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, null, null);   //member cannot edit org
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);
            Organization org = new Organization();

            //Act
            bool result = service.UpdateOrganization(org);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateOrganization_Should_Throw_Exception_If_Subdomain_Is_Taken()
        {
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);
            string takenSubdomain = "takensubdomain";
            int takenSubdomainId = createTestOrg(takenSubdomain); //Subdomain "takensubdomain" is taken by a different org

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, null, null);    //owner can edit org
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);

            Organization updatingOrg = new Organization();
            updatingOrg.Name = orgName;
            updatingOrg.OrganizationId = orgId;
            updatingOrg.Subdomain = takenSubdomain; //attempting to change subdomain to "takensubdomain"

            //Act
            try
            {
                bool result = service.UpdateOrganization(updatingOrg);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentException);
            }

            //Clean up
            deleteTestOrg(takenSubdomainId);
            deleteTestOrg(orgId);
            deleteTestUser(email);
        }

        [TestMethod]
        public void UpdateOrganization_Should_Return_True_If_Succeed()
        {
            string email = "testuser@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(email);
            int orgId = createTestOrg(orgName);

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, null, null);    //owner can edit org
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);

            Organization updatingOrg = new Organization();
            updatingOrg.Name = "NewName";   
            updatingOrg.OrganizationId = orgId;
            updatingOrg.Subdomain = "newsubdomain";

            //Act
            bool result = service.UpdateOrganization(updatingOrg);

            string selectStmt = "SELECT [Name], [Subdomain] FROM [Auth].[Organization] WHERE [OrganizationId] = @orgId";
            SqlDataReader reader;
            string updatedName = "";
            string updatedSubdomain = "";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        updatedName = (string)reader["Name"];
                        updatedSubdomain = (string)reader["Subdomain"];
                    }
                    reader.Close();
                    connection.Close();
                }
            }

            //Clean up
            deleteTestOrg(orgId);
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(result && updatedName == "NewName" && updatedSubdomain == "newsubdomain");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "User Id cannot be 0 or negative.")]
        public void UpdateActiveOrganization_Should_Throw_Exception_For_Invalid_UserId()
        {
            var service = new Service(connectionStr);
            service.UpdateActiveOrganization(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Organization Id cannot be negative.")]
        public void UpdateActiveOrganization_Should_Throw_Exception_For_Invalid_OrgId()
        {
            var service = new Service(connectionStr);
            service.UpdateActiveOrganization(1, -1);
        }

        [TestMethod]
        public void UpdateActiveOrganization_Should_Update_Users_ActiveOrganizationId()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            var service = new Service(connectionStr);

            //Act
            service.UpdateActiveOrganization(userId, 1);    //update ActiveOrgId to 1

            string selectStmt = "SELECT [ActiveOrganizationId] FROM [Auth].[User] WHERE [UserId] = @userId";
            SqlDataReader reader;
            int activeOrgId = 0;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        activeOrgId = (int)reader["ActiveOrganizationId"];
                    }
                    reader.Close();
                    connection.Close();
                }
            }

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(activeOrgId == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Organization Id cannot be negative.")]
        public void GetOrgRole_Should_Throw_Exception_For_Invalid_OrgId()
        {
            var service = new Service(connectionStr);
            service.GetOrgRole(-1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "User Id cannot be 0 or negative.")]
        public void GetOrgRole_Should_Throw_Exception_For_Invalid_UserId()
        {
            var service = new Service(connectionStr);
            service.GetOrgRole(1, -1);
        }

        [TestMethod]
        public void GetOrgRole_Should_Return_Null_If_User_Has_No_Role_In_The_Org()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            var service = new Service(connectionStr);

            //Act
            OrgRole result = service.GetOrgRole(1, userId);

            //Clean up
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void GetOrgRole_Should_Return_Correct_OrgRole_If_Succeed()
        {
            //Arrange
            string email = "testuser@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(email);
            int orgId = createTestOrg(orgName);
            createOrgUser(userId, orgId, 2, "111");
            var service = new Service(connectionStr);

            //Act
            OrgRole result = service.GetOrgRole(orgId, userId);

            //Clean up
            deleteOrgUser(userId, orgId);
            deleteTestOrg(orgId);
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(result.OrgRoleId == 2 && result.OrgRoleName == "Owner");
        }

        [TestMethod]
        public void DeleteOrganization_Should_Return_False_If_User_Is_Not_Authorized_To_Perform_Action()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = 1;
            string orgName = "UnitTestOrg";
            int orgId = 1;

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, null, null);   //member cannot edit org
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);
            Organization org = new Organization();

            //Act
            bool result = service.DeleteOrganization();

            //Assert
            Assert.IsFalse(result);
        }

        //this method does not delete the Org from the database, but set its IsActive to False in all tables
        [TestMethod]
        public void DeleteOrganization_Should_Return_True_If_Succeed()
        {
            string email = "testuser@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(email);
            int orgId = createTestOrg(orgName);

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, null, null);    //owner can edit org
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            bool result = service.DeleteOrganization();

            string selectStmt = "SELECT [IsActive] FROM [Auth].[Organization] WHERE [OrganizationId] = @orgId";
            SqlDataReader reader;
            bool deactivated = false;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        deactivated = ((bool)reader["IsActive"] == false);
                    }
                    reader.Close();
                    connection.Close();
                }
            }

            //Clean up
            deleteTestOrg(orgId);
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(result && deactivated);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Url must have a value.")]
        public async Task InviteUser_Should_Throw_Exception_For_Null_Url()
        {
            var service = new Service(connectionStr);
            InvitationInfo invitationInfo = new InvitationInfo();
            await service.InviteUser(null, invitationInfo, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Url must have a value.")]
        public async Task InviteUser_Should_Throw_Exception_For_Empty_Url()
        {
            var service = new Service(connectionStr);
            InvitationInfo invitationInfo = new InvitationInfo();
            await service.InviteUser("", invitationInfo, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Invitation info object must not be null.")]
        public async Task InviteUser_Should_Throw_Exception_For_Null_InvitationInfo()
        {
            var service = new Service(connectionStr);
            await service.InviteUser("http://allyisapps.com", null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Email address is not valid")]
        public async Task InviteUser_Should_Throw_Exception_For_Null_Email_Address()
        {
            var service = new Service(connectionStr);
            InvitationInfo inviInfo = new InvitationInfo();
            await service.InviteUser("http://allyisapps.com", inviInfo, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Email address is not valid")]
        public async Task InviteUser_Should_Throw_Exception_For_Empty_Email_Address()
        {
            var service = new Service(connectionStr);
            InvitationInfo inviInfo = new InvitationInfo();
            inviInfo.Email = "";
            await service.InviteUser("http://allyisapps.com", inviInfo, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Email address is not valid")]
        public async Task InviteUser_Should_Throw_Exception_For_Invalid_Email_Address()
        {
            var service = new Service(connectionStr);
            InvitationInfo inviInfo = new InvitationInfo();
            inviInfo.Email = "invalid";
            await service.InviteUser("http://allyisapps.com", inviInfo, null, null);
        }

        [TestMethod]
        public async Task InviteUser_Should_Throw_Exception_For_User_Is_Already_A_Member_Of_Org()
        {
            //Arrange
            string email = "testuser@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(email);
            int orgId = createTestOrg(orgName);
            createOrgUser(userId, orgId, 1, "111"); //user is member of org

            UserContext userContext = new UserContext(1, email, email, orgId, 0, null, 1); // invitingUserId = 1
            var service = new Service(connectionStr, userContext);
            InvitationInfo inviInfo = new InvitationInfo();
            inviInfo.Email = email;
            inviInfo.EmployeeId = "110";
            inviInfo.FirstName = "Test";
            inviInfo.LastName = "User";
            inviInfo.OrganizationId = orgId;
            inviInfo.OrgRole = 1;
            
            //Act
            try
            {
                await service.InviteUser("http://allyisapps.com", inviInfo, null, null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InvalidOperationException);
            }

            //Clean up
            deleteOrgUser(userId, orgId);
            deleteTestOrg(orgId);
            deleteTestUser(email);
        }

        [TestMethod]
        public async Task InviteUser_Should_Throw_Exception_For_Duplicate_EmployeeId()
        {
            //Arrange
            string email = "testuser@test.com";
            string otherMember = "othermember@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(email);
            int otherUserId = createTestUser(otherMember);
            int orgId = createTestOrg(orgName);
            createOrgUser(otherUserId, orgId, 1, "111"); //otherUserId is member of org with employeeId 111

            UserContext userContext = new UserContext(1, email, email, orgId, 0, null, 1); // invitingUserId = 1
            var service = new Service(connectionStr, userContext);
            InvitationInfo inviInfo = new InvitationInfo();
            inviInfo.Email = email;
            inviInfo.EmployeeId = "111";    //duplicate employeeId
            inviInfo.OrganizationId = orgId;
            inviInfo.OrgRole = 1;

            //Act
            try
            {
                await service.InviteUser("http://allyisapps.com", inviInfo, null, null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is DuplicateNameException);
            }

            //Clean up
            deleteOrgUser(otherUserId, orgId);
            deleteTestUser(otherMember);
            deleteTestUser(email);
            deleteTestOrg(orgId);
        }

        [TestMethod]
        public async Task InviteUser_Should_Return_InvitationId_If_Succeed()
        {
            //Arrange
            string email = "testuser@test.com";
            string invitingUser = "othermember@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(email);
            int invitingUserId = createTestUser(invitingUser);
            int orgId = createTestOrg(orgName);
            createOrgUser(userId, 1, 1, "110"); //user is member of another org. This shouldn't affect the result of this method

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, null, null);
            List<UserOrganizationInfo> userOrgInfoList = new List<UserOrganizationInfo>() { userOrgInfo };
            UserContext userContext = new UserContext(invitingUserId, invitingUser, invitingUser, orgId, 0, userOrgInfoList, 1);
            var service = new Service(connectionStr, userContext);
            InvitationInfo inviInfo = new InvitationInfo();
            inviInfo.Email = email;
            inviInfo.EmployeeId = "111";    
            inviInfo.OrganizationId = orgId;
            inviInfo.OrgRole = 1;
            inviInfo.FirstName = "Test";
            inviInfo.LastName = "User";

            //Act
            int inviId = await service.InviteUser("http://allyisapps.com/Account/ConfirmEmail?userId=%7BuserId%7D&code=%7BaccessCode%7D", inviInfo, null, null);

            //Clean up
            deleteOrgUser(userId, 1);
            deleteUserInvitation(email);
            deleteTestUser(invitingUser);
            deleteTestUser(email);
            deleteTestOrg(orgId);

            //Assert
            Assert.IsTrue(inviId > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Invitation Id cannot be 0 or negative.")]
        public void RemoveInvitation_Should_Throw_Exception_For_Invalid_InvitationId()
        {
            var service = new Service(connectionStr);
            service.RemoveInvitation(-1);
        }

        [TestMethod]
        public void RemoveInvitation_Should_Return_False_If_User_Is_Not_Authorized_To_Perform_Action()
        {
            //Arrange
            string email = "testuser@test.com";
            int userId = createTestUser(email);
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);
            int inviId = createUserInvitation(email, orgId, 1, "111");

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, null, null);   //member cannot edit org
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, 0, infoList, 1);
            var service = new Service(connectionStr, userContext);
            Organization org = new Organization();

            //Act
            bool result = service.RemoveInvitation(inviId);

            //Clean up
            deleteUserInvitation(email);
            deleteTestOrg(orgId);
            deleteTestUser(email);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveInvitation_Should_Return_True_If_Succeed()
        {
            string email = "testuser@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(email);
            int orgId = createTestOrg(orgName);
            int inviId = createUserInvitation(email, orgId, 1, "111");

            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, null, null);    //owner can edit org
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(1, "calling@email.com", "calling@email.com", orgId, 0, infoList, 1); //calling user has id=1
            var service = new Service(connectionStr, userContext);

            //Act
            bool result = service.RemoveInvitation(inviId);

            string selectStmt = "SELECT [InvitationId] FROM [Auth].[Invitation] WHERE [InvitationId] = @inviId";
            SqlDataReader reader;
            bool deleted = false;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@inviId", SqlDbType.Int).Value = inviId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    if (!reader.HasRows) deleted = true;
                    reader.Close();
                    connection.Close();
                }
            }

            //Clean up
            deleteTestOrg(orgId);
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(result && deleted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateSubscriptionUserProductRole_Should_Return_Exception_For_Negative_Selected_Role()
        {
            var service = new Service(connectionStr);
            service.UpdateSubscriptionUserProductRole(-1, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateSubscriptionUserProductRole_Should_Return_Exception_For_Negative_SubscriptionId()
        {
            var service = new Service(connectionStr);
            service.UpdateSubscriptionUserProductRole(1, -1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UpdateSubscriptionUserProductRole_Should_Return_Exception_For_Invalid_UserId()
        {
            var service = new Service(connectionStr);
            service.UpdateSubscriptionUserProductRole(1, 1, -1);
        }

        [TestMethod]
        public void UpdateSubscriptionUserProductRole_Should_Update_Users_Sub_Product_Role_If_Succeed()
        {
            //Arrange
            string email = "testuser@test.com";
            string orgName = "UnitTestOrg";
            int userId = createTestUser(email);
            int orgId = createTestOrg(orgName);
            int subId = createSubscription(orgId, 1, 100);
            createSubUser(subId, userId, 1);    //this user currently has product role User

            //Act
            var service = new Service(connectionStr);
            service.UpdateSubscriptionUserProductRole(2, subId, userId);    //update to Manager role

            string selectStmt = "SELECT [ProductRoleId] FROM [Billing].[SubscriptionUser] WHERE [SubscriptionId] = @SubscriptionId AND [UserId] = @UserId";
            SqlDataReader reader;
            bool updated = false;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@SubscriptionId", SqlDbType.Int).Value = subId;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    // open connection, execute command, close connection
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        updated = ((int)reader["ProductRoleId"] == 2);
                    }
                    reader.Close();
                    connection.Close();
                }
            }

            //Clean up
            deleteSubUser(userId, subId);
            deleteSubscription(subId);
            deleteTestOrg(orgId);
            deleteTestUser(email);

            //Assert
            Assert.IsTrue(updated);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetOrganizationMemberList_Should_Throw_Exception_For_Negative_OrgId()
        {
            var service = new Service(connectionStr);
            service.GetOrganizationMemberList(-1);
        }

        [TestMethod]
        public void GetOrganizationMemberList_Should_Return_Member_List_Of_Org()
        {
            //Arrange
            string member1 = "testuser1@test.com";
            string member2 = "testuser2@test.com";
            string orgName = "UnitTestOrg";
            int orgId = createTestOrg(orgName);
            int userId1 = createTestUser(member1);
            int userId2 = createTestUser(member2);
            createOrgUser(userId1, orgId, 1, "111");    //member
            createOrgUser(userId2, orgId, 2, "112");    //owner

            //Act
            var service = new Service(connectionStr);
            IEnumerable<OrganizationUserInfo> memberList = service.GetOrganizationMemberList(orgId);

            //Clean up
            deleteOrgUser(userId2, orgId);
            deleteOrgUser(userId1, orgId);
            deleteTestUser(member2);
            deleteTestUser(member1);
            deleteTestOrg(orgId);

            //Assert
            OrganizationUserInfo mem1 = memberList.ElementAt(0);
            bool correctMember1 = (mem1.UserId == userId1 && mem1.OrgRoleId == 1 && mem1.EmployeeId == "111");
            OrganizationUserInfo mem2 = memberList.ElementAt(1);
            bool correctMember2 = (mem2.UserId == userId2 && mem2.OrgRoleId == 2 && mem2.EmployeeId == "112");

            Assert.IsTrue(memberList.Count() == 2 && correctMember1 && correctMember2);
        }

        
    }
}
