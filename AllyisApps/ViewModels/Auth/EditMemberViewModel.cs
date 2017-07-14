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
		/// Gets or sets the employee type.
		/// </summary>
		public int EmployeeTypeId { get; set; }

		/// <summary>
		/// Gets or sets the employee role.
		/// </summary>
		public int EmployeeRoleId { get; set; }
	}
}