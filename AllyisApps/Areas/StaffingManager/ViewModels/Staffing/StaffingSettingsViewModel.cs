using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Staffing home page view model.
	/// </summary>
	public class StaffingSettingsViewModel
	{
		/// <summary>
		/// a list of the short-hand position objects used to populate the views main table
		/// </summary>
		public List<PositionThumbnailInfoViewModel> positions { get; set; }

		/// <summary>
		/// organizations identifier number
		/// </summary>
		public int organizationId { get; set; }

		/// <summary>
		/// subscriptions identifier number
		/// </summary>
		public int subscriptionId { get; set; }

		/// <summary>
		/// Address identifier number
		/// </summary>
		public int addressId { get; set; }

		/// <summary>
		/// subscription name
		/// </summary>
		public string subscriptionName { get; set; }

		/// <summary>
		/// users identification number
		/// </summary>
		public int userId { get; set; }

		/// <summary>
		/// list fo tag objects used by the organization
		/// </summary>
		public List<TagViewModel> tags { get; set; }

		/// <summary>
		/// customers from the company
		/// </summary>
		public List<Customer> customers { get; set; }

		/// <summary>
		/// list of employment types used by the organization
		/// </summary>
		public List<EmploymentTypeSelectViewModel> employmentTypes { get; set; }

		/// <summary>
		/// list of position levels used by the organization
		/// </summary>
		public List<PositionLevelSelectViewModel> positionLevels { get; set; }

		/// <summary>
		/// list of position statuses used by the organization
		/// </summary>
		public List<PositionStatusSelectViewModel> positionStatuses { get; set; }

		/// <summary>
		/// deafult status identification number
		/// </summary>
		public int defaultPositionStatus { get; set; }

		/// <summary>
		/// Gets information for the positions Address
		/// </summary>
		public AddressViewModel PositionAddress { get; set; }

		/// <summary>
		/// Gets localized Counties for the address information
		/// </summary>
		public Dictionary<string, string> LocalizedCountries { get; set; }

		/// <summary>
		/// Gets Localized States
		/// </summary>
		public Dictionary<string, string> LocalizedStates { get; set; }
	}
}