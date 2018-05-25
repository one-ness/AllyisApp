using AllyisApps.Lib;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.TimeTracker;
using System;
using System.Collections.Generic;
using System.Linq;

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
		/// <returns>A Dictionary(string, string) object, that contains country code and localized name</returns>
		public static Dictionary<string, string> GetLocalizedCountries(Dictionary<string, Country> countries)
		{
			if (countries == null) throw new ArgumentNullException(nameof(countries));

			var result = new Dictionary<string, string>();

			// create localized countries
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
		/// Get a list of states, name localized.
		/// </summary>
		/// <returns>A Dictionary(string, string) object, that contains state id and state name.</returns>
		public static Dictionary<string, string> GetLocalizedStates(List<State> states)
		{
			var result = new Dictionary<string, string>();
			// some countries dont have states. exclude from localizing them.
			if (states != null && states.Count > 0)
			{
				foreach (State item in states)
				{
					string stateName = Utility.AggregateSpaces(item.StateName);
					string localized = Resources.States.ResourceManager.GetString(stateName) ?? item.StateName;
					result.Add(item.StateId.ToString(), localized);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns a list of roles for the given product
		/// </summary>
		/// <param name="product">the enum of the product to get the roles from
		/// e.g. time tracker, expense tracker, or staffing manager</param>
		/// <returns></returns>
		public static Dictionary<int, string> GetRolesList(ProductIdEnum product)
		{
			switch (product)
			{
				case ProductIdEnum.None:
					return null;
				case ProductIdEnum.AllyisApps:
					return GetOrgRolesList();
				case ProductIdEnum.TimeTracker:
					return GetTimeTrackerRolesList();
				case ProductIdEnum.ExpenseTracker:
					return GetExpenseTrackerRolesList();
				case ProductIdEnum.StaffingManager:
					return GetStaffingManagerRolesList();
				default:
					throw new ArgumentOutOfRangeException(nameof(product), product, null);
			}
		}

		/// <summary>
		/// get org roles
		/// </summary>
		public static Dictionary<int, string> GetOrgRolesList()
		{
			var result = new Dictionary<int, string>();
			result.Add((int)OrganizationRoleEnum.Member, Resources.Strings.Member);
			result.Add((int)OrganizationRoleEnum.Admin, Resources.Strings.Owner);
			return result;
		}

		/// <summary>
		/// get time tracker roles
		/// </summary>
		public static Dictionary<int, string> GetTimeTrackerRolesList()
		{
			var result = new Dictionary<int, string>();
			result.Add((int)TimeTrackerRole.NotInProduct, Resources.Strings.Unassigned);
			result.Add((int)TimeTrackerRole.User, Resources.Strings.User);
			result.Add((int)TimeTrackerRole.Admin, Resources.Strings.Manager);
			return result;
		}

		/// <summary>
		/// get expense tracker roles
		/// </summary>
		public static Dictionary<int, string> GetExpenseTrackerRolesList()
		{
			var result = new Dictionary<int, string>();
			result.Add((int)ExpenseTrackerRole.NotInProduct, Resources.Strings.Unassigned);
			result.Add((int)ExpenseTrackerRole.User, Resources.Strings.User);
			result.Add((int)ExpenseTrackerRole.Manager, Resources.Strings.Manager);
			result.Add((int)ExpenseTrackerRole.Admin, Resources.Strings.SuperUser);
			return result;
		}

		/// <summary>
		/// get staffing manager roles
		/// </summary>
		public static Dictionary<int, string> GetStaffingManagerRolesList()
		{
			var result = new Dictionary<int, string>();
			result.Add((int)StaffingManagerRole.NotInProduct, Resources.Strings.Unassigned);
			result.Add((int)StaffingManagerRole.User, Resources.Strings.User);
			result.Add((int)StaffingManagerRole.Admin, Resources.Strings.Manager);
			return result;
		}

		/// <summary>
		/// Gets a dictionary of all time entry statuses, localized.
		/// </summary>
		/// <returns>A dictionary of all time entry statuses, localized.</returns>
		public static Dictionary<int, string> GetLocalizedTimeEntryStatuses()
		{
			return Enum
				.GetValues(typeof(TimeEntryStatus))
				.Cast<TimeEntryStatus>()
				.ToDictionary(
					enumValue => (int)enumValue,
					enumValue => Resources.Strings.ResourceManager.GetString(enumValue.ToString()));
		}

		/// <summary>
		/// Returns all the options available for overtime period.
		/// </summary>
		/// <returns>All the options available for overtime period.</returns>
		public static Dictionary<string, string> GetOvertimePeriodOptions()
		{
			return new Dictionary<string, string>
			{
				{ "Day", "Day" },
				{ "Week", "Week" },
				{ "Month", "Month" }
			};
		}
	}
}