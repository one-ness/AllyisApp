//------------------------------------------------------------------------------
// <copyright file="RegisterViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents an editable view of user's information, complete with password.
	/// </summary>
	public class RegisterViewModel : EditProfileViewModel
	{
		/// <summary>
		/// Gets or sets the user's password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "CreatePasswordValidation")]
		[StringLength(100, ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PasswordLengthValidation", MinimumLength = User.PasswordMinLength)]  // If you change this minimum length value, please update the string resource.
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the user's confirmed password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "ConfirmPasswordValidation")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PasswordCompareValidation")]
		public string ConfirmPassword { get; set; }
	}
}