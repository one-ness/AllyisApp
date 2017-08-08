//------------------------------------------------------------------------------
// <copyright file="ExpenseTrackerAreaRegistration.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Extensions.RouteExtensions;

namespace AllyisApps.Areas.TimeTracker
{
	/// <summary>
	/// Class which handles the registration of the ExpenseTracker
	///		site sub-area.
	/// </summary>
	public class ExpenseTrackerAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// Gets the area's name.
		/// </summary>
		public override string AreaName
		{
			get
			{
				return ProductNameConstants.ExpenseTracker;
			}
		}

		/// <summary>
		/// Registers the area within the site.
		/// </summary>
		/// <param name="context">The site area registration context.</param>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.MapSubdomainRoute(
				name: "ExpenseTracker_Default",
				url: "expensetracker/{subscriptionId}/{controller}/{action}/{id}",
				area: this.AreaName,
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.ExpenseTracker.Controllers" });
		}
	}
}
