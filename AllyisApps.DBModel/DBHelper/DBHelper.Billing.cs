//------------------------------------------------------------------------------
// <copyright file="DBHelper.Billing.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Billing;

//using AllyisApps.DBModel.Cache;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// Provides DB access methods relating to organizations.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Retrieves the area to route to for a specific subscription.
		/// </summary>
		/// <param name="subscriptionID">The subscription ID.</param>
		/// <returns>The product area string.</returns>
		public string GetProductAreaBySubscription(int subscriptionID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@subscriptionID", subscriptionID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Billing].[GetProductAreaBySubscription]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Retrieves the Product ID by Product Name.
		/// </summary>
		/// <param name="productName">The name of the product.</param>
		/// <returns>The ID of the product.</returns>
		public int GetProductIDByName(string productName)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@productName", productName);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[Billing].[GetProductIdByName]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Updates the subscriptionUser table entry for a specific user of subscription to be productRoleId.
		/// If no subscriptionUser table entry exists, one will be created.
		/// </summary>
		/// <param name="productRoleId">The id of the product role the user will get.</param>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="userId">The User's Id.</param>
		public void UpdateSubscriptionUserProductRole(int productRoleId, int subscriptionId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ProductRoleId", productRoleId);
			parameters.Add("@subscriptionID", subscriptionId);
			parameters.Add("@UserId", userId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Query("[Billing].[UpdateSubscriptionUserProductRole]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a subscriptionUser.
		/// </summary>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="userId">The user's Id.</param>
		public void DeleteSubscriptionUser(int subscriptionId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@subscriptionID", subscriptionId);
			parameters.Add("@UserId", userId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Query("[Billing].[DeleteSubscriptionUser]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Assigns a time tracker role to a list of users.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="organizationId">The Organization Id.</param>
		/// <param name="timeTrackerRoleId">Time tracker role to assign (or -1 to remove from organization).</param>
		/// <returns>The number of updated/removed users, and the number of newly added users (or -1 if subscription is too full).</returns>
		public Tuple<int, int> EditSubscriptionUsers(List<int> userIds, int organizationId, int timeTrackerRoleId)
		{
			if (timeTrackerRoleId == 0)
			{
				throw new ArgumentException("Role cannot be 0.");
			}

			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds)
			{
				userIdsTable.Rows.Add(userId);
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserIDs", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@OrganizationId", organizationId);
			parameters.Add("@TimeTrackerRole", timeTrackerRoleId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Billing].[EditSubscriptionUsers]",
					parameters,
					commandType: CommandType.StoredProcedure);
				int usersUpdated = results.Read<int>().SingleOrDefault();
				if (usersUpdated == -1)
				{
					return Tuple.Create(-1, 0); // Indicates no subscription to TimeTracker for this organization.
												//throw new InvalidOperationException("No subscription to TimeTracker for this organization.");
				}

				if (timeTrackerRoleId == -1) // If removing from subscription, return only the number of users succesfully removed.
				{
					return Tuple.Create(usersUpdated, 0);
				}

				// If changing roles, return the number of users updated and the number of users added.
				int usersAdded = results.Read<int>().SingleOrDefault(); // Note: this number may be -1, indicating too many users in the subscription to add any.
				return Tuple.Create(usersUpdated, usersAdded);
			}
		}

		/// <summary>
		/// Unsubscribe method.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The name of the Sku for the deleted subscription, or null if none was found.</returns>
		public string Unsubscribe(int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();

			parameters.Add("@subscriptionID", subscriptionId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Billing].[DeleteSubscription]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Get Subscription Details by Id.
		/// </summary>
		/// <param name="subscriptionId"> Subscription Id.</param>
		/// <returns>SubscriptionDBEntity obj.</returns>
		public SubscriptionDBEntity GetSubscriptionDetailsById(int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@subscriptionID", subscriptionId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default blank object
				return connection.Query<SubscriptionDBEntity>("[Billing].[GetSubscriptionDetailsById]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Executes [Billing].[GetProductById].
		/// </summary>
		/// <param name="productID">Sets ProductID.</param>
		/// <returns>ProductDBEntity obj.</returns>
		public ProductDBEntity GetProductById(int productID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ProductId", productID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<ProductDBEntity>("[Billing].[GetProductById]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Executes [Billing].[GetProductList].
		/// </summary>
		/// <returns>List of ProductDBEntity.</returns>
		public List<ProductDBEntity> GetProductList()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default blank list
				return connection.Query<ProductDBEntity>("[Billing].[GetProductList]").ToList();
			}
		}

		/// <summary>
		/// Executes [Billing].[UpdateSubscription].
		/// </summary>
		/// <param name="organizationId">Sets OrganizationId.</param>
		/// <param name="skuId">Set to 0 for unsubscribe.</param>
		/// <param name="productId"> Param @productId. </param>
		/// <param name="numberOfUsers">Number of the users the subscription.</param>
		/// <param name="subscriptionName">The subscription name</param>
		/// <returns>The ret ID.</returns>
		public int ChangeSubscription(int organizationId, int skuId, int productId, int numberOfUsers, string subscriptionName)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationId);
			parameters.Add("@SkuId", skuId);
			parameters.Add("@NumberOfUsers", numberOfUsers);
			parameters.Add("@SubscriptionName", subscriptionName);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);
			/*if (skuId == 0)
			{
				parameters.Add("@ProductId", productId);
			}
			else
			{ */
			parameters.Add("@ProductId", productId);
			////}

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Billing].[UpdateSubscription]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@retId");
		}

		/// <summary>
		/// Executes [Billing].[UpdateNumberOfUsersSubscription].
		/// </summary>
		/// <param name="organizationId">Sets OrganizationId.</param>
		/// <param name="skuId">Set to 0 for unsubscribe.</param>
		/// <param name="numberOfUsers">Number of the users the subscription.</param>
		public void UpdateSubscriptionUsers(int organizationId, int skuId, int numberOfUsers)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationId);
			parameters.Add("@SkuId", skuId);
			parameters.Add("@NumberOfUsers", numberOfUsers);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Billing].[UpdateNumberOfUsersSubscription]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes [Billing].[GetSkuById].
		/// </summary>
		/// <param name="skuId">Sets SkuId.</param>
		/// <returns>SkuDBEntity obj.</returns>
		public SkuDBEntity GetSkuDetails(int skuId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@SkuId", skuId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty object
				return connection.Query<SkuDBEntity>("[Billing].[GetSkuById]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Adds an entry to the customer subscription table, and adds a billing history item.
		/// </summary>
		/// <param name="stripeTokenCustId">The id of the stripe customer.</param>
		/// <param name="stripeTokenSubId">The id of the stripe subscription.</param>
		/// <param name="price">The price of the subscription.</param>
		/// <param name="numberOfUsers">The number of users for the subscription.</param>
		/// <param name="productID">The id of the subscription product.</param>
		/// <param name="organizationID">The id of the organization that the subscription belongs to.</param>
		/// <param name="userId">The id of the user adding the subscription plan.</param>
		/// <param name="skuId">The selected sku id, for the billing history item.</param>
		/// <param name="description">A description for the billing history item.</param>
		/// <returns>Subscription ID.</returns>
		public void AddCustomerSubscription(string stripeTokenCustId, string stripeTokenSubId, int price, int numberOfUsers, int productID, int organizationID, int userId, int? skuId, string description)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationID);
			parameters.Add("@stripeTokenCustId", stripeTokenCustId);
			parameters.Add("@stripeTokenSubId", stripeTokenSubId);
			parameters.Add("@Price", price);
			parameters.Add("@NumberOfUsers", numberOfUsers);
			parameters.Add("@ProductId", productID);
			parameters.Add("@UserId", productID);
			parameters.Add("@SkuId", productID);
			parameters.Add("@Description", productID);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Query(
				   "[Billing].[CreateSubscriptionPlan]",
				   parameters,
				   commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Adds an entry to the organization customer table, and adds a billing history item.
		/// </summary>
		/// <param name="orgId">The id of the organization.</param>
		/// <param name="userId">The id of the user adding the billing customer.</param>
		/// <param name="customerId">The id of the customer object.</param>
		/// <param name="skuId">The id of the selected sku, for the billing history item.</param>
		/// <param name="description">A description for the billing history item.</param>
		public void AddOrgCustomer(int orgId, int userId, string customerId, int? skuId, string description)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", orgId);
			parameters.Add("@UserId", userId);
			parameters.Add("@customerID", customerId);
			parameters.Add("@SkuId", skuId);
			parameters.Add("@Description", description);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Query(
				   "[Billing].[CreateStripeOrgCustomer]",
				   parameters,
				   commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the customer id for the organization.
		/// </summary>
		/// <param name="orgId">The id of the organization.</param>
		/// <returns>The Organization custormer.</returns>
		public string GetOrgCustomer(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", orgId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				return connection.Query<string>(
				   "[Billing].[GetStripeOrgCustomer]",
				   parameters,
				   commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Gets the subscription id for the organization.
		/// </summary>
		/// <param name="orgid">The id of the organization.</param>
		/// <param name="customerid">The id of the customer.</param>
		/// <returns>The subscription plan.</returns>
		public string GetSubscriptionPlan(int orgid, string customerid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", orgid);
			parameters.Add("@customerID", customerid);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				return connection.Query<string>(
				   "[Billing].[GetSubscriptionPlan]",
				   parameters,
				   commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Updates the subscription plan for the organization, and adds a billing history item.
		/// </summary>
		/// <param name="customerID">The id of the customer.</param>
		/// <param name="subscriptionPlanID">The id of the subscription plan to be updated.</param>
		/// <param name="price">The price of the subscription.</param>
		/// <param name="numberOfUsers">The number of users for the subscription.</param>
		/// <param name="orgId">The organization id, for the billing history item.</param>
		/// <param name="userId">The id of the user updating the subscription plan, for the billing history item.</param>
		/// <param name="skuId">The selected sku id, for the billing history item.</param>
		/// <param name="description">A description for the billing history item.</param>
		/// <returns>The subscription ID.</returns>
		public void UpdateSubscriptionPlan(string customerID, string subscriptionPlanID, int price, int numberOfUsers, int orgId, int userId, int? skuId, string description)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerID", customerID);
			parameters.Add("@SubPlanId", subscriptionPlanID);
			parameters.Add("@NumberOfUsers", numberOfUsers);
			parameters.Add("@Price", price);
			parameters.Add("@OrganizationId", orgId);
			parameters.Add("@UserId", userId);
			parameters.Add("@SkuId", skuId);
			parameters.Add("@Description", description);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Query(
				   "[Billing].[UpdateCustomerSubscription]",
				   parameters,
				   commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Looks for a subscription plan with the given organization Id and stripe customer Id. If found,
		/// deletes it and adds a history item using the given user Id, skuId, and description.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="customerId">Customer Id.</param>
		/// <param name="userId">User Id for the history item.</param>
		/// <param name="skuId">Sku Id for the history item.</param>
		/// <param name="description">Description for the history item.</param>
		/// <returns>The subscription plan id of the delted subscription, or null if none found.</returns>
		public string DeleteSubscriptionPlanAndAddHistory(int orgId, string customerId, int userId, int? skuId, string description)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", orgId);
			parameters.Add("@customerID", customerId);
			parameters.Add("@UserId", userId);
			parameters.Add("@SkuId", skuId);
			parameters.Add("@Description", description);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>(
				   "[Billing].[DeleteSubPlanAndAddHistory]",
				   parameters,
				   commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Deletes a customer subscription.
		/// </summary>
		/// <param name="subscriptionid">The id of the subscription plan to be deleted.</param>
		/// <returns>The deleted subscription plan ID, or -1 on failure.</returns>
		public string DeleteSubscriptionPlan(string subscriptionid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@stripeTokenSubId", subscriptionid);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				return connection.Query<string>(
				   "[Billing].[DeleteCustomerSubscription]",
				   parameters,
				   commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Adds entry to billing history table.
		/// </summary>
		/// <param name="description">What is happening.</param>
		/// <param name="orgid">The associated organization id.</param>
		/// <param name="userid">The associated user id.</param>
		/// <param name="skuid">Optional product id.</param>
		public void AddBillingHistory(string description, int orgid, int userid, int? skuid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@Description", description);
			parameters.Add("@OrganizationId", orgid);
			parameters.Add("@UserId", userid);
			parameters.Add("@SkuId", skuid);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Query<int>(
				  "[Billing].[CreateBillingHistory]",
				  parameters,
				  commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Removes billing information for an organization.
		/// </summary>
		/// <param name="orgid">The id of the organization to remove the billing information for.</param>
		public void RemoveBilling(int orgid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", orgid);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Query<int>(
				  "[Billing].[DeleteBillingInformation]",
				  parameters,
				  commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Executes [Billing].[GetSubscriptionPlanPrices].
		/// </summary>
		/// <param name="orgid">Sets productID.</param>
		/// <returns>List of prices.</returns>
		public IEnumerable<int> GetSubscriptionPlanPrices(int orgid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", orgid);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<int>(
				  "[Billing].[GetSubscriptionPlanPricesByOrg]",
				  parameters,
				  commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the collection of subscriptions for the specified organization.
		/// </summary>
		/// <param name="organizationId">The organization's ID.</param>
		/// <returns>The collection of OrganizationSubscription objects for this org.</returns>
		public IEnumerable<SubscriptionDBEntity> GetSubscriptionDetails(int organizationId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<SubscriptionDBEntity>("[Billing].[GetOrgSkus]", new { OrganizationId = organizationId }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes [Billing].[GetSubscriptionsDisplayByOrg].
		/// </summary>
		/// <param name="organizationId">Sets OrganizationId.</param>
		/// <returns>List of SubscriptionDisplay's.</returns>
		public IEnumerable<SubscriptionDisplayDBEntity> GetSubscriptionsDisplayByOrg(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<SubscriptionDisplayDBEntity>("[Billing].[GetSubscriptionsDisplayByOrg]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes Billing.GetProductRolesFromSubscription.
		/// </summary>
		/// <param name="subscriptionId">Sets SubscriptionId.</param>
		/// <returns>List of SubscriptionRole.</returns>
		public IEnumerable<SubscriptionRoleDBEntity> GetProductRolesFromSubscription(int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@SubscriptionId", subscriptionId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<SubscriptionRoleDBEntity>("[Billing].[GetProductRolesFromSubscription]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes Billing.GetBillingHistoryByOrg.
		/// </summary>
		/// <param name="orgID">Organization ID.</param>
		/// <returns>List of BillingHistoryItem.</returns>
		public IEnumerable<BillingHistoryItemDBEntity> GetBillingHistoryByOrg(int orgID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", orgID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<BillingHistoryItemDBEntity>("[Billing].[GetBillingHistoryByOrg]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Returns a ProductDBEntity for the product that the given SKU belongs to, a SubscriptionDBEntity for the given org's
		/// subscription to that product (or null if none), a list of SkuDBEntities for all the skus for
		/// that product, the Stripe billing token for the given org (or null if none), and the total
		/// number of users in the org with roles in the subscription for the product.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="skuId">Product Id.</param>
		/// <returns></returns>
		public Tuple<ProductDBEntity, SubscriptionDBEntity, List<SkuDBEntity>, string, int> GetProductSubscriptionInfo(int orgId, int skuId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			parameters.Add("@skuId", skuId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Billing].[GetProductSubscriptionInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<ProductDBEntity>().SingleOrDefault(),
					results.Read<SubscriptionDBEntity>().SingleOrDefault(),
					results.Read<SkuDBEntity>().ToList(),
					results.Read<string>().SingleOrDefault(),
					results.Read<int>().SingleOrDefault());
			}
		}

		/// <summary>
		/// Returns a list of active products and each product's active skus
		/// </summary>
		public Tuple<List<ProductDBEntity>, List<SkuDBEntity>> GetAllActiveProductsAndSkus()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[Billing].[GetAllActiveProductsAndSkus]",
					commandType: CommandType.StoredProcedure);
				return Tuple.Create(
					results.Read<ProductDBEntity>().ToList(),
					results.Read<SkuDBEntity>().ToList());
			}
		}
	}
}
