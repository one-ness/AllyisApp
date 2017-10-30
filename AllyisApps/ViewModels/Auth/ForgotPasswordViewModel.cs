//------------------------------------------------------------------------------
// <copyright file="ForgotPasswordViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the forgotton password view.
	/// </summary>
	public class ForgotPasswordViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the account e-mail.
		/// </summary>
		[Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "EmailValidation")]
		[EmailAddress(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "EmailFormatValidation")]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}