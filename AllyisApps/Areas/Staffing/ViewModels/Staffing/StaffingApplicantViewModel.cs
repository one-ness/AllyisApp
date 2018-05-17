using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
		[ScaffoldColumn(false)]
		public int ApplicantId { get; set; }

		/// <summary>
		/// Gets or sets FirstName.
		/// </summary>
		[Required]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets LastName.
		/// </summary>
		[Required]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		[Required]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets user's Address Id.
		/// </summary>
		[ScaffoldColumn(false)]
		public int AddressId { get; set; }

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		[Required]
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets City.
		/// </summary>
		[Required]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets State.
		/// </summary>
		[Required]
		public string State { get; set; }

		/// <summary>
		/// Gets or sets Country.
		/// </summary>
		[Required]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets PostalCode.
		/// </summary>
		[Required]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		[Required]
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