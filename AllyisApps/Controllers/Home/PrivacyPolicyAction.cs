//------------------------------------------------------------------------------
// <copyright file="PrivacyPolicyAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for home pages / site-wide functions.
	/// </summary>
	public partial class HomeController : BaseController
	{
		/// <summary>
		/// Shows the Privacy Policy.
		/// </summary>
		/// <returns>
		/// Returns privacy policy view.
		/// .</returns>
		public ActionResult PrivacyPolicy()
		{
			return this.View();
		}
	}
}
