#pragma warning disable 1591

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// view model for the sidebar shown in manage org pages
	/// </summary>
	public class ManageOrgSideBarViewModel : BaseViewModel
	{
		public string DetailsActive { get; set; }
		public string MembersActive { get; set; }
		public string PermissionsActive { get; set; }
		public string SubscriptionsActive { get; set; }
		public string BillingActive { get; set; }
	}
}
