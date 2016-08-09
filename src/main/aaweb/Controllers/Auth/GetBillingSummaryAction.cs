//------------------------------------------------------------------------------
// <copyright file="GetBillingSummaryAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.BillingServices.Common.Types;
using AllyisApps.Core;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.Utilities;
using AllyisApps.ViewModels;

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
			BillingServicesCustomerId customerId = CrmService.GetOrgBillingServicesCustomerId();
			if (customerId != null)
			{
				foreach (BillingInvoice invoice in CrmService.ListInvoices(customerId))
				{
					result.Add(new BillingHistoryItemViewModel
					{
						Date = ConvertUTCDateTimeToEpoch(invoice.Date.Value),
						ID = invoice.Id,
						Description = string.Format("Stripe invoice - Amount due: {0:C}", invoice.AmountDue / 100.0), // Only works for USD right now
						ProductName = invoice.StripeInvoiceLineItems.Data[0].Plan.Name,
						Username = string.Empty
					});
				}

				foreach (BillingCharge charge in CrmService.ListCharges(customerId))
				{
					result.Add(new BillingHistoryItemViewModel
					{
						Date = ConvertUTCDateTimeToEpoch(charge.Created),
						ID = charge.Id,
						Description = string.Format("Stripe charge - Amount paid: {0:C}", charge.Amount / 100.0), // Only works for USD right now
						ProductName = charge.StatementDescriptor,
						Username = string.Empty
					});
				}
			}

			// Creation of items from our database
			foreach (BillingHistoryItemInfo item in CrmService.GetBillingHistory())
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
