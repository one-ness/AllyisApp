//------------------------------------------------------------------------------
// <copyright file="SubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.Services.Crm;
using AllyisApps.Utilities;
using AllyisApps.ViewModels;
using AllyisApps.BillingServices.Common.Types;
using AllyisApps.BillingServices;

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

			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization))
			{
				ProductSubscriptionViewModel model = this.ConstructProductSubscriptionViewModel(productId);
				if (!model.IsValid)
				{
					return this.View(ViewConstants.Details, orgId);
				}

				CrmService.InitializeSettingsForProduct(model.ProductId);

				// SubscriptionInfo sub = CrmService.CheckSubscription(orgId, productId);

				model.CurrentUsers = CrmService.GetUsersWithSubscriptionToProductInOrganization(orgId, productId).Count();

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
			ProductInfo productInfo = CrmService.GetProductById(productId);
			if (productInfo != null)
			{
				SubscriptionInfo currentSubscription = CrmService.CheckSubscription(productId);
				int selectedSku = currentSubscription == null ? 0 : currentSubscription.SkuId;

				return new ProductSubscriptionViewModel
				{
					IsValid = true,
					OrganizationId = UserContext.ChosenOrganizationId,
					ProductId = productInfo.ProductId,
					ProductName = productInfo.ProductName,
					ProductDescription = productInfo.ProductDescription,
					CurrentSubscription = currentSubscription,
					Skus = CrmService.GetSkuForProduct(productId),
					SelectedSku = selectedSku,
					SelectedSkuName = selectedSku > 0 ? CrmService.GetSkuDetails(selectedSku).Name : string.Empty,
					PreviousSku = selectedSku,
					StripeToken = CrmService.GetOrgBillingServicesCustomerId()
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
		/// <param name="stripeToken">Response from stripe.</param>
		/// <param name="stripeEmail">The billing email.</param>
		/// <param name="cost">The cost.</param>
		/// <returns>A page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[CLSCompliant(false)]
		public ActionResult Subscribe(ProductSubscriptionViewModel model, string stripeToken, string stripeEmail, string cost)
		{
			if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization))
			{
				Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
				ViewBag.ErrorInfo = "Permission";

				return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditSubscriptionsMessage), ControllerConstants.Account, ActionConstants.Subscribe));
			}

			if (model == null)
			{
				return this.View(ViewConstants.Error, new HandleErrorInfo(new ArgumentNullException(@Resources.Errors.ModelNullMessage), ControllerConstants.Account, ActionConstants.Subscribe));
			}

			if (model.NumberOfUsers < CrmService.GetUsersWithSubscriptionToProductInOrganization(model.OrganizationId, model.ProductId).Count())
			{
				Notifications.Add(new BootstrapAlert("You must first remove users from your subscription before reducing the number of users.", Variety.Danger));

				return this.RedirectToAction(ActionConstants.Subscribe, new { productName = model.ProductId });
			}

			if (model.Billing.Amount == 0)
			{
				model.Billing.Amount = (model.NumberOfUsers - 5) * 100;
			}

			if (!string.IsNullOrEmpty(stripeToken) && string.IsNullOrEmpty(model.StripeToken))
			{
				StripeToken t = CrmService.GenerateToken(stripeToken);

				BillingCustomer customer = CrmService.CreateStripeCustomer(t, stripeEmail);

				CrmService.AddOrgCustomer(customer.Id);
				model.Billing.Customer = customer;
				model.StripeToken = stripeToken;
				CrmService.AddBillingHistory("Adding stripe customer data", null);
			}
			else
			{
				model.Billing.Customer = BillingServicesHandler.RetrieveCustomer(CrmService.GetOrgBillingServicesCustomerId());
			}

			if (model.Billing.Amount != 0)
			{
				if (string.IsNullOrEmpty(stripeToken) && string.IsNullOrEmpty(model.StripeToken))
				{
					return this.View(ViewConstants.AddBillingToSubscribe, model);
				}

				string orgCustomer = CrmService.GetOrgBillingServicesCustomerId();
				if (orgCustomer == null)
				{
					model.Billing.Customer = CrmService.CreateStripeCustomer(CrmService.GenerateToken(stripeToken), stripeEmail);
					CrmService.AddOrgCustomer(model.Billing.Customer.Id);
				}
				else
				{
					model.Billing.Customer = BillingServicesHandler.RetrieveCustomer(orgCustomer);
				}

				string subscriptionId = CrmService.GetSubscriptionId(model.Billing.Customer.Id);

				if (subscriptionId == null)
				{
					CrmService.AddCustomerSubscriptionPlan(model.Billing.Amount, model.Billing.Customer, model.NumberOfUsers, model.ProductId, model.ProductName);
					CrmService.InitializeSettingsForProduct(model.ProductId);
					CrmService.AddBillingHistory(string.Format("Adding new subscription data for {0}.", model.ProductName), model.SelectedSku);
				}
				else
				{
					string test = CrmService.UpdateSubscriptionPlan(subscriptionId, model.Billing.Amount, model.Billing.Customer, model.NumberOfUsers, model.ProductName);
					CrmService.AddBillingHistory(string.Format("Updating subscription data for {0}", model.ProductName), model.SelectedSku);
				}
			}
			else
			{
				try
				{
					model.Billing.Customer = BillingServicesHandler.RetrieveCustomer(CrmService.GetOrgBillingServicesCustomerId());

					if (model.Billing.Customer != null)
					{
						// check if there is a subscription to cancel
						string subscriptionId = CrmService.GetSubscriptionId(model.Billing.Customer.Id);
						if (subscriptionId != null)
						{
							CrmService.DeleteSubscriptionPlan(subscriptionId);

							StripeWrapper.SubscriptionCancel(model.Billing.Customer.Id, subscriptionId);
							CrmService.AddBillingHistory("Switching to free subscription, canceling stripe susbcription", model.SelectedSku);
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
				CrmService.AddSubscriptionOfSkuToOrganization(model.OrganizationId, model.SelectedSku, model.ProductId, model.NumberOfUsers);
			}
			else
			{
				CrmService.UpdateSubscriptionUsers(model.SelectedSku, model.NumberOfUsers);
			}

			return this.RedirectToAction(ActionConstants.Manage);
		}
	}
}
