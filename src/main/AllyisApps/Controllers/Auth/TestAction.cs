//------------------------------------------------------------------------------
// <copyright file="TestAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
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
            return this.View();
        }

        /// <summary>
        /// POST: /Account/Test.
        /// Sends a test email to the filled in email address.
        /// </summary>
        /// <param name="model">View Model with email filled out.</param>
        /// <returns>Redirects to Test GET.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Test(TestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await Lib.Mailer.SendEmailAsync("test@allyis.com", model.Email, "Test message", "<div><h1>Hello World!</h1></div>");
                Notifications.Add(new BootstrapAlert("Test email sent to " + model.Email + ".", Variety.Success));
            }
            else
            {
                Notifications.Add(new BootstrapAlert("Please use a valid email address.", Variety.Danger));
            }

            return this.RedirectToAction("Test", ControllerConstants.Account);
        }
    }
}