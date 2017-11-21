//------------------------------------------------------------------------------
// <copyright file="DBHelper.Billing.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Billing;
using Dapper;

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
		/// <param name="subscriptionId">The subscription Id.</param>
		/// <returns>The product area string.</returns>
		public async Task<string> GetProductAreaBySubscription(int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@subscriptionId", subscriptionId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<string>("[Billing].[GetProductAreaBySubscription]", parameters, commandType: CommandType.StoredProcedure);
				return results.FirstOrDefault();
			}
		}

		/// <summary>
		/// Retrieves the Product Id by Product Name.
		/// </summary>
		/// <param name="productName">The name of the product.</param>
		/// <returns>The Id of the product.</returns>
		public int GetProductIdByName(string productName)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@productName", productName);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
		public async Task UpdateSubscriptionUserProductRole(int productRoleId, int subscriptionId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@productRoleId", productRoleId);
			parameters.Add("@subscriptionId", subscriptionId);
			parameters.Add("@userId", userId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Billing].[UpdateSubscriptionUserProductRole]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a subscriptionUser.
		/// </summary>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <param name="userId">The user's Id.</param>
		public async Task DeleteSubscriptionUser(int subscriptionId, int userId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@subscriptionId", subscriptionId);
			parameters.Add("@userId", userId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Billing].[DeleteSubscriptionUser]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Assigns a time tracker role to a list of users.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="organizationId">The Organization Id.</param>
		/// <param name="productRoleId">Product role to assign (or -1 to remove from organization).</param>
		/// <param name="productId">ID of Product in question.</param>
		/// <returns>The number of updated and number of added users.</returns>
		public async Task<Tuple<int, int>> UpdateSubscriptionUserRoles(List<int> userIds, int organizationId, int productRoleId, int productId)
		{
			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds) { userIdsTable.Rows.Add(userId); }

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userIds", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@productRoleId", productRoleId);

			// TODO: instead of providing product id, provide subscription id of the subscription to be modified
			parameters.Add("@productId", productId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// TODO: split updating user roles and creating new sub users
				var results = await connection.QueryMultipleAsync("[Billing].[UpdateSubscriptionUserRoles]", parameters, commandType: CommandType.StoredProcedure);
				int usersUpdated = results.Read<int>().SingleOrDefault();
				int usersAdded = results.Read<int>().SingleOrDefault();
				return Tuple.Create(usersUpdated, usersAdded);
			}
		}



		/// <summary>Deletes the given users in the given organization's subscription</summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="organizationId">The Organization Id.</param>
		/// <param name="productId">ID of Product in question.</param>
		/// <returns>count of deleted users.</returns>
		public void DeleteSubscriptionUsers(List<int> userIds, int organizationId, int productId)
		{
			DataTable userIdsTable = new DataTable();
			userIdsTable.Columns.Add("userId", typeof(int));
			foreach (int userId in userIds) { userIdsTable.Rows.Add(userId); }

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@userIds", userIdsTable.AsTableValuedParameter("[Auth].[UserTable]"));
			parameters.Add("@organizationId", organizationId);

			// TODO: instead of providing product id, provide subscription id of the subscription to be modified
			parameters.Add("@productId", productId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[Billing].[DeleteSubscriptionUsers]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Unsubscribe method.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The name of the Sku for the deleted subscription, or null if none was found.</returns>
		public async Task<string> Unsubscribe(int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();

			parameters.Add("@subscriptionId", subscriptionId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<string>("[Billing].[DeleteSubscription]", parameters, commandType: CommandType.StoredProcedure);
				return results.FirstOrDefault();
			}
		}

		/// <summary>
		/// Get Subscription Details by Id.
		/// </summary>
		public async Task<dynamic> GetSubscriptionDetailsById(int subscriptionId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				// default blank object
				var results = await con.QueryAsync<dynamic>("[Billing].[GetSubscriptionDetailsById] @a", new { a = subscriptionId });
				return results.FirstOrDefault();
			}
		}

		/// <summary>
		/// get the list of subscriptions for the given organization
		/// </summary>
		public async Task<dynamic> GetSubscriptionsAsync(int orgId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return (await con.QueryAsync<dynamic>("Billing.GetSubscriptions @a", new { a = orgId })).ToList();
			}
		}

		/// <summary>
		/// Returns subscription users for a particlar subscription
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public IEnumerable<SubscriptionUserDBEntity> GetSubscriptionUsersBySubscriptionId(int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@subscriptionId", subscriptionId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default blank object
				return connection.Query<SubscriptionUserDBEntity>("[Billing].[GetSubscriptionUsersBySubscriptionId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Get just subscription Name by subscriptionid.
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public async Task<string> GetSubscriptionName(int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@subscriptionId", subscriptionId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<string>("[Billing].[GetSubscriptionName]", parameters, commandType: CommandType.StoredProcedure);
				return results.SingleOrDefault();
			}
		}

		/// <summary>
		/// Executes [Billing].[GetProductById].
		/// </summary>
		/// <param name="productId">Sets ProductId.</param>
		/// <returns>ProductDBEntity obj.</returns>
		public ProductDBEntity GetProductById(int productId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@productId", productId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default blank list
				return connection.Query<ProductDBEntity>("[Billing].[GetProductList]").ToList();
			}
		}

		/// <summary>
		/// Updates subscription:
		///  - changes the subscription name.
		/// </summary>
		public void UpdateSubscriptionName(int subscriptionId, string subscriptionName)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				con.Execute("Billing.UpdateSubscriptionName @a, @b", new { a = subscriptionId, b = subscriptionName });
			}
		}

		/// <summary>
		/// Creates subscription
		/// Adds all the organization users as users to the subscription
		/// Adds the user who subscribed to the subscription as a manager.
		/// </summary>
		/// <param name="organizationId">Sets OrganizationId.</param>
		/// <param name="skuId">Sku -- the subscription item you're subscribing to.</param>
		/// <param name="subscriptionName">The subscription name.</param>
		/// <param name="userId">The user who is subscribing -- we need to make them manager.</param>
		/// <param name="productRoleId">product role</param>
		/// <returns>The new subscription id.</returns>
		public async Task<int> CreateSubscription(int organizationId, int skuId, string subscriptionName, int userId, int productRoleId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@skuId", skuId);
			parameters.Add("@subscriptionName", subscriptionName);
			parameters.Add("@userId", userId);
			parameters.Add("@productRoleId", productRoleId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<int>("[Billing].[CreateSubscription]", parameters, commandType: CommandType.StoredProcedure);
				return results.SingleOrDefault();
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
			parameters.Add("@skuId", skuId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default empty object
				return connection.Query<SkuDBEntity>("[Billing].[GetSkuById]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// get all skus in the system
		/// </summary>
		public List<SkuDBEntity> GetAllSkus()
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return con.Query<SkuDBEntity>("Billing.GetAllSkus").ToList();
			}
		}

		/// <summary>
		/// Adds an entry to the customer subscription table, and adds a billing history item.
		/// </summary>
		/// <param name="stripeTokenCustId">The id of the stripe customer.</param>
		/// <param name="stripeTokenSubId">The id of the stripe subscription.</param>
		/// <param name="price">The price of the subscription.</param>
		/// <param name="numberOfUsers">The number of users for the subscription.</param>
		/// <param name="productId">The id of the subscription product.</param>
		/// <param name="organizationId">The id of the organization that the subscription belongs to.</param>
		/// <param name="userId">The id of the user adding the subscription plan.</param>
		/// <param name="skuId">The selected sku id, for the billing history item.</param>
		/// <param name="description">A description for the billing history item.</param>
		/// <returns>Subscription Id.</returns>
		public void AddCustomerSubscription(string stripeTokenCustId, string stripeTokenSubId, int price, int numberOfUsers, int productId, int organizationId, int userId, int? skuId, string description)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@stripeTokenCustId", stripeTokenCustId);
			parameters.Add("@stripeTokenSubId", stripeTokenSubId);
			parameters.Add("@price", price);
			parameters.Add("@numberOfUsers", numberOfUsers);
			parameters.Add("@productId", productId);
			parameters.Add("@userId", productId);
			parameters.Add("@skuId", productId);
			parameters.Add("@description", productId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[Billing].[CreateSubscriptionPlan]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds an entry to the organization customer table, and adds a billing history item.
		/// </summary>
		/// <param name="organizationId">The id of the organization.</param>
		/// <param name="userId">The id of the user adding the billing customer.</param>
		/// <param name="customerId">The id of the customer object.</param>
		/// <param name="skuId">The id of the selected sku, for the billing history item.</param>
		/// <param name="description">A description for the billing history item.</param>
		public async Task CreateStripeOrganizationCustomer(int organizationId, int userId, string customerId, int? skuId, string description)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@userId", userId);
			parameters.Add("@customerId", customerId);
			parameters.Add("@skuId", skuId);
			parameters.Add("@description", description);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.QueryAsync("[Billing].[CreateStripeOrganizationCustomer]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets the customer id for the organization.
		/// </summary>
		/// <param name="orgId">The id of the organization.</param>
		/// <returns>The Organization custormer.</returns>
		public async Task<string> GetOrgCustomer(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default -1
				var queryResults = await connection.QueryAsync<string>(
				   "[Billing].[GetStripeOrgCustomer]",
				   parameters,
				   commandType: CommandType.StoredProcedure);
				return queryResults.SingleOrDefault();
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
			parameters.Add("@organizationId", orgid);
			parameters.Add("@customerId", customerid);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
		/// <param name="customerId">The id of the customer.</param>
		/// <param name="subscriptionPlanId">The id of the subscription plan to be updated.</param>
		/// <param name="price">The price of the subscription.</param>
		/// <param name="numberOfUsers">The number of users for the subscription.</param>
		/// <param name="orgId">The organization id, for the billing history item.</param>
		/// <param name="userId">The id of the user updating the subscription plan, for the billing history item.</param>
		/// <param name="skuId">The selected sku id, for the billing history item.</param>
		/// <param name="description">A description for the billing history item.</param>
		/// <returns>The subscription Id.</returns>
		public void UpdateSubscriptionPlan(string customerId, string subscriptionPlanId, int price, int numberOfUsers, int orgId, int userId, int? skuId, string description)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerId", customerId);
			parameters.Add("@subPlanId", subscriptionPlanId);
			parameters.Add("@numberOfUsers", numberOfUsers);
			parameters.Add("@price", price);
			parameters.Add("@organizationId", orgId);
			parameters.Add("@userId", userId);
			parameters.Add("@skuId", skuId);
			parameters.Add("@description", description);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Query(
				   "[Billing].[UpdateCustomerSubscription]",
				   parameters,
				   commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// updates the sku and name for the given subscription
		/// </summary>
		public async void UpdateSubscriptionSkuAndName(int subscriptionId, string subscriptionName, int skuId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				await con.ExecuteAsync("Billing.UpdateSubscriptionSkuAndName @a, @b, @c", new { a = subscriptionId, b = subscriptionName, c = skuId });
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
		public async Task<string> DeleteSubscriptionPlanAndAddHistory(int orgId, string customerId, int userId, int? skuId, string description)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);
			parameters.Add("@customerId", customerId);
			parameters.Add("@userId", userId);
			parameters.Add("@skuId", skuId);
			parameters.Add("@description", description);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = await connection.QueryAsync<string>(
				   "[Billing].[DeleteSubPlanAndAddHistory]",
				   parameters,
				   commandType: CommandType.StoredProcedure);
				return results.FirstOrDefault();
			}
		}

		/// <summary>
		/// Sets subscription IsActive to false.
		/// </summary>
		public async void DeactivateSubscription(int subscriptionid)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				await con.ExecuteAsync("[Billing].[DeactivateSubscription]", new { subscriptionid }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds entry to billing history table.
		/// </summary>
		/// <param name="description">What is happening.</param>
		/// <param name="orgid">The associated organization id.</param>
		/// <param name="userid">The associated user id.</param>
		/// <param name="skuid">Optional product id.</param>
		public async Task AddBillingHistory(string description, int orgid, int userid, int? skuid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@description", description);
			parameters.Add("@organizationId", orgid);
			parameters.Add("@userId", userid);
			parameters.Add("@skuId", skuid);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				await connection.ExecuteAsync("[Billing].[CreateBillingHistory]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Removes billing information for an organization.
		/// </summary>
		/// <param name="orgid">The id of the organization to remove the billing information for.</param>
		public async void RemoveBilling(int orgid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgid);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default -1
				var results = await connection.QueryAsync<int>(
				  "[Billing].[DeleteBillingInformation]",
				  parameters,
				  commandType: CommandType.StoredProcedure);
				results.SingleOrDefault();
			}
		}

		/// <summary>
		/// Executes [Billing].[GetSubscriptionPlanPrices].
		/// </summary>
		/// <param name="orgid">Sets productId.</param>
		/// <returns>List of prices.</returns>
		public async Task<IEnumerable<int>> GetSubscriptionPlanPrices(int orgid)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgid);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default empty list
				var results = await connection.QueryAsync<int>(
				  "[Billing].[GetSubscriptionPlanPricesByOrg]",
				  parameters,
				  commandType: CommandType.StoredProcedure);
				return results;
			}
		}

		/// <summary>
		/// Retrieves the collection of subscriptions for the specified organization.
		/// </summary>
		/// <param name="organizationId">The organization's Id.</param>
		/// <returns>The collection of OrganizationSubscription objects for this org.</returns>
		public IEnumerable<SubscriptionDBEntity> GetSubscriptionDetails(int organizationId)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
		public async Task<IEnumerable<SubscriptionDisplayDBEntity>> GetSubscriptionsDisplayByOrg(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default empty list
				return await connection.QueryAsync<SubscriptionDisplayDBEntity>("[Billing].[GetSubscriptionsDisplayByOrg]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes Billing.GetBillingHistoryByOrg.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>List of BillingHistoryItem.</returns>
		public async Task<IEnumerable<BillingHistoryItemDBEntity>> GetBillingHistoryByOrg(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return await connection.QueryAsync<BillingHistoryItemDBEntity>("[Billing].[GetBillingHistoryByOrg]", parameters, commandType: CommandType.StoredProcedure);
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
		/// <returns>.</returns>
		public Tuple<ProductDBEntity, SubscriptionDBEntity, List<SkuDBEntity>, string, int> GetProductSubscriptionInfo(int orgId, int skuId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);
			parameters.Add("@skuId", skuId);
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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
		/// Returns a list of active products and each product's active skus.
		/// </summary>
		public Tuple<List<ProductDBEntity>, List<SkuDBEntity>> GetAllActiveProductsAndSkus()
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
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