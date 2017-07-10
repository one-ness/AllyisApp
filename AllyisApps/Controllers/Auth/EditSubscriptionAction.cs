using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public class EditSubscriptionAction : BaseController
    {
		/// <summary>
		/// GET: /Account/EditSubscription.
		/// </summary>
		public ActionResult EditSubscription(int subscriptionId)
        {
            return View();
        }
    }
}