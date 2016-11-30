//------------------------------------------------------------------------------
// <copyright file="TestAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Utilities;
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
                ApiKey = Helpers.ReadAppSetting("sendGridApiKey")
            };
            return this.View(model);
        }
    }
}