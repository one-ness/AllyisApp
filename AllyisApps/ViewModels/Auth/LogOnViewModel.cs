//------------------------------------------------------------------------------
// <copyright file="LogOnViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the LogOn view.
	/// </summary>
	public class LogOnViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the account e-mail.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "EmailValidation")]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "PasswordValidation")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to remember the user.
		/// </summary>
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}
