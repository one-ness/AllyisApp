//------------------------------------------------------------------------------
// <copyright file="StaffingManagerAreaRegistration.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Extensions.RouteExtensions;

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
        /// <param name="context">The context.</param>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.MapSubdomainRoute(
				name: "StaffingManager_Default",
				url: "staffingmanager/{subscriptionId}/{controller}/{action}",
				area: this.AreaName,
				defaults: new { controller = "Home", action = "Index" },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.StaffingManager.Controllers" });
		}
	}
}
