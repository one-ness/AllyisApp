//------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Extensions.RouteExtensions;
using System.Web.Mvc;
using System.Web.Routing;

namespace AllyisApps
{
	/// <summary>
	/// Configures routing for URLs.
	/// </summary>
	public static class RouteConfig
	{
		/// <summary>
		/// Registers routes for use with the application.
		/// </summary>
		/// <param name="routes">The collection of routes for this application.</param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapSubdomainRoute(
				name: "Application_Redirect",
				url: "{controller}/{action}/{productId}/{OrganizationId}",
				defaults: new { controller = "Home", productId = UrlParameter.Optional, organizationId = UrlParameter.Optional },
				constraints: new { action = "ApplicationRedirect" },
				namespaces: new string[] { "AllyisApps.Controllers" });

			routes.MapSubdomainRoute(
				name: "Subdomain_Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = /*string.Empty*/ "Account", /*determined by route*/ action = "RouteHome", id = UrlParameter.Optional },
				namespaces: new string[] { "AllyisApps.Controllers" });

			routes.Add(
				"Default",
				new Route(
					url: "{controller}/{action}/{id}",
					defaults: new RouteValueDictionary(new { controller = "Account", action = "RouteHome", id = UrlParameter.Optional }),
					constraints: null,
					dataTokens: new RouteValueDictionary(
						new
						{
							Namespaces = new string[] { "AllyisApps.Controllers" },
							UseNamespaceFallback = false // Use ONLY this namespace.
						}),
					routeHandler: new MvcRouteHandler()));
		}
	}
}
