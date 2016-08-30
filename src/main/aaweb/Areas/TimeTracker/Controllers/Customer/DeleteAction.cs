//------------------------------------------------------------------------------
// <copyright file="DeleteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseProductController
	{
		/// <summary>
		/// POST: Customer/Delete.
		/// </summary>
		/// <param name="id">The Customer id.</param>
		/// <returns>The Customer index.</returns>
		public ActionResult Delete(int id)
		{
			if (CrmService.DeleteCustomer(id))
			{
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.CustomerDeleteNotification, Variety.Success));

				return this.RedirectToAction(ActionConstants.Index);
			}

			// Permission failure
			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));

			return this.RedirectToAction(ActionConstants.Index);
		}
	}
}