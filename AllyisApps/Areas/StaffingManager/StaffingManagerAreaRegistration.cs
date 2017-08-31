//------------------------------------------------------------------------------
// <copyright file="StaffingManagerAreaRegistration.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Extensions.RouteExtensions;
using System.Web.Mvc;

namespace AllyisApps.Areas.StaffingManager
{
	/// <summary>
	/// Class which handles the registration of the StaffingManager area.
	/// </summary>
	public class StaffingManagerAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// Gets the area's name.
		/// </summary>
		public override string AreaName
		{
			get
			{
				return ProductNameConstants.StaffingManager;
			}
		}

		/// <summary>
		/// Registers the area within the site.
		/// </summary>
		/// <param name="context">The context object to add the route to.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "MVC Framework managed.")]
		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.MapSubdomainRoute(
				name: "StaffingManager_Default",
				url: "staffingmanager/{subscriptionId}/{controller}/{action}",
				area: this.AreaName,
				defaults: new { controller = "Home", action = "Index" },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.StaffingManager.Controllers" });

			context.Routes.MapSubdomainRoute(
				name: "StaffingManager",
				url: "staffingmanager/{subscriptionId}/{controller}/{action}/{userId}",
				area: this.AreaName,
				defaults: new { controller = "Staffing", action = "Index", userId = UrlParameter.Optional, },
				constraints: new { subscriptionId = @"\d+", userId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.StaffingManager.Controllers" });
		}
		
	}
}
