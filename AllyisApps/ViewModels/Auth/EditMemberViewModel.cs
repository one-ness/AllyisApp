//------------------------------------------------------------------------------
// <copyright file="EditMemberViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents an editable view of user information.
	/// </summary>
	public class EditMemberViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets/sets the user's info for display
		/// </summary>
		public User UserInfo { get; set; }

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
		/// Gets/sets the user's organization id
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
		/// Gets or sets the is invited status.
		/// </summary>
		public bool IsInvited { get; set; }
	}
}