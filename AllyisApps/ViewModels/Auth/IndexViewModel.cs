using System.Collections.Generic;

#pragma warning disable 1591

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View model for /account/index.
	/// </summary>
	public class IndexViewModel : BaseViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IndexViewModel"/> class.
		/// </summary>
		public IndexViewModel()
		{
			this.Invitations = new List<InvitationViewModel>();
			this.Organizations = new List<OrganizationViewModel>();
			this.User = new UserViewModel();
		}

		/// <summary>
		/// Gets or sets the user model.
		/// </summary>
		public UserViewModel User { get; set; }

		/// <summary>
		/// Gets or sets the invitations model list.
		/// </summary>
		public List<InvitationViewModel> Invitations { get; set; }

		/// <summary>
		/// Gets or sets the organization model list.
		/// </summary>
		public List<OrganizationViewModel> Organizations { get; set; }

		/// <summary>
		/// Logged in user information.
		/// </summary>
		public class UserViewModel
		{
			/// <summary>
			/// Gets or sets FirstName.
			/// </summary>
			public string FirstName { get; set; }

			/// <summary>
			/// Gets or sets LastName.
			/// </summary>
			public string LastName { get; set; }

			/// <summary>
			/// Gets or sets Email.
			/// </summary>
			public string Email { get; set; }

			/// <summary>
			/// Gets or sets PhoneNumber.
			/// </summary>
			public string PhoneNumber { get; set; }

			/// <summary>
			/// Gets or sets PhoneExtension.
			/// </summary>
			public string PhoneExtension { get; set; }

			/// <summary>
			/// Gets or sets Address1.
			/// </summary>
			public string Address1 { get; set; }

			/// <summary>
			/// Gets or sets Address2.
			/// </summary>
			public string Address2 { get; set; }

			/// <summary>
			/// Gets or sets City.
			/// </summary>
			public string City { get; set; }

			/// <summary>
			/// Gets or sets State.
			/// </summary>
			public string State { get; set; }

			/// <summary>
			/// Gets or sets PostalCode.
			/// </summary>
			public string PostalCode { get; set; }

			/// <summary>
			/// Gets or sets Country.
			/// </summary>
			public string Country { get; set; }
		}

		/// <summary>
		/// List of invitations pending for the logged in user.
		/// </summary>
		public class InvitationViewModel
		{
			/// <summary>
			/// Gets or sets the invitation id.
			/// </summary>
			public int InvitationId { get; set; }

			/// <summary>
			/// Gets or sets the organization name.
			/// </summary>
			public string OrganizationName { get; set; }
		}

		/// <summary>
		/// List of organizations that the user is member of.
		/// </summary>
		public class OrganizationViewModel
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="OrganizationViewModel"/> class.
			/// </summary>
			public OrganizationViewModel()
			{
				this.Subscriptions = new List<SubscriptionViewModel>();
			}

			/// <summary>
			/// Gets or sets the OrganizationId.
			/// </summary>
			public int OrganizationId { get; set; }

			/// <summary>
			/// Gets or sets the OrganizationId.
			/// </summary>
			public string OrganizationName { get; set; }

			/// <summary>
			/// Gets or sets the PhoneNumber.
			/// </summary>
			public string PhoneNumber { get; set; }

			/// <summary>
			/// Gets or sets the PhoneExtension.
			/// </summary>
			public string PhoneExtension { get; set; }

			/// <summary>
			/// Gets or sets the Address1.
			/// </summary>
			public string Address1 { get; set; }

			/// <summary>
			/// Gets or sets the Address2.
			/// </summary>
			public string Address2 { get; set; }

			/// <summary>
			/// Gets or sets the City.
			/// </summary>
			public string City { get; set; }

			/// <summary>
			/// Gets or sets the State.
			/// </summary>
			public string State { get; set; }

			/// <summary>
			/// Gets or sets the PostalCode.
			/// </summary>
			public string PostalCode { get; set; }

			/// <summary>
			/// Gets or sets the Country.
			/// </summary>
			public string Country { get; set; }

			/// <summary>
			/// Gets or sets the SiteUrl.
			/// </summary>
			public string SiteUrl { get; set; }

			/// <summary>
			/// Gets or sets the FaxNumber.
			/// </summary>
			public string FaxNumber { get; set; }

			/// <summary>
			/// Gets or sets a value indicating whether the user has manage permissions.
			/// </summary>
			public bool IsManageAllowed { get; set; }

			/// <summary>
			/// Gets or sets the subscriptions model list.
			/// </summary>
			public List<SubscriptionViewModel> Subscriptions { get; set; }

			/// <summary>
			/// List of this organization's subscription, that user is member of.
			/// </summary>
			public class SubscriptionViewModel
			{
				/// <summary>
				/// Gets or sets the SubscriptionId.
				/// </summary>
				public int SubscriptionId { get; set; }

				/// <summary>
				/// Gets or sets the ProductName.
				/// </summary>
				public string ProductName { get; set; }

				/// <summary>
				/// Gets or sets the SubscriptionName.
				/// </summary>
				public string SubscriptionName { get; set; }
			}
		}
	}
}
