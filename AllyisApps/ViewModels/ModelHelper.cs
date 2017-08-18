using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.Lib;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// utility methods that have to be called from the model
	/// </summary>
	public static class ModelHelper
	{
		/// <summary>
		/// get a list of countries, name localized
		/// </summary>
		public static Dictionary<string, string> GetLocalizedCountries(AppService service)
		{
			var result = new Dictionary<string, string>();

			// create localized countries
			var countries = service.GetCountries();
			foreach (var item in countries)
			{
				// get the country name
				string countryName = Utility.AggregateSpaces(item.Value);

				// use the country name in the resource file to get it's localized name
				string localized = Resources.Countries.ResourceManager.GetString(countryName) ?? item.Value;

				result.Add(item.Key, localized);
			}

			return result;
		}

		/// <summary>
		/// get a list of states for the given country, name localized
		/// </summary>
		public static Dictionary<string, string> GetLocalizedStates(AppService service, string countryCode)
		{
			var result = new Dictionary<string, string>();

			var states = service.GetStates(countryCode);
			foreach (var item in states)
			{
				var stateName = Utility.AggregateSpaces(item.Value);
				var localized = Resources.States.ResourceManager.GetString(stateName) ?? item.Value;
				result.Add(item.Key.ToString(), localized);
			}

			return result;
		}
	}
}