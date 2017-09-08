//------------------------------------------------------------------------------
// <copyright file="EditMemberViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents an editable view of user information.
	/// </summary>
	public class EditMemberViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the user's info for display.
		/// </summary>
		public int CurrentUserId { get; set; }

		/// <summary>
		/// Gets or sets the user's info for display.
		/// </summary>
		public UserInfoViewModel UserInfo { get; set; }

		/// <summary>
		/// Gets or sets the user's first name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "FirstNameValidation")]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user's last name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "LastNameValidation")]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the user's organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the employee id.
		/// </summary>
		[DataType(DataType.Text)]
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "EmployeeIdValidation")]
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the employee role.
		/// </summary>
		public int EmployeeRoleId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user is invited.
		/// </summary>
		public bool IsInvited { get; set; }
	}
}
