//------------------------------------------------------------------------------
// <copyright file="ApplicationRedirectAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for home pages / site-wide functions.
	/// </summary>
	public partial class HomeController : BaseController
	{
		/// <summary>
		/// Redirects to the correct application given the productId.
		/// </summary>
		/// <param name="productId">The application's product Id.</param>
		/// <param name="organizationId">The organization's Id.</param>
		/// <returns>A redirection to the application's Home/Index Action, if implemented.</returns>
		public ActionResult ApplicationRedirect(int productId, int organizationId)
		{
			if (Request.IsAuthenticated)
			{
				Product product = AppService.GetProductById(productId);
				if (product != null && !string.IsNullOrWhiteSpace(product.ProductName))
				{
					return this.RedirectToSubDomainAction(organizationId, product.ProductName, null, ControllerConstants.Home);
				}
			}

			return this.RouteHome();
		}
	}
}
