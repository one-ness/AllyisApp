//------------------------------------------------------------------------------
// <copyright file="GetStatesAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Controllers.Home
{
	/// <summary>
	/// Controller for home pages / site-wide functions.
	/// </summary>
	public partial class HomeController : BaseController
	{
		private const string CharsToReplace = @"""/\[]:|<>+=; ,?*'`()@";

		/// <summary>
		/// Retrieves a list of states for the specified country.
		/// </summary>
		/// <param name="countryCode">The country code.</param>
		/// <returns>A JsonResult object.</returns>
		[HttpPost]
		[AllowAnonymous]
		public JsonResult GetStates(string countryCode)
		{
			// NOTE: to serialize a dictionary to json, it can contain only strings
			return Json(ModelHelper.GetLocalizedStates(this.AppService.GetStates(countryCode)));
		}
	}
}