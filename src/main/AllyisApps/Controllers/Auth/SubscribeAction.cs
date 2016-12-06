﻿//------------------------------------------------------------------------------
// <copyright file="SubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Subscription/Subscribe/ProductId=#.
		/// </summary>
		/// <param name="productId">The product being subscribed to.</param>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public ActionResult Subscribe(int productId)
		{
			int orgId = UserContext.ChosenOrganizationId;

			// ViewBag.Organizations = OrgService.GetOrganizationsWhereUserIsAdmin(UserContext.UserId);

			// ViewBag.Products = Services.Crm.CrmService.GetProductInfoList();

			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				ProductSubscriptionViewModel model = this.ConstructProductSubscriptionViewModel(productId);
				if (!model.IsValid)
				{
					return this.View(ViewConstants.Details, orgId);
				}

				Service.InitializeSettingsForProduct(model.ProductId);

				// SubscriptionInfo sub = CrmService.CheckSubscription(orgId, productId);

				model.CurrentUsers = Service.GetUsersWithSubscriptionToProductInOrganization(orgId, productId).Count();

				return this.View(ViewConstants.Subscribe, model);
			}

			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));

			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditSubscriptionsMessage), ControllerConstants.Subscription, ActionConstants.Subscribe));
		}

		/// <summary>
		/// Uses services to populate a <see cref="ProductSubscriptionViewModel"/> and returns it.
		/// </summary>
		/// <param name="productId">Product ID.</param>
		/// <returns>The ProductSubscriptionViewModel.</returns>
		[CLSCompliant(false)]
		public ProductSubscriptionViewModel ConstructProductSubscriptionViewModel(int productId)
		{
			ProductInfo productInfo = Service.GetProductById(productId);
			if (productInfo != null)
			{
				SubscriptionInfo currentSubscription = Service.CheckSubscription(productId);
				int selectedSku = currentSubscription == null ? 0 : currentSubscription.SkuId;

				return new ProductSubscriptionViewModel
				{
					IsValid = true,
					OrganizationId = UserContext.ChosenOrganizationId,
					ProductId = productInfo.ProductId,
					ProductName = productInfo.ProductName,
					ProductDescription = productInfo.ProductDescription,
					CurrentSubscription = currentSubscription,
					Skus = Service.GetSkuForProduct(productId),
					SelectedSku = selectedSku,
					SelectedSkuName = selectedSku > 0 ? Service.GetSkuDetails(selectedSku).Name : string.Empty,
					PreviousSku = selectedSku,
					CustomerId = Service.GetOrgBillingServicesCustomerId(),
					Token = new BillingServicesToken(Service.GetOrgBillingServicesCustomerId().ToString())
				};
			}

			return new ProductSubscriptionViewModel
			{
				IsValid = false
			};
		}

		/// <summary>
		/// Subscribe to a product.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="token">Response from the billing service.</param>
		/// <param name="billingServicesEmail">The billing email.</param>
		/// <param name="cost">The cost.</param>
		/// <returns>A page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[CLSCompliant(false)]
		public ActionResult Subscribe(ProductSubscriptionViewModel model, BillingServicesToken token, string billingServicesEmail, string cost)
		{
			if (!Service.Can(Actions.CoreAction.EditOrganization))
			{
				Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
				ViewBag.ErrorInfo = "Permission";

				return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditSubscriptionsMessage), ControllerConstants.Account, ActionConstants.Subscribe));
			}

			if (model == null)
			{
				return this.View(ViewConstants.Error, new HandleErrorInfo(new ArgumentNullException(@Resources.Errors.ModelNullMessage), ControllerConstants.Account, ActionConstants.Subscribe));
			}

			if (model.NumberOfUsers < Service.GetUsersWithSubscriptionToProductInOrganization(model.OrganizationId, model.ProductId).Count())
			{
				Notifications.Add(new BootstrapAlert("You must first remove users from your subscription before reducing the number of users.", Variety.Danger));

				return this.RedirectToAction(ActionConstants.Subscribe, new { productName = model.ProductId });
			}

			if (model.Billing.Amount == 0)
			{
				model.Billing.Amount = (model.NumberOfUsers - 500) * 100;
			}

			if (!(token == null) && (model.Token == null))
			{
				BillingServicesCustomerId customerId = Service.CreateBillingServicesCustomer(billingServicesEmail, token);

				Service.AddOrgCustomer(customerId);
				model.Billing.Customer = Service.RetrieveCustomer(customerId);
				model.Token = token;
				Service.AddBillingHistory("Adding stripe customer data", null);
			}
			else
			{
				model.Billing.Customer = Service.RetrieveCustomer(Service.GetOrgBillingServicesCustomerId());
			}

			if (model.Billing.Amount > 0) // Users >= 500 (the hardcoded free amount) will not trigger this
			{
				if ((token == null) && (model.Token == null))
				{
					return this.View(ViewConstants.AddBillingToSubscribe, model);
				}

				BillingServicesCustomerId customerId = Service.GetOrgBillingServicesCustomerId();
				if (customerId == null)
				{
					model.Billing.Customer = Service.RetrieveCustomer(Service.CreateBillingServicesCustomer(billingServicesEmail, token));
					Service.AddOrgCustomer(model.Billing.Customer.Id);
				}
				else
				{
					model.Billing.Customer = Service.RetrieveCustomer(customerId);
				}

				string subscriptionId = Service.GetSubscriptionId(model.Billing.Customer.Id);

				if (subscriptionId == null)
				{
					Service.AddCustomerSubscriptionPlan(model.Billing.Amount, model.Billing.Customer.Id, model.NumberOfUsers, model.ProductId, model.ProductName);
					Service.InitializeSettingsForProduct(model.ProductId);
					Service.AddBillingHistory(string.Format("Adding new subscription data for {0}.", model.ProductName), model.SelectedSku);
				}
				else
				{
					string test = Service.UpdateSubscriptionPlan(model.Billing.Amount, model.ProductName, model.NumberOfUsers, subscriptionId, model.Billing.Customer.Id);
					Service.AddBillingHistory(string.Format("Updating subscription data for {0}", model.ProductName), model.SelectedSku);
				}
			}
			else
			{
				try
				{
					model.Billing.Customer = Service.RetrieveCustomer(Service.GetOrgBillingServicesCustomerId());

					if (model.Billing.Customer != null)
					{
						// check if there is a subscription to cancel
						string subscriptionId = Service.GetSubscriptionId(model.Billing.Customer.Id);
						if (subscriptionId != null)
						{
							Service.DeleteSubscriptionPlan(subscriptionId);

							Service.DeleteSubscription(model.Billing.Customer.Id, subscriptionId);
							Service.AddBillingHistory("Switching to free subscription, canceling stripe susbcription", model.SelectedSku);
						}
					}
				}
				catch (Exception e)
				{
					Notifications.Add(new BootstrapAlert(e.ToString(), Variety.Warning));
				}
			}

			if (model.SelectedSku != model.PreviousSku)
			{
				Service.AddSubscriptionOfSkuToOrganization(model.OrganizationId, model.SelectedSku, model.ProductId, model.NumberOfUsers);
			}
			else
			{
				Service.UpdateSubscriptionUsers(model.SelectedSku, model.NumberOfUsers);
			}

			return this.RedirectToAction(ActionConstants.Manage);
		}
	}
}