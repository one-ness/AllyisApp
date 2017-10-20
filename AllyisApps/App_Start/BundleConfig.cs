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

			bundles.UseCdn = true;

			// SCRIPT BUNDLES

			// Time Tracker
			bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
				"~/Scripts/jquery.comiseo.daterangepicker-0.5.0.js",
				"~/Areas/TimeTracker/Scripts/allyis-drp-init.js"));

			bundles.Add(new ScriptBundle("~/bundles/TTCustomerIndex").Include(
				"~/Areas/TimeTracker/Scripts/allyis-customer-index.js"));

			bundles.Add(new ScriptBundle("~/bundles/TTProjectEditCreate").Include(
				"~/Areas/TimeTracker/Scripts/allyis-project-editcreate.js"));

			bundles.Add(new ScriptBundle("~/bundles/TTTimeEntryIndex").Include(
				"~/Areas/TimeTracker/Scripts/allyis-timeentry-index.js"));

			bundles.Add(new ScriptBundle("~/bundles/TTTimeEntryReport").Include(
				"~/Areas/TimeTracker/Scripts/allyis-timeentry-report.js"));

			bundles.Add(new ScriptBundle("~/bundles/TTTimeEntrySettings").Include(
				"~/Areas/TimeTracker/Scripts/allyis-timeentry-settings.js"));

			bundles.Add(new ScriptBundle("~/bundles/TTTimeEntryUserEdit").Include(
				"~/Areas/TimeTracker/Scripts/allyis-timeentry-useredit.js"));

			// General AllyisApps scripts
			bundles.Add(new ScriptBundle("~/bundles/pageswithsearch").Include(
				"~/Scripts/allyis-list-group-search.js",
				"~/Scripts/allyis-paginate.js"));

			bundles.Add(new ScriptBundle("~/bundles/AccountAdd").Include(
				"~/Scripts/allyis-account-add.js"));

			bundles.Add(new ScriptBundle("~/bundles/AccountCreateOrg").Include(
				"~/Scripts/allyis-account-createorg.js"));

			bundles.Add(new ScriptBundle("~/bundles/AccountManage").Include(
				"~/Scripts/allyis-account-manage.js"/*,
                "~/Scripts/allyis-pages-with-filter.js"*/));

			bundles.Add(new ScriptBundle("~/bundles/AccountPermission").Include(
				"~/Scripts/allyis-account-permission.js"));

			bundles.Add(new ScriptBundle("~/bundles/AccountPermission2").Include(
				"~/Scripts/allyis-account-permission2.js"));

			bundles.Add(new ScriptBundle("~/bundles/AccountSubscribe").Include(
				"~/Scripts/allyis-account-subscribe.js"));

			bundles.Add(new ScriptBundle("~/bundles/CapsCheck").Include(
				"~/Scripts/allyis-capscheck.js"));

			bundles.Add(new ScriptBundle("~/bundles/UpdateStateDDL").Include(
				"~/Scripts/allyis-updateStateDDL.js"));

			bundles.Add(new ScriptBundle("~/bundles/BootstrapSelect").Include(
				"~/Scripts/bootstrap-select.min.js"));

			// CDN links with fallbacks
			Bundle jQueryBundle = new ScriptBundle("~/bundles/jquery"/*, "//ajax.aspnetcdn.com/ajax/jquery/jquery-3.1.1.min.js"*/).Include(
						"~/Scripts/jquery-{version}.js");
			jQueryBundle.CdnFallbackExpression = "window.jQuery";
			bundles.Add(jQueryBundle);

			Bundle jQueryValBundle = new ScriptBundle("~/bundles/jqueryval", "//ajax.aspnetcdn.com/ajax/jquery.validate/1.16.0/jquery.validate.min.js").Include(
						"~/Scripts/jquery.validate.js", "~/Scripts/jquery.validate.min.js");
			jQueryValBundle.CdnFallbackExpression = "$.validator";
			bundles.Add(jQueryValBundle);

			Bundle jQueryValUOBundle = new ScriptBundle("~/bundles/jqueryuoval", "//cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.6/jquery.validate.unobtrusive.min.js").Include(
						"~/Scripts/jquery.validate.unobtrusive*");
			jQueryValUOBundle.CdnFallbackExpression = "window.jQuery.validator.unobtrusive";
			bundles.Add(jQueryValUOBundle);

			Bundle jQueryTransitBundle = new ScriptBundle("~/bundles/jquerytransit", "//cdnjs.cloudflare.com/ajax/libs/jquery.transit/0.9.12/jquery.transit.min.js").Include(
						"~/Scripts/jquery.transit*");
			jQueryTransitBundle.CdnFallbackExpression = "$.transit";
			bundles.Add(jQueryTransitBundle);

			Bundle jQueryUIBundle = new ScriptBundle("~/bundles/jqueryui", "//code.jquery.com/ui/1.12.1/jquery-ui.min.js").Include(
						"~/Scripts/jquery-ui.js");
			jQueryUIBundle.CdnFallbackExpression = "window.jQuery.ui";
			bundles.Add(jQueryUIBundle);

			Bundle momentBundle = new ScriptBundle("~/bundles/moment", "//cdnjs.cloudflare.com/ajax/libs/moment.js/2.14.1/moment.min.js").Include(
						"~/Scripts/moment-{version}.js");
			momentBundle.CdnFallbackExpression = "window.moment";
			bundles.Add(momentBundle);

			Bundle underscore = new ScriptBundle("~/bundles/underscore", "//cdnjs.cloudflare.com/ajax/libs/underscore.js/1.8.3/underscore-min.js").Include(
						"~/Scripts/underscore-{version}.js");
			underscore.CdnFallbackExpression = "window._";
			bundles.Add(underscore);

			Bundle bootstrapJSBundle = new ScriptBundle("~/bundles/bootstrap", "//ajax.aspnetcdn.com/ajax/bootstrap/3.3.7/bootstrap.min.js").Include(
						"~/Scripts/bootstrap/*.js");
			bootstrapJSBundle.CdnFallbackExpression = "$.fn.modal";
			bundles.Add(bootstrapJSBundle);

			// STYLE BUNDLES

			// General Allyis styles
			bundles.Add(new StyleBundle("~/Content/Site").Include(
					  ////"~/Content/flex_box.css",
					  ////"~/Content/bootstrap.css",
					  "~/Content/Site.css"));

			bundles.Add(new StyleBundle("~/Content/Timetracker").Include(
					  "~/Content/timetracker.css"));

			bundles.Add(new StyleBundle("~/Content/BootstrapSelect").Include(
					  "~/Content/bootstrap-select.min.css"));

			// CDN links - fallbacks are much trickier with CSS as we cannot simply use a javascript variable as above. For now, no fallback provided.
			bundles.Add(new StyleBundle("~/Content/Bootstrap", "//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css"));

			bundles.Add(new StyleBundle("~/Content/FontAwesome", "//maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css"));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
			BundleTable.EnableOptimizations = true;
		}
	}
}