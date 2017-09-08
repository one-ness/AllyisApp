//------------------------------------------------------------------------------
// <copyright file="ChangePasswordViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the Change Password view.
	/// </summary>
	public class ChangePasswordViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the user's old password.
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current Password")]
		public string OldPassword { get; set; }

		/// <summary>
		/// Gets or sets the user's new password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "CreatePasswordValidation")]
		[StringLength(100, ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PasswordLengthValidation", MinimumLength = User.PasswordMinLength)]  // If you change this minimum length value, please update the string resource.
		[DataType(DataType.Password)]
		[Display(Name = "New Password")]
		public string NewPassword { get; set; }

		/// <summary>
		/// Gets or sets the user's confirmed password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "ConfirmPasswordValidation")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm New Password")]
		[Compare("NewPassword", ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PasswordCompareValidation")]
		public string ConfirmPassword { get; set; }
	}
}
