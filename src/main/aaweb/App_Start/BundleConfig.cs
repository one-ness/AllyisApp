//------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Optimization;

namespace AllyisApps
{
	/// <summary>
	/// Handles the bundling and optimization of required resources.
	/// </summary>
	public static class BundleConfig
	{
		/// <summary>
		/// Bundles external resources (Bootstrap, JS, CSS, etc.).
		/// </summary>
		/// <param name="bundles">The collection of resources.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "MVC framework impl.")]
		public static void RegisterBundles(BundleCollection bundles)
		{
			// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
			bundles.Add(new ScriptBundle("~/bundles/allyis").Include(
						"~/Scripts/allyis-*"));

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
						"~/Scripts/bootstrap/*.js"));

			bundles.Add(new StyleBundle("~/Content/Site").Include(
					  ////"~/Content/flex_box.css",
					  ////"~/Content/bootstrap.css",
					  "~/Content/Site.css"));

			bundles.Add(new StyleBundle("~/Content/Timetracker").Include(
					  "~/Content/timetracker.css"));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
			BundleTable.EnableOptimizations = true;
		}
	}
}
