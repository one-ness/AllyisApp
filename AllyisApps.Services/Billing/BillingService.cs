//------------------------------------------------------------------------------
// <copyright file="BillingService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;
using AllyisApps.DBModel.Billing;
using AllyisApps.Lib;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AllyisApps.Services
{
	/// <summary>
	/// Services for Billing related functions (billing, subscriptions).
	/// </summary>
	public partial class AppService : BaseService
	{
		/// <summary>
		/// Gets a list of <see cref="Product"/>s for all available products.
		/// </summary>
		/// <returns>A list of <see cref="Product"/>s for all available products.</returns>
		public static List<Product> GetProductList()
		{
			return DBHelper.Instance.GetProductList().Select(p => new Product
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
		/// Edits or creates billing information for the current chosen organization.
		/// </summary>
		/// <param name="billingServicesEmail">Customer email address.</param>
		/// <param name="token">The BillingServicesToken being used for this charge.</param>
		/// <param name="orgId"></param>
		[CLSCompliant(false)]
		public void UpdateBillingInfo(string billingServicesEmail, BillingServicesToken token, int orgId)
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
			else if (!Utility.IsValidEmail(billingServicesEmail))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			#endregion Validation

			//// Either get the existing billing service information for the org or create some if the org has none
			BillingServicesCustomerId customerId = this.GetOrgBillingServicesCustomerId(orgId);
			if (customerId == null)
			{
				string serviceType = "Stripe";
				BillingServicesHandler handler = new BillingServicesHandler(serviceType);
				customerId = handler.CreateCustomer(billingServicesEmail, token);
				this.AddOrgCustomer(customerId, null);
				//this.AddBillingHistory(string.Format("Adding {0} customer data", serviceType), null);
			}
			else
			{
				string serviceType = "Stripe";
				BillingServicesHandler handler = new BillingServicesHandler(serviceType);
				handler.UpdateCustomer(customerId, token);
				this.AddBillingHistory(string.Format("Updating {0} customer data", serviceType), null);
			}
		}

		///// <summary>
		///// Deletes a billing services subscription.
		///// </summary>
		///// <param name="id">The customer id associated with the subscription to be deleted.</param>
		///// <param name="subscriptionId">The id of the subscription to delete.</param>
		//[CLSCompliant(false)]
		//public void DeleteSubscription(BillingServicesCustomerId id, string subscriptionId)
		//{
		//	string serviceType = "Stripe";
		//	BillingServicesHandler handler = new BillingServicesHandler(serviceType);

		//	//TODO complete this
		//}

		/// <summary>
		/// Deletes a subscription plan for the current organization with the given stripe customer id, if one exists.
		/// On success, adds a billing history item with the given sku id and description (and current user Id).
		/// </summary>
		/// <param name="stripeCustomerId">Stripe customer id.</param>
		/// <param name="skuId">Sku it for billing history item.</param>
		/// <param name="description">Description for billing history item.</param>
		/// <returns>The subscription plan id of the deleted subscription plan, or null if none found.</returns>
		public string DeleteSubscriptionPlanAndAddHistory(string stripeCustomerId, int skuId, string description)
		{
			#region Validation

			if (string.IsNullOrEmpty(stripeCustomerId))
			{
				throw new ArgumentNullException("stripeCustomerId", "Stripe customer id must have a value.");
			}

			if (skuId <= 0)
			{
				throw new ArgumentOutOfRangeException("skuId", "Sku id cannot be zero or negative");
			}

			#endregion Validation

			return DBHelper.DeleteSubscriptionPlanAndAddHistory(UserContext.ChosenOrganizationId, stripeCustomerId, UserContext.UserId, skuId, description);
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
			else if (!Utility.IsValidEmail(billingServicesEmail))
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
		public bool RemoveBilling(int orgId)
		{
			this.CheckOrgAction(OrgAction.DeleteBilling, orgId);
			DBHelper.RemoveBilling(orgId);
			return true;
		}

		/// <summary>
		/// Gets the stripe customer Id for the current organization.
		/// </summary>
		/// <returns>The customer ID.</returns>
		[CLSCompliant(false)]
		public BillingServicesCustomerId GetOrgBillingServicesCustomerId(int orgId)
		{
			string id = DBHelper.GetOrgCustomer(orgId);
			return new BillingServicesCustomerId(id);
		}

		/// <summary>
		/// Adds a stripe customer for the current organization, and adds a billing history item.
		/// </summary>
		/// <param name="customerId">Billing Services customer id.</param>
		/// <param name="selectedSku">Selected sku id, for the billing history item.</param>
		[CLSCompliant(false)]
		public void AddOrgCustomer(BillingServicesCustomerId customerId, int? selectedSku)
		{
			if (customerId == null)
			{
				throw new ArgumentNullException("customerId", "Billing Services customer id must have a value.");
			}

			DBHelper.AddOrgCustomer(UserContext.ChosenOrganizationId, UserContext.UserId, customerId.Id, selectedSku, "Adding stripe customer data.");
		}

		/// <summary>
		/// Creates a customer subscription plan and adds a monthly subscription for the current organization.
		/// </summary>
		/// <param name="amount">Price of subscription.</param>
		/// <param name="customerId">The StripeCustomer.</param>
		/// <param name="numUsers">Number of subscription users.</param>
		/// <param name="productId">Product Id.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <param name="skuId">Selected sku id, for the billing history item.</param>
		[CLSCompliant(false)]
		public void AddCustomerSubscriptionPlan(int amount, BillingServicesCustomerId customerId, int numUsers, int productId, string planName, int? skuId)
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
			DBHelper.AddCustomerSubscription(customerId.Id, subId.Id, amount, numUsers, productId, UserContext.ChosenOrganizationId, UserContext.UserId, skuId, string.Format("Adding new subscription data for {0}.", planName));
		}

		/// <summary>
		/// Updates a customer subscription, and adds a billing history item.
		/// </summary>
		/// <param name="amount">Price of subscription.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <param name="numUsers">Number of Users.</param>
		/// <param name="subscriptionId">Subscription Id, as a string.</param>
		/// <param name="customerId">The Billing Services Customer ID.</param>
		/// <param name="skuId">Selected sku id, for the billing history item.</param>
		[CLSCompliant(false)]
		public void UpdateSubscriptionPlan(int amount, string planName, int numUsers, string subscriptionId, BillingServicesCustomerId customerId, int? skuId)
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
			DBHelper.UpdateSubscriptionPlan(customerId.Id, subscriptionId, amount, numUsers, UserContext.ChosenOrganizationId, UserContext.UserId, skuId, string.Format("Updating subscription data for {0}", planName));
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
		/// Assigns a new TimeTracker role to the given users for the current organization.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="newTimeTrackerRole">TimeTracker role to assign, or -1 to remove from subscription.</param>
		/// <param name="orgId">The Organization Id</param>
		/// <returns>A tuple containing the number of updated users and the number of added users. If the updated users is -1,
		/// there is no TimeTracker subscription. If the number added is -1, there are too many subscription users already to
		/// add the given list.</returns>
		public Tuple<int, int> ChangeSubscriptionUserRoles(List<int> userIds, int newTimeTrackerRole, int orgId)
		{
			#region Validation

			if (!Enum.IsDefined(typeof(TimeTrackerRole), newTimeTrackerRole) && newTimeTrackerRole != -1)
			{
				throw new ArgumentOutOfRangeException("newTimeTrackerRole", "TimeTracker role must either be -1 or match a value of the ProductRoleIdEnum enum.");
			}
			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentException("No user ids provided.", "userIds");
			}

			#endregion Validation

			return DBHelper.EditSubscriptionUsers(userIds, orgId, newTimeTrackerRole);
		}

		/// <summary>
		/// Gets a <see cref="Product"/>.
		/// </summary>
		/// <param name="productId">Product Id.</param>
		/// <returns>A ProductInfo instance.</returns>
		public Product GetProductById(int productId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			ProductDBEntity product = DBHelper.GetProductById(productId);
			return new Product
			{
				ProductId = product.ProductId,
				ProductName = product.Name,
				ProductDescription = product.Description,
				AreaUrl = product.AreaUrl
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
		/// <param name="customerId">The Billing Services Customer ID.</param>
		/// <param name="skuId">Selected sku id, for the billing history item.</param>
		[CLSCompliant(false)]
		public void DeleteSubscriptionPlan(string subscriptionId, BillingServicesCustomerId customerId, int? skuId)
		{
			#region Validation

			if (string.IsNullOrEmpty(subscriptionId))
			{
				throw new ArgumentNullException("subscriptionId", "Subscriptoin id must have a value.");
			}

			if (customerId == null)
			{
				throw new ArgumentNullException("customerId", "customerId cannot be null.");
			}

			#endregion Validation

			string service = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(service);
			handler.DeleteSubscription(customerId, subscriptionId);
			//DBHelper.DeleteSubscriptionPlan(subscriptionId);
			DBHelper.DeleteSubscriptionPlanAndAddHistory(UserContext.ChosenOrganizationId, customerId.Id, UserContext.UserId, skuId, "Switching to free subscription, canceling stripe susbcription");
		}

		/// <summary>
		/// Gets a list of <see cref="User"/>'s for users in the organization with subscriptions to the given product.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="productId">Product Id.</param>
		/// <returns>A list of User's for users in the organization with subscriptions to the given product.</returns>
		public IEnumerable<User> GetUsersWithSubscriptionToProductInOrganization(int orgId, int productId)
		{
			if (orgId <= 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be 0 or negative.");
			}

			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			return DBHelper.GetUsersWithSubscriptionToProductInOrganization(orgId, productId).Select(u => InitializeUser(u));
		}

		/// <summary>
		/// Gets a list of <see cref="SubscriptionDisplayInfo"/>s for all subscriptions in the chosen organization.
		/// </summary>
		/// <returns>List of SubscriptionDisplayInfos.</returns>
		public IEnumerable<SubscriptionDisplayInfo> GetSubscriptionsDisplay(int organizationId)
		{
			return DBHelper.Instance.GetSubscriptionsDisplayByOrg(organizationId).Select(s => InitializeSubscriptionDisplayInfo(s)).ToList();
		}

		/// <summary>
		/// Gets a list of available <see cref="ProductRole"/>s for a subscription.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>List of SubscriptionRoleInfos.</returns>
		public IEnumerable<ProductRole> GetProductRolesFromSubscription(int subscriptionId)
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

				return new ProductRole
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
				BillingFrequency = (BillingFrequencyEnum)sku.BillingFrequency
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
		/// Removes the billing subscription plan for the current organization and adds a billing history
		/// item. If the supllied subscription id has a value, also removes that subscription and returns
		/// a message to place in a notification.
		/// </summary>
		/// <param name="SelectedSku">Sku id, for the billing history item.</param>
		/// <param name="subscriptionId">Subscription id to unsubscribe from.</param>
		/// <returns>A notification string, or null.</returns>
		public string UnsubscribeAndRemoveBillingSubscription(int SelectedSku, int? subscriptionId)
		{
			var orgId = this.UserContext.UserSubscriptions[subscriptionId.Value].OrganizationId;
			BillingServicesCustomer custId = this.RetrieveCustomer(this.GetOrgBillingServicesCustomerId(orgId));
			if (custId != null)
			{
				this.DeleteSubscriptionPlanAndAddHistory(custId.Id.Id, SelectedSku, "Unsubscribing from product.");
				//string subscriptionPlanId = this.DeleteSubscriptionPlanAndAddHistory(custId.Id.Id, SelectedSku, "Unsubscribing from product.");
				//if (subscriptionPlanId != null)
				//{
				//	this.DeleteSubscription(custId.Id, subscriptionPlanId);
				//}
			}

			if (subscriptionId != null)
			{
				string skuName = this.Unsubscribe(subscriptionId.Value);
				return string.Format("{0} has been unsubscribed from the license {1}.", UserContext.UserSubscriptions[subscriptionId.Value].OrganizationName, skuName);
			}

			return null;
		}

		/// <summary>
		/// Subscribes the current organization to a product or updates the organization's subscription to the product,
		/// and creates/updates/removes the billing subscription plan accordingly.
		/// </summary>
		/// <param name="numberOfUsers">Number of users.</param>
		/// <param name="productId">Product id.</param>
		/// <param name="productName">Product name.</param>
		/// <param name="selectedSku">Selected sku id.</param>
		/// <param name="previousSku">The previous sku id.</param>
		/// <param name="billingAmount">Billing amount, as an int in cents.</param>
		/// <param name="existingToken">The existing BillingServicesToken, if any.</param>
		/// <param name="addingBillingCustomer">A value indicating whether a new billing customer is being added.</param>
		/// <param name="newBillingEmail">The email for the new billing customer, if being added.</param>
		/// <param name="newBillingToken">The new BillingServicesToken, if being added.</param>
		/// <param name="orgId"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public bool Subscribe(int numberOfUsers, int productId, string productName, int selectedSku, int previousSku, int billingAmount, BillingServicesToken existingToken, bool addingBillingCustomer, string newBillingEmail, BillingServicesToken newBillingToken, int orgId)
		{
			if (numberOfUsers < this.GetUsersWithSubscriptionToProductInOrganization(orgId, productId).Count())
			{
				return false;
			}

			BillingServicesCustomer customer;
			BillingServicesToken token;
			if (addingBillingCustomer)
			{
				BillingServicesCustomerId customerId = this.CreateBillingServicesCustomer(newBillingEmail, newBillingToken);

				this.AddOrgCustomer(customerId, null);
				customer = this.RetrieveCustomer(customerId);
				token = newBillingToken;
			}
			else
			{
				customer = this.RetrieveCustomer(this.GetOrgBillingServicesCustomerId(orgId));
				token = existingToken;
			}

			// Users >= 500 (the hardcoded free amount) will not trigger this
			if (billingAmount > 0)
			{
				BillingServicesCustomerId customerId = this.GetOrgBillingServicesCustomerId(orgId);
				if (customerId == null)
				{
					customer = this.RetrieveCustomer(this.CreateBillingServicesCustomer(newBillingEmail, token));
					this.AddOrgCustomer(customer.Id, null);
				}
				else
				{
					customer = this.RetrieveCustomer(customerId);
				}

				string subscriptionId = this.GetSubscriptionId(customer.Id);

				if (subscriptionId == null)
				{
					this.AddCustomerSubscriptionPlan(billingAmount, customer.Id, numberOfUsers, productId, productName, selectedSku);
				}
				else
				{
					this.UpdateSubscriptionPlan(billingAmount, productName, numberOfUsers, subscriptionId, customer.Id, selectedSku);
				}
			}
			else
			{
				customer = this.RetrieveCustomer(this.GetOrgBillingServicesCustomerId(orgId));

				if (customer != null)
				{
					// check if there is a subscription to cancel
					string subscriptionId = this.GetSubscriptionId(customer.Id);
					if (subscriptionId != null)
					{
						this.DeleteSubscriptionPlan(subscriptionId, customer.Id, selectedSku);
					}
				}
			}

			this.InitializeSettingsForProduct(productId, orgId);

			if (selectedSku != previousSku)
			{
				this.AddSubscriptionOfSkuToOrganization(orgId, selectedSku, productId, numberOfUsers);
			}
			else
			{
				this.UpdateSubscriptionUsers(selectedSku, numberOfUsers);
			}

			return true;
		}

		/// <summary>
		/// Unsubscribes a subscription.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <return>The name of the sku for the removed subscription.</return>
		public string Unsubscribe(int subscriptionId)
		{
			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription ID cannot be 0 or negative.");
			}

			return DBHelper.Unsubscribe(subscriptionId);
		}

		/// <summary>
		/// Creates default settings for a product.
		/// TODO: Is it possible to reduce hard code here?.
		/// </summary>
		/// <param name="productId">Product Id.</param>
		/// <param name="orgId"></param>
		public void InitializeSettingsForProduct(int productId, int orgId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product ID cannot be 0 or negative.");
			}

			if (productId == AppService.GetProductIdByName("Time Tracker"))
			{
				DBHelper.InitializeTimeTrackerSettings(orgId);
			}
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
		/// Returns a Product for the given product, a SubscriptionInfo for the current org's
		/// subscription to that product (or null if none), a list of SkuInfos for all the skus for
		/// that product, the Stripe billing token for the current org (or null if none), and the total
		/// number of users in the org with roles in the subscription for the product.
		/// </summary>
		/// <param name="orgId"></param>
		/// <param name="skuId">Product Id.</param>
		/// <returns></returns>
		public Tuple<Product, SubscriptionInfo, List<SkuInfo>, string, int> GetProductSubscriptionInfo(int orgId, int skuId)
		{
			if (skuId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			var spResults = DBHelper.GetProductSubscriptionInfo(orgId, skuId);
			return Tuple.Create(
				InitializeProduct(spResults.Item1),
				InitializeSubscriptionInfo(spResults.Item2),
				spResults.Item3.Select(sdb => InitializeSkuInfo(sdb)).ToList(),
				spResults.Item4,
				spResults.Item5);
		}

		/// <summary>
		/// Returns a list of active products and each product's active skus
		/// </summary>
		public Tuple<List<Product>, List<SkuInfo>> GetAllActiveProductsAndSkus()
		{
			var spResults = DBHelper.GetAllActiveProductsAndSkus();
			return Tuple.Create(
				spResults.Item1.Select(pdb => InitializeProduct(pdb)).ToList(),
				spResults.Item2.Select(sdb => InitializeSkuInfo(sdb)).ToList());
		}

		#region Info-DBEntity Conversions

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
				BillingFrequency = (BillingFrequencyEnum)sku.BillingFrequency,
				Name = sku.Name,
				Price = sku.Price,
				ProductId = sku.ProductId,
				SkuId = sku.SkuId,
				SubscriptionId = sku.SubscriptionId,
				UserLimit = sku.UserLimit,
				Description = sku.Description
			};
		}

		/// <summary>
		/// Translates a <see cref="SubscriptionRoleDBEntity"/> into a <see cref="ProductRole"/>.
		/// </summary>
		/// <param name="subscriptionRole">SubscriptionRoleDBEntity instance.</param>
		/// <returns>SubscriptionRole instance.</returns>
		public static ProductRole InitializeSubscriptionRoleInfo(SubscriptionRoleDBEntity subscriptionRole)
		{
			if (subscriptionRole == null)
			{
				return null;
			}

			return new ProductRole
			{
				Name = subscriptionRole.Name,
				ProductRoleId = subscriptionRole.ProductRoleId,
				ProductId = subscriptionRole.ProductId
			};
		}

		#endregion Info-DBEntity Conversions
	}
}
