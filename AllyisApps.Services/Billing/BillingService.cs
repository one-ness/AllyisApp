//------------------------------------------------------------------------------
// <copyright file="BillingService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Billing;
using AllyisApps.Lib;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using System.Threading.Tasks;
using AllyisApps.Services.Cache;

namespace AllyisApps.Services
{
	/// <summary>
	/// Services for Billing related functions (billing, subscriptions).
	/// </summary>
	public partial class AppService : BaseService
	{
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
		/// <param name="orgId">.</param>
		[CLSCompliant(false)]
		async public Task UpdateBillingInfo(string billingServicesEmail, BillingServicesToken token, int orgId)
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
			BillingServicesCustomerId customerId = await this.GetOrgBillingServicesCustomerId(orgId);
			if (customerId == null)
			{
				string serviceType = "Stripe";
				BillingServicesHandler handler = new BillingServicesHandler(serviceType);
				customerId = handler.CreateCustomer(billingServicesEmail, token);
				this.CreateStripeOrganizationCustomer(customerId, null, orgId);
				// this.AddBillingHistory(string.Format("Adding {0} customer data", serviceType), null);
			}
			else
			{
				string serviceType = "Stripe";
				BillingServicesHandler handler = new BillingServicesHandler(serviceType);
				handler.UpdateCustomer(customerId, token);
				this.AddBillingHistory(string.Format("Updating {0} customer data", serviceType), null, orgId);
			}
		}

		/// <summary>
		/// Deletes a subscription plan for the current organization with the given stripe customer id, if one exists.
		/// On success, adds a billing history item with the given sku id and description (and current user Id).
		/// </summary>
		/// <param name="stripeCustomerId">Stripe customer id.</param>
		/// <param name="skuId">Sku it for billing history item.</param>
		/// <param name="description">Description for billing history item.</param>
		/// <param name="orgId">.</param>
		/// <returns>The subscription plan id of the deleted subscription plan, or null if none found.</returns>
		async public Task<string> DeleteSubscriptionPlanAndAddHistory(string stripeCustomerId, SkuIdEnum skuId, string description, int orgId)
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

			return await DBHelper.DeleteSubscriptionPlanAndAddHistory(orgId, stripeCustomerId, UserContext.UserId, (int)skuId, description);
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
		/// <param name="orgId">.</param>
		async public void AddBillingHistory(string description, int? skuId, int orgId)
		{
			if (skuId.HasValue && skuId.Value <= 0)
			{
				throw new ArgumentOutOfRangeException("skuId", "Sku Id cannot be 0 or negative. Use null instead of 0.");
			}

			await DBHelper.AddBillingHistory(description, orgId, UserContext.UserId, skuId);
		}

		/// <summary>
		/// Removes billing from the current organization.
		/// </summary>
		/// <returns>Returns false if authorization fails.</returns>
		public bool RemoveBilling(int orgId)
		{
			this.CheckOrgAction(OrgAction.EditBilling, orgId);
			DBHelper.RemoveBilling(orgId);
			return true;
		}

		/// <summary>
		/// Gets the stripe customer Id for the current organization.
		/// </summary>
		/// <returns>The customer Id.</returns>
		[CLSCompliant(false)]
		async public Task<BillingServicesCustomerId> GetOrgBillingServicesCustomerId(int orgId)
		{
			string id = await DBHelper.GetOrgCustomer(orgId);
			return new BillingServicesCustomerId(id);
		}

		/// <summary>
		/// Adds a stripe customer for the current organization, and adds a billing history item.
		/// </summary>
		/// <param name="customerId">Billing Services customer id.</param>
		/// <param name="selectedSku">Selected sku id, for the billing history item.</param>
		/// <param name="orgId">.</param>
		[CLSCompliant(false)]
		async public void CreateStripeOrganizationCustomer(BillingServicesCustomerId customerId, int? selectedSku, int orgId)
		{
			if (customerId == null)
			{
				throw new ArgumentNullException("customerId", "Billing Services customer id must have a value.");
			}

			await DBHelper.CreateStripeOrganizationCustomer(orgId, UserContext.UserId, customerId.Id, selectedSku, "Adding stripe customer data.");
		}

		/// <summary>
		/// Creates a customer subscription plan and adds a monthly subscription for the current organization.
		/// </summary>
		/// <param name="amount">Price of subscription.</param>
		/// <param name="customerId">The StripeCustomer.</param>
		/// <param name="productId">Product Id.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <param name="skuId">Selected sku id, for the billing history item.</param>
		/// <param name="orgId">.</param>
		[CLSCompliant(false)]
		public void AddCustomerSubscriptionPlan(int amount, BillingServicesCustomerId customerId, int productId, string planName, int? skuId, int orgId)
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

			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			#endregion Validation

			string service = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(service);
			BillingServicesSubscriptionId subId = handler.CreateSubscription(amount, "month", planName, customerId);
			DBHelper.AddCustomerSubscription(customerId.Id, subId.Id, amount, 0, productId, orgId, UserContext.UserId, skuId, string.Format("Adding new subscription data for {0}.", planName));
		}

		/// <summary>
		/// Updates a customer subscription, and adds a billing history item.
		/// </summary>
		/// <param name="amount">Price of subscription.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <param name="subscriptionId">Subscription Id, as a string.</param>
		/// <param name="customerId">The Billing Services Customer Id.</param>
		/// <param name="skuId">Selected sku id, for the billing history item.</param>
		/// <param name="orgId">.</param>
		[CLSCompliant(false)]
		public void UpdateSubscriptionPlan(int amount, string planName, string subscriptionId, BillingServicesCustomerId customerId, int? skuId, int orgId)
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

			#endregion Validation

			string serviceType = "Stripe";
			BillingServicesHandler handler = new BillingServicesHandler(serviceType); // TODO: make this check the database instead of hardcoding Stripe

			handler.UpdateSubscription(amount, "month", planName, subscriptionId.Trim(), customerId);
			DBHelper.UpdateSubscriptionPlan(customerId.Id, subscriptionId, amount, 0, orgId, UserContext.UserId, skuId, string.Format("Updating subscription data for {0}", planName));
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
		/// <param name="newProductRole">Product role to assign.</param>
		/// <param name="orgId">The Organization Id.</param>
		/// <param name="productId">The subscribed Product Id.</param>
		/// <returns>A tuple containing the number of updated users and the number of added users.</returns>
		async public Task<UpdateSubscriptionUserRolesResuts> UpdateSubscriptionUserRoles(List<int> userIds, int newProductRole, int orgId, int productId)
		{
			#region Validation

			if (!Enum.IsDefined(typeof(TimeTrackerRole), newProductRole) && !Enum.IsDefined(typeof(ExpenseTrackerRole), newProductRole))
			{
				throw new ArgumentOutOfRangeException("newProductRole", "Product role must match a value of the ProductRoleIdEnum enum.");
			}

			if (!Enum.IsDefined(typeof(ProductIdEnum), productId))
			{
				throw new ArgumentOutOfRangeException("newProductRole", "Product role must match a value of the ProductRoleIdEnum enum.");
			}

			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentException("userIds", "No user ids provided.");
			}

			#endregion Validation

			// TODO: split updating user roles and creating new sub users
			var UpdatedRows = await DBHelper.UpdateSubscriptionUserRoles(userIds, orgId, newProductRole, productId);
			return new UpdateSubscriptionUserRolesResuts()
			{
				UsersChanged = UpdatedRows.Item1,
				UsersAddedToSubscription = UpdatedRows.Item2
			};
		}

		/// <summary>Deletes the given users in the given organization's subscription</summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="orgId">The Organization Id.</param>
		/// <param name="productId">The subscribed Product Id.</param>
		/// <returns>count of deleted users.</returns>
		public void DeleteSubscriptionUsers(List<int> userIds, int orgId, int productId)
		{
			#region Validation

			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentException("userIds", "No user ids provided.");
			}

			#endregion Validation

			DBHelper.DeleteSubscriptionUsers(userIds, orgId, productId);
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
				ProductId = (ProductIdEnum)product.ProductId,
				ProductName = product.ProductName,
				ProductDescription = product.Description,
				AreaUrl = product.AreaUrl
			};
		}

		/// <summary>
		/// Gets a <see cref="Subscription"/>.
		/// </summary>
		/// <param name="subscriptionId">Product Id.</param>
		/// <returns>A SubscriptionDBEntity.</returns>
		async public Task<Subscription> GetSubscription(int subscriptionId)
		{
			if (subscriptionId <= 0) throw new ArgumentOutOfRangeException("subscriptionId");

			int orgId = 0;
			this.CheckSubscriptionAction(OrgAction.ReadSubscription, subscriptionId, out orgId);

			// get from db
			var sub = await this.DBHelper.GetSubscriptionDetailsById(subscriptionId);
			if (sub == null) throw new InvalidOperationException("subscriptionId");

			// copy to subscription
			var result = new Subscription();
			result.ProductAreaUrl = sub.ArealUrl;
			result.SkuIconUrl = sub.IconUrl;
			result.IsActive = sub.IsActive;
			result.NumberOfUsers = sub.NumberOfUsers;
			result.OrganizationId = sub.OrganizationId;
			result.ProductDescription = sub.ProductDescription;
			result.ProductId = (ProductIdEnum)sub.ProductId;
			result.ProductName = sub.ProductName;
			result.PromoExpirationDateUtc = sub.PromoExpirationDateUtc;
			result.SkuDescription = sub.SkuDescription;
			result.SkuId = (SkuIdEnum)sub.SkuId;
			result.SkuName = sub.SkuName;
			result.SubscriptionCreatedUtc = sub.SubscriptionCreatedUtc;
			result.SubscriptionId = subscriptionId;
			result.SubscriptionName = sub.SubscriptionName;

			// return sub
			return result;
		}

		public List<SubscriptionUser> GetSubscriptionUsers(int subscriptionId)
		{
			var subUsers = DBHelper.GetSubscriptionUsersBySubscriptionId(subscriptionId);
			return subUsers.Select(su => InitializeSubscriptionUser(su)).ToList();
		}

		/// <summary>
		/// Get Name of Subscription
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		async public Task<string> GetSubscriptionName(int subscriptionId)
		{
			return await DBHelper.GetSubscriptionName(subscriptionId);
		}

		/// <summary>
		/// Gets a subscription Id for a customer of the current organization.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <param name="orgId">.</param>
		/// <returns>Subscription Id.</returns>
		[CLSCompliant(false)]
		public string GetSubscriptionId(BillingServicesCustomerId customerId, int orgId)
		{
			if (customerId == null)
			{
				throw new ArgumentNullException("customerId", "Customer id must have a value.");
			}

			return DBHelper.GetSubscriptionPlan(orgId, customerId.Id);
		}

		/// <summary>
		/// Gets a list of non-zero subscription plan prices used by the current organization.
		/// </summary>
		/// <returns>List of prices, as ints.</returns>
		async public Task<IEnumerable<int>> GetSubscriptionPlanPrices(int orgId)
		{
			return await DBHelper.GetSubscriptionPlanPrices(orgId);
		}

		/// <summary>
		/// Deletes a subscription plan.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <param name="customerId">The Billing Services Customer Id.</param>
		/// <param name="skuId">Selected sku id, for the billing history item.</param>
		/// <param name="orgId">.</param>
		[CLSCompliant(false)]
		async public void DeleteSubscriptionPlan(string subscriptionId, BillingServicesCustomerId customerId, int? skuId, int orgId)
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
			// DBHelper.DeleteSubscriptionPlan(subscriptionId);
			await DBHelper.DeleteSubscriptionPlanAndAddHistory(orgId, customerId.Id, UserContext.UserId, skuId, "Switching to free subscription, canceling stripe susbcription");
		}

		/// <summary>
		/// delete the given subscription
		/// </summary>
		async public Task<int> DeleteSubscription(int subscriptionId)
		{
			if (subscriptionId <= 0) throw new ArgumentOutOfRangeException("subscriptionId");

			// TODO: after deleting (setting isactive = 0) subscription:
			/*
			 * - bill for the last partial month
			 * - stop future billing
			 */
			var sub = await this.GetSubscription(subscriptionId);
			this.CheckOrgAction(OrgAction.DeleteSubscritpion, sub.OrganizationId);
			this.DBHelper.DeleteSubscription(subscriptionId);
			return sub.OrganizationId;
		}

		/// <summary>
		/// Gets a list of <see cref="User"/>'s for users in the organization with subscriptions to the given product.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="productId">Product Id.</param>
		/// <returns>A list of User's for users in the organization with subscriptions to the given product.</returns>
		async public Task<IEnumerable<User>> GetUsersWithSubscriptionToProductInOrganization(int orgId, int productId)
		{
			if (orgId <= 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be 0 or negative.");
			}

			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			var results = await DBHelper.GetUsersWithSubscriptionToProductInOrganization(orgId, productId);
			return results.Select(u => InitializeUser(u, false));
		}

		/// <summary>
		/// Gets a list of <see cref="SubscriptionDisplay"/>s for all subscriptions in the chosen organization.
		/// </summary>
		/// <returns>List of SubscriptionDisplayInfos.</returns>
		public IEnumerable<Subscription> GetSubscriptionsByOrg(int organizationId)
		{
			return this.DBHelper.GetSubscriptionsDisplayByOrg(organizationId).Select(s => InitializeSubscription(s)).ToList();
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
					ProductRoleName = s.ProductRoleName,
					ProductRoleId = s.ProductRoleId
				};
			});
		}

		/// <summary>
		/// Gets a <see cref="SkuInfo"/>.
		/// </summary>
		/// <param name="skuId">Sku Id.</param>
		/// <returns>The SKU details.</returns>
		public SkuInfo GetSkuDetails(SkuIdEnum skuId)
		{
			if (skuId <= 0)
			{
				throw new ArgumentOutOfRangeException("skuId", "Sku Id cannot be 0 or negative.");
			}

			SkuDBEntity sku = DBHelper.GetSkuDetails((int)skuId);
			if (sku == null)
			{
				return null;
			}

			return new SkuInfo
			{
				SkuId = (SkuIdEnum)sku.SkuId,
				ProductId = (ProductIdEnum)sku.ProductId,
				SkuName = sku.SkuName,
				Price = sku.CostPerBlock,
				UserLimit = sku.UserLimit,
				BillingFrequency = (BillingFrequencyEnum)sku.BillingFrequency,
				Description = sku.Description,
				IconUrl = sku.IconUrl,
			};
		}

		/// <summary>
		/// Removes the billing subscription plan for the current organization and adds a billing history
		/// item. If the supllied subscription id has a value, also removes that subscription and returns
		/// a message to place in a notification.
		/// </summary>
		/// <param name="SelectedSku">Sku id, for the billing history item.</param>
		/// <param name="subscriptionId">Subscription id to unsubscribe from.</param>
		/// <returns>A notification string, or null.</returns>
		async public Task<string> UnsubscribeAndRemoveBillingSubscription(SkuIdEnum SelectedSku, int? subscriptionId)
		{
			var subscripiton = await this.GetSubscription(subscriptionId.Value);
			var orgId = subscripiton.OrganizationId;

			BillingServicesCustomer custId = this.RetrieveCustomer(await this.GetOrgBillingServicesCustomerId(orgId));
			if (custId != null)
			{
				await this.DeleteSubscriptionPlanAndAddHistory(custId.Id.Id, SelectedSku, "Unsubscribing from product.", orgId);
				// string subscriptionPlanId = this.DeleteSubscriptionPlanAndAddHistory(custId.Id.Id, SelectedSku, "Unsubscribing from product.");
				// if (subscriptionPlanId != null)
				//{
				//	this.DeleteSubscription(custId.Id, subscriptionPlanId);
				//}
			}

			if (subscriptionId != null)
			{
				string skuName = await this.Unsubscribe(subscriptionId.Value);
				//return string.Format("{0} has been unsubscribed from {1}.", UserContext.UserSubscriptions[subscriptionId.Value].OrganizationName, skuName);
			}

			return null;
		}

		/// <summary>
		/// Subscribes the current organization to a product or updates the organization's subscription to the product.
		/// </summary>
		public async Task Subscribe(int organizationId, SkuIdEnum skuId, string subscriptionName)
		{
			if (organizationId <= 0) throw new ArgumentOutOfRangeException();
			if (string.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException();

			// get all subscriptions of the given organization
			var collection = await this.GetSubscriptionsAsync(organizationId);
			foreach (var item in collection)
			{
				// is it an existing sku?
				if (item.SkuId == skuId)
				{
					// yes, already subscribed
					return;
				}
				else
				{
					// no, get the product of this subscription
					var pid = item.ProductId;
					Product product = null;
					if (!CacheContainer.ProductsCache.TryGetValue(pid, out product) || !product.IsActive)
					{
						// inactive or invalid product
						throw new InvalidOperationException("You selected an invalid product to subscribe to.");
					}

					// is it a sku of this product?
					List<Sku> skus = null;
					if (CacheContainer.SkusCache.TryGetValue(pid, out skus))
					{
						var sku = skus.Where(x => x.IsActive && x.SkuId == skuId).FirstOrDefault();
						if (sku != null)
						{
							// yes, user is subscribing to another sku of an existing subscription (i.e., product)
							this.UpdateSubscriptionSkuAndName(organizationId, item.SubscriptionId, subscriptionName, skuId);
							return;
						}
					}
				}
			}

			// reached here indicates user is subscribing to a new product with a new sku
			var selectedSku = CacheContainer.AllSkusCache.Where(x => x.IsActive && x.SkuId == skuId).FirstOrDefault();
			if (selectedSku == null)
			{
				// inactive or invalid sku id
				throw new InvalidOperationException("You selected an invalid sku to subscribe to.");
			}

			// get product for the sku
			Product selectedProduct = null;
			if (!CacheContainer.ProductsCache.TryGetValue(selectedSku.ProductId, out selectedProduct) || !selectedProduct.IsActive)
			{
				// inactive or invalid product
				throw new InvalidOperationException("You selected an invalid product to subscribe to.");
			}

			// set the creating user as highest level
			int productRoleId = 0;
			switch (selectedSku.ProductId)
			{
				case ProductIdEnum.ExpenseTracker:
					productRoleId = (int)ExpenseTrackerRole.SuperUser;
					break;

				case ProductIdEnum.StaffingManager:
					productRoleId = (int)StaffingManagerRole.Manager;
					break;

				case ProductIdEnum.TimeTracker:
					productRoleId = (int)TimeTrackerRole.Manager;
					break;

				default:
					// inactive or invalid product
					throw new InvalidOperationException("You selected an invalid product to subscribe to.");
			}

			this.InitializeSettingsForProduct(selectedSku.ProductId, organizationId);

			// create new subscription
			await this.DBHelper.CreateSubscription(organizationId, (int)skuId, subscriptionName, productRoleId);
		}

		async public Task UpdateSubscriptionName(int subscriptionId, string subscriptionName)
		{
			if (subscriptionId <= 0) throw new ArgumentOutOfRangeException("subscriptionId");
			if (string.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException("subscriptionName");

			var sub = await this.GetSubscription(subscriptionId);
			this.CheckOrgAction(OrgAction.EditSubscription, sub.OrganizationId);
			this.DBHelper.UpdateSubscriptionName(subscriptionId, subscriptionName);
		}

		async private void UpdateSubscriptionSkuAndName(int organizationId, int subscriptionId, string subscriptionName, SkuIdEnum skuId)
		{
			this.CheckOrgAction(OrgAction.EditSubscription, organizationId);
			this.DBHelper.UpdateSubscriptionSkuAndName(subscriptionId, subscriptionName, (int)skuId);
			await Task.Delay(0);
		}

		async public void CreateAndUpdateAndDeleteSubscriptionPlan(int productId, string productName, int selectedSku, int previousSku, int billingAmount, BillingServicesToken existingToken, bool addingBillingCustomer, string newBillingEmail, BillingServicesToken newBillingToken, int orgId)
		{
			BillingServicesCustomer customer;
			BillingServicesToken token;
			if (addingBillingCustomer)
			{
				BillingServicesCustomerId customerId = this.CreateBillingServicesCustomer(newBillingEmail, newBillingToken);

				this.CreateStripeOrganizationCustomer(customerId, null, orgId);
				customer = this.RetrieveCustomer(customerId);
				token = newBillingToken;
			}
			else
			{
				customer = this.RetrieveCustomer(await this.GetOrgBillingServicesCustomerId(orgId));
				token = existingToken;
			}

			if (billingAmount > 0)
			{
				BillingServicesCustomerId customerId = await this.GetOrgBillingServicesCustomerId(orgId);
				if (customerId == null)
				{
					customer = this.RetrieveCustomer(this.CreateBillingServicesCustomer(newBillingEmail, token));
					this.CreateStripeOrganizationCustomer(customer.Id, null, orgId);
				}
				else
				{
					customer = this.RetrieveCustomer(customerId);
				}

				string subscriptionId = this.GetSubscriptionId(customer.Id, orgId);

				if (subscriptionId == null)
				{
					this.AddCustomerSubscriptionPlan(billingAmount, customer.Id, productId, productName, selectedSku, orgId);
				}
				else
				{
					this.UpdateSubscriptionPlan(billingAmount, productName, subscriptionId, customer.Id, selectedSku, orgId);
				}
			}
			else
			{
				customer = this.RetrieveCustomer(await this.GetOrgBillingServicesCustomerId(orgId));

				if (customer != null)
				{
					// check if there is a subscription to cancel
					string subscriptionId = this.GetSubscriptionId(customer.Id, orgId);
					if (subscriptionId != null)
					{
						this.DeleteSubscriptionPlan(subscriptionId, customer.Id, selectedSku, orgId);
					}
				}
			}
		}

		/// <summary>
		/// Unsubscribes a subscription.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <return>The name of the sku for the removed subscription.</return>
		async public Task<string> Unsubscribe(int subscriptionId)
		{
			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			return await DBHelper.Unsubscribe(subscriptionId);
		}

		/// <summary>
		/// Creates default settings for a product.
		/// TODO: Is it possible to reduce hard code here?.
		/// </summary>
		/// <param name="productId">Product Id.</param>
		/// <param name="orgId">.</param>
		public void InitializeSettingsForProduct(ProductIdEnum productId, int orgId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			if (productId == ProductIdEnum.TimeTracker)
			{
				DBHelper.InitializeTimeTrackerSettings(orgId);
			}

			if (productId == ProductIdEnum.StaffingManager)
			{
				DBHelper.CreateStaffingSettings(orgId);
			}
		}

		/// <summary>
		/// Gets a product name via the subscription id associated with that product.
		/// </summary>
		/// <param name="subscriptionId">The subscription id for which you wish to know the product name.</param>
		/// <returns>The product name.</returns>
		async public Task<string> GetProductNameBySubscriptionId(int subscriptionId)
		{
			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			return await DBHelper.GetProductAreaBySubscription(subscriptionId);
		}

		/// <summary>
		/// Gets a list of <see cref="BillingHistoryItemInfo"/>s for the billing history of the current organization.
		/// </summary>
		/// <returns>List of billing history items.</returns>
		async public Task<IEnumerable<BillingHistoryItemInfo>> GetBillingHistory(int orgId)
		{
			var results = await DBHelper.GetBillingHistoryByOrg(orgId);
			return results.Select(i =>
			{
				return new BillingHistoryItemInfo
				{
					Date = i.Date,
					Description = i.Description,
					OrganizationId = i.OrganizationId,
					ProductId = i.ProductId,
					ProductName = i.ProductName,
					SkuId = i.SkuId,
					SkuName = i.SkuName,
					UserId = i.UserId,
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
		/// <param name="orgId">.</param>
		/// <param name="skuId">Product Id.</param>
		/// <returns>.</returns>
		public ProductSubscription GetProductSubscriptionInfo(int orgId, SkuIdEnum skuId)
		{
			if (skuId <= 0)
			{
				throw new ArgumentOutOfRangeException("skuId", "SKU Id cannot be 0 or negative.");
			}
			var spResults = DBHelper.GetProductSubscriptionInfo(orgId, (int)skuId);
			var product = InitializeProduct(spResults.Item1);
			//product.Skus = spResults.Item3.Select(sdb => InitializeSkuInfo(sdb)).ToList();

			var subscription = InitializeSubscription(spResults.Item2);
			if (subscription != null)
			{
				subscription.NumberOfUsers = spResults.Item5;
			}
			return new ProductSubscription(
				product,
				InitializeSubscription(spResults.Item2),
				spResults.Item3.Select(sdb => InitializeSkuInfo(sdb)).ToList(),
				spResults.Item4,
				spResults.Item5
			);
		}

		/// <summary>
		/// Returns a list of active products and each product's active skus.
		/// </summary>
		public List<Product> GetAllActiveProductsAndSkus()
		{
			// get only active products and with sku products
			var result = CacheContainer.ProductsCache.Values.Where(x => x.IsActive && x.Skus.Count > 0).ToList();

			// reduce to show products
			foreach (var item in result)
			{
				// reduce the skus list to only active skus
				item.Skus = item.Skus.Where(x => x.IsActive).ToList();
			}

			return result;
		}

		#region Info-DBEntity Conversions

		/// <summary>
		/// Translates a <see cref="SubscriptionDisplayDBEntity"/> into a <see cref="SubscriptionDisplay"/>.
		/// </summary>
		/// <param name="subscriptionDisplay">SubscriptionDisplayDBEntity instance.</param>
		/// <returns>SubscriptionDisplay instance.</returns>
		public static Subscription InitializeSubscription(SubscriptionDisplayDBEntity subscriptionDisplay)
		{
			if (subscriptionDisplay == null)
			{
				return null;
			}

			return new Subscription
			{
				ProductAreaUrl = subscriptionDisplay.AreaUrl,
				SubscriptionCreatedUtc = subscriptionDisplay.CreatedUtc,
				NumberOfUsers = subscriptionDisplay.NumberOfUsers,
				OrganizationId = subscriptionDisplay.OrganizationId,
				ProductId = (ProductIdEnum)subscriptionDisplay.ProductId,
				ProductName = subscriptionDisplay.ProductName,
				SkuId = (SkuIdEnum)subscriptionDisplay.SkuId,
				SkuName = subscriptionDisplay.SkuName,
				SubscriptionId = subscriptionDisplay.SubscriptionId,
				SubscriptionName = subscriptionDisplay.SubscriptionName,
			};
		}

		/// <summary>
		/// Translates a <see cref="SubscriptionDBEntity"/> into a <see cref="Subscription"/>.
		/// </summary>
		/// <param name="subscription">SubscriptionDBEntity instance.</param>
		/// <returns>SubscriptionInfo instance.</returns>
		public static Subscription InitializeSubscription(SubscriptionDBEntity subscription)
		{
			if (subscription == null)
			{
				return null;
			}

			return new Subscription
			{
				SubscriptionCreatedUtc = subscription.SubscriptionCreatedUtc,
				IsActive = subscription.IsActive,
				SubscriptionName = subscription.SubscriptionName,
				NumberOfUsers = subscription.NumberOfUsers,
				OrganizationId = subscription.OrganizationId,
				SkuId = (SkuIdEnum)subscription.SkuId,
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
				SkuName = sku.SkuName,
				Price = sku.CostPerBlock,
				ProductId = (ProductIdEnum)sku.ProductId,
				SkuId = (SkuIdEnum)sku.SkuId,
				UserLimit = sku.UserLimit,
				Description = sku.Description,
				IconUrl = sku.IconUrl
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
				ProductRoleName = subscriptionRole.ProductRoleName,
				ProductRoleId = subscriptionRole.ProductRoleId,
				ProductId = (ProductIdEnum)subscriptionRole.ProductId
			};
		}

		#endregion Info-DBEntity Conversions
	}
}