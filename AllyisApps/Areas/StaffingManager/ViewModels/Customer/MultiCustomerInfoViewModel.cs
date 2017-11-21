using System.Collections.Generic;

namespace AllyisApps.ViewModels.StaffingManager.Customer
{
	/// <inheritdoc />
	/// <summary>
	/// List of CustomerInfoViewModels for the customers view page.
	/// </summary>
	public class MultiCustomerInfoViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets a list of customers.
		/// </summary>
		public List<CustomerInfoViewModel> CustomerList { get; set; }
	}
}