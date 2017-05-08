//------------------------------------------------------------------------------
// <copyright file="TestViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// A simple view model for passing desired information to the view for testing.
	/// </summary>
	public class TestViewModel
	{
		/// <summary>
		/// Gets or sets the email address to send a test email to.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(Resources.Strings)), ErrorMessageResourceName = "EmailValidation")]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }
	}
}
