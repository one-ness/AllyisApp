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
    public class CrmTest
    {
        public static string connectionStr = "Data Source=(local);Initial Catalog=AllyisAppsDB;User Id=aaUser;Password=BlueSky23#;";

        [TestMethod]
        public void CreateCustomer_Should_Return_Null_If_Authorization_Fails()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser); //TT user cannot edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            Customer customer = new Customer { Name = "CorpA", CustomerOrgId = "CUST1", OrganizationId = orgId };

            try
            {
                //Act
                var result = ttService.CreateCustomer(customer);

                //Assert
                Assert.IsTrue(result == null);
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
        public void CreateCustomer_Should_Return_Negative_One_If_CustomerOrgId_Already_Exists()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);
            int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager); //TT manager can edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            Customer customer = new Customer { Name = "CorpA1", CustomerOrgId = "CUST1", OrganizationId = orgId };

            try
            {
                //Act
                var result = ttService.CreateCustomer(customer);

                //Assert
                Assert.IsTrue(result == -1);
            }
            finally
            {
                //Clean up
                AuthTest.deleteSubUserBySubId(subId);
                AuthTest.deleteSubscription(subId);
                AuthTest.deleteCustomer(custId);
                AuthTest.deleteTestOrg(orgId);
                AuthTest.deleteTestUser(email);
            }
        }

        [TestMethod]
        public void CreateCustomer_Should_Return_CustomerId_If_Succeed()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager); //TT manager can edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            Customer customer = new Customer { Name = "CorpA1", CustomerOrgId = "CUST1", OrganizationId = orgId };
            int custId = -1;
            try
            {
                //Act
                var result = ttService.CreateCustomer(customer);
                if (result.HasValue) { custId = result.Value; }

                //Assert
                Assert.IsTrue(custId > 0);
            }
            finally
            {
                //Clean up
                AuthTest.deleteSubUserBySubId(subId);
                AuthTest.deleteSubscription(subId);
                AuthTest.deleteCustomer(custId);
                AuthTest.deleteTestOrg(orgId);
                AuthTest.deleteTestUser(email);
            }
        }

        [TestMethod]
        public void DeleteCustomer_Should_Return_Null_If_Authorization_Fails()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);
            int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser); //TT user cannot edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            try
            {
                //Act
                string result = ttService.DeleteCustomer(custId);

                //Assert
                Assert.IsTrue(result == null);
            }
            finally
            {
                //Clean up
                AuthTest.deleteSubUserBySubId(subId);
                AuthTest.deleteSubscription(subId);
                AuthTest.deleteCustomer(custId);
                AuthTest.deleteTestOrg(orgId);
                AuthTest.deleteTestUser(email);
            }
        }

        [TestMethod]
        public void DeleteCustomer_Should_Return_Empty_String_If_Customer_Not_Found()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);
            int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
            AuthTest.deleteCustomer(custId);    //no customer with custId exists in db

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager); // TT manager can edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            try
            {
                //Act
                string result = ttService.DeleteCustomer(custId);

                //Assert
                Assert.IsTrue(result == "");
            }
            finally
            {
                //Clean up
                AuthTest.deleteSubUserBySubId(subId);
                AuthTest.deleteSubscription(subId);
                AuthTest.deleteCustomer(custId);
                AuthTest.deleteTestOrg(orgId);
                AuthTest.deleteTestUser(email);
            }
        }

        [TestMethod]
        public void DeleteCustomer_Should_Return_Customers_Name_If_Succeed()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);
            int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager); // TT manager can edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Owner, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            try
            {
                //Act
                string result = ttService.DeleteCustomer(custId);

                //Assert
                Assert.IsTrue(result == "CorpA");
            }
            finally
            {
                //Clean up
                AuthTest.deleteSubUserBySubId(subId);
                AuthTest.deleteSubscription(subId);
                AuthTest.deleteCustomer(custId);
                AuthTest.deleteTestOrg(orgId);
                AuthTest.deleteTestUser(email);
            }
        }

        [TestMethod]
        public void UpdateCustomer_Should_Return_Null_If_Authorization_Fails()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);
            int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerUser); //TT user cannot edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            Customer customer = new Customer { CustomerId = custId, Name = "CorpA_update", CustomerOrgId = "CUST1", OrganizationId = orgId };

            try
            {
                //Act
                var result = ttService.UpdateCustomer(customer);

                //Assert
                Assert.IsTrue(result == null);
            }
            finally
            {
                //Clean up
                AuthTest.deleteSubUserBySubId(subId);
                AuthTest.deleteSubscription(subId);
                AuthTest.deleteCustomer(custId);
                AuthTest.deleteTestOrg(orgId);
                AuthTest.deleteTestUser(email);
            }
        }

        [TestMethod]
        public void UpdateCustomer_Should_Return_Negative_One_If_CustOrgId_Is_Taken()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);
            int custId1 = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");
            int custId2 = AuthTest.createCustomer("CorpB", orgId, 1, "CUST2");

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager); //TT manager can edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            Customer customer = new Customer { CustomerId = custId2, Name = "CorpB_update", CustomerOrgId = "CUST1", OrganizationId = orgId };

            try
            {
                //Act
                var result = ttService.UpdateCustomer(customer);

                //Assert
                Assert.IsTrue(result == -1);
            }
            finally
            {
                //Clean up
                AuthTest.deleteSubUserBySubId(subId);
                AuthTest.deleteSubscription(subId);
                AuthTest.deleteCustomer(custId1);
                AuthTest.deleteCustomer(custId2);
                AuthTest.deleteTestOrg(orgId);
                AuthTest.deleteTestUser(email);
            }
        }

        [TestMethod]
        public void UpdateCustomer_Should_Return_One_If_Succeed()
        {
            //Arrange
            string email = "test_user@unittestemail.com";
            string orgName = "UnitTestOrg";
            int userId = AuthTest.createTestUser(email);
            int orgId = AuthTest.createTestOrg(orgName);
            int custId = AuthTest.createCustomer("CorpA", orgId, 1, "CUST1");

            int subId = AuthTest.createSubscription(orgId, 1, 50);
            AuthTest.createSubUser(subId, userId, 1);   //user is a User of the subscription
            UserSubscriptionInfo subInfo = new UserSubscriptionInfo(subId, 1, "Time Tracker", ProductRoleIdEnum.TimeTrackerManager); //TT manager can edit customer
            subInfo.ProductId = ProductIdEnum.TimeTracker;
            List<UserSubscriptionInfo> subInfoList = new List<UserSubscriptionInfo> { subInfo };
            UserOrganizationInfo userOrgInfo = new UserOrganizationInfo(orgId, orgName, OrganizationRole.Member, subInfoList, null);
            List<UserOrganizationInfo> infoList = new List<UserOrganizationInfo> { userOrgInfo };
            UserContext userContext = new UserContext(userId, email, email, orgId, subId, infoList, 1);
            AppService ttService = new AppService(connectionStr, userContext);

            Customer customer = new Customer { CustomerId = custId, Name = "CorpB_update", CustomerOrgId = "CUST1", OrganizationId = orgId };

            try
            {
                //Act
                var result = ttService.UpdateCustomer(customer);

                //Assert
                Assert.IsTrue(result == 1);
            }
            finally
            {
                //Clean up
                AuthTest.deleteSubUserBySubId(subId);
                AuthTest.deleteSubscription(subId);
                AuthTest.deleteCustomer(custId);
                AuthTest.deleteTestOrg(orgId);
                AuthTest.deleteTestUser(email);
            }
        }
    }
}
