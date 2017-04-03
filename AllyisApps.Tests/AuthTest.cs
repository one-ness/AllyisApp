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
            string insertStmt = "INSERT INTO [Auth].[Organization]([Name], [IsActive]) " +
                                "VALUES(@name, @isActive)";
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
        public static void deleteTestOrg(string orgName)
        {
            var service = new Service(connectionStr);
            string stmt1 = "DELETE FROM [Auth].[Organization] WHERE [Name] = @orgName";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                //open connection
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(stmt1, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgName", SqlDbType.VarChar, 100).Value = orgName;

                    //execute command
                    int result = cmd.ExecuteNonQuery();
                }

                //close connection
                connection.Close();
            }
        }

        //create a simple invitation (without subscription or role) and return the id
        public static int createUserInvitation(string email, int orgId)
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
                    cmd.Parameters.Add("@employeeId", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@dob", SqlDbType.DateTime2).Value = "1980-01-01";
                    cmd.Parameters.Add("@accessCode", SqlDbType.VarChar, 100).Value = "fakecode";
                    cmd.Parameters.Add("@orgRole", SqlDbType.Int).Value = 1;

                    int result = cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
                {
                    // set up the command's parameters
                    cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
                    cmd.Parameters.Add("email", SqlDbType.VarChar, 100).Value = email;

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

        public static void createOrgUser(int userId, int orgId)
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
                    cmd.Parameters.Add("@employeeId", SqlDbType.VarChar, 16).Value = 1;
                    cmd.Parameters.Add("@orgRoleId", SqlDbType.Int).Value = 1;

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
            createUserInvitation(userEmail, orgId);

            var service = new Service(connectionStr);

            //Act
            List<InvitationInfo> invi = service.GetInvitationsByUser(userEmail);

            //Clean up
            deleteUserInvitation(userEmail);
            deleteTestOrg(orgName);
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
            int inviId = createUserInvitation(userEmail, orgId);

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
                    cmd.Parameters.Add("@employeeId", SqlDbType.Int).Value = 1;
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
            deleteTestOrg(orgName);

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
            int inviId = createUserInvitation(userEmail, orgId);

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
                    cmd.Parameters.Add("@employeeId", SqlDbType.Int).Value = 1;
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
            deleteTestOrg(orgName);

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
        //[ExpectedException(typeof(SqlException))]
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
            createOrgUser(userId, orgId);

            var service = new Service(connectionStr);

            //Act
            UserContext populated = service.PopulateUserContext(userId);

            //Cleanup
            deleteOrgUser(userId, orgId);
            deleteTestOrg(orgName);
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
            createOrgUser(userId, org1Id);  //test user is part of Org1
            int inviId = createUserInvitation(email, org2Id);    //test user is invited to Org2

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
            deleteTestOrg(orgName1);
            deleteTestOrg(orgName2);
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
            createOrgUser(userId, orgId);

            UserContext userContext = new UserContext(userId, userEmail, userEmail, 0, 0, null, 1);
            var service = new Service(connectionStr, userContext);

            //Act
            IEnumerable<Organization> orgList = service.GetOrganizationsByUserId();

            //Clean up
            deleteOrgUser(userId, orgId);
            deleteTestOrg(orgName);
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
    }
}
