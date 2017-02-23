//------------------------------------------------------------------------------
// <copyright file="DBHelper.Billing.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Cache;
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
		///// <summary>
		///// Product ID List.
		///// </summary>
		//private IEnumerable<int> productIDList;

		///// <summary>
		///// Gets a list of available product IDs.
		///// </summary>
		//public IEnumerable<int> ProductIDList
		//{
		//	get
		//	{
		//		if (this.productIDList == null)
		//		{
		//			this.InitializeProducts();
		//		}

		//		return this.productIDList;
		//	}
		//}

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
		/// Gets a list of all subscription users.
		/// </summary>
		/// <returns>List of SubscriptionUserDBEntities.</returns>
		public List<SubscriptionUserDBEntity> GetSubscriptionUserList()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<SubscriptionUserDBEntity>("[Billing].[GetSubscriptionUserList]", commandType: CommandType.StoredProcedure).ToList();
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
		/// Executes [Billing].[GetSubscription].
		/// </summary>
		/// <param name="organizationId">Sets organizationId.</param>
		/// <param name="productId">Sets productId.</param>
		/// <returns>Requested Organization.</returns>
		public SubscriptionDBEntity CheckSubscription(int organizationId, int productId)
		{
			DynamicParameters parameters = new DynamicParameters();

			parameters.Add("@OrganizationId", organizationId);
			parameters.Add("@ProductId", productId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<SubscriptionDBEntity>("[Billing].[GetSubscription]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Unsubscribe method.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		public void Unsubscribe(int subscriptionId)
		{
			DynamicParameters parameters = new DynamicParameters();

			parameters.Add("@subscriptionID", subscriptionId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[Billing].[DeleteSubscription]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		///// <summary>
		///// Get subscription by User.
		///// </summary>
		///// <param name="userId">The UserId.</param>
		///// <param name="organizationId">The organizationId.</param>
		///// <returns>List of TableSubscriptions.</returns>
		//public IEnumerable<SubscriptionDBEntity> GetSubscriptionByUser(int userId, int organizationId = -1)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@UserId", userId);
		//	if (organizationId != -1)
		//	{
		//		parameters.Add("@OrganizationId", organizationId);
		//	}

		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		// default empty list
		//		return connection.Query<SubscriptionDBEntity>("[Billing].[GetSubscriptionByUser]", parameters, commandType: CommandType.StoredProcedure);
		//	}
		//}

		/// <summary>
		/// Gets the date that the user joined the subscription.
		/// </summary>
		/// <param name="userId">The user in question.</param>
		/// <param name="subId">The subscription in question.</param>
		/// <returns>The date that the user was added to the subscription.</returns>
		public DateTime GetDateAddedToSubscriptionByUserId(int userId, int subId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@UserId", userId);
			parameters.Add("@subscriptionID", subId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				IEnumerable<SubscriptionUserDBEntity> result = connection.Query<SubscriptionUserDBEntity>(
					"[Billing].[GetDateAddedToSubscriptionByUserId]",
					parameters,
					commandType: CommandType.StoredProcedure);
				return result == null ? DateTime.MinValue : result.Count() == 0 ? DateTime.MinValue : result.Single().CreatedUTC;
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

		///// <summary>
		///// Executes [Billing].[GetSubscriptionsByOrg].
		///// </summary>
		///// <param name="organizationId">Sets OrganizationId.</param>
		///// <returns>List of SkuDBEntity's.</returns>
		//public IEnumerable<SkuDBEntity> GetSubscriptionsByOrg(int organizationId)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@OrganizationId", organizationId);
		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		// default empty list
		//		return connection.Query<SkuDBEntity>("[Billing].[GetSubscriptionsByOrg]", parameters, commandType: CommandType.StoredProcedure);
		//	}
		//}

		/// <summary>
		/// Executes [Billing].[GetSubscriptionByOrgAndProduct].
		/// </summary>
		/// <param name="organizationId">Sets OrganizationId.</param>
		/// <param name="productId">Sets ProductId.</param>
		/// <returns>List of SkuDBEntity's.</returns>
		public SkuDBEntity GetSubscriptionByOrgAndProduct(int organizationId, int productId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationId);
			parameters.Add("@ProductId", productId);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<SkuDBEntity>("[Billing].[GetSubscriptionByOrgAndProduct]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Executes [Billing].[GetSkuforProduct].
		/// </summary>
		/// <param name="productID">Sets ProductID.</param>
		/// <returns>List of SkuDBEntity's.</returns>
		public IEnumerable<SkuDBEntity> GetSkuforProduct(int productID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@ProductId", productID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<SkuDBEntity>("[Billing].[GetSkuforProduct]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Executes [Billing].[GetProductById].
		/// </summary>
		/// <param name="productID">Sets ProductID.</param>
		/// <returns>ProductDBEntity obj.</returns>
		public ProductDBEntity GetProductById(int productID)
		{
			return ProductDBEntityCache.Instance.GetItemById(productID);
		}

		///// <summary>
		///// Executes [Billing].[GetFreeSku].
		///// </summary>
		///// <param name="productID">Sets ProductID.</param>
		///// <returns>SkuDBEntity obj.</returns>
		//public SkuDBEntity GetFreeSku(int productID)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@ProductID", productID);
		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		// default null
		//		return connection.Query<SkuDBEntity>("[Billing].[GetFreeSku]", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
		//	}
		//}

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
		/// <returns>The ret ID.</returns>
		public int ChangeSubscription(int organizationId, int skuId, int productId, int numberOfUsers)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationId);
			parameters.Add("@SkuId", skuId);
			parameters.Add("@NumberOfUsers", numberOfUsers);
			parameters.Add("@retId", -1, DbType.Int32, direction: ParameterDirection.Output);
			/*if (skuId == 0)
			{
				parameters.Add("@ProductId", productId);
			}
			else
			{ */
			parameters.Add("@ProductId", null);
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
		/// Executes [Billing].[GetSkuList].
		/// </summary>
		/// <returns>List of SkuDBEntity's.</returns>
		public IEnumerable<SkuDBEntity> GetSkuList()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default empty list
				return connection.Query<SkuDBEntity>("[Billing].[GetSkuList]", commandType: CommandType.StoredProcedure);
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
		/// Adds an entry to the customer subscription table.
		/// </summary>
		/// <param name="stripeTokenCustId">The id of the stripe customer.</param>
		/// <param name="stripeTokenSubId">The id of the stripe subscription.</param>
		/// <param name="price">The price of the subscription.</param>
		/// <param name="numberOfUsers">The number of users for the subscription.</param>
		/// <param name="productID">The id of the subscription product.</param>
		/// <param name="organizationID">The id of the organization that the subscription belongs to.</param>
		/// <returns>Subscription ID.</returns>
		public string AddCustomerSubscription(string stripeTokenCustId, string stripeTokenSubId, int price, int numberOfUsers, int productID, int organizationID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationID);
			parameters.Add("@stripeTokenCustId", stripeTokenCustId);
			parameters.Add("@stripeTokenSubId", stripeTokenSubId);
			parameters.Add("@Price", price);
			parameters.Add("@NumberOfUsers", numberOfUsers);
			parameters.Add("@ProductId", productID);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				return connection.Query<string>(
				   "[Billing].[CreateSubscriptionPlan]",
				   parameters,
				   commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Adds an entry to the organization customer table.
		/// </summary>
		/// <param name="orgId">The id of the organization.</param>
		/// <param name="customerID">The id of the customer object.</param>
		public void AddOrgCustomer(int orgId, string customerID)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrgId", orgId);
			parameters.Add("@customerID", customerID);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Query<int>(
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
		/// Gets the subscription id for the organization.
		/// </summary>
		/// <param name="customerID">The id of the customer.</param>
		/// <param name="subscriptionPlanID">The id of the subscription plan to be updated.</param>
		/// <param name="price">The price of the subscription.</param>
		/// <param name="numberOfUsers">The number of users for the subscription.</param>
		/// <returns>The subscription ID.</returns>
		public string UpdateSubscriptionPlan(string customerID, string subscriptionPlanID, int price, int numberOfUsers)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@customerID", customerID);
			parameters.Add("@SubPlanId", subscriptionPlanID);
			parameters.Add("@NumberOfUsers", numberOfUsers);
			parameters.Add("@Price", price);
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				return connection.Query<string>(
				   "[Billing].[UpdateCustomerSubscription]",
				   parameters,
				   commandType: CommandType.StoredProcedure).SingleOrDefault();
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

		///// <summary>
		///// Gets the price of the subscription.
		///// </summary>
		///// <param name="subscriptionid">The id of the subscription in question.</param>
		///// <returns>The price of the subscription.</returns>
		//public int GetSubscriptionPlanPrice(string subscriptionid)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@subscriptionID", subscriptionid);
		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		// default -1
		//		return connection.Query<int>(
		//		   "[Billing].[GetStripeSubscriptionPlanPrice]",
		//		   parameters,
		//		   commandType: CommandType.StoredProcedure).SingleOrDefault();
		//	}
		//}

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

		///// <summary>
		///// Gets all the susbcription ids for an organization.
		///// </summary>
		///// <param name="orgid">The organization id.</param>
		///// <returns>The subscription ids.</returns>
		//public IEnumerable<string> GetSubscriptionIds(int orgid)
		//{
		//	DynamicParameters parameters = new DynamicParameters();
		//	parameters.Add("@OrgId", orgid);
		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		// default empty list
		//		return connection.Query<string>(
		//		  "[Billing].[GetAllSubscriptionsForOrganization]",
		//		  parameters,
		//		  commandType: CommandType.StoredProcedure);
		//	}
		//}

		/// <summary>
		/// Gets a list of all subscriptions.
		/// </summary>
		/// <returns>A list of SubscriptionDBEntity entities.</returns>
		public List<SubscriptionDBEntity> GetSubscriptionList()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<SubscriptionDBEntity>("[Billing].[GetSubscriptionList]", commandType: CommandType.StoredProcedure).ToList();
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
		/// Returns the organization information crossed with subscription and role information, from a user id.
		/// </summary>
		/// <param name="userId">The user ID.</param>
		/// <returns>A collection of SubscriptionDisplay information.</returns>
		public IEnumerable<SubscriptionDisplayDBEntity> GetUserSubscriptionOrganizationList(int userId)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default null
				return connection.Query<SubscriptionDisplayDBEntity>(
					"[Billing].[GetSubscriptionDetailsByUser]",
					new
					{
						userId = userId
					},
					commandType: CommandType.StoredProcedure);
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

		//private void InitializeProducts()
		//{
		//	using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
		//	{
		//		this.productIDList = connection.Query<int>("[Billing].[GetProductIds]", commandType: CommandType.StoredProcedure);
		//	}
		//}
	}
}
