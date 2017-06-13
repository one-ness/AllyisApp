﻿//------------------------------------------------------------------------------
// <copyright file="GetStatesAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Controllers
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
		/// <param name="country">The country to query for.</param>
		/// <returns>The JSON collection of state names.</returns>
		[HttpPost]
		[AllowAnonymous]
		public JsonResult GetStates(string country)
		{
			if (string.IsNullOrWhiteSpace(country))
			{
				// No states
				return this.Json(new List<string>());
			}

			Dictionary<string, string> localizedStates = new Dictionary<string, string>();

			foreach (string state in AppService.ValidStates(country))
			{
				string stateKey = Clean(state);
				localizedStates.Add(state, Resources.States.ResourceManager.GetString(stateKey) ?? state);
			}

			return this.Json(localizedStates);
		}

		private string Clean(string stringToClean)
		{
			return CharsToReplace.Aggregate(stringToClean, (str, l) => str.Replace(string.Empty + l, string.Empty));
		}
	}
}
