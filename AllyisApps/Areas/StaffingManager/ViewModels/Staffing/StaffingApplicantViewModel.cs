using System.Collections.Generic;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Staffing applicant view model.
	/// </summary>
	public class StaffingApplicantViewModel
	{
		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		public int ApplicantId { get; set; }

		/// <summary>
		/// Gets or sets FirstName.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets LastName.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets user's Address Id.
		/// </summary>
		public int AddressId { get; set; }

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets City.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets State.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Gets or sets Country.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets PostalCode.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets Notes.
		/// </summary>
		public string Notes { get; set; }

		/// <summary>
		/// Gets or sets Applications.
		/// </summary>
		public List<StaffingApplicationViewModel> Applications { get; set; }
	}
}