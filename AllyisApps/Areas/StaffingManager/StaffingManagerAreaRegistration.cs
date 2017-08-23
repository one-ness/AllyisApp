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
		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.MapSubdomainRoute(
				name: "StaffingManager_Default",
				url: "staffingmanager/{subscriptionId}/{controller}/{action}/{id}",
				area: this.AreaName,
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				constraints: new { subscriptionId = @"\d+" },
				namespaces: new string[] { "AllyisApps.Areas.StaffingManager.Controllers" });
		}
	}
}
