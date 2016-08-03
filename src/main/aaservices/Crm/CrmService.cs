//------------------------------------------------------------------------------
// <copyright file="CrmService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using AllyisApps.DBModel;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using AllyisApps.Services.Account;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.Utilities;
using Stripe;

namespace AllyisApps.Services.Crm
{
	/// <summary>
	/// Services for Cutomer Relationship Management related functions (billing, subscriptions).
	/// </summary>
	public partial class CrmService : BaseService
	{
		/// <summary>
		/// Authorization in use for select methods.
		/// </summary>
		private AuthorizationService authorizationService;

		/// <summary>
		/// Initializes a new instance of the <see cref="CrmService"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public CrmService(string connectionString) : base(connectionString)
		{
			this.authorizationService = new AuthorizationService(connectionString);
		}

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
		/// Sets the UserContext.
		/// </summary>
		/// <param name="userContext">The UserContext.</param>
		public new void SetUserContext(UserContext userContext)
		{
			base.SetUserContext(userContext);
			this.authorizationService.SetUserContext(userContext);
		}

		/// <summary>
		/// Gets a <see cref="CustomerInfo"/>.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>The customer entity.</returns>
		public CustomerInfo GetCustomer(int customerId)
		{
			CustomerDBEntity dbe = DBHelper.GetCustomerInfo(customerId);
			if (dbe == null)
			{
				return null;
			}

			return new CustomerInfo
			{
				CustomerId = dbe.CustomerId,
				Name = dbe.Name,
				Address = dbe.Address,
				City = dbe.City,
				State = dbe.State,
				Country = dbe.Country,
				PostalCode = dbe.PostalCode,
				ContactEmail = dbe.ContactEmail,
				ContactPhoneNumber = dbe.ContactPhoneNumber,
				FaxNumber = dbe.FaxNumber,
				Website = dbe.Website,
				EIN = dbe.EIN,
				CreatedUTC = dbe.CreatedUTC,
				OrganizationId = dbe.OrganizationId
			};
		}

		/// <summary>
		/// Creates a customer.
		/// </summary>
		/// <param name="customer">Customer info.</param>
		/// <returns>Customer id.</returns>
		public int? CreateCustomer(CustomerInfo customer)
		{
			if (this.authorizationService.Can(Actions.CoreAction.EditCustomer) && customer != null)
			{
				CustomerDBEntity dbe = new CustomerDBEntity
				{
					CustomerId = customer.CustomerId,
					Name = customer.Name,
					Address = customer.Address,
					City = customer.City,
					State = customer.State,
					Country = customer.Country,
					PostalCode = customer.PostalCode,
					ContactEmail = customer.ContactEmail,
					ContactPhoneNumber = customer.ContactPhoneNumber,
					FaxNumber = customer.FaxNumber,
					Website = customer.Website,
					EIN = customer.EIN,
					CreatedUTC = customer.CreatedUTC,
					OrganizationId = customer.OrganizationId
				};

				return DBHelper.CreateCustomerInfo(dbe);
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
			if (this.authorizationService.Can(Actions.CoreAction.EditCustomer) && customer != null)
			{
				CustomerDBEntity dbe = new CustomerDBEntity
				{
					CustomerId = customer.CustomerId,
					Name = customer.Name,
					Address = customer.Address,
					City = customer.City,
					State = customer.State,
					Country = customer.Country,
					PostalCode = customer.PostalCode,
					ContactEmail = customer.ContactEmail,
					ContactPhoneNumber = customer.ContactPhoneNumber,
					FaxNumber = customer.FaxNumber,
					Website = customer.Website,
					EIN = customer.EIN,
					CreatedUTC = customer.CreatedUTC,
					OrganizationId = customer.OrganizationId
				};

				DBHelper.UpdateCustomer(dbe);
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
			if (this.authorizationService.Can(Actions.CoreAction.EditCustomer))
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
					list.Add(new CustomerInfo
					{
						CustomerId = dbe.CustomerId,
						Name = dbe.Name,
						Address = dbe.Address,
						City = dbe.City,
						State = dbe.State,
						Country = dbe.Country,
						PostalCode = dbe.PostalCode,
						ContactEmail = dbe.ContactEmail,
						ContactPhoneNumber = dbe.ContactPhoneNumber,
						FaxNumber = dbe.FaxNumber,
						Website = dbe.Website,
						EIN = dbe.EIN,
						CreatedUTC = dbe.CreatedUTC,
						OrganizationId = dbe.OrganizationId
					});
				}
			}

			return list;
		}

		/// <summary>
		/// Edits or creates billing information for the current chosen organization.
		/// </summary>
		/// <param name="stripeToken">The stripe token being used for this charge.</param>
		/// <param name="stripeEmail">Customer email address.</param>
		public void UpdateBillingInfo(string stripeToken, string stripeEmail)
		{
			#region Validation
			if (string.IsNullOrEmpty(stripeToken))
			{
				throw new ArgumentNullException("stripeToken", "Stripe token must have a value.");
			}

			if (string.IsNullOrEmpty(stripeEmail))
			{
				throw new ArgumentNullException("stripeEmail", "Email address must have a value.");
			}
			else if (!AccountService.IsEmailAddressValid(stripeEmail))
			{
				throw new FormatException("Email address must be in a valid format.");
			}
			#endregion

			//// Either get the existing stripe information for the org or create some if the org has none
			string orgCustomer = this.GetOrgCustomer();
			if (orgCustomer == null)
			{
				StripeToken t = this.GenerateToken(stripeToken);
				StripeCustomer customer = this.CreateStripeCustomer(t, stripeEmail);
				this.AddOrgCustomer(customer.Id);
				this.AddBillingHistory("Adding stripe customer data", null);
			}
			else
			{
				StripeToken t = this.GenerateToken(stripeToken);
				StripeWrapper.UpdateStripeCustomer(t, orgCustomer);
				this.AddBillingHistory("Updating stripe customer data", null);
			}
		}

		/// <summary>
		/// Generates the stripe token.
		/// </summary>
		/// <param name="id">The token id.</param>
		/// <returns>A token.</returns>
		[CLSCompliant(false)]
		public StripeToken GenerateToken(string id)
		{
			var tokenService = new StripeTokenService();
			return tokenService.Get(id);
		}

		/// <summary>
		/// Creates a stripe customer.
		/// </summary>
		/// <param name="t">The stripe token for creating the customer.</param>
		/// <param name="email">The user's email.</param>
		/// <returns>A new StripeCustomer.</returns>
		[CLSCompliant(false)]
		public StripeCustomer CreateStripeCustomer(StripeToken t, string email)
		{
			#region Validation
			if (t == null)
			{
				throw new ArgumentNullException("t", "Stripe token must not be null.");
			}

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email", "Email address must have a value.");
			}
			else if (!AccountService.IsEmailAddressValid(email))
			{
				throw new FormatException("Email address must be in a valid format.");
			}
			#endregion

			return StripeWrapper.CreateCustomer(email, t.Id);
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
			if (this.authorizationService.Can(Actions.CoreAction.EditOrganization))
			{
				DBHelper.RemoveBilling(UserContext.ChosenOrganizationId);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets the stripe customer Id for the current organization.
		/// </summary>
		/// <returns>The customer ID as a string.</returns>
		public string GetOrgCustomer()
		{
			return DBHelper.GetOrgCustomer(UserContext.ChosenOrganizationId);
		}

		/// <summary>
		/// Adds a stripe customer for the current organization.
		/// </summary>
		/// <param name="stripeTokenCustomerId">Stripe token customer id.</param>
		public void AddOrgCustomer(string stripeTokenCustomerId)
		{
			if (string.IsNullOrEmpty(stripeTokenCustomerId))
			{
				throw new ArgumentNullException("stripeTokenCustomerId", "Stripe token customer id must have a value.");
			}

			DBHelper.AddOrgCustomer(UserContext.ChosenOrganizationId, stripeTokenCustomerId);
		}

		/// <summary>
		/// Creates a customer subscription plan and adds a monthly subscription for the current organization.
		/// </summary>
		/// <param name="amount">Price of subscription.</param>
		/// <param name="customer">The StripeCustomer.</param>
		/// <param name="numberOfUsers">Number of subscription users.</param>
		/// <param name="productId">Product Id.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <returns>Subscription Id.</returns>
		[CLSCompliant(false)]
		public string AddCustomerSubscriptionPlan(int amount, StripeCustomer customer, int numberOfUsers, int productId, string planName)
		{
			#region Validation
			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException("amount", "Price cannot be negative.");
			}

			if (customer == null)
			{
				throw new ArgumentNullException("customer", "Customer cannot be null.");
			}

			if (numberOfUsers < 0)
			{ // TODO: Figure out if this can be 0 or not
				throw new ArgumentOutOfRangeException("numberOfUsers", "Number of users cannot be negative.");
			}

			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}
			#endregion

			string planId = StripeWrapper.Subscription("month", amount, customer, planName);
			return DBHelper.AddCustomerSubscription(customer.Id, planId, amount, numberOfUsers, productId, UserContext.ChosenOrganizationId);
		}

		/// <summary>
		/// Updates a customer subscription.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id, as a string.</param>
		/// <param name="amount">Price of subscription.</param>
		/// <param name="customer">The StripeCustomer.</param>
		/// <param name="numberOfUsers">Number of Users.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <returns>The customer subscription.</returns>
		[CLSCompliant(false)]
		public string UpdateSubscriptionPlan(string subscriptionId, int amount, StripeCustomer customer, int numberOfUsers, string planName)
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

			if (customer == null)
			{
				throw new ArgumentNullException("customer", "Customer cannot be null.");
			}

			if (numberOfUsers < 0)
			{ // TODO: Figure out if this can be 0 or not
				throw new ArgumentOutOfRangeException("numberOfUsers", "Number of users cannot be negative.");
			}
			#endregion

			string s = StripeWrapper.SubscriptionUpdate(subscriptionId.Trim(), amount, "month", customer, planName);
			return DBHelper.UpdateSubscriptionPlan(customer.Id, subscriptionId, amount, numberOfUsers);
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

		// TODO: Caching for ProductInfo shared by the two methods below

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
		public string GetSubscriptionId(string customerId)
		{
			if (string.IsNullOrEmpty(customerId))
			{
				throw new ArgumentNullException("customerId", "Customer id must have a value.");
			}

			return DBHelper.GetSubscriptionPlan(UserContext.ChosenOrganizationId, customerId);
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

			return DBHelper.GetUsersWithSubscriptionToProductInOrganization(orgId, productId).Select(u => BusinessObjectsHelper.InitializeUserInfo(u));
		}

		/// <summary>
		/// Gets a <see cref="SkuInfo"/> for a given product in the current organization.
		/// </summary>
		/// <param name="productId">Product Id.</param>
		/// <returns>A SkuInfo.</returns>
		public SkuInfo GetSubscriptionByOrgAndProduct(int productId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			SkuDBEntity sku = DBHelper.GetSubscriptionByOrgAndProduct(UserContext.ChosenOrganizationId, productId);
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
				BillingFrequency = sku.BillingFrequency,
				SubscriptionId = sku.SubscriptionId
			};
		}

		/// <summary>
		/// Gets a <see cref="SubscriptionInfo"/> for a given product in the current organization.
		/// </summary>
		/// <param name="productId">Product Id.</param>
		/// <returns>A SubscriptionDBEntity.</returns>
		public SubscriptionInfo CheckSubscription(int productId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product Id cannot be 0 or negative.");
			}

			SubscriptionDBEntity si = DBHelper.CheckSubscription(UserContext.ChosenOrganizationId, productId);
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
		/// Gets a list of <see cref="SubscriptionDisplayInfo"/>s with subscription, organization, and role information for the current user.
		/// </summary>
		/// <returns>List of SubscriptionDisplayInfos.</returns>
		public IEnumerable<SubscriptionDisplayInfo> GetUserSubscriptionOrganizationList()
		{
			return DBHelper.GetUserSubscriptionOrganizationList(UserContext.UserId).Select(s => BusinessObjectsHelper.InitializeSubscriptionDisplayInfo(s));
		}

		/// <summary>
		/// Gets a list of <see cref="SubscriptionDisplayInfo"/>s for all subscriptions in the chosen organization.
		/// </summary>
		/// <returns>List of SubscriptionDisplayInfos.</returns>
		public IEnumerable<SubscriptionDisplayInfo> GetSubscriptionsDisplay()
		{
			return DBHelper.Instance.GetSubscriptionsDisplayByOrg(UserContext.ChosenOrganizationId).Select(s => BusinessObjectsHelper.InitializeSubscriptionDisplayInfo(s));
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
		/// <param name="productId">Product Id.</param>
		/// <returns>A list of SkuInfo objects based on given productID.</returns>
		public IEnumerable<SkuInfo> GetSkuForProduct(int productId)
		{
			if (productId <= 0)
			{
				throw new ArgumentOutOfRangeException("productId", "Product ID cannot be 0 or negative.");
			}

			IEnumerable<SkuDBEntity> skus = DBHelper.GetSkuforProduct(productId);
			List<SkuInfo> list = new List<SkuInfo>();
			foreach (SkuDBEntity sku in skus)
			{
				if (sku != null)
				{
					list.Add(new SkuInfo
					{
						SkuId = sku.SkuId,
						ProductId = sku.ProductId,
						Name = sku.Name,
						Price = sku.Price,
						UserLimit = sku.UserLimit,
						BillingFrequency = sku.BillingFrequency
					});
				}
			}

			return list;
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
			#endregion

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

			if (productId == CrmService.GetProductIdByName("TimeTracker"))
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
			#endregion

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
	}
}