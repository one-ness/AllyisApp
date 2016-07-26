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
		public override string AreaName
		{
			get
			{
				return "TimeTracker";
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
				name: "TimeEntry_Export",
				url: "TimeTracker/{controller}/{action}/{OrganizationId}/{userId}/{startingDate}-{endingDate}",
				area: this.AreaName,
				defaults: new { controller = "Home", startingDate = UrlParameter.Optional, endingDate = UrlParameter.Optional },
				constraints: new { action = "Export", organizationId = @"\d+", userId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.TimeTracker.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "TimeEntry_Index_User_TimeSheet",
				url: "TimeTracker/{controller}/{action}/{OrganizationId}/{userId}/{startDate}-{endDate}",
				area: this.AreaName,
				defaults: new { controller = "Home", action = "Index" },
				constraints: new { organizationId = @"\d+", userId = @"\d+", startDate = @"\d{4}-\d{2}-\d{2}", endDate = @"\d{4}-\d{2}-\d{2}" },
				namespaces: new string[] { "AllyisApps.Areas.TimeTracker.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "TimeEntry_Index_Current_Week",
				url: "TimeTracker/{controller}/{action}/{OrganizationId}/{userId}",
				area: this.AreaName,
				defaults: new { controller = "Home", action = "Index" },
				constraints: new { organizationId = @"\d+", userId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.TimeTracker.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "TimeTracker_Default",
				url: "TimeTracker/{controller}/{action}/{id}",
				area: this.AreaName,
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				namespaces: new string[] { "AllyisApps.Areas.TimeTracker.Controllers" });
		}
	}
}