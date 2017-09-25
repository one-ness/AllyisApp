//------------------------------------------------------------------------------
// <copyright file="ResetPasswordViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services;
using AllyisApps.Services.Auth;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the reset password view.
	/// </summary>
	public class ResetPasswordViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the new password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "CreatePasswordValidation")]
		[StringLength(100, ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PasswordLengthValidation", MinimumLength = User.PasswordMinLength)]  // If you change this minimum length value, please update the string resource.
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the confirmed password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "ConfirmPasswordValidation")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PasswordCompareValidation")]
		public string ConfirmPassword { get; set; }

		/// <summary>
		/// Gets or sets the verification code.
		/// </summary>
		public Guid Code { get; set; }
	}
}