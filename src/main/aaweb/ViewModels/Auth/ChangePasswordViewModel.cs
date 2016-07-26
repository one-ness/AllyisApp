//------------------------------------------------------------------------------
// <copyright file="ChangePasswordViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels
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
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "New Password")]
		public string NewPassword { get; set; }

		/// <summary>
		/// Gets or sets the user's confirmed password.
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm New Password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}