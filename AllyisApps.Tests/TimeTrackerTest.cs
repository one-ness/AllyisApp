using AllyisApps.Services.TimeTracker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AllyisApps.Services.Tests
{
	[TestClass]
	public class TimeTrackerTest
	{
		public static string connectionStr = "Data Source=(local);Initial Catalog=AllyisAppsDB;User Id=aaUser;Password=BlueSky23#;";

		[TestMethod]
		public void GetDateFromDays_Should_Return_Null_For_Negative_Days()
		{
			var date = Service.GetDateFromDays(-2);
			Assert.IsTrue(date == null);
		}

		[TestMethod]
		public void GetDateFromDays_Should_Return_Correct_Date()
		{
			var date = Service.GetDateFromDays(10);
			Assert.IsTrue(date == new DateTime(0001, 01, 11));
		}

		[TestMethod]
		public void GetDayFromDateTime_Should_Return_Negative_One_For_Null_DateTime()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			int days = ttService.GetDayFromDateTime(null);
			Assert.IsTrue(days == -1);
		}

		[TestMethod]
		public void GetDayFromDateTime_Should_Return_Correct_Days()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			DateTime date = new DateTime(0001, 01, 21);
			int days = ttService.GetDayFromDateTime(date);
			Assert.IsTrue(days == 20);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void GetTimeEntry_Should_Throw_Exception_For_Invalid_timeEntryId()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.GetTimeEntry(-1);
		}

		[TestMethod]
		public void GetTimeEntry_Should_Return_Correct_TimeEntry()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			DateTime date = new DateTime(2017, 1, 1);
			int userId = AuthTest.createTestUser(email);
			string orgName = "UnitTestOrg";
			int orgId = AuthTest.createTestOrg(orgName);
			int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
			int projId = AuthTest.createProject(custId, "sampleProj", 1, "PROJ1");
			int timeEntryId = AuthTest.createTimeEntry(userId, projId, date, 1, 8);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);

			try
			{
				//Act
				TimeEntryInfo result = ttService.GetTimeEntry(timeEntryId);

				//Assert
				Assert.IsTrue(result.Date == date && result.Duration == 8 && result.PayClassId == 1 && result.UserId == userId && result.ProjectId == projId);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntry(timeEntryId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateTimeEntry_Should_Throw_Exception_For_Null_Entry()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.CreateTimeEntry(null);
		}

		[TestMethod]
		public void CreateTimeEntry_Should_Return_The_Correct_TimeEntryId_Of_The_Entry_Created()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			DateTime date = new DateTime(2017, 1, 1);
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
			int projId = AuthTest.createProject(custId, "sampleProj", 1, "PROJ1");
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);

			TimeEntryInfo entryInfo = new TimeEntryInfo
			{
				Date = date,
				Duration = 6,
				PayClassId = 1,
				ProjectId = projId,
				UserId = userId
			};
			int entryId = 0;
			try
			{
				//Act
				int result = ttService.CreateTimeEntry(entryInfo);

				string selectStmt = "SELECT [TimeEntryId] FROM [TimeTracker].[TimeEntry] WHERE [UserId] = @userId AND [ProjectId] = @projId";
				SqlDataReader reader;

				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
						cmd.Parameters.Add("@projId", SqlDbType.Int).Value = projId;

						// open connection, execute command, close connection
						connection.Open();
						reader = cmd.ExecuteReader();
						while (reader.Read())
						{
							entryId = (int)reader["TimeEntryId"];
						}
						reader.Close();
						connection.Close();
					}
				}

				//Assert
				Assert.IsTrue(result == entryId);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntry(entryId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateTimeEntry_Should_Throw_Exception_For_Null_Entry()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.UpdateTimeEntry(null);
		}

		[TestMethod]
		public void UpdateTimeEntry_Should_Update_The_Entry_Correctly()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			DateTime date = new DateTime(2017, 1, 1);
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
			int projId = AuthTest.createProject(custId, "sampleProj", 1, "PROJ1");
			int timeEntryId = AuthTest.createTimeEntry(userId, projId, date, 1, 8); //existing time entry
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);

			TimeEntryInfo updatedEntry = new TimeEntryInfo
			{
				Duration = 6,   //update the duration
				PayClassId = 1,
				ProjectId = projId,
				UserId = userId,
				TimeEntryId = timeEntryId
			};

			try
			{
				//Act
				ttService.UpdateTimeEntry(updatedEntry);

				string selectStmt = "SELECT [Duration] FROM [TimeTracker].[TimeEntry] WHERE [UserId] = @userId AND [ProjectId] = @projId";
				SqlDataReader reader;
				float duration = 0;
				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
						cmd.Parameters.Add("@projId", SqlDbType.Int).Value = projId;

						// open connection, execute command, close connection
						connection.Open();
						reader = cmd.ExecuteReader();
						while (reader.Read())
						{
							duration = (float)(double)reader["Duration"];
						}
						reader.Close();
						connection.Close();
					}
				}

				//Assert
				Assert.IsTrue(duration == 6);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntry(timeEntryId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void DeleteTimeEntry_Should_Throw_Exception_For_Invalid_TimeEntryId()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.DeleteTimeEntry(-1);
		}

		[TestMethod]
		public void DeleteTimeEntry_Should_Delete_The_Correct_Entry()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			DateTime date = new DateTime(2017, 1, 1);
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
			int projId = AuthTest.createProject(custId, "sampleProj", 1, "PROJ1");
			int timeEntryId = AuthTest.createTimeEntry(userId, projId, date, 1, 8); //existing time entry
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);

			try
			{
				//Act
				ttService.DeleteTimeEntry(timeEntryId);

				string selectStmt = "SELECT [TimeEntryId] FROM [TimeTracker].[TimeEntry] WHERE [UserId] = @userId AND [ProjectId] = @projId";
				SqlDataReader reader;
				bool deleted = false;
				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
						cmd.Parameters.Add("@projId", SqlDbType.Int).Value = projId;

						// open connection, execute command, close connection
						connection.Open();
						reader = cmd.ExecuteReader();
						if (!reader.HasRows) deleted = true;
						reader.Close();
						connection.Close();
					}
				}

				//Assert
				Assert.IsTrue(deleted);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntry(timeEntryId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		//No need to test null arguments because it's not possible to pass in null DateTime
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetTimeEntriesOverDateRange_Should_Throw_Exception_For_Invalid_Time_Range()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			DateTime start = new DateTime(2017, 2, 1);
			DateTime end = new DateTime(2017, 1, 1);    //start > end
			ttService.GetTimeEntriesOverDateRange(start, end);
		}

		[TestMethod]
		public void GetTimeEntriesOverDateRange_Should_Return_Correct_Entries_For_Current_Organization()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			DateTime date1 = new DateTime(2017, 1, 1);
			DateTime date2 = new DateTime(2017, 1, 20);
			DateTime date3 = new DateTime(2017, 2, 1);
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int regPayClassId = AuthTest.createPayClass(orgId);
			AuthTest.createOrgUser(userId, orgId, 1, "111");
			int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
			int projId = AuthTest.createProject(custId, "sampleProj", 1, "PROJ1");
			int timeEntryId1 = AuthTest.createTimeEntry(userId, projId, date1, regPayClassId, 1); //entry 1
			int timeEntryId2 = AuthTest.createTimeEntry(userId, projId, date2, regPayClassId, 2); //entry 2
			int timeEntryId3 = AuthTest.createTimeEntry(userId, projId, date3, regPayClassId, 3); //entry 3

			UserContext userContext = new UserContext(userId, email, email, orgId, 0, null, 1);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);

			DateTime start = new DateTime(2017, 1, 1);
			DateTime end = new DateTime(2017, 1, 31);

			try
			{
				//Act
				IEnumerable<TimeEntryInfo> entryList = ttService.GetTimeEntriesOverDateRange(start, end);

				//Assert
				Assert.IsTrue(entryList.Count() == 2 && entryList.ElementAt(0).TimeEntryId == timeEntryId1 && entryList.ElementAt(1).TimeEntryId == timeEntryId2);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntriesByProjectId(projId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deleteOrgUser(userId, orgId);
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetTimeEntriesByUserOverDateRange_Should_Thow_Exception_For_Empty_UserIds_List()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			List<int> userIds = new List<int>();
			DateTime start = new DateTime(2017, 1, 1);
			DateTime end = new DateTime(2017, 1, 31);
			ttService.GetTimeEntriesByUserOverDateRange(userIds, start, end);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetTimeEntriesByUserOverDateRange_Should_Thow_Exception_For_Null_UserIds_List()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			DateTime start = new DateTime(2017, 1, 1);
			DateTime end = new DateTime(2017, 1, 31);
			ttService.GetTimeEntriesByUserOverDateRange(null, start, end);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void GetTimeEntriesByUserOverDateRange_Should_Thow_Exception_For_Invalid_UserId()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			List<int> userIds = new List<int> { -1, 1 };
			DateTime start = new DateTime(2017, 1, 1);
			DateTime end = new DateTime(2017, 1, 31);
			ttService.GetTimeEntriesByUserOverDateRange(userIds, start, end);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetTimeEntriesByUserOverDateRange_Should_Thow_Exception_For_Invalid_Date_Range()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			List<int> userIds = new List<int> { 1 };
			DateTime end = new DateTime(2017, 1, 1);
			DateTime start = new DateTime(2017, 1, 31);
			ttService.GetTimeEntriesByUserOverDateRange(userIds, start, end);
		}

		[TestMethod]
		public void GetTimeEntriesByUserOverDateRange_Should_Return_Correct_Entries_For_Given_Users()
		{
			//Arrange
			string email1 = "testuser1@test.com";
			string email2 = "testuser2@test.com";
			string orgName = "UnitTestOrg";
			DateTime date1 = new DateTime(2017, 1, 1);
			DateTime date2 = new DateTime(2017, 1, 20);
			DateTime date3 = new DateTime(2017, 2, 1);

			int userId1 = AuthTest.createTestUser(email1);
			int userId2 = AuthTest.createTestUser(email2);
			int orgId = AuthTest.createTestOrg(orgName);
			int regPayClassId = AuthTest.createPayClass(orgId);
			AuthTest.createOrgUser(userId1, orgId, 1, "111");
			AuthTest.createOrgUser(userId2, orgId, 1, "112");
			int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
			int projId = AuthTest.createProject(custId, "sampleProj", 1, "PROJ1");
			int timeEntryId1 = AuthTest.createTimeEntry(userId1, projId, date1, regPayClassId, 1); //entry 1
			int timeEntryId2 = AuthTest.createTimeEntry(userId1, projId, date2, regPayClassId, 2); //entry 2
			int timeEntryId3 = AuthTest.createTimeEntry(userId2, projId, date3, regPayClassId, 3); //entry 3

			UserContext userContext = new UserContext(userId1, email1, email1, orgId, 0, null, 1);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);

			DateTime start = new DateTime(2017, 1, 1);
			DateTime end = new DateTime(2017, 2, 1);

			List<int> userIds = new List<int> { userId1, userId2 };

			try
			{
				//Act
				IEnumerable<TimeEntryInfo> entryList = ttService.GetTimeEntriesByUserOverDateRange(userIds, start, end);

				//Assert
				Assert.IsTrue(entryList.Count() == 3 && entryList.ElementAt(0).TimeEntryId == timeEntryId1 && entryList.ElementAt(1).TimeEntryId == timeEntryId2 && entryList.ElementAt(2).TimeEntryId == timeEntryId3);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntriesByProjectId(projId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deleteOrgUser(userId1, orgId);
				AuthTest.deleteOrgUser(userId2, orgId);
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email1);
				AuthTest.deleteTestUser(email2);
			}
		}

		[TestMethod]
		public void GetReportInfo_Should_Return_All_Requested_Data()
		{
			//Arrange
			string email1 = "testuser1@test.com";
			string email2 = "testuser2@test.com";
			string orgName = "UnitTestOrg";

			int userId1 = AuthTest.createTestUser(email1);
			int userId2 = AuthTest.createTestUser(email2);
			int orgId = AuthTest.createTestOrg(orgName);
			int regPayClassId = AuthTest.createPayClass(orgId);
			AuthTest.createOrgUser(userId1, orgId, 1, "111");
			AuthTest.createOrgUser(userId2, orgId, 1, "112");

			int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");   //1 customer
			int projId = AuthTest.createProject(custId, "sampleProj", 1, "PROJ1");  //1 project
			int subId = AuthTest.createSubscription(orgId, 1, 50);      // 1 subscription with 2 sub users
			AuthTest.createSubUser(subId, userId1, 1);
			AuthTest.createSubUser(subId, userId2, 2);

			UserContext userContext = new UserContext(userId1, email1, email1, orgId, subId, null, 1);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);

			try
			{
				//Act
				Tuple<List<Customer>, List<CompleteProjectInfo>, List<SubscriptionUserInfo>> tuple = ttService.GetReportInfo();

				//Assert
				List<Customer> customerList = tuple.Item1;
				bool correctCustomerList = (customerList.Count == 1 && customerList.ElementAt(0).CustomerId == custId);
				List<CompleteProjectInfo> projList = tuple.Item2;
				bool correctProjList = (projList.Count == 1 && projList.ElementAt(0).ProjectId == projId);
				List<SubscriptionUserInfo> subUserList = tuple.Item3;
				bool correctSubUserList = (subUserList.Count == 2 && subUserList.ElementAt(0).UserId == userId1 && subUserList.ElementAt(1).UserId == userId2);
				Assert.IsTrue(correctCustomerList && correctProjList && correctSubUserList);
			}
			finally
			{
				//Clean up
				AuthTest.deleteSubUserBySubId(subId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deleteOrgUserByOrgId(orgId);
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email1);
				AuthTest.deleteTestUser(email2);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateHoliday_Should_Throw_Exception_For_Null_Holiday()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.CreateHoliday(null);
		}

		[TestMethod]
		public void CreateHoliday_Should_Return_False_If_Authorization_Fails()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);   //member cannot edit org
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			Holiday newHoliday = new Holiday
			{
				HolidayName = "randomHoliday",
				Date = new DateTime(2017, 12, 1)
			};

			try
			{
				//Act
				bool result = ttService.CreateHoliday(newHoliday);

				//Assert
				Assert.IsFalse(result);
			}
			finally
			{
				//Clean up
				AuthTest.deleteSubUserBySubId(subId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void CreateHoliday_Should_Return_True_If_Succeed()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			AuthTest.createSubUser(subId, userId, 2);   //user is a Manager of the subscription

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			Holiday newHoliday = new Holiday
			{
				HolidayName = "randomHoliday",
				Date = new DateTime(2017, 12, 1),
				OrganizationId = orgId
			};

			int holidayId = -1;
			int timeEntryId = -1;

			try
			{
				//Act
				bool result = ttService.CreateHoliday(newHoliday);

				//Look into Holiday table and TimeEntry table to make sure a new holiday is created and new time entry for that holiday is added
				string selectStmt1 = "SELECT [HolidayId] FROM [TimeTracker].[Holiday] WHERE [OrganizationId] = @orgId AND [HolidayName] = @holidayName";
				string selectStmt2 = "SELECT [TimeEntryId] FROM [TimeTracker].[TimeEntry] WHERE [UserId] = @userId AND [Description] = @holidayName";
				SqlDataReader reader;

				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					connection.Open();
					using (SqlCommand cmd = new SqlCommand(selectStmt1, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
						cmd.Parameters.Add("@holidayName", SqlDbType.VarChar, 100).Value = "randomHoliday";

						// open connection, execute command, close connection
						reader = cmd.ExecuteReader();
						while (reader.Read()) { holidayId = (int)reader["HolidayId"]; }
						reader.Close();
					}
					using (SqlCommand cmd = new SqlCommand(selectStmt2, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
						cmd.Parameters.Add("@holidayName", SqlDbType.VarChar, 100).Value = "randomHoliday";

						// open connection, execute command, close connection
						reader = cmd.ExecuteReader();
						while (reader.Read()) { timeEntryId = (int)reader["TimeEntryId"]; }
						reader.Close();
					}
					connection.Close();
				}

				//Assert
				Assert.IsTrue(result && holidayId != -1 && timeEntryId != -1);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntry(timeEntryId);
				AuthTest.deleteHoliday(holidayId);
				AuthTest.deleteSubUserBySubId(subId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DeleteHoliday_Should_Throw_Exception_For_Invalid_HolidayId()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.DeleteHoliday(-1);
		}

		[TestMethod]
		public void DeleteHoliday_Should_Return_False_If_Authorization_Fails()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);   //member cannot edit org
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			int holidayId = AuthTest.createHoliday("randomHoliday", new DateTime(2017, 12, 1), orgId);

			try
			{
				//Act
				bool result = ttService.DeleteHoliday(holidayId);

				//Assert
				Assert.IsFalse(result);
			}
			finally
			{
				//Clean up
				AuthTest.deleteHoliday(holidayId);
				AuthTest.deleteSubUserBySubId(subId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void DeleteHoliday_Should_Return_True_If_Succeed()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
			int projId = AuthTest.createProject(custId, "sampleProj", 1, "PROJ1");
			AuthTest.createSubUser(subId, userId, 2);   //user is a Manager of the subscription

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			List<int> projList = new List<int> { projId };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, projList);
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			//create holiday
			int holidayId = AuthTest.createHoliday("randomHoliday", new DateTime(2017, 12, 1), orgId);

			//create time entry for the new holiday
			string stmt = "SELECT TOP 1 [PayClassID] FROM [TimeTracker].[PayClass] WITH (NOLOCK) WHERE[Name] = 'Holiday'";
			SqlDataReader payClassIdReader;
			int payClassId = -1;
			using (SqlConnection connection = new SqlConnection(connectionStr))
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(stmt, connection))
				{
					payClassIdReader = cmd.ExecuteReader();
					while (payClassIdReader.Read()) { payClassId = (int)payClassIdReader["PayClassID"]; }
					payClassIdReader.Close();
				}
				connection.Close();
			}
			int timeEntryId = AuthTest.createTimeEntry(userId, projId, new DateTime(2017, 12, 1), payClassId, 8);    //this time entry should be deleted

			try
			{
				//Act
				bool result = ttService.DeleteHoliday(holidayId);

				//Look into Holiday table and TimeEntry table to make sure both the holiday and the related time entry are deleted
				string selectStmt1 = "SELECT [HolidayId] FROM [TimeTracker].[Holiday] WHERE [OrganizationId] = @orgId AND [HolidayName] = @holidayName";
				string selectStmt2 = "SELECT [TimeEntryId] FROM [TimeTracker].[TimeEntry] WHERE [UserId] = @userId AND [PayClassId] = @payClassId";
				SqlDataReader reader;
				bool holidayDeleted = false;
				bool timeEntryDeleted = false;
				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					connection.Open();
					using (SqlCommand cmd = new SqlCommand(selectStmt1, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
						cmd.Parameters.Add("@holidayName", SqlDbType.VarChar, 100).Value = "randomHoliday";

						// open connection, execute command, close connection
						reader = cmd.ExecuteReader();
						if (!reader.HasRows) { holidayDeleted = true; }
						reader.Close();
					}
					using (SqlCommand cmd = new SqlCommand(selectStmt2, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
						cmd.Parameters.Add("@payClassId", SqlDbType.Int).Value = payClassId;

						// open connection, execute command, close connection
						reader = cmd.ExecuteReader();
						if (!reader.HasRows) { timeEntryDeleted = true; }
						reader.Close();
					}
					connection.Close();
				}

				//Assert
				Assert.IsTrue(result & holidayDeleted && timeEntryDeleted);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntry(timeEntryId);
				AuthTest.deleteHoliday(holidayId);
				AuthTest.deleteSubUserBySubId(subId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void GetLockDate_Should_Return_Null_If_LockDate_Is_Not_Used()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			AuthTest.createTimeTrackerSetting(orgId, 0);

			UserContext userContext = new UserContext(userId, email, email, orgId, 0, null, 1);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);

			try
			{
				//Act
				DateTime? lockDate = ttService.GetLockDate();

				//Assert
				Assert.IsTrue(lockDate == null);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void GetLockDate_Should_Return_Correct_LockDate()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			AuthTest.createTimeTrackerSetting(orgId, 1);    //lockDate used

			UserContext userContext = new UserContext(userId, email, email, orgId, 0, null, 1);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);

			try
			{
				//Act
				DateTime lockDate = ttService.GetLockDate().Value;
				string result = lockDate.ToString("yyyy-MM-dd");
				string expected = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");

				//Assert
				Assert.IsTrue(result == expected);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreatePayClass_Should_Throw_Exception_For_Null_PayClassName()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.CreatePayClass(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreatePayClass_Should_Throw_Exception_For_Empty_PayClassName()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.CreatePayClass("");
		}

		[TestMethod]
		public void CreatePayClass_Should_Throw_Exception_For_Duplicate_PayClass()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int orgId = AuthTest.createTestOrg(orgName);
			int userId = AuthTest.createTestUser(email);
			int regPayClassId = AuthTest.createPayClass(orgId);

			//Act
			UserContext userContext = new UserContext(userId, email, email, orgId, 0, null, 1);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			try
			{
				ttService.CreatePayClass("Regular");    //already existed
			}
			catch (Exception e)
			{
				Assert.IsTrue(e is ArgumentException);
			}
			finally
			{
				//Clean up
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void CreatePayClass_Should_Return_False_If_Authorization_Fails()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int regPayClassId = AuthTest.createPayClass(orgId);
			int subId = AuthTest.createSubscription(orgId, 1, 50);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);   //member cannot edit org
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.CreatePayClass("NewPayClass");

				//Assert
				Assert.IsFalse(result);
			}
			finally
			{
				//Clean up
				AuthTest.deleteSubscription(subId);
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void CreatePayClass_Should_Return_True_If_Succeed()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int regPayClassId = AuthTest.createPayClass(orgId);
			int subId = AuthTest.createSubscription(orgId, 1, 50);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);   //member cannot edit org
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.CreatePayClass("NewPayClass");

				string selectStmt = "SELECT [PayClassID] FROM [TimeTracker].[PayClass] WHERE [OrganizationId] = @orgId AND [Name] = @name";
				SqlDataReader reader;
				bool created = false;
				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@orgId", SqlDbType.Int).Value = orgId;
						cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = "NewPayClass";

						// open connection, execute command, close connection
						connection.Open();
						reader = cmd.ExecuteReader();
						if (reader.HasRows) created = true;
						reader.Close();
						connection.Close();
					}
				}

				//Assert
				Assert.IsTrue(result && created);
			}
			finally
			{
				//Clean up
				AuthTest.deleteSubscription(subId);
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void DeletePayClass_Should_Throw_Exception_For_Invalid_PayClassId()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.DeletePayClass(-1);
		}

		[TestMethod]
		public void DeletePayClass_Should_Return_False_If_Authorization_Fails()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int regPayClassId = AuthTest.createPayClass(orgId);
			int subId = AuthTest.createSubscription(orgId, 1, 50);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);   //member cannot edit org
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.DeletePayClass(regPayClassId);

				//Assert
				Assert.IsFalse(result);
			}
			finally
			{
				//Clean up
				AuthTest.deleteSubscription(subId);
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void DeletePayClass_Should_Return_True_If_Succeed()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int regPayClassId = AuthTest.createPayClass(orgId);
			int subId = AuthTest.createSubscription(orgId, 1, 50);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);   //member cannot edit org
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.DeletePayClass(regPayClassId);  //delete the Regular payclass

				string selectStmt = "SELECT [PayClassID] FROM [TimeTracker].[PayClass] WHERE [PayClassID] = @id";
				SqlDataReader reader;
				bool deleted = false;
				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@id", SqlDbType.Int).Value = regPayClassId;

						// open connection, execute command, close connection
						connection.Open();
						reader = cmd.ExecuteReader();
						if (!reader.HasRows) deleted = true;
						reader.Close();
						connection.Close();
					}
				}

				//Assert
				Assert.IsTrue(result && deleted);
			}
			finally
			{
				//Clean up
				AuthTest.deleteSubscription(subId);
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void GetPayClasses_Should_Return_A_List_Of_PayClass_For_Given_Organization()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int regPayClassId = AuthTest.createPayClass(orgId); //8 pay classes are created

			UserContext userContext = new UserContext(userId, email, email, orgId, 0, null, 1);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);

			try
			{
				//Act
				IEnumerable<PayClass> payclasses = ttService.GetPayClasses();

				//Assert
				Assert.IsTrue(payclasses.Count() == 8 && payclasses.ElementAt(0).PayClassID == regPayClassId);
			}
			finally
			{
				//Clean up
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UpdateStartOfWeek_Should_Throw_Exception_For_Invalid_startOfWeek()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.UpdateStartOfWeek(8);
		}

		[TestMethod]
		public void UpdateStartOfWeek_Should_Return_False_If_Authorization_Fails()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			AuthTest.createTimeTrackerSetting(orgId, 1);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.UpdateStartOfWeek(3);

				//Assert
				Assert.IsFalse(result);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void UpdateStartOfWeek_Should_Return_True_If_Succeed()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			AuthTest.createTimeTrackerSetting(orgId, 1);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.UpdateStartOfWeek(3);   //update to Wed

				string selectStmt = "SELECT [StartOfWeek] FROM [TimeTracker].[Setting] WHERE [OrganizationId] = @id";
				SqlDataReader reader;
				bool updated = false;
				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@id", SqlDbType.Int).Value = orgId;

						// open connection, execute command, close connection
						connection.Open();
						reader = cmd.ExecuteReader();
						while (reader.Read())
						{
							updated = ((int)reader["StartOfWeek"] == 3);
						}
						reader.Close();
						connection.Close();
					}
				}

				//Assert
				Assert.IsTrue(result && updated);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void UpdateOvertime_Should_Throw_Exception_For_Invalid_overtimeHours()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.UpdateOvertime(-2, "Weeks", 2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UpdateOvertime_Should_Throw_Exception_For_Invalid_overtimePeriod()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.UpdateOvertime(30, "Quarter", 2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void UpdateOvertime_Should_Throw_Exception_For_Invalid_overtimeMultiplier()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.UpdateOvertime(30, "Week", 0.50f);
		}

		[TestMethod]
		public void UpdateOvertime_Should_Return_False_If_Authorization_Fails()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			AuthTest.createTimeTrackerSetting(orgId, 1);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.UpdateOvertime(30, "Week", 2.0f);

				//Assert
				Assert.IsFalse(result);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void UpdateOvertime_Should_Return_True_If_Succeed()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			AuthTest.createTimeTrackerSetting(orgId, 1);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.UpdateOvertime(8, "Day", 2.0f);

				string selectStmt = "SELECT [OvertimeHours], [OvertimePeriod], [OvertimeMultiplier] FROM [TimeTracker].[Setting] WHERE [OrganizationId] = @id";
				SqlDataReader reader;
				bool updated = false;
				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@id", SqlDbType.Int).Value = orgId;

						// open connection, execute command, close connection
						connection.Open();
						reader = cmd.ExecuteReader();
						while (reader.Read())
						{
							updated = ((int)reader["OvertimeHours"] == 8 && (string)reader["OverTimePeriod"] == "Day" && decimal.Parse(reader["OvertimeMultiplier"].ToString()) == 2);
						}
						reader.Close();
						connection.Close();
					}
				}

				//Assert
				Assert.IsTrue(result && updated);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UpdateLockDate_Should_Throw_Exception_For_Invalid_lockDatePeriod()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.UpdateLockDate(true, "Years", 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UpdateLockDate_Should_Throw_Exception_For_Invalid_lockDateQuantity()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			ttService.UpdateLockDate(true, "Months", -1);
		}

		[TestMethod]
		public void UpdateLockDate_Should_Return_True_If_Succeed()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			AuthTest.createTimeTrackerSetting(orgId, 1);

			UserContext userContext = new UserContext(userId, email, email, orgId, 0, null, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				bool result = ttService.UpdateLockDate(false, "Months", 1);

				string selectStmt = "SELECT [LockDateUsed], [LockDatePeriod], [LockDateQuantity] FROM [TimeTracker].[Setting] WHERE [OrganizationId] = @id";
				SqlDataReader reader;
				bool updated = false;
				using (SqlConnection connection = new SqlConnection(connectionStr))
				{
					using (SqlCommand cmd = new SqlCommand(selectStmt, connection))
					{
						// set up the command's parameters
						cmd.Parameters.Add("@id", SqlDbType.Int).Value = orgId;

						// open connection, execute command, close connection
						connection.Open();
						reader = cmd.ExecuteReader();
						while (reader.Read())
						{
							updated = ((bool)reader["LockDateUsed"] == false && (string)reader["LockDatePeriod"] == "Months" && (int)reader["LockDateQuantity"] == 1);
						}
						reader.Close();
						connection.Close();
					}
				}

				//Assert
				Assert.IsTrue(result && updated);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		public void GetAllSettings_Should_Return_All_Setting_Info()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			AuthTest.createTimeTrackerSetting(orgId, 1);
			int regPayClassId = AuthTest.createPayClass(orgId);

			UserContext userContext = new UserContext(userId, email, email, orgId, 0, null, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			try
			{
				//Act
				Tuple<Setting, List<PayClass>, List<Holiday>> tuple = ttService.GetAllSettings();

				//Assert
				Setting setting = tuple.Item1;
				bool correctSetting = (setting.OrganizationId == orgId && setting.StartOfWeek == 1 && setting.OvertimePeriod == "Week" && setting.OvertimeHours == 40);
				List<PayClass> payclassList = tuple.Item2;
				bool correctPayclassList = (payclassList.Count() == 8 && payclassList.ElementAt(0).PayClassID == regPayClassId);
				List<Holiday> holidayList = tuple.Item3;
				bool correctHoliday = (holidayList.Count() == 16 && holidayList.ElementAt(0).HolidayName == "New Year's Day");

				Assert.IsTrue(correctSetting && correctPayclassList && correctHoliday);
			}
			finally
			{
				//Clean up
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetTimeEntryIndexInfo_Should_Throw_Exception_For_Invalid_UserId()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			DateTime start = new DateTime(2017, 1, 1);
			DateTime end = new DateTime(2017, 1, 31);
			ttService.GetTimeEntryIndexInfo(start, end, -2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetTimeEntryIndexInfo_Should_Throw_Exception_For_Invalid_Date_Range()
		{
			TimeTrackerService ttService = new TimeTrackerService(connectionStr);
			DateTime end = new DateTime(2017, 1, 1);
			DateTime start = new DateTime(2017, 1, 31);
			ttService.GetTimeEntryIndexInfo(start, end, 1);
		}

		[TestMethod]
		public void GetTimeEntryIndexInfo_Should_Return_All_Setting_Info()
		{
			//Arrange
			string email = "test_user@unittestemail.com";
			string orgName = "UnitTestOrg";
			int userId = AuthTest.createTestUser(email);
			int orgId = AuthTest.createTestOrg(orgName);
			AuthTest.createOrgUser(userId, orgId, 1, "111");
			AuthTest.createTimeTrackerSetting(orgId, 1);
			int regPayClassId = AuthTest.createPayClass(orgId);

			int custId = AuthTest.createCustomer("CorporationA", orgId, 1, "CUST001");
			int projId = AuthTest.createProject(custId, "Project1", 1, "PROJ001");
			AuthTest.createProjectUser(projId, userId, 1);
			int subId = AuthTest.createSubscription(orgId, 1, 50);
			AuthTest.createSubUser(subId, userId, 1);
			int timeEntryId = AuthTest.createTimeEntry(userId, projId, new DateTime(2017, 1, 15), regPayClassId, 8);

			UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager);
			subInfo.ProductId = ProductIdEnum.TimeTracker;
			List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
			List<int> projectList = new List<int> { projId };
			UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, projectList);
			List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
			UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
			Service service = new Service(connectionStr, userContext);
			TimeTrackerService ttService = new TimeTrackerService(connectionStr, userContext);
			ttService.SetService(service);

			DateTime start = new DateTime(2017, 1, 1);
			DateTime end = new DateTime(2017, 1, 31);

			try
			{
				//Act
				Tuple<Setting, List<PayClass>, List<Holiday>, List<CompleteProjectInfo>, List<User>, List<TimeEntryInfo>> tuple = ttService.GetTimeEntryIndexInfo(start, end, userId);

				//Assert
				Setting setting = tuple.Item1;
				bool correctSetting = (setting.StartOfWeek == 1 && setting.LockDatePeriod == "Weeks" && setting.LockDateQuantity == 2 && setting.LockDateUsed == true);

				List<PayClass> payclassList = tuple.Item2;
				bool correctPayclassList = (payclassList.Count() == 8 && payclassList.ElementAt(0).PayClassID == regPayClassId);

				List<Holiday> holidayList = tuple.Item3;
				bool correctHoliday = (holidayList.Count() == 16 && holidayList.ElementAt(0).HolidayName == "New Year's Day");

				List<CompleteProjectInfo> projects = tuple.Item4;
				bool correctProjects = (projects.Count() == 2 && projects.ElementAt(1).ProjectId == projId);    //first project returned is the default project

				List<User> users = tuple.Item5;
				bool correctUsers = (users.Count() == 1 && users.ElementAt(0).UserId == userId);

				List<TimeEntryInfo> timeEntries = tuple.Item6;
				bool correctTimeEntries = (timeEntries.Count() == 1 && timeEntries.ElementAt(0).TimeEntryId == timeEntryId);

				Assert.IsTrue(correctSetting && correctPayclassList && correctHoliday && correctProjects && correctUsers && correctTimeEntries);
			}
			finally
			{
				//Clean up
				AuthTest.deleteTimeEntry(timeEntryId);
				AuthTest.deleteSubUser(userId, subId);
				AuthTest.deleteSubscription(subId);
				AuthTest.deleteProjectUserByProjectId(projId);
				AuthTest.deleteProject(projId);
				AuthTest.deleteCustomer(custId);
				AuthTest.deletePayClass(orgId);
				AuthTest.deleteTimeTrackerSetting(orgId);
				AuthTest.deleteOrgUser(userId, orgId);
				AuthTest.deleteTestOrg(orgId);
				AuthTest.deleteTestUser(email);
			}
		}
	}
}
