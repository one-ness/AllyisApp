﻿//------------------------------------------------------------------------------
// <copyright file="CrmService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AllyisApps.Services
{
	/// <summary>
	/// Services for Cutomer Relationship Management related functions (billing, subscriptions).
	/// </summary>
	public partial class Service : BaseService
	{
		/// <summary>
		/// Gets a list of <see cref="ProductInfo"/>s for all available products.
		/// </summary>
		/// <returns>A list of <see cref="ProductInfo"/>s for all available products.</returns>
		public static List<ProductInfo> GetProductInfoList()
		{
			return DBHelper.Instance.GetProductList().Select(p => new ProductInfo
			{
				ProductId = p.ProductId,
				ProductName = p.Name,
				ProductDescription = p.Description
			}).ToList();
		}

		/// <summary>
		/// Gets the product Id from the product name.
		/// This method is static so that it can be accessed in BaseProductController constructor initializations.
		/// </summary>
		/// <param name="name">Product name.</param>
		/// <returns>Product Id.</returns>
		public static int GetProductIdByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name", "Product name must have a value.");
			}

			return DBHelper.Instance.GetProductIDByName(name);
		}

		/// <summary>
		/// Lists Billing Service invoices for a given customer.
		/// </summary>
		/// <param name="customerId">The id of the customer for which the invoices should be listed.</param>
		/// <returns>The list of invoices.</returns>
		[CLSCompliant(false)]
		public List<BillingServicesInvoice> ListInvoices(BillingServicesCustomerId customerId)
		{
			string serviceType = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(serviceType);
			return handler.ListInvoices(customerId);
		}

		/// <summary>
		/// Lists Billing Service charges for a given customer.
		/// </summary>
		/// <param name="customerId">The id of the customer for which the charges should be listed.</param>
		/// <returns>The list of charges.</returns>
		[CLSCompliant(false)]
		public List<BillingServicesCharge> ListCharges(BillingServicesCustomerId customerId)
		{
			string serviceType = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(serviceType);
			return handler.ListCharges(customerId);
		}

		/// <summary>
		/// Gets a <see cref="CustomerInfo"/>.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>The customer entity.</returns>
		public CustomerInfo GetCustomer(int customerId)
		{
			return InitializeCustomerInfo(DBHelper.GetCustomerInfo(customerId));
		}

		/// <summary>
		/// Gets the next logical customer id for the current organization, and a list of
		/// valid country names.
		/// </summary>
		/// <returns></returns>
		public Tuple<string, List<string>> GetNextCustIdAndCountries()
		{
			var spResults = DBHelper.GetNextCustIdAndCountries(UserContext.ChosenOrganizationId);
			return Tuple.Create(
				spResults.Item1 == null ? "0000000000000000" : new string(IncrementAlphanumericCharArray(spResults.Item1.ToCharArray())),
				spResults.Item2);
		}

		/// <summary>
		/// Gets a CustomerInfo for the given customer, and a list of valid country names.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns></returns>
		public Tuple<CustomerInfo, List<string>> GetCustomerAndCountries(int customerId)
		{
			var spResults = DBHelper.GetCustomerCountries(customerId);
			return Tuple.Create(
				InitializeCustomerInfo(spResults.Item1),
				spResults.Item2);
		}

		/// <summary>
		/// Creates a customer.
		/// </summary>
		/// <param name="customer">Customer info.</param>
		/// <returns>Customer id.</returns>
		public int? CreateCustomer(CustomerInfo customer)
		{
			if (this.Can(Actions.CoreAction.EditCustomer) && customer != null)
			{
				return DBHelper.CreateCustomerInfo(GetDBEntityFromCustomerInfo(customer));
			}

			return null;
		}

		/// <summary>
		/// Updates a customer in the database.
		/// </summary>
		/// <param name="customer">Updated customer info.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateCustomer(CustomerInfo customer)
		{
			if (this.Can(Actions.CoreAction.EditCustomer) && customer != null)
			{
				DBHelper.UpdateCustomer(GetDBEntityFromCustomerInfo(customer));
				return true;
			}

			return false;
		}

		/// <summary>
		/// Deletes a customer.
		/// </summary>
		/// <param name="customerId">Customer id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool DeleteCustomer(int customerId)
		{
			if (this.Can(Actions.CoreAction.EditCustomer))
			{
				DBHelper.DeleteCustomer(customerId);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets a list of <see cref="CustomerInfo"/>'s for an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns><see cref="IEnumerable{CustomerDBEntity}"/>.</returns>
		public IEnumerable<CustomerInfo> GetCustomerList(int orgId)
		{
			IEnumerable<CustomerDBEntity> dbeList = DBHelper.GetCustomerList(orgId);
			List<CustomerInfo> list = new List<CustomerInfo>();
			foreach (CustomerDBEntity dbe in dbeList)
			{
				if (dbe != null)
				{
					list.Add(InitializeCustomerInfo(dbe));
				}
			}

			return list;
		}

		/// <summary>
		/// Returns a list of CompleteProjectInfos for the current organization with the IsProjectUser field filled
		/// out for the current user, and a list of CustomerInfos for the organization.
		/// </summary>
		/// <returns></returns>
		public Tuple<List<CompleteProjectInfo>, List<CustomerInfo>> GetProjectsAndCustomersForOrgAndUser()
		{
			var spResults = DBHelper.GetProjectsAndCustomersForOrgAndUser(UserContext.ChosenOrganizationId, UserContext.UserId);
			return Tuple.Create(
				spResults.Item1.Select(cpdb => InitializeCompleteProjectInfo(cpdb)).ToList(),
				spResults.Item2.Select(cdb => InitializeCustomerInfo(cdb)).ToList());
		}

		/// <summary>
		/// Edits or creates billing information for the current chosen organization.
		/// </summary>
		/// <param name="billingServicesEmail">Customer email address.</param>
		/// <param name="token">The BillingServicesToken being used for this charge.</param>
		[CLSCompliant(false)]
		public void UpdateBillingInfo(string billingServicesEmail, BillingServicesToken token)
		{
			#region Validation

			if (token == null)
			{
				throw new ArgumentNullException("token", "billingServicesToken must have a value.");
			}

			if (string.IsNullOrEmpty(billingServicesEmail))
			{
				throw new ArgumentNullException("billingServicesEmail", "Email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(billingServicesEmail))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			#endregion Validation

			//// Either get the existing billing service information for the org or create some if the org has none
			BillingServicesCustomerId customerId = this.GetOrgBillingServicesCustomerId();
			if (customerId == null)
			{
				string serviceType = "Stripe";
				BillingServicesHandler handler = new BillingServicesHandler(serviceType);
				customerId = handler.CreateCustomer(billingServicesEmail, token);
				this.AddOrgCustomer(customerId);
				this.AddBillingHistory(string.Format("Adding {0} customer data", serviceType), null);
			}
			else
			{
				string serviceType = "Stripe";
				BillingServicesHandler handler = new BillingServicesHandler(serviceType);
				handler.UpdateCustomer(customerId, token);
				this.AddBillingHistory(string.Format("Updating {0} customer data", serviceType), null);
			}
		}

		/// <summary>
		/// Deletes a billing services subscription.
		/// </summary>
		/// <param name="id">The customer id associated with the subscription to be deleted.</param>
		/// <param name="subscriptionId">The id of the subscription to delete.</param>
		[CLSCompliant(false)]
		public void DeleteSubscription(BillingServicesCustomerId id, string subscriptionId)
		{
			string serviceType = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(serviceType);
		}

		/// <summary>
		/// Creates a billingServices customer.
		/// </summary>
		/// <param name="billingServicesEmail">The user's email.</param>
		/// <param name="token">The BillingServicesToken for creating the customer.</param>
		/// <returns>A new StripeCustomer.</returns>
		[CLSCompliant(false)]
		public BillingServicesCustomerId CreateBillingServicesCustomer(string billingServicesEmail, BillingServicesToken token)
		{
			#region Validation

			if (token == null)
			{
				throw new ArgumentNullException("token", "Billing Services token must not be null.");
			}

			if (string.IsNullOrEmpty(billingServicesEmail))
			{
				throw new ArgumentNullException("billingServicesEmail", "Email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(billingServicesEmail))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			#endregion Validation

			string serviceType = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(serviceType);
			return handler.CreateCustomer(billingServicesEmail, token);
		}

		/// <summary>
		/// Adds an item to billing history.
		/// </summary>
		/// <param name="description">A description for the item.</param>
		/// <param name="skuId">Sku Id.</param>
		public void AddBillingHistory(string description, int? skuId)
		{
			if (skuId.HasValue && skuId.Value <= 0)
			{
				throw new ArgumentOutOfRangeException("skuId", "Sku Id cannot be 0 or negative. Use null instead of 0.");
			}

			DBHelper.AddBillingHistory(description, UserContext.ChosenOrganizationId, UserContext.UserId, skuId);
		}

		/// <summary>
		/// Removes billing from the current organization.
		/// </summary>
		/// <returns>Returns false if authorization fails.</returns>
		public bool RemoveBilling()
		{
			if (this.Can(Actions.CoreAction.EditOrganization))
			{
				DBHelper.RemoveBilling(UserContext.ChosenOrganizationId);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets the stripe customer Id for the current organization.
		/// </summary>
		/// <returns>The customer ID.</returns>
		[CLSCompliant(false)]
		public BillingServicesCustomerId GetOrgBillingServicesCustomerId()
		{
			string id = DBHelper.GetOrgCustomer(UserContext.ChosenOrganizationId);
			return new BillingServicesCustomerId(id);
		}

		/// <summary>
		/// Adds a stripe customer for the current organization.
		/// </summary>
		/// <param name="customerId">Billing Services customer id.</param>
		[CLSCompliant(false)]
		public void AddOrgCustomer(BillingServicesCustomerId customerId)
		{
			if (customerId == null)
			{
				throw new ArgumentNullException("customerId", "Billing Services customer id must have a value.");
			}

			DBHelper.AddOrgCustomer(UserContext.ChosenOrganizationId, customerId.Id);
		}

		/// <summary>
		/// Creates a customer subscription plan and adds a monthly subscription for the current organization.
		/// </summary>
		/// <param name="amount">Price of subscription.</param>
		/// <param name="customerId">The StripeCustomer.</param>
		/// <param name="numUsers">Number of subscription users.</param>
		/// <param name="productId">Product Id.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <returns>Subscription Id.</returns>
		[CLSCompliant(false)]
		public string AddCustomerSubscriptionPlan(int amount, BillingServicesCustomerId customerId, int numUsers, int productId, string planName)
		{
			#region Validation

			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException("amount", "Price cannot be negative.");
			}

			if (customerId == null)
			{
				throw new ArgumentNullException("customerId", "customerId cannot be null.");
			}

			if (numUsers < 0)
			{
				// TODO: Figure out if this can be 0 or not
				throw new ArgumentOutOfRangeException("numUsers", "Number of users cannot be negative.");
			}

			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			#endregion Validation

			string service = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(service);
			BillingServicesSubscriptionId subId = handler.CreateSubscription(amount, "month", planName, customerId);
			return DBHelper.AddCustomerSubscription(customerId.Id, subId.Id, amount, numUsers, productId, UserContext.ChosenOrganizationId);
		}

		/// <summary>
		/// Updates a customer subscription.
		/// </summary>
		/// <param name="amount">Price of subscription.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <param name="numUsers">Number of Users.</param>
		/// <param name="subscriptionId">Subscription Id, as a string.</param>
		/// <param name="customerId">The Billing Services Customer ID.</param>
		/// <returns>The customer subscription.</returns>
		[CLSCompliant(false)]
		public string UpdateSubscriptionPlan(int amount, string planName, int numUsers, string subscriptionId, BillingServicesCustomerId customerId)
		{
			#region Validation

			if (string.IsNullOrEmpty(subscriptionId))
			{
				throw new ArgumentNullException("subscriptionId", "Subscription id must have a value.");
			}

			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException("amount", "Price cannot be negative.");
			}

			if (customerId == null)
			{
				throw new ArgumentNullException("customerId", "customerId cannot be null.");
			}

			if (numUsers < 0)
			{
				// TODO: Figure out if this can be 0 or not
				throw new ArgumentOutOfRangeException("numUsers", "Number of users cannot be negative.");
			}

			#endregion Validation

			string serviceType = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(serviceType); // TODO: make this check the database instead of hardcoding Stripe

			handler.UpdateSubscription(amount, "month", planName, subscriptionId.Trim(), customerId);
			return DBHelper.UpdateSubscriptionPlan(customerId.Id, subscriptionId, amount, numUsers);
		}

		/// <summary>
		/// Updates the active subsciption for the current user.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		public void UpdateActiveSubscription(int? subscriptionId)
		{
			if (subscriptionId.HasValue && subscriptionId.Value <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative. Use null instead of 0.");
			}

			DBHelper.UpdateActiveSubscription(UserContext.UserId, subscriptionId);
		}

		/// <summary>
		/// Updates the number of subscription users for a sku in the current organization.
		/// </summary>
		/// <param name="skuId">Sku Id.</param>
		/// <param name="numberOfUsers">New number of users.</param>
		public void UpdateSubscriptionUsers(int skuId, int numberOfUsers)
		{
			if (skuId <= 0)
			{
				throw new ArgumentOutOfRangeException("skuId", "Sku Id cannot be 0 or negative.");
			}

			if (numberOfUsers < 0)
			{
				throw new ArgumentOutOfRangeException("numberOfUsers", "Number of users cannot be negative.");
			}

			DBHelper.UpdateSubscriptionUsers(UserContext.ChosenOrganizationId, skuId, numberOfUsers);
		}

		/// <summary>
		/// Deletes a subsciption user.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <param name="userId">User Id.</param>
		public void DeleteSubscriptionUser(int subscriptionId, int userId)
		{
			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			DBHelper.DeleteSubscriptionUser(subscriptionId, userId);
		}

		/// <summary>
		/// Gets a <see cref="ProductInfo"/>.
		/// </summary>
		/// <param name="productId">Product Id.</param>
		/// <returns>A ProductInfo instance.</returns>
		public ProductInfo GetProductById(int productId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			ProductDBEntity product = DBHelper.GetProductById(productId);
			return new ProductInfo
			{
				ProductId = product.ProductId,
				ProductName = product.Name,
				ProductDescription = product.Description
			};
		}

		/// <summary>
		/// Gets a <see cref="SubscriptionInfo"/>.
		/// </summary>
		/// <param name="subscriptionId">Product Id.</param>
		/// <returns>A SubscriptionDBEntity.</returns>
		public SubscriptionInfo GetSubscription(int subscriptionId)
		{
			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			SubscriptionDBEntity si = DBHelper.GetSubscriptionDetailsById(subscriptionId);
			if (si == null)
			{
				return null;
			}

			return new SubscriptionInfo
			{
				OrganizationName = si.OrganizationName,
				OrganizationId = si.OrganizationId,
				SubscriptionId = si.SubscriptionId,
				SkuId = si.SkuId,
				NumberOfUsers = si.NumberOfUsers,
				Licenses = si.Licenses,
				CreatedUTC = si.CreatedUTC,
				IsActive = si.IsActive,
				Name = si.Name
			};
		}

		/// <summary>
		/// Gets a subscription Id for a customer of the current organization.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>Subscription Id.</returns>
		[CLSCompliant(false)]
		public string GetSubscriptionId(BillingServicesCustomerId customerId)
		{
			if (customerId == null)
			{
				throw new ArgumentNullException("customerId", "Customer id must have a value.");
			}

			return DBHelper.GetSubscriptionPlan(UserContext.ChosenOrganizationId, customerId.Id);
		}

		/// <summary>
		/// Gets a list of non-zero subscription plan prices used by the current organization.
		/// </summary>
		/// <returns>List of prices, as ints.</returns>
		public IEnumerable<int> GetSubscriptionPlanPrices()
		{
			return DBHelper.GetSubscriptionPlanPrices(UserContext.ChosenOrganizationId);
		}

		/// <summary>
		/// Deletes a subscription plan.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		public void DeleteSubscriptionPlan(string subscriptionId)
		{
			if (string.IsNullOrEmpty(subscriptionId))
			{
				throw new ArgumentNullException("subscriptionId", "Subscriptoin id must have a value.");
			}

			DBHelper.DeleteSubscriptionPlan(subscriptionId);
		}

		/// <summary>
		/// Gets a list of <see cref="UserInfo"/>'s for users in the organization with subscriptions to the given product.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="productId">Product Id.</param>
		/// <returns>A list of UserInfo's for users in the organization with subscriptions to the given product.</returns>
		public IEnumerable<UserInfo> GetUsersWithSubscriptionToProductInOrganization(int orgId, int productId)
		{
			if (orgId <= 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be 0 or negative.");
			}

			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			return DBHelper.GetUsersWithSubscriptionToProductInOrganization(orgId, productId).Select(u => InitializeUserInfo(u));
		}

		/// <summary>
		/// Gets a list of <see cref="SubscriptionDisplayInfo"/>s for all subscriptions in the chosen organization.
		/// </summary>
		/// <returns>List of SubscriptionDisplayInfos.</returns>
		public IEnumerable<SubscriptionDisplayInfo> GetSubscriptionsDisplay(int organizationId = -1)
		{
			if (organizationId == -1) organizationId = UserContext.ChosenOrganizationId;

			return DBHelper.Instance.GetSubscriptionsDisplayByOrg(organizationId).Select(s => InitializeSubscriptionDisplayInfo(s)).ToList();
		}

		/// <summary>
		/// Gets a list of available <see cref="SubscriptionRoleInfo"/>s for a subscription.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>List of SubscriptionRoleInfos.</returns>
		public IEnumerable<SubscriptionRoleInfo> GetProductRolesFromSubscription(int subscriptionId)
		{
			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			return DBHelper.GetProductRolesFromSubscription(subscriptionId).Select(s =>
			{
				if (s == null)
				{
					return null;
				}

				return new SubscriptionRoleInfo
				{
					Name = s.Name,
					ProductRoleId = s.ProductRoleId
				};
			});
		}

		/// <summary>
		/// Gets a <see cref="SkuInfo"/>.
		/// </summary>
		/// <param name="skuId">Sku Id.</param>
		/// <returns>The SKU details.</returns>
		public SkuInfo GetSkuDetails(int skuId)
		{
			if (skuId <= 0)
			{
				throw new ArgumentOutOfRangeException("skuId", "Sku ID cannot be 0 or negative.");
			}

			SkuDBEntity sku = DBHelper.GetSkuDetails(skuId);
			if (sku == null)
			{
				return null;
			}

			return new SkuInfo
			{
				SkuId = sku.SkuId,
				ProductId = sku.ProductId,
				Name = sku.Name,
				Price = sku.Price,
				UserLimit = sku.UserLimit,
				BillingFrequency = sku.BillingFrequency
			};
		}

		/// <summary>
		/// Creates a new Subscription in the database.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="selectedSku">Selected Sku.</param>
		/// <param name="productId">Product Id.</param>
		/// <param name="numberOfUsers">The number of users.</param>
		public void AddSubscriptionOfSkuToOrganization(int orgId, int selectedSku, int productId, int numberOfUsers)
		{
			#region Validation

			if (orgId <= 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be 0 or negative.");
			}

			if (selectedSku <= 0)
			{
				throw new ArgumentOutOfRangeException("selectedSku", "Sku Id cannot be 0 or negative.");
			}

			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			if (numberOfUsers < 0)
			{ // TODO: Figure out if this can be 0 or not
				throw new ArgumentOutOfRangeException("numberOfUsers", "Number of users cannot be negative.");
			}

			#endregion Validation

			int subID = DBHelper.ChangeSubscription(orgId, selectedSku, productId, numberOfUsers);
			if (subID != 0)
			{
				DBHelper.UpdateSubscriptionUserProductRole(this.GetProductRolesFromSubscription(subID).Where(x => x.Name == "Manager").Single().ProductRoleId, subID, UserContext.UserId);
			}
		}

		/// <summary>
		/// Unsubscribes a subscription.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		public void Unsubscribe(int subscriptionId)
		{
			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription ID cannot be 0 or negative.");
			}

			DBHelper.Unsubscribe(subscriptionId);
		}

		/// <summary>
		/// Creates default settings for a product.
		/// TODO: Is it possible to reduce hard code here?.
		/// </summary>
		/// <param name="productId">Product Id.</param>
		public void InitializeSettingsForProduct(int productId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product ID cannot be 0 or negative.");
			}

			if (productId == Service.GetProductIdByName("TimeTracker"))
			{
				DBHelper.InitializeTimeTrackerSettings(UserContext.ChosenOrganizationId);
			}
		}

		/// <summary>
		/// Gets the product role for a user.
		/// </summary>
		/// <param name="productName">Product name.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>The product role.</returns>
		public string GetProductRoleForUser(string productName, int userId)
		{
			#region Validation

			if (string.IsNullOrEmpty(productName))
			{
				throw new ArgumentNullException("productName", "Product name must have a value.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			#endregion Validation

			return DBHelper.GetProductRoleForUser(productName, UserContext.ChosenOrganizationId, userId);
		}

		/// <summary>
		/// Gets a product name via the subscription id associated with that product.
		/// </summary>
		/// <param name="subscriptionId">The subscription id for which you wish to know the product name.</param>
		/// <returns>The product name.</returns>
		public string GetProductNameBySubscriptionID(int subscriptionId)
		{
			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription ID cannot be 0 or negative.");
			}

			return DBHelper.GetProductAreaBySubscription(subscriptionId);
		}

		/// <summary>
		/// Gets a list of <see cref="BillingHistoryItemInfo"/>s for the billing history of the current organization.
		/// </summary>
		/// <returns>List of billing history items.</returns>
		public IEnumerable<BillingHistoryItemInfo> GetBillingHistory()
		{
			return DBHelper.GetBillingHistoryByOrg(UserContext.ChosenOrganizationId).Select(i =>
			{
				return new BillingHistoryItemInfo
				{
					Date = i.Date,
					Description = i.Description,
					OrganizationID = i.OrganizationID,
					ProductID = i.ProductID,
					ProductName = i.ProductName,
					SkuID = i.SkuID,
					SkuName = i.SkuName,
					UserID = i.UserID,
					UserName = i.UserName
				};
			});
		}

		/// <summary>
		/// Retreives the BillingCustomer object corresponding to a given customerId.
		/// </summary>
		/// <param name="customerId">The customerId to use when retrieving the BillingCustomer.</param>
		/// <returns>The billing customer.</returns>
		[CLSCompliant(false)]
		public BillingServicesCustomer RetrieveCustomer(BillingServicesCustomerId customerId)
		{
			if (string.IsNullOrEmpty(customerId.Id))
			{
				// return null, as there is no customer.
				return null;
			}

			string serviceType = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(serviceType);
			return handler.RetrieveCustomer(customerId);
		}

		/// <summary>
		/// Returns a ProductInfo for the given product, a SubscriptionInfo for the current org's
		/// subscription to that product (or null if none), a list of SkuInfos for all the skus for
		/// that product, the Stripe billing token for the current org (or null if none), and the total
		/// number of users in the org with roles in the subscription for the product.
		/// </summary>
		/// <param name="productId">Product Id.</param>
		/// <returns></returns>
		public Tuple<ProductInfo, SubscriptionInfo, List<SkuInfo>, string, int> GetProductSubscriptionInfo(int productId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			var spResults = DBHelper.GetProductSubscriptionInfo(UserContext.ChosenOrganizationId, productId);
			return Tuple.Create(
				InitializeProductInfo(spResults.Item1),
				InitializeSubscriptionInfo(spResults.Item2),
				spResults.Item3.Select(sdb => InitializeSkuInfo(sdb)).ToList(),
				spResults.Item4,
				spResults.Item5);
		}

		/// <summary>
		/// Returns a list of ProjectInfos for projects the given user is assigned to in the given organization 
		/// (current organization by default), another list of ProjectDBEntities for all projects in the organization,
		/// the name of the user (as "Firstname Lastname"), and the user's email.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="orgId">Organization Id.</param>
		/// <returns></returns>
		public Tuple<List<ProjectInfo>, List<ProjectInfo>, string, string> GetProjectsForOrgAndUser(int userId, int orgId = -1)
		{
			if (userId <= 0)
			{
				throw new ArgumentException("User Id cannot be zero or negative.", "userId");
			}

			if (orgId <= 0)
			{
				orgId = UserContext.ChosenOrganizationId;
			}

			var spResults = DBHelper.GetProjectsForOrgAndUser(userId, orgId);
			var userDBEntity = spResults.Item3;
			string name = string.Format("{0} {1}", userDBEntity.FirstName, userDBEntity.LastName);
			return Tuple.Create(
				spResults.Item1.Select(pdb => InitializeProjectInfo(pdb)).ToList(),
				spResults.Item2.Select(pdb => InitializeProjectInfo(pdb)).ToList(),
				name,
				userDBEntity.Email);
		}

		#region Info-DBEntity Conversions
		/// <summary>
		/// Initializes a <see cref="CustomerInfo"/> from a <see cref="CustomerDBEntity"/>.
		/// </summary>
		/// <param name="customer">The CustomerDBEntity to use.</param>
		/// <returns>A CustomerInfo object.</returns>
		public static CustomerInfo InitializeCustomerInfo(CustomerDBEntity customer)
		{
			if (customer == null)
			{
				return null;
			}

			return new CustomerInfo()
			{
				Address = customer.Address,
				City = customer.City,
				ContactEmail = customer.ContactEmail,
				ContactPhoneNumber = customer.ContactPhoneNumber,
				Country = customer.Country,
				CreatedUTC = customer.CreatedUTC,
				CustomerId = customer.CustomerId,
				CustomerOrgId = customer.CustomerOrgId,
				EIN = customer.EIN,
				FaxNumber = customer.FaxNumber,
				Name = customer.Name,
				OrganizationId = customer.OrganizationId,
				PostalCode = customer.PostalCode,
				State = customer.State,
				Website = customer.Website
			};
		}

		/// <summary>
		/// Initializes a <see cref="CustomerDBEntity"/> from a <see cref="CustomerInfo"/>.
		/// </summary>
		/// <param name="customer">The CustomerInfo to use.</param>
		/// <returns>A CustomerDBEntity object.</returns>
		public static CustomerDBEntity GetDBEntityFromCustomerInfo(CustomerInfo customer)
		{
			if (customer == null)
			{
				return null;
			}

			return new CustomerDBEntity()
			{
				Address = customer.Address,
				City = customer.City,
				ContactEmail = customer.ContactEmail,
				ContactPhoneNumber = customer.ContactPhoneNumber,
				Country = customer.Country,
				CreatedUTC = customer.CreatedUTC,
				CustomerId = customer.CustomerId,
				CustomerOrgId = customer.CustomerOrgId,
				EIN = customer.EIN,
				FaxNumber = customer.FaxNumber,
				Name = customer.Name,
				OrganizationId = customer.OrganizationId,
				PostalCode = customer.PostalCode,
				State = customer.State,
				Website = customer.Website
			};
		}

		/// <summary>
		/// Translates a ProductRoleDBEntity into a ProductRoleInfo business object.
		/// </summary>
		/// <param name="productRole">ProductRoleDBEntity instance.</param>
		/// <returns>ProductRoleInfo instance.</returns>
		public static ProductRoleInfo InitializeProductRoleInfo(ProductRoleDBEntity productRole)
		{
			if (productRole == null)
			{
				return null;
			}

			return new ProductRoleInfo
			{
				CreatedUTC = productRole.CreatedUTC,
				ModifiedUTC = productRole.ModifiedUTC,
				ProductRoleName = productRole.Name,
				PermissionAdmin = productRole.PermissionAdmin,
				ProductId = productRole.ProductId,
				ProductRoleId = productRole.ProductRoleId
			};
		}

		/// <summary>
		/// Translates a <see cref="SubscriptionDisplayDBEntity"/> into a <see cref="SubscriptionDisplayInfo"/>.
		/// </summary>
		/// <param name="subscriptionDisplay">SubscriptionDisplayDBEntity instance.</param>
		/// <returns>SubscriptionDisplayInfo instance.</returns>
		public static SubscriptionDisplayInfo InitializeSubscriptionDisplayInfo(SubscriptionDisplayDBEntity subscriptionDisplay)
		{
			if (subscriptionDisplay == null)
			{
				return null;
			}

			return new SubscriptionDisplayInfo
			{
				CanViewSubscription = subscriptionDisplay.CanViewSubscription,
				CreatedUTC = subscriptionDisplay.CreatedUTC,
				NumberOfUsers = subscriptionDisplay.NumberOfUsers,
				OrganizationId = subscriptionDisplay.OrganizationId,
				OrganizationName = subscriptionDisplay.OrganizationName,
				ProductId = subscriptionDisplay.ProductId,
				ProductName = subscriptionDisplay.ProductName,
				SkuId = subscriptionDisplay.SkuId,
				SkuName = subscriptionDisplay.SkuName,
				SubscriptionId = subscriptionDisplay.SubscriptionId,
				SubscriptionsUsed = subscriptionDisplay.SubscriptionsUsed,
				Tier = subscriptionDisplay.Tier
			};
		}

		/// <summary>
		/// Translates a <see cref="SubscriptionDBEntity"/> into a <see cref="SubscriptionInfo"/>.
		/// </summary>
		/// <param name="subscription">SubscriptionDBEntity instance.</param>
		/// <returns>SubscriptionInfo instance.</returns>
		public static SubscriptionInfo InitializeSubscriptionInfo(SubscriptionDBEntity subscription)
		{
			if (subscription == null)
			{
				return null;
			}

			return new SubscriptionInfo
			{
				CreatedUTC = subscription.CreatedUTC,
				IsActive = subscription.IsActive,
				Licenses = subscription.Licenses,
				Name = subscription.Name,
				NumberOfUsers = subscription.NumberOfUsers,
				OrganizationId = subscription.OrganizationId,
				OrganizationName = subscription.OrganizationName,
				SkuId = subscription.SkuId,
				SubscriptionId = subscription.SubscriptionId
			};
		}

		/// <summary>
		/// Translates a <see cref="SkuDBEntity"/> into a <see cref="SkuInfo"/>.
		/// </summary>
		/// <param name="sku">SkuDBEntity instance.</param>
		/// <returns>SkuInfo instance.</returns>
		public static SkuInfo InitializeSkuInfo(SkuDBEntity sku)
		{
			if (sku == null)
			{
				return null;
			}

			return new SkuInfo
			{
				BillingFrequency = sku.BillingFrequency,
				Name = sku.Name,
				Price = sku.Price,
				ProductId = sku.ProductId,
				SkuId = sku.SkuId,
				SubscriptionId = sku.SubscriptionId,
				UserLimit = sku.UserLimit
			};
		}

		/// <summary>
		/// Translates a <see cref="SubscriptionRoleDBEntity"/> into a <see cref="SubscriptionRoleInfo"/>.
		/// </summary>
		/// <param name="subscriptionRole">SubscriptionRoleDBEntity instance.</param>
		/// <returns>SubscriptionRole instance.</returns>
		public static SubscriptionRoleInfo InitializeSubscriptionRoleInfo(SubscriptionRoleDBEntity subscriptionRole)
		{
			if (subscriptionRole == null)
			{
				return null;
			}

			return new SubscriptionRoleInfo
			{
				Name = subscriptionRole.Name,
				ProductRoleId = subscriptionRole.ProductRoleId,
				ProductId = subscriptionRole.ProductId
			};
		}
		#endregion
	}
}
