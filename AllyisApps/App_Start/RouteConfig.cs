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

			routes.Add(
				"default",
				new Route(
					url: "{controller}/{action}/{id}/{productId}",
					defaults: new RouteValueDictionary(new { controller = "Home", action = "Index", id = UrlParameter.Optional, productId = UrlParameter.Optional }),
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
