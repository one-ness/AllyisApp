//------------------------------------------------------------------------------
// <copyright file="DetailsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.ViewModels;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseProductController
	{
		/// <summary>
		/// GET: Customer/Details.
		/// </summary>
		/// <param name="id">The Customer id.</param>
		/// <returns>The resulting details.</returns>
		[HttpGet]
		public ActionResult Details(int id)
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.ViewCustomer))
			{
				CustomerInfo customerInfo = CrmService.GetCustomer(id);
				return this.View(
					ViewConstants.Details,
					new CustomerInfoViewModel
					{
						CustomerInfo = customerInfo,
						OrganizationName = OrgService.GetOrganization(customerInfo.OrganizationId).Name
					});
			}

			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.View(ViewConstants.Index);
		}
	}
}