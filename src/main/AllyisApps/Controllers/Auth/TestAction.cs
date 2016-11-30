//------------------------------------------------------------------------------
// <copyright file="TestAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers
{
    /// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
    {
        /// <summary>
        /// GET: /Account/Test. 
        /// </summary>
        /// <returns>The action's result.</returns>
        [AllowAnonymous]
        public ActionResult Test()
        {
            TestViewModel model = new TestViewModel
            {
                apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY")
            };
            return this.View(model);
        }
    }
}