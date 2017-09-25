﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The view model for the Add Member page.
	/// </summary>
	public class AddMemberViewModel
	{
		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets user first name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "FirstNameValidationAddMember")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets user last name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "LastNameValidationAddMember")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets UserInput.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "EmailValidation")]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		[Required]
		[Display(Name = "Employee Id")]
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to add as an owner.
		/// </summary>
		public bool AddAsOwner { get; set; }
	}
}