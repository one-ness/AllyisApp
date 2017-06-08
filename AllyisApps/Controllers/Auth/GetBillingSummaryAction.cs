//------------------------------------------------------------------------------
// <copyright file="GetBillingSummaryAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Gets the billing summary page.
		/// </summary>
		/// <returns>The Billing summary page.</returns>
		[HttpGet]
		public ActionResult GetBillingSummary()
		{
			IEnumerable<BillingHistoryItemViewModel> model = this.ConstructBillingHistoryViewModel();

			return this.View(model);
		}

		/// <summary>
		/// Uses services and utilities to initialize an <see cref="IEnumerable{BillingHistoryItemViewModel}"/>.
		/// </summary>
		/// <returns>Populated list of BillingHistoryItemViewModels.</returns>
		public IEnumerable<BillingHistoryItemViewModel> ConstructBillingHistoryViewModel()
		{
			List<BillingHistoryItemViewModel> result = new List<BillingHistoryItemViewModel>();

			// Creation of items from Stripe data
			BillingServicesCustomerId customerId = AppService.GetOrgBillingServicesCustomerId();
			if (customerId != null)
			{
				foreach (BillingServicesInvoice invoice in AppService.ListInvoices(customerId))
				{
					result.Add(new BillingHistoryItemViewModel
					{
						Date = ConvertUTCDateTimeToEpoch(invoice.Date.Value),
						ID = invoice.Id,
						Description = string.Format("{0} invoice - Amount due: {1:C}", invoice.Service, invoice.AmountDue / 100.0), // Only works for USD right now //LANGUAGE Update to use resource file to change message language
						ProductName = invoice.ProductName,
						Username = string.Empty
					});
				}

				foreach (BillingServicesCharge charge in AppService.ListCharges(customerId))
				{
					result.Add(new BillingHistoryItemViewModel
					{
						Date = ConvertUTCDateTimeToEpoch(charge.Created),
						ID = charge.Id,
						Description = string.Format("{0} charge - Amount paid: {1:C}", charge.Service, charge.Amount / 100.0), // Only works for USD right now //LANGUAGE Update to use resource file to change message language
						ProductName = charge.StatementDescriptor,
						Username = string.Empty
					});
				}
			}

			// Creation of items from our database
			foreach (BillingHistoryItemInfo item in AppService.GetBillingHistory())
			{
				result.Add(new BillingHistoryItemViewModel
				{
					Date = ConvertUTCDateTimeToEpoch(item.Date),
					ID = string.Empty,
					Description = item.Description,
					ProductName = item.SkuName,
					Username = item.UserName
				});
			}

			return result.OrderBy(i => i.Date).Reverse();
		}

		/// <summary>
		/// Helper method for getting history item dates into a format that javascript can convert into local time on the client side.
		/// </summary>
		/// <param name="utcDate">Datetime, in UTC.</param>
		/// <returns>Datetime in ms since the BEGINNING OF ALL TIME (Jan 1st, 1970).</returns>
		public long ConvertUTCDateTimeToEpoch(DateTime utcDate)
		{
			return (long)utcDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}
	}
}
