//------------------------------------------------------------------------------
// <copyright file="RouteExtensions.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Routing;
using AllyisApps.Core;

namespace AllyisApps.Extensions.RouteExtensions
{
	/// <summary>
	/// Extensions for adding routes to the routeConfig.
	/// </summary>
	public static class RouteExtensions
	{
		/// <summary>
		/// Maps a new subdomain route to the route collection.
		/// </summary>
		/// <param name="routes">The route collection to add to.</param>
		/// <param name="name">The name of the route.</param>
		/// <param name="url">The url form.</param>
		/// <param name="defaults">The default values of the url form.</param>
		/// <param name="constraints">Constraints for this route.</param>
		/// <param name="namespaces">Namespaces where this route defaults to.</param>
		public static void MapSubdomainRoute(this RouteCollection routes, string name, string url, object defaults = null, object constraints = null, string[] namespaces = null)
		{
			routes.Add(
				name,
				new SubdomainRoute(url)
				{
					Defaults = new RouteValueDictionary(defaults),
					Constraints = new RouteValueDictionary(constraints),
					DataTokens = new RouteValueDictionary { { "Namespaces", namespaces } }
				});
		}

		/// <summary>
		/// Maps a new subdomain route to the route collection.
		/// </summary>
		/// <param name="routes">The route collection to add to.</param>
		/// <param name="name">The name of the route.</param>
		/// <param name="url">The url form.</param>
		/// <param name="area">The area the route is defined for.</param>
		/// <param name="defaults">The default values of the url form.</param>
		/// <param name="constraints">Constraints for this route.</param>
		/// <param name="namespaces">Namespaces where this route defaults to.</param>
		public static void MapSubdomainRoute(this RouteCollection routes, string name, string url, string area, object defaults = null, object constraints = null, string[] namespaces = null)
		{
			routes.Add(
				name,
				new SubdomainRoute(url, area)
				{
					Defaults = new RouteValueDictionary(defaults),
					Constraints = new RouteValueDictionary(constraints),
					DataTokens = new RouteValueDictionary { { "Namespaces", namespaces }, { "UseNamespaceFallback", false } }
				});
		}
	}
}