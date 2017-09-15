//------------------------------------------------------------------------------
// <copyright file="GetBillingSummaryAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;

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
		/// <param name="organizationId">Organization id.</param>
		/// <returns>The Billing summary page.</returns>
		[HttpGet]
		public ActionResult GetBillingSummary(int organizationId)
		{
			IEnumerable<BillingHistoryItemViewModel> model = this.ConstructBillingHistoryViewModel(organizationId);

			return this.View(model);
		}

		/// <summary>
		/// Uses services and utilities to initialize an <see cref="IEnumerable{BillingHistoryItemViewModel}"/>.
		/// </summary>
		/// <param name="organizationId">Organization id.</param>
		/// <returns>Populated list of BillingHistoryItemViewModels.</returns>
		public IEnumerable<BillingHistoryItemViewModel> ConstructBillingHistoryViewModel(int organizationId)
		{
			List<BillingHistoryItemViewModel> result = new List<BillingHistoryItemViewModel>();

			// Creation of items from Stripe data
			BillingServicesCustomerId customerId = AppService.GetOrgBillingServicesCustomerId(organizationId);
			if (customerId != null)
			{
				foreach (BillingServicesInvoice invoice in AppService.ListInvoices(customerId))
				{
					result.Add(new BillingHistoryItemViewModel
					{
						Date = ConvertUtcDateTimeToEpoch(invoice.Date.Value),
						Id = invoice.Id,
						Description = string.Format("{0} invoice - Amount due: {1:C}", invoice.Service, invoice.AmountDue / 100.0), // Only works for USD right now // LANGUAGE Update to use resource file to change message language
						ProductName = invoice.ProductName,
						Username = string.Empty
					});
				}

				foreach (BillingServicesCharge charge in AppService.ListCharges(customerId))
				{
					result.Add(new BillingHistoryItemViewModel
					{
						Date = ConvertUtcDateTimeToEpoch(charge.Created),
						Id = charge.Id,
						Description = string.Format("{0} charge - Amount paid: {1:C}", charge.Service, charge.Amount / 100.0), // Only works for USD right now // LANGUAGE Update to use resource file to change message language
						ProductName = charge.StatementDescriptor,
						Username = string.Empty
					});
				}
			}

			// Creation of items from our database
			foreach (BillingHistoryItemInfo item in AppService.GetBillingHistory(organizationId))
			{
				result.Add(new BillingHistoryItemViewModel
				{
					Date = ConvertUtcDateTimeToEpoch(item.Date),
					Id = string.Empty,
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
		/// <param name="utcDate">Datetime, in Utc.</param>
		/// <returns>Datetime in ms since the BEGINNING OF ALL TIME (Jan 1st, 1970).</returns>
		public long ConvertUtcDateTimeToEpoch(DateTime utcDate)
		{
			return (long)utcDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}
	}
}