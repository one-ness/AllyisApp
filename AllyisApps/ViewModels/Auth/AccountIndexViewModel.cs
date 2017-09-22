using System.Collections.Generic;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;

#pragma warning disable 1591

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View model for /account/index.
	/// </summary>
	public class AccountIndexViewModel : BaseViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AccountIndexViewModel"/> class.
		/// </summary>
		public AccountIndexViewModel()
		{
			this.Invitations = new List<InvitationViewModel>();
			this.Organizations = new List<OrganizationViewModel>();
			this.UserInfo = new UserViewModel();
		}

		/// <summary>
		/// Gets or sets the user model.
		/// </summary>
		public UserViewModel UserInfo { get; set; }

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
			/// Initializes a new instance of the <see cref="UserViewModel"/> class.
			/// </summary>
			public UserViewModel()
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="UserViewModel"/> class.
			/// </summary>
			/// <param name="user">The user object.</param>
			public UserViewModel(User user)
			{
				if (user != null)
				{
					this.FirstName = user.FirstName;
					this.LastName = user.LastName;
					this.Email = user.Email;
					this.PhoneNumber = user.PhoneNumber;
					this.PhoneExtension = user.PhoneExtension;
					this.Address1 = user.Address?.Address1;
					this.Address2 = user.Address?.Address2;
					this.City = user.Address?.City;
					this.State = user.Address?.StateName;
					this.PostalCode = user.Address?.PostalCode;
					this.Country = user.Address?.CountryName;
				}
			}

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
			/// Initializes a new instance of the <see cref="OrganizationViewModel"/> class.
			/// </summary>
			/// <param name="curorg">The organization.</param>
			/// <param name="isManageAllowed">Determines whether management permissions are allowed.</param>
			public OrganizationViewModel(Organization curorg, bool isManageAllowed) : this()
			{
				this.OrganizationId = curorg.OrganizationId;
				this.OrganizationName = curorg.OrganizationName;
				this.PhoneNumber = curorg.PhoneNumber;
				this.PhoneExtension = null; // TODO: Fill with values
				this.Address1 = curorg.Address?.Address1;
				this.City = curorg.Address?.City;
				this.State = curorg.Address?.StateName;
				this.PostalCode = curorg.Address?.PostalCode;
				this.Country = curorg.Address?.CountryName;
				this.SiteUrl = curorg.SiteUrl;
				this.FaxNumber = curorg.FaxNumber;

				// TODO: Infomation is dependent on curent user
				this.IsManageAllowed = isManageAllowed;
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
				/// Initializes a new instance of the <see cref="SubscriptionViewModel"/> class.
				/// </summary>
				public SubscriptionViewModel()
				{
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SubscriptionViewModel"/> class.
				/// </summary>
				/// <param name="userSubInfo">The UserSubscription object.</param>
				/// <param name="description">The description.</param>
				public SubscriptionViewModel(UserSubscription userSubInfo, string description)
				{
					this.ProductName = userSubInfo.Subscription.ProductName;
					this.SubscriptionId = userSubInfo.Subscription.SubscriptionId;
					this.SubscriptionName = userSubInfo.Subscription.SubscriptionName;
					this.ProductDescription = description;
					this.ProductId = userSubInfo.Subscription.ProductId;
					this.AreaUrl = userSubInfo.Subscription.AreaUrl;
				}

				/// <summary>
				/// Gets or sets the ProductDescription.
				/// </summary>
				public string ProductDescription { get; set; }

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

				/// <summary>
				/// Gets or sets the AreaUrl.
				/// </summary>
				public string AreaUrl { get; set; }

				/// <summary>
				/// Gets or sets the ProductId.
				/// </summary>
				public ProductIdEnum ProductId { get; set; }

				/// <summary>
				/// Gets or sets the ProductGoToUrl.
				/// </summary>
				public string ProductGoToUrl { get; set; }

				/// <summary>
				/// Gets or sets the IconUrl.
				/// </summary>
				public string IconUrl { get; set; }
			}
		}
	}
}