//------------------------------------------------------------------------------
// <copyright file="TimeTrackerAreaRegistration.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Extensions.RouteExtensions;

namespace AllyisApps.Areas.TimeTracker
{
	/// <summary>
	/// Class which handles the registration of the TimeTracker
	///		site sub-area.
	/// </summary>
	public class TimeTrackerAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// Gets the area's name.
		/// </summary>
		public override string AreaName => ProductNameConstants.TimeTracker;

		/// <summary>
		/// Registers the area within the site.
		/// </summary>
		/// <param name="context">The site area registration context.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "MVC Framework managed.")]
		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.MapSubdomainRoute(
				name: "TimeEntry_Index_User_TimeSheet",
				url: "timetracker/{subscriptionId}/{controller}/{startDate}/{endDate}/{userId}",
				area: AreaName,
				defaults: new { controller = "Home", action = "Index", userId = UrlParameter.Optional },
				constraints: new { subscriptionId = @"\d+", userId = @"(\d+)?", startDate = @"\d+", endDate = @"\d+" },
				namespaces: new[] { "AllyisApps.Areas.TimeTracker.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "TimeEntry_Export",
				url: "timetracker/{subscriptionId}/{controller}/{action}/{userId}/{startingDate}-{endingDate}",
				area: AreaName,
				defaults: new { controller = "Home", startingDate = UrlParameter.Optional, endingDate = UrlParameter.Optional },
				constraints: new { action = "Export", subscriptionId = @"\d+", userId = @"\d+" },
				namespaces: new[] { "AllyisApps.Areas.TimeTracker.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "TimeEntry_Index_Current_Week",
				url: "timetracker/{subscriptionId}/{controller}/{action}/{userId}",
				area: AreaName,
				defaults: new { controller = "Home", action = "Index" },
				constraints: new { subscriptionId = @"\d+", userId = @"\d+" },
				namespaces: new[] { "AllyisApps.Areas.TimeTracker.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "TimeTracker_Default",
				url: "timetracker/{subscriptionId}/{controller}/{action}/{id}",
				area: AreaName,
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new[] { "AllyisApps.Areas.TimeTracker.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "Customer_Projects",
				url: "timetracker/{subscriptionId}/{controller}/{customerId}/{isActive}",
				area: AreaName,
				defaults: new { controller = "projects", isActive = "0"},
				constraints: new { customerId = @"\d+", isActive = @"\d+"},
				namespaces: new[] { "AllyisApps.Areas.TimeTracker.Controllers" });
		}
	}
}