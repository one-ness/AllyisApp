﻿//------------------------------------------------------------------------------
// <copyright file="ExpenseTrackerAreaRegistration.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Extensions.RouteExtensions;

namespace AllyisApps.Areas.ExpenseTracker
{
	/// <summary>
	/// Class which handles the registration of the ExpenseTracker area.
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "MVC Framework managed.")]
		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.MapSubdomainRoute(
				name: "ExpenseTracker_Detail",
				url: "expensetracker/{subscriptionId}/{controller}/{action}/{reportId}",
				area: AreaName,
				defaults: new { controller = "expense", action = "index" },
				constraints: new { subscriptionId = @"\d+", reportId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.ExpenseTracker.Controllers", "AllyisApps.Areas.ExpenseTracker.Controllers.Expense" });

			context.Routes.MapSubdomainRoute(
				name: "ExpenseTracker_Action",
				url: "expensetracker/{subscriptionId}/{controller}/{action}",
				area: AreaName,
				defaults: new { controller = "expense", action = "index" },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.ExpenseTracker.Controllers", "AllyisApps.Areas.ExpenseTracker.Controllers.Expense" });

			context.Routes.MapSubdomainRoute(
				name: "ExpenseTracker_Default",
				url: "expensetracker/{subscriptionId}/{controller}",
				area: AreaName,
				defaults: new { controller = "expense", action = "index" },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.ExpenseTracker.Controllers", "AllyisApps.Areas.ExpenseTracker.Controllers.Expense" });
		}
	}
}