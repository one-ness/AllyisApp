using System;
using System.Collections.Generic;
using System.Linq;
using AllyisApps.Lib;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
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
				string localized = Countries.ResourceManager.GetString(countryName) ?? item.Value.CountryName;

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
				string localized = States.ResourceManager.GetString(stateName) ?? item.StateName;
				result.Add(item.StateId.ToString(), localized);
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
			result.Add((int)OrganizationRoleEnum.Member, Strings.Member);
			result.Add((int)OrganizationRoleEnum.Owner, Strings.Owner);
			return result;
		}

		/// <summary>
		/// get time tracker roles
		/// </summary>
		public static Dictionary<int, string> GetTimeTrackerRolesList()
		{
			var result = new Dictionary<int, string>();
			result.Add((int)TimeTrackerRole.NotInProduct, Strings.Unassigned);
			result.Add((int)TimeTrackerRole.User, Strings.User);
			result.Add((int)TimeTrackerRole.Manager, Strings.Manager);
			return result;
		}

		/// <summary>
		/// get expense tracker roles
		/// </summary>
		public static Dictionary<int, string> GetExpenseTrackerRolesList()
		{
			var result = new Dictionary<int, string>();
			result.Add((int)ExpenseTrackerRole.NotInProduct, Strings.Unassigned);
			result.Add((int)ExpenseTrackerRole.User, Strings.User);
			result.Add((int)ExpenseTrackerRole.Manager, Strings.Manager);
			result.Add((int)ExpenseTrackerRole.SuperUser, Strings.SuperUser);
			return result;
		}

		/// <summary>
		/// get staffing manager roles
		/// </summary>
		public static Dictionary<int, string> GetStaffingManagerRolesList()
		{
			var result = new Dictionary<int, string>();
			result.Add((int)StaffingManagerRole.NotInProduct, Strings.Unassigned);
			result.Add((int)StaffingManagerRole.User, Strings.User);
			result.Add((int)StaffingManagerRole.Manager, Strings.Manager);
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
					enumValue => Strings.ResourceManager.GetString(enumValue.ToString()));
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