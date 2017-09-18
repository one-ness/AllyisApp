//------------------------------------------------------------------------------
// <copyright file="AllyisAppsWebViewPage.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Core
{
	/// <summary>
	/// Custom Web View Page allowing pages to check for permissions.
	/// </summary>
	/// <typeparam name="TModel">Generic for the Model passed to the view.</typeparam>
	public abstract class AllyisAppsWebViewPage<TModel> : WebViewPage<TModel>
	{
		/// <summary>
		/// Gets rendering hints for the layout.
		/// </summary>
		public DisplayHints DisplayHints
		{
			get
			{
				const string ViewDataKey = "DisplayHints";
				if (!this.ViewData.ContainsKey(ViewDataKey))
				{
					this.ViewData[ViewDataKey] = new DisplayHints();
				}

				return (DisplayHints)this.ViewData[ViewDataKey];
			}
		}

		/// <summary>
		/// Initializes the permission set for the current view.
		/// </summary>
		public override void InitHelpers()
		{
			base.InitHelpers();
		}
	}

	/// <summary>
	/// A set of rendering hints for the layout.
	/// </summary>
	public class DisplayHints
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayHints" /> class.
		/// </summary>
		public DisplayHints()
		{
			this.LayoutThemeBundle = "~/Content/Site";
			this.ShowBreadcrumbs = true;
			this.PageBodyId = "none";
			this.PageBodyClasses = "none";
		}

		/// <summary>
		/// Gets or sets string specifying the bundle to use for styling. Defaults to "~/Content/Site".
		/// </summary>
		public string LayoutThemeBundle { get; set; }

		/// <summary>
		/// Gets or sets string specifying the location of a partial to include for the breadcrumb navbar.
		/// </summary>
		public string BreadcrumbNavPartialLocation { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not to show the breadcrumbs. Default true.
		/// </summary>
		public bool ShowBreadcrumbs { get; set; }

		/// <summary>
		/// Gets or sets the HTML Id for the page's body element. Use to restrict CSS styles to specific pages.
		/// </summary>
		public string PageBodyId { get; set; }

		/// <summary>
		/// Gets or sets the HTML Class String for the page's body element. Use to restrict CSS styles to specific pages.
		/// </summary>
		public string PageBodyClasses { get; set; }
	}
}