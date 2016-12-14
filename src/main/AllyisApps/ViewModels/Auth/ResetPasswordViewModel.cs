//------------------------------------------------------------------------------
// <copyright file="ResetPasswordViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the reset password view.
	/// </summary>
	public class ResetPasswordViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "EmailValidation")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the new password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "CreatePasswordValidation")]
		[StringLength(100, ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "PasswordLengthValidation", MinimumLength = 6)]  // If you change this minimum length value, please update the string resource.
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the confirmed password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "ConfirmPasswordValidation")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Auth.Strings)), ErrorMessageResourceName = "PasswordCompareValidation")]
		public string ConfirmPassword { get; set; }

		/// <summary>
		/// Gets or sets the verification code.
		/// </summary>
		public string Code { get; set; }
	}
}