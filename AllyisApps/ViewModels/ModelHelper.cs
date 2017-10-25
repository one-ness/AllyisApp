using System;
using System.Collections.Generic;
using System.Linq;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// Utility methods that have to be called from the model.
	/// </summary>
	public static class ModelHelper
	{
		/// <summary>
		/// Get a list of countries, name localized.
		/// </summary>
		/// <param name="service">The AppService object.</param>
		/// <returns>A Dictionary(string, string) object.</returns>
		public static Dictionary<string, string> GetLocalizedCountries(AppService service)
		{
			var result = new Dictionary<string, string>();

			// create localized countries
			var countries = service.GetCountries();
			foreach (var item in countries)
			{
				// get the country name
				string countryName = Utility.AggregateSpaces(item.Value.CountryName);

				// use the country name in the resource file to get it's localized name
				string localized = Resources.Countries.ResourceManager.GetString(countryName) ?? item.Value.CountryName;

				result.Add(item.Key, localized);
			}

			return result;
		}

		/// <summary>
		/// Get a list of states for the given country, name localized.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <param name="countryCode">The country code.</param>
		/// <returns>A Dictionary(string, string) object.</returns>
		public static Dictionary<string, string> GetLocalizedStates(AppService service, string countryCode)
		{
			var result = new Dictionary<string, string>();

			if (string.IsNullOrWhiteSpace(countryCode)) return result;

			var states = service.GetStates(countryCode);
			foreach (State item in states)
			{
				string stateName = Utility.AggregateSpaces(item.StateName);
				string localized = Resources.States.ResourceManager.GetString(stateName) ?? item.StateName;
				result.Add(item.StateId.ToString(), localized);
			}

			return result;
		}

		/// <summary>
		/// Gets a dictionary of all time entry statuses, localized.
		/// </summary>
		/// <param name="service">AppService object.</param>
		/// <returns>A dictionary of all time entry statuses, localized.</returns>
		public static Dictionary<int, string> GetLocalizedTimeEntryStatuses(AppService service)
		{
			return Enum
				.GetValues(typeof(TimeEntryStatus))
				.Cast<TimeEntryStatus>()
				.ToDictionary(
					enumValue => (int)enumValue,
					enumValue => Resources.Strings.ResourceManager.GetString(enumValue.ToString()));
		}
	}
}