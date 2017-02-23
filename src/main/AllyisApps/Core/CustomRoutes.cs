//------------------------------------------------------------------------------
// <copyright file="CustomRoutes.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using System.Web.Routing;

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
