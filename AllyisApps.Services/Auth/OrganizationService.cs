//------------------------------------------------------------------------------
// <copyright file="OrgService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Lookup;
using AllyisApps.Lib;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Lookup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Crm;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all Organization related services.
	/// </summary>
	public partial class AppService : BaseService
	{
		/// <summary>
		/// Creates an organization.
		/// </summary>
		/// <param name="organization">Organization.</param>
		/// <param name="employeeId">Organization owner employee Id.</param>
		/// <returns>Organizaiton Id, or -1 if the subdomain name is taken.</returns>
		public int SetupOrganization(Organization organization, string employeeId)
		{
			#region Validation

			if (organization == null)
			{
				throw new ArgumentNullException("organization", "Organization object must not be null.");
			}

			if (string.IsNullOrWhiteSpace(employeeId))
			{
				throw new ArgumentNullException("employeeId", "EmployeeId must not be null");
			}
			AddressDBEntity address = null;
			if (organization.Address != null)
			{
				address = GetDBEntityFromAddress(organization.Address);
			}

			#endregion Validation

			return DBHelper.SetupOrganization(GetDBEntityFromOrganization(organization),
				GetDBEntityFromAddress(organization.Address), this.UserContext.UserId, (int)OrganizationRole.Owner, employeeId);
		}

		/// <summary>
		/// Gets an <see cref="Organization"/>.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>The Organization.</returns>
		public Organization GetOrganization(int orgId)
		{
			#region Validation

			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			#endregion Validation

			return InitializeOrganization(DBHelper.GetOrganization(orgId));
		}

		/// <summary>
		/// Gets the Organization for the current chosen organization, along with OrganizationUserInfos for each user in the
		/// organization, SubscriptionDisplayInfos for any subscriptions in the organization, InvitationInfos for any invitiations
		/// pending in the organization, the organization's billing stripe handle, and a list of all products.
		/// </summary>
		/// <returns>.</returns>
		public Organization GetOrganizationManagementInfo(int orgId)
		{
			var spResults = DBHelper.GetOrganizationManagementInfo(orgId);
			Organization org = InitializeOrganization(spResults.Item1);
			org.Users = spResults.Item2.Select(oudb => InitializeOrganizationUser(oudb)).ToList();
			org.Subscriptions = spResults.Item3.Select(sddb => InitializeSubscription(sddb)).ToList();
			org.Invitations = spResults.Item4.Select(idb => InitializeInvitationInfo(idb)).ToList();
			org.StripeToken = spResults.Item5;
			return org;
		}

		/// <summary>
		/// Gets the Organization for the current chosen organization, along with the list of valid countries and the
		/// employee id for the current user in the current chosen organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>.</returns>
		public Organization GetOrgWithNextEmployeeId(int orgId)
		{
			var spResults = DBHelper.GetOrgWithNextEmployeeId(orgId, UserContext.UserId);
			Organization org = InitializeOrganization(spResults.Item1);
			org.NextEmpolyeeID = spResults.Item2;
			return org;
		}

		/// <summary>
		/// Gets the next recommended employee id by existing users, a list of SubscriptionDisplayInfos for subscriptions in
		/// the organization, a list of SubscriptionRoleInfos for roles within the subscriptions of the organization,
		/// a list of CompleteProjectInfos for TimeTracker projects in the organization, and the next recommended employee id
		/// by invitations.
		/// </summary>
		/// <param name="orgId">The Organization Id.</param>
		/// <returns>.</returns>
		public Tuple<Organization, List<ProductRole>> GetAddMemberInfo(int orgId)
		{
			var spResults = DBHelper.GetAddMemberInfo(orgId);
			Organization org = new Organization();
			org.NextEmpolyeeID = string.Compare(spResults.Item1, spResults.Item4) > 0 ? spResults.Item1 : spResults.Item4;
			org.Subscriptions = spResults.Item2.Select(sddb => InitializeSubscription(sddb)).ToList();

			return Tuple.Create(
				org,
				spResults.Item3.Select(srdb => InitializeSubscriptionRoleInfo(srdb)).ToList()
			);
		}

		/// <summary>
		/// Gets a list of UserRolesInfos for users in the current organization and their roles/subscription roles,
		/// and a list of Subscriptions (with only SubscriptionId, ProductId, and ProductName populated) for
		/// all subscriptions in the current organization.
		/// </summary>
		/// <param name="orgId">The Organization Id.</param>
		/// <returns>.</returns>
		public Tuple<List<UserRole>, List<Subscription>> GetOrgAndSubRoles(int orgId)
		{
			var spResults = DBHelper.GetOrgAndSubRoles(orgId);
			return Tuple.Create(
				spResults.Item1.Select(urdb => InitializeUserRole(urdb)).ToList(),
				spResults.Item2.Select(sddb => InitializeSubscription(sddb)).ToList());
		}

		/// <summary>
		/// Updates an organization chosen by the current user.
		/// </summary>
		/// <param name="organization">Updated organization.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateOrganization(Organization organization)
		{
			if (organization == null) throw new ArgumentException("organization");
			this.CheckOrgAction(OrgAction.EditOrganization, organization.OrganizationId);
			AddressDBEntity address = null;
			if (organization.Address != null)
			{
				address = GetDBEntityFromAddress(organization.Address);
			}
			return DBHelper.UpdateOrganization(GetDBEntityFromOrganization(organization), address) > 0;
		}

		/// <summary>
		/// Deletes the user's current chosen organization.
		/// </summary>
		/// <returns>Returns false if permissions fail.</returns>
		public void DeleteOrganization(int orgId)
		{
			this.CheckOrgAction(OrgAction.EditOrganization, orgId);
			DBHelper.DeleteOrganization(orgId);
		}

		/// <summary>
		/// Creates an invitation for a new user in the database, and also sends an email to the new user with their access code.
		/// </summary>
		/// <param name="url">The url for Account/Index with the accessCode value as "{accessCode}".</param>
		/// <param name="invitationInfo">An <see cref="Invitation"/> with invitee information filled out.</param>
		/// <returns>The invitation Id, or -1 if the employee id is already taken.</returns>
		public int InviteUser(string url, Invitation invitationInfo)
		{
			#region Validation

			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url", "Url must have a value.");
			}

			if (invitationInfo == null)
			{
				throw new ArgumentNullException("invitationInfo", "Invitation info object must not be null.");
			}

			if (string.IsNullOrEmpty(invitationInfo.Email) || !Utility.IsValidEmail(invitationInfo.Email))
			{
				throw new ArgumentException("Email address is not valid", "invitationInfo.Email");
			}

			#endregion Validation

			// Creation of invitation
			var spResults = DBHelper.CreateInvitation(UserContext.UserId, GetDBEntityFromInvitationInfo(invitationInfo));
			if (spResults.Item1 == -1)
			{
				throw new DuplicateNameException("User is already a member of the organization.");
			}

			if (spResults.Item1 == -2)
			{
				throw new InvalidOperationException("Employee Id is already taken.");
			}

			// Send invitation email
			string orgName = string.Empty;
			string htmlbody = string.Format(
				"{0} {1} has requested you join their organization on Allyis Apps{2}!<br /> Click <a href={3}>Here</a> to create an account and join!",
				spResults.Item2,
				spResults.Item3,
				orgName,
				url);

			string msgbody = new System.Web.HtmlString(htmlbody).ToString();
			bool email = Lib.Mailer.SendEmailAsync(
				this.ServiceSettings.SupportEmail,
				invitationInfo.Email,
				"Join Allyis Apps!",
				msgbody).Result;

			// Return invitation id
			return spResults.Item1;
		}

		public async void NotifyInviteAcceptAsync(int inviteId)
		{
			InvitationDBEntity invitation = DBHelper.GetUserInvitationByInviteId(inviteId);
			dynamic org = DBHelper.GetOrganization(invitation.OrganizationId);
			IEnumerable<dynamic> owners = DBHelper.GetOrgOwnerEmails(invitation.OrganizationId);

			string htmlbody = string.Format(
				"{0} has joined the organization {1} on Allyis Apps.",
				invitation.FirstName + " " + invitation.LastName,
				org.OrganizationName);

			string msgbody = new System.Web.HtmlString(htmlbody).ToString();
			foreach (dynamic owner in owners)
			{
				await Mailer.SendEmailAsync(
					this.ServiceSettings.SupportEmail,
					owner.Email,
					"Join Allyis Apps!",
					msgbody);
			}
		}

		public async void NotifyInviteRejectAsync(int inviteId)
		{
			InvitationDBEntity invitation = DBHelper.GetUserInvitationByInviteId(inviteId);
			dynamic org = DBHelper.GetOrganization(invitation.OrganizationId);
			IEnumerable<dynamic> owners = DBHelper.GetOrgOwnerEmails(invitation.OrganizationId);

			string htmlbody = string.Format(
				"{0} has rejected joining the organization {1} on Allyis Apps.",
				invitation.FirstName + " " + invitation.LastName,
				org.OrganizationName);

			string msgbody = new System.Web.HtmlString(htmlbody).ToString();
			foreach (dynamic owner in owners)
			{
				await Mailer.SendEmailAsync(
					this.ServiceSettings.SupportEmail,
					owner.Email,
					"User rejected invite Allyis Apps!",
					msgbody);
			}
		}

		/// <summary>
		/// Removes an invitation.
		/// </summary>
		/// <param name="orgId">Organization id.</param>
		/// <param name="invitationId">Invitation Id.</param>
		/// <returns>Returns false if permissions fail.</returns>
		public bool RemoveInvitation(int orgId, int invitationId)
		{
			if (orgId <= 0) throw new ArgumentException("orgId");
			if (invitationId <= 0) throw new ArgumentException("invitationId");
			this.CheckOrgAction(OrgAction.EditInvitation, orgId);
			return DBHelper.DeleteInvitation(invitationId);
		}

		///// <summary>
		///// Getst a list of the user invitations for the current organization.
		///// </summary>
		///// <returns>List of InvitationInfos of organization's user invitations.</returns>
		// public IEnumerable<Invitation> GetUserInvitations()
		//{
		//	return DBHelper.GetUserInvitationsByOrgId(UserContext.ChosenOrganizationId).Select(i => InitializeInvitationInfo(i));
		//}

		///// <summary>
		///// Creates a subscription role for an invitation.
		///// </summary>
		///// <param name="invitationId">Invitation id.</param>
		///// <param name="subscriptionId">Subscription id.</param>
		///// <param name="selectedRole">Selected role.</param>
		// public void CreateInvitationSubRole(int invitationId, int subscriptionId, int selectedRole)
		//{
		//    #region Validation

		//    if (invitationId <= 0)
		//    {
		//        throw new ArgumentOutOfRangeException("invitationId", "Invitation Id cannot be 0 or negative.");
		//    }

		//    if (subscriptionId <= 0)
		//    {
		//        throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
		//    }

		//    if (selectedRole <= 0)
		//    { // TODO: Figure out if there is any further validation that can be done for this number.
		//        throw new ArgumentOutOfRangeException("selectedRole", "Selected role cannot be negative.");
		//    }

		//    #endregion Validation

		//    DBHelper.CreateInvitationSubRole(invitationId, subscriptionId, selectedRole);
		//}

		/// <summary>
		/// Updates a user's subscription product role.
		/// </summary>
		/// <param name="selectedRole">The Role.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <param name="userId">User Id.</param>
		public void UpdateSubscriptionUserProductRole(int selectedRole, int subscriptionId, int userId)
		{
			#region Validation

			if (selectedRole <= 0)
			{ // TODO: Figure out if there is any further validation that can be done for this number.
				throw new ArgumentOutOfRangeException("selectedRole", "Selected role cannot be negative.");
			}

			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			#endregion Validation

			DBHelper.UpdateSubscriptionUserProductRole(selectedRole, subscriptionId, userId);
		}

		/// <summary>
		/// Gets the member list for an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>The member list.</returns>
		public IEnumerable<OrganizationUser> GetOrganizationMemberList(int orgId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return DBHelper.GetOrganizationMemberList(orgId).Select(o => InitializeOrganizationUser(o));
		}

		// TODO: Look more closely at the use of this method in UploadCsvFileAction to see if some other existing service method can be used instead, and this one retired.

		///// <summary>
		///// Gets the first name of a user in the current organization by email.
		///// </summary>
		///// <param name="email">Email address.</param>
		///// <returns>User's first name.</returns>
		// public string GetOrgUserFirstName(string email)
		//{
		//	#region Validation

		//	if (string.IsNullOrEmpty(email))
		//	{
		//		throw new ArgumentNullException("email", "Email address must have a value.");
		//	}
		//	else if (!Utility.IsValidEmail(email))
		//	{
		//		throw new FormatException("Email address must be in a valid format.");
		//	}

		//	#endregion Validation

		//	return DBHelper.GetOrgUserFirstName(UserContext.ChosenOrganizationId, email);
		//}

		///// <summary>
		///// Gets a list of subscription details for the current organization.
		///// </summary>
		///// <returns><see cref="IEnumerable{SubscriptionInfo}"/>.</returns>
		// public IEnumerable<SubscriptionInfo> GetSubscriptionDetails()
		//{
		//	IEnumerable<SubscriptionDBEntity> subDBEList = DBHelper.GetSubscriptionDetails(UserContext.ChosenOrganizationId);
		//	List<SubscriptionInfo> list = new List<SubscriptionInfo>();
		//	foreach (SubscriptionDBEntity subDBE in subDBEList)
		//	{
		//		if (subDBE != null)
		//		{
		//			list.Add(new SubscriptionInfo
		//			{
		//				OrganizationName = subDBE.OrganizationName,
		//				OrganizationId = subDBE.OrganizationId,
		//				SubscriptionId = subDBE.SubscriptionId,
		//				SkuId = subDBE.SkuId,
		//				NumberOfUsers = subDBE.NumberOfUsers,
		//				Licenses = subDBE.Licenses,
		//				CreatedUtc = subDBE.CreatedUtc,
		//				IsActive = subDBE.IsActive,
		//				Name = subDBE.Name
		//			});
		//		}
		//	}

		//	return list;
		//}

		/// <summary>
		/// Gets the EmployeeId for the given user and org.
		/// </summary>
		/// <param name="userId">The userId.</param>
		/// <param name="orgId">The orgId.</param>
		/// <returns>The employeeId.</returns>
		public string GetEmployeeId(int userId, int orgId)
		{
			#region Validation

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			#endregion Validation

			return DBHelper.GetEmployeeId(userId, orgId);
		}

		/// <summary>
		/// Removes an organization user.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="userId">User Id.</param>
		public void RemoveOrganizationUser(int orgId, int userId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			DBHelper.RemoveOrganizationUser(orgId, userId);
		}

		/// <summary>
		/// Gets all the projects in an organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="onlyActive">True (default) to only return active projects, false to include all projects, active or not.</param>
		/// <returns>A list of project info objects based on Organization.</returns>
		public IEnumerable<CompleteProject> GetProjectsByOrganization(int orgId, bool onlyActive = true)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return DBHelper.GetProjectsByOrgId(orgId, onlyActive ? 1 : 0).Select(c => InitializeCompleteProjectInfo(c));
		}

		/// <summary>
		/// Assigns a new organization role to the given users for the current organization.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="newOrganizationRole">Organization role to assign, or -1 to remove from organization.</param>
		/// <param name="organizationId">The organization Id.</param>
		/// <returns>The number of affected users.</returns>
		public int UpdateOrganizationUsersRole(List<int> userIds, int newOrganizationRole, int organizationId)
		{
			#region Validation

			if (!Enum.IsDefined(typeof(OrganizationRole), newOrganizationRole))
			{
				throw new ArgumentOutOfRangeException("newOrganizationRole", "Organization role must match a value of the OrganizationRole enum.");
			}

			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentException("userIds", "No user ids provided.");
			}

			#endregion Validation

			return DBHelper.UpdateOrganizationUsersRole(userIds, organizationId, newOrganizationRole);
		}

		/// <summary>
		/// Deletes all users from the org that are in the given userIds list.
		/// </summary>
		/// <param name="userIds">List of user Ids.</param>
		/// <param name="organizationId">The organization Id.</param>
		/// <returns>The number of affected users.</returns>
		public int DeleteOrganizationUsers(List<int> userIds, int organizationId)
		{
			#region Validation

			if (userIds == null || userIds.Count == 0)
			{
				throw new ArgumentException("No user ids provided.", "userIds");
			}

			#endregion Validation

			return DBHelper.DeleteOrganizationUsers(userIds, organizationId);
		}

		/// <summary>
		/// Gets the product role for a user.
		/// </summary>
		/// <param name="productName">Product name.</param>
		/// <param name="userId">User Id.</param>
		/// <param name="orgId"> The Organization Id.</param>
		/// <returns>The product role.</returns>
		public string GetProductRoleForUser(string productName, int userId, int orgId)
		{
			#region Validation

			if (string.IsNullOrEmpty(productName)) throw new ArgumentNullException("productName", "Product name must have a value.");
			if (userId <= 0) throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			if (orgId <= 0) throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be 0 or negative.");

			#endregion Validation

			return DBHelper.GetProductRoleForUser(productName, orgId, userId);
		}

		#region Info-DBEntity Conversions

		/// <summary>
		/// Translates an OrganizationUserDBEntity into an OrganizationUser business object.
		/// </summary>
		/// <param name="organizationUser">OrganizationUserDBEntity instance.</param>
		/// <returns>OrganizationUser instance.</returns>
		public static OrganizationUser InitializeOrganizationUser(OrganizationUserDBEntity organizationUser)
		{
			if (organizationUser == null)
			{
				return null;
			}

			return new OrganizationUser
			{
				FirstName = organizationUser.FirstName,
				LastName = organizationUser.LastName,
				Email = organizationUser.Email,
				CreatedUtc = organizationUser.CreatedUtc,
				EmployeeId = organizationUser.EmployeeId,
				OrganizationId = organizationUser.OrganizationId,
				OrganizationRoleId = organizationUser.OrganizationRoleId,
				UserId = organizationUser.UserId,
				MaxAmount = organizationUser.MaxAmount
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="organizationEntity"></param>
		/// <param name="loadAddress"></param>
		/// <returns></returns>
		public Organization InitializeOrganization(OrganizationDBEntity organizationEntity, bool loadAddress = true)
		{
			return new Organization
			{
				CreatedUtc = organizationEntity.CreatedUtc,
				FaxNumber = organizationEntity.FaxNumber,
				OrganizationName = organizationEntity.OrganizationName,
				OrganizationId = organizationEntity.OrganizationId,
				PhoneNumber = organizationEntity.PhoneNumber,
				SiteUrl = organizationEntity.SiteUrl,
				Subdomain = organizationEntity.Subdomain,
				Address = loadAddress ? getAddress(organizationEntity.AddressId) : null
			};
		}

		/// <summary>
		/// Translates an OrganizationDBEntity into an Organization business object.
		/// </summary>
		/// <param name="organizationInfo">OrganizationDBEntity instance.</param>
		/// <returns>Organization instance.</returns>
		public static Organization InitializeOrganization(dynamic organizationInfo)
		{
			if (organizationInfo == null)
			{
				return null;
			}
			Address address = null;

			if (organizationInfo.AddressId != null)
			{
				address = InitializeAddress(organizationInfo);
			}

			return new Organization
			{
				CreatedUtc = organizationInfo.CreatedUtc,
				FaxNumber = organizationInfo.FaxNumber,
				OrganizationName = organizationInfo.OrganizationName,
				OrganizationId = organizationInfo.OrganizationId,
				PhoneNumber = organizationInfo.PhoneNumber,
				SiteUrl = organizationInfo.SiteUrl,
				Subdomain = organizationInfo.Subdomain,
				Address = address
			};
		}

		/// <summary>
		/// Translates an Organization business object into an OrganizationDBEntity.
		/// </summary>
		/// <param name="organization">Organization instance.</param>
		/// <returns>OrganizationDBEntity instance.</returns>
		public OrganizationDBEntity GetDBEntityFromOrganization(Organization organization)
		{
			if (organization == null)
			{
				return null;
			}

			//Ensure that organizion buisneess object does not attempt to set CountryName or state name without ID
			//Should not happen but can from import service.
			organization.Address?.EnsureDBRef(this);

			return new OrganizationDBEntity
			{
				AddressId = organization.Address?.AddressId,
				CreatedUtc = organization.CreatedUtc,
				FaxNumber = organization.FaxNumber,
				OrganizationName = organization.OrganizationName,
				OrganizationId = organization.OrganizationId,
				PhoneNumber = organization.PhoneNumber,
				SiteUrl = organization.SiteUrl,
				Subdomain = organization.Subdomain,
			};
		}

		/// <summary>
		/// Toodo: Good idea need time to implement fully
		/// </summary>
		/// <param name="organization"></param>
		/// <returns></returns>
		public Tuple<OrganizationDBEntity, AddressDBEntity> GetDBEntitiesFormOrganization(Organization organization)
		{
			if (organization == null)
			{
				return null;
			}
			organization.Address?.EnsureDBRef(this);

			return new Tuple<OrganizationDBEntity, AddressDBEntity>(
				GetDBEntityFromOrganization(organization),
				GetDBEntityFromAddress(organization.Address));
		}

		/// <summary>
		/// Translates an InvitationDBEntity into an Invitation business object.
		/// </summary>
		/// <param name="invitation">InvitationDBEntity instance.</param>
		/// <returns>Invitation instance.</returns>
		public static Invitation InitializeInvitationInfo(InvitationDBEntity invitation)
		{
			if (invitation == null)
			{
				return null;
			}

			return new Invitation
			{
				Email = invitation.Email,
				CompressedEmail = Utility.GetCompressedEmail(invitation.Email),
				FirstName = invitation.FirstName,
				InvitationId = invitation.InvitationId,
				LastName = invitation.LastName,
				OrganizationId = invitation.OrganizationId,
				OrganizationRole = (OrganizationRole)invitation.OrganizationRoleId,
				OrganizationName = invitation.OrganizationName,
				EmployeeId = invitation.EmployeeId,
				DecisionDateUtc = invitation.DecisionDateUtc,
				status = (InvitationStatusEnum)invitation.InvitationStatusId
			};
		}

		/// <summary>
		/// Translates a ProductDBEntity into a Product business object.
		/// </summary>
		/// <param name="product">ProductDBEntity instance.</param>
		/// <returns>Product instance.</returns>
		public static Product InitializeProduct(ProductDBEntity product)
		{
			if (product == null)
			{
				return null;
			}

			return new Product
			{
				ProductDescription = product.Description,
				ProductId = product.ProductId,
				ProductName = product.Name,
				AreaUrl = product.AreaUrl
			};
		}

		/// <summary>
		/// Translates an Invitation business object into an InvitationDBEntity.
		/// </summary>
		/// <param name="invitation">Invitation instance.</param>
		/// <returns>InvitationDBEntity instance.</returns>
		public static InvitationDBEntity GetDBEntityFromInvitationInfo(Invitation invitation)
		{
			if (invitation == null)
			{
				return null;
			}

			return new InvitationDBEntity
			{
				Email = invitation.Email,
				FirstName = invitation.FirstName,
				InvitationId = invitation.InvitationId,
				LastName = invitation.LastName,
				OrganizationId = invitation.OrganizationId,
				OrganizationRoleId = (int)invitation.OrganizationRole,
				EmployeeId = invitation.EmployeeId,
				DecisionDateUtc = invitation.DecisionDateUtc,
				InvitationStatusId = (int)invitation.status
			};
		}

		#endregion Info-DBEntity Conversions
	}
}