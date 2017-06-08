﻿using AllyisApps.Services.TimeTracker;
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
    }
}
