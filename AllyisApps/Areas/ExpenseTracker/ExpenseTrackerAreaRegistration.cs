//------------------------------------------------------------------------------
// <copyright file="ExpenseTrackerAreaRegistration.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Extensions.RouteExtensions;
using System.Web.Mvc;

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
				name: "ExpenseReport",
				url: "ExpenseTracker/{subscriptionId}/{controller}/{action}",
				area: this.AreaName,
				defaults: new { controller = "Expense", action = "Create" },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.ExpenseTracker.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "ExpenseTracker_Default",
				url: "ExpenseTracker/{subscriptionId}/{controller}",
				area: this.AreaName,
				defaults: new { controller = "Expense", action = "Index" },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.ExpenseTracker.Controllers" });
		}
	}
}
