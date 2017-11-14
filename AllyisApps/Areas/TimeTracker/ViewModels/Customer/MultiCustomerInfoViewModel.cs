using System.Collections.Generic;
using AllyisApps.ViewModels;


namespace AllyisApps.ViewModels.TimeTracker.Customer
{
	/// <summary>
	///
	/// </summary>
	public class MultiCustomerInfoViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets a list of customers.
		/// </summary>
		public List<CustomerInfoViewModel> CustomerList { get; set; }
	}
	
}