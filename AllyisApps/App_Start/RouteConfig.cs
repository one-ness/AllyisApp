//------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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

			// enable attribute routes
			routes.MapMvcAttributeRoutes();

			routes.Add("resetpassword",
				new Route(url: string.Format("{0}/{1}/{{id}}/{{code}}", ControllerConstants.Account, ActionConstants.ResetPassword),
					defaults: new RouteValueDictionary(new { controller = ControllerConstants.Account, action = ActionConstants.ResetPassword }),
					constraints: new RouteValueDictionary(new { id = @"\d+" }),
					dataTokens: new RouteValueDictionary(new { Namespaces = new string[] { "AllyisApps.Controllers" }, UseNamespaceFallback = false /*Use ONLY this namespace */}),
					routeHandler: new MvcRouteHandler()));

			routes.Add("default",
				new Route(url: "{controller}/{action}/{id}/{idTwo}",
					defaults: new RouteValueDictionary(new { controller = ControllerConstants.Home, action = ActionConstants.Index, id = UrlParameter.Optional, idTwo = UrlParameter.Optional }),
					constraints: null,
					dataTokens: new RouteValueDictionary(new { Namespaces = new string[] { "AllyisApps.Controllers" }, UseNamespaceFallback = false /*Use ONLY this namespace */}),
					routeHandler: new MvcRouteHandler()));
		}
	}
}
