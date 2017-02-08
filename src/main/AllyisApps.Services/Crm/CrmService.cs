//------------------------------------------------------------------------------
// <copyright file="CrmService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;

using AllyisApps.DBModel;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.Services.Utilities;
using AllyisApps.DBModel.Auth;

namespace AllyisApps.Services { 
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
				OrganizationId = dbe.OrganizationId,
                CustomerOrgId = dbe.CustomerOrgId
			};
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
					OrganizationId = customer.OrganizationId,
                    CustomerOrgId = customer.CustomerOrgId
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
			if (this.Can(Actions.CoreAction.EditCustomer) && customer != null)
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
					OrganizationId = customer.OrganizationId,
                    CustomerOrgId = customer.CustomerOrgId
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
			return DBHelper.GetUserSubscriptionOrganizationList(UserContext.UserId).Select(s => InitializeSubscriptionDisplayInfo(s)).ToList();
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
        /// Gets a list of <see cref="SubscriptionDisplayInfo"/>s for all subscriptions in the given organizaiton.
        /// </summary>
        /// <param name="orgId">Organization Id.</param>
        /// <returns>List of SubscriptionDisplayInfos.</returns>
        public IEnumerable<SubscriptionDisplayInfo> GetSubscriptionsDisplayByOrg(int orgId)
        {
            return DBHelper.Instance.GetSubscriptionsDisplayByOrg(orgId).Select(s => InitializeSubscriptionDisplayInfo(s));
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
        /// Gets the next recommended customer id, by incrementing the highest one that currently exists, or returning all 0's if none exist.
        /// </summary>
        /// <returns>The next logical unique customer id.</returns>
        public string GetRecommendedCustomerId()
        {
            var customers = this.GetCustomerList(this.UserContext.ChosenOrganizationId);
            if (customers.Count() > 0)
            {
                return new string(this.IncrementAlphanumericCharArray(customers.Select(c => c.CustomerOrgId).ToList().OrderBy(id => id).LastOrDefault().ToCharArray()));
            }
            else
            {
                return "0000000000000000"; // 16 character max, arbitrary default id
            }
        }


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
		/// <returns>SubscriptionDisplay instance.</returns>
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
	}
}