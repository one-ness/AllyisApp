﻿//------------------------------------------------------------------------------
// <copyright file="CustomRoutes.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AllyisApps.DBModel;

namespace AllyisApps.Core
{
	/// <summary>
	/// Route object for handling subdomains.
	/// </summary>
	public class SubdomainRoute : Route, IRouteWithArea
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SubdomainRoute" /> class.
		/// </summary>
		/// <param name="url">The url to be routed.</param>
		/// <param name="area">The area to be routed to.</param>
		public SubdomainRoute(string url, string area) : base(url, new MvcRouteHandler())
		{
			this.Area = area;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SubdomainRoute" /> class.
		/// </summary>
		/// <param name="url">The url to be routed.</param>
		public SubdomainRoute(string url) : base(url, new MvcRouteHandler())
		{
		}

		/// <summary>
		/// Gets the name of the Area to be routed to.
		/// </summary>
		public string Area { get; internal set; }

		/////// <summary>
		/////// Gets the routeData and adds the subdomain information if possible.
		/////// </summary>
		/////// <param name="httpContext">The http context.</param>
		/////// <returns>The (potentially) updated route data.</returns>
		////public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
		////{
		////	RouteData routeData = base.GetRouteData(httpContext);
		////	if (routeData != null)
		////	{
		////		string subdomain = null;
		////		string host = httpContext.Request.Headers["Host"];

		////		// Assuming sub.domain.tld, get the index of the first '.'
		////		int index = host.IndexOf('.');
		////		int lastIndex = host.LastIndexOf('.');
		////		if (index >= 0 && index != lastIndex)
		////		{
		////			subdomain = host.Substring(0, index);
		////		}

		////		if (subdomain == null)
		////		{
		////			subdomain = httpContext.Request.Params["subdomain"];
		////		}

		////		string orgId = httpContext.Request.Params["OrganizationId"];
		////		if (subdomain != null && (orgId == null))
		////		{
		////			//routeData.Values["OrganizationId"] = Services.Org.OrgService.GetIdBySubdomain(subdomain);
		////			//if (subdomain != "www" && routeData.Values["OrganizationId"].Equals(0))
		////			//{
		////				//routeData.Values["controller"] = "Shared";
		////				//routeData.Values["action"] = "Error";
		////				//return routeData;
		////			//}
		////		}

		////		string controller = routeData.Values["controller"] as string;

		////		if (string.IsNullOrEmpty(routeData.Values["controller"] as string))
		////		{
		////			// '||' prevents www subdomain from routing to Org page
		////			if (string.IsNullOrEmpty(subdomain) || string.Equals(subdomain, "www"))
		////			{
		////				//routeData.Values["controller"] = "Home";
		////			}
		////			else
		////			{
		////				//routeData.Values["controller"] = "Account";
		////			}
		////		}
		////	}

		////	return routeData;
		////}

		/// <summary>
		/// Retrieves the relative path of the request based on the route.
		/// </summary>
		/// <param name="requestContext">The request context.</param>
		/// <param name="values">The request values.</param>
		/// <returns>The path as a string.</returns>
		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			object subdomainParam = requestContext.HttpContext.Request.Params["subdomain"];
			if (subdomainParam != null)
			{
				values["subdomain"] = subdomainParam;
			}

			return base.GetVirtualPath(requestContext, values);
		}
	}
}