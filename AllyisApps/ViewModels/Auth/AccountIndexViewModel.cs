using AllyisApps.Services;
using System.Collections.Generic;

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
			public UserViewModel()
			{
			}

			public UserViewModel(User user)
			{
				if (user != null)
				{
					FirstName = user.FirstName;
					LastName = user.LastName;
					Email = user.Email;
					PhoneNumber = user.PhoneNumber;
					PhoneExtension = user.PhoneExtension;
					Address1 = user.Address?.Address1;
					Address2 = user.Address?.Address2;
					City = user.Address?.City;
					State = user.Address?.StateName;
					PostalCode = user.Address?.PostalCode;
					Country = user.Address?.CountryName;
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

			public OrganizationViewModel(Organization curorg, bool isManageAllowed) : this()
			{
				OrganizationId = curorg.OrganizationId;
				OrganizationName = curorg.OrganizationName;
				PhoneNumber = curorg.PhoneNumber;
				PhoneExtension = null;//TODO: Fill with values
				this.Address1 = curorg.Address?.Address1;
				City = curorg.Address?.City;
				State = curorg.Address?.StateName;
				PostalCode = curorg.Address?.PostalCode;
				Country = curorg.Address?.CountryName;
				SiteUrl = curorg.SiteUrl;
				FaxNumber = curorg.FaxNumber;
				//TODO: Infomation is dependent on curent user
				IsManageAllowed = isManageAllowed;
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
				public SubscriptionViewModel()
				{
				}

				public SubscriptionViewModel(UserSubscription userSubInfo, string description)
				{
					ProductName = userSubInfo.ProductName;
					SubscriptionId = userSubInfo.SubscriptionId;
					SubscriptionName = userSubInfo.SubscriptionName;
					ProductDescription = description;
					productId = userSubInfo.ProductId;
					AreaUrl = userSubInfo.AreaUrl;
				}

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

				public string AreaUrl { get; set; }
				public ProductIdEnum productId { get; set; }
				public string ProductGoToUrl { get; set; }
				public string IconUrl { get; set; }
			}
		}
	}
}
