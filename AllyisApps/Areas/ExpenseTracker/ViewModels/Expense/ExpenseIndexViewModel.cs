using AllyisApps.ViewModels;
using System;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.ExpenseTracker.Expense
{
	/// <summary>
	/// Information needed for the Expense report view.
	/// </summary>
	public class ExpenseIndexViewModel : BaseViewModel
	{
        /// <summary>
        /// The Subscription Id
        /// </summary>
        public int SubscriptionId { get; set; }

        /// <summary>
        /// The current user's id
        /// </summary>
        public int CurrentUser { get; set; }
        
        /// <summary>
        /// Start date for the expense reports
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date for the expense reports
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Value indicating if user can manage the items.
        /// </summary>
        public bool CanManage { get; internal set; }

        /// <summary>
        /// A List of expense reports
        /// </summary>
        public List<ExpenseItemViewModel> Reports { get; set; }

        /// <summary>
		/// Gets the type of product.
		/// </summary>
		public int ProductRole { get; set; }
    }
}
