//------------------------------------------------------------------------------
// <copyright file="ApplicantDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.StaffingManager
{
	/// <summary>
	/// Represents the Applicant table in the database.
	/// </summary>
	public class ApplicantDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		public int ApplicantId { get; set; }

		/// <summary>
		/// Gets or sets the applicant's city.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the applicant's state Id.
		/// </summary>
		public int StateId { get; set; }

		/// <summary>
		/// Gets or sets the applicant's country.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the applicant's postal code.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the applicant's Address Id.
		/// </summary>
		public string Address1 { get; set; }

		/// <summary>
		/// Gets or sets the applicant's Address Id.
		/// </summary>
		public string Address2 { get; set; }

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
		/// Gets or sets PhoneNumber.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets Notes.
		/// </summary>
		public string Notes { get; set; }
	}
}