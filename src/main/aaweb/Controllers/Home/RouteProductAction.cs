//------------------------------------------------------------------------------
// <copyright file="RouteProductAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;

namespace AllyisApps.Controllers
{
    /// <summary>
    /// Controller for home pages / site-wide functions.
    /// </summary>
    public partial class HomeController : BaseController
    {
        // TODO: Add methods for getting the default action, controller and area based off of productId.

        /// <summary>
        /// Redirects as necessary based on subscriptions to the given product.
        /// </summary>
        /// <param name="productId">The id of the product.</param>
        /// <returns>The proper redirect for the product.</returns>
        public ActionResult RouteProduct(int productId)
        {
            if (Request.IsAuthenticated)
            {
                // TODO: In the subscription rerouting, add error messages for usability (Check svn log if needed)
                return this.RedirectToSubDomainAction(
                    UserContext.ChosenOrganizationId,
                    CrmService.GetProductById(productId).ProductName,
                    "Index",
                    "Home");
            }

            return this.RedirectToAction("LogOn", "Account");
        }
    }
}
