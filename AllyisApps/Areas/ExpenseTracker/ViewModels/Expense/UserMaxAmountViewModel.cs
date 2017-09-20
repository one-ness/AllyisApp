using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AllyisApps.DBModel.Finance;
using AllyisApps.Services;
using AllyisApps.Services.Expense;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Model for Create Expense Report View.
	/// </summary>
	public class UserMaxAmountViewModel
	{
		/// <summary>
		/// Gets or sets the max amount.
		/// </summary>
		[Required(ErrorMessage = "Max Amount is required.")]
		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public decimal MaxAmount { get; set; }

		/// <summary>
		/// Gets or sets the user's first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user's last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the user's id.
		/// </summary>
		public int UserId { get; set; }
	}
}