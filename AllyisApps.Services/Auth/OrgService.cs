//------------------------------------------------------------------------------
// <copyright file="OrgService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.Services.Billing;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all Organization related services.
	/// </summary>
	public partial class Service : BaseService
	{
		/// <summary>
		/// Gets the subdomain name from the organization Id.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>Subdomain name.</returns>
		public static string GetSubdomainById(int orgId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return DBHelper.Instance.GetSubdomainById(orgId);
		}

		/// <summary>
		/// Gets the organization Id from the subdomain name.
		/// </summary>
		/// <param name="subdomain">Subdomain name.</param>
		/// <returns>Organization id.</returns>
		public static int GetIdBySubdomain(string subdomain)
		{
			//// This is actually checked automatically on any change of the subdomain field, even selecting (or clearing) it.
			//// A null value needs to be allowed or an exception is thrown before you even type anything.
			////if (subdomain == null)
			////{
			////	throw new ArgumentNullException("subdomain", "Subdomain name must not be null.");
			////}

			return DBHelper.Instance.GetIdBySubdomain(subdomain);
		}

		/// <summary>
		/// Creates an organization.
		/// </summary>
		/// <param name="organization">Organization.</param>
		/// <param name="ownerId">Organization owner user Id.</param>
		/// <param name="employeeId">Organization owner employee Id.</param>
		/// <returns>Organizaiton Id.</returns>
		public int CreateOrganization(Organization organization, int ownerId, string employeeId)
		{
			if (organization == null)
			{
				throw new ArgumentNullException("organization", "Organization must not be null.");
			}

			if (ownerId <= 0)
			{
				throw new ArgumentOutOfRangeException("ownerId", "Organization owner's user id cannot be 0 or negative.");
			}

			return DBHelper.CreateOrganization(GetDBEntityFromOrganization(organization), ownerId, (int)OrganizationRole.Owner, employeeId);
		}

		/// <summary>
		/// Gets an <see cref="Organization"/>.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>The Organization.</returns>
		public Organization GetOrganization(int orgId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return InitializeOrganization(DBHelper.GetOrganization(orgId));
		}

		/// <summary>
		/// Gets the Organization for the current chosen organization, along with OrganizationUserInfos for each user in the
		/// organization, SubscriptionDisplayInfos for any subscriptions in the organization, InvitationInfos for any invitiations
		/// pending in the organization, the organization's billing stripe handle, and a list of all products.
		/// </summary>
		/// <returns></returns>
		public Tuple<Organization, List<OrganizationUserInfo>, List<SubscriptionDisplayInfo>, List<InvitationInfo>, string, List<Product>> GetOrganizationManagementInfo()
		{
			var spResults = DBHelper.GetOrganizationManagementInfo(UserContext.ChosenOrganizationId);
			return Tuple.Create(
				InitializeOrganization(spResults.Item1),
				spResults.Item2.Select(oudb => InitializeOrganizationUserInfo(oudb)).ToList(),
				spResults.Item3.Select(sddb => InitializeSubscriptionDisplayInfo(sddb)).ToList(),
				spResults.Item4.Select(idb => InitializeInvitationInfo(idb)).ToList(),
				spResults.Item5,
				spResults.Item6.Select(pdb => InitializeProduct(pdb)).ToList());
		}

		/// <summary>
		/// Gets the Organization for the current chosen organization, along with the list of valid countries and the
		/// employee id for the current user in the current chosen organization.
		/// </summary>
		/// <returns></returns>
		public Tuple<Organization, List<string>, string> GetOrgWithCountriesAndEmployeeId()
		{
			var spResults = DBHelper.GetOrgWithCountriesAndEmployeeId(UserContext.ChosenOrganizationId, UserContext.UserId);
			return Tuple.Create(
				InitializeOrganization(spResults.Item1),
				spResults.Item2,
				spResults.Item3);
		}

		/// <summary>
		/// Gets the next recommended employee id by existing users, a list of SubscriptionDisplayInfos for subscriptions in
		/// the organization, a list of SubscriptionRoleInfos for roles within the subscriptions of the organization,
		/// a list of CompleteProjectInfos for TimeTracker projects in the organization, and the next recommended employee id
		/// by invitations.
		/// </summary>
		/// <returns></returns>
		public Tuple<string, List<SubscriptionDisplayInfo>, List<SubscriptionRole>, List<CompleteProjectInfo>, string> GetAddMemberInfo()
		{
			var spResults = DBHelper.GetAddMemberInfo(UserContext.ChosenOrganizationId);
			return Tuple.Create(
				spResults.Item1,
				spResults.Item2.Select(sddb => InitializeSubscriptionDisplayInfo(sddb)).ToList(),
				spResults.Item3.Select(srdb => InitializeSubscriptionRoleInfo(srdb)).ToList(),
				spResults.Item4.Select(cpdb => InitializeCompleteProjectInfo(cpdb)).ToList(),
				spResults.Item5);
		}

		/// <summary>
		/// Gets a list of UserRolesInfos for users in the current organization and their roles/subscription roles,
		/// and a list of SubscriptionRoles (with only SubscriptionId, ProductId, and ProductName populated) for
		/// all subscriptions in the current organization.
		/// </summary>
		/// <returns></returns>
		public Tuple<List<UserRolesInfo>, List<SubscriptionDisplayInfo>> GetOrgAndSubRoles()
		{
			var spResults = DBHelper.GetOrgAndSubRoles(UserContext.ChosenOrganizationId);
			return Tuple.Create(
				spResults.Item1.Select(urdb => InitializeUserRolesInfo(urdb)).ToList(),
				spResults.Item2.Select(sddb => InitializeSubscriptionDisplayInfo(sddb)).ToList());
		}

		/// <summary>
		/// Updates an organization chosen by the current user.
		/// </summary>
		/// <param name="organization">Updated organization.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateOrganization(Organization organization)
		{
			if (organization == null)
			{
				throw new ArgumentNullException("organization", "Organization must not be null.");
			}
			int existingSubdomainId = Service.GetIdBySubdomain(organization.Subdomain);
			if (existingSubdomainId != 0 && existingSubdomainId != organization.OrganizationId)
			{
				throw new ArgumentException("organization", "Subdomain is already taken.");
			}

			if (this.Can(Actions.CoreAction.EditOrganization))
			{
				DBHelper.UpdateOrganization(GetDBEntityFromOrganization(organization));

				return true;
			}

			return false;
		}

		/// <summary>
		/// Updates the active organization.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="orgId">Organization Id.</param>
		public void UpdateActiveOrganization(int userId, int orgId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			// Note: This method cannot use UserContext.UserId because this method is called before the service obejct's UserContext is set.
			DBHelper.UpdateActiveOrganization(userId, orgId);
		}

		/// <summary>
		/// Gets the role of the given user in the given organization.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>An OrgRole instance.</returns>
		public OrgRole GetOrgRole(int orgId, int userId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			OrgRoleDBEntity role = DBHelper.GetPermissionLevel(orgId, userId);
			if (role == null)
			{
				return null;
			}

			return new OrgRole
			{
				OrgRoleId = role.OrgRoleId,
				OrgRoleName = role.Name
			};
		}

		/// <summary>
		/// Deletes the user's current chosen organization.
		/// </summary>
		/// <returns>Returns false if permissions fail.</returns>
		public bool DeleteOrganization()
		{
			if (this.Can(Actions.CoreAction.EditOrganization))
			{
				DBHelper.DeleteOrganization(UserContext.ChosenOrganizationId);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates an invitation for a new user in the database, and also sends an email to the new user with their access code.
		/// </summary>
		/// <param name="requestingUserFullName">Full name of requesting user (not invitee).</param>
		/// <param name="webRoot">The url webroot, taken from Global Settings.</param>
		/// <param name="invitationInfo">An <see cref="InvitationInfo"/> with invitee information filled out.</param>
		/// <returns>The invitation Id.</returns>
		public async Task<int> InviteNewUser(string requestingUserFullName, string webRoot, InvitationInfo invitationInfo)
		{
			#region Validation

			if (string.IsNullOrEmpty(requestingUserFullName))
			{
				throw new ArgumentNullException("requestingUserFullName", "Requesting user full name must have a value.");
			}

			if (string.IsNullOrEmpty(webRoot))
			{
				throw new ArgumentNullException("webRoot", "Webroot must have a value.");
			}

			if (invitationInfo == null)
			{
				throw new ArgumentNullException("invitationInfo", "Invitation info object must not be null.");
			}

			#endregion Validation

			Organization orgInfo = this.GetOrganization(invitationInfo.OrganizationId);

			string htmlbody = string.Format(
				"{0} has requested you join their organization on Allyis Apps, {1}!<br /> Click <a href=http://{2}/Account/Index?accessCode={3}>Here</a> to create an account and join!",
				requestingUserFullName,
				orgInfo.Name,
				webRoot,
				invitationInfo.AccessCode);

			string msgbody = new System.Web.HtmlString(htmlbody).ToString();
			await Lib.Mailer.SendEmailAsync(
				"noreply@allyis.com",
				invitationInfo.Email,
				"Join Allyis Apps!",
				msgbody);

			return DBHelper.CreateUserInvitation(GetDBEntityFromInvitationInfo(invitationInfo));
		}

		/// <summary>
		/// Removes an invitation.
		/// </summary>
		/// <param name="invitationId">Invitation Id.</param>
		/// <returns>Returns false if permissions fail.</returns>
		public bool RemoveInvitation(int invitationId)
		{
			if (invitationId <= 0)
			{
				throw new ArgumentOutOfRangeException("invitationId", "Invitation Id cannot be 0 or negative.");
			}

			if (this.Can(Actions.CoreAction.EditOrganization))
			{
				IEnumerable<InvitationInfo> invites = this.GetUserInvitations();
				InvitationInfo thisInvite = invites.Where(x => x.InvitationId == invitationId).SingleOrDefault();
				IEnumerable<SubscriptionDisplayInfo> subs = this.DBHelper.GetSubscriptionsDisplayByOrg(UserContext.ChosenOrganizationId).Select(s => InitializeSubscriptionDisplayInfo(s));
				IEnumerable<InvitationSubRole> subRoles = DBHelper.GetInvitationSubRolesByInvitationId(invitationId).Select(i => InitializeInvitationSubRole(i));
				foreach (InvitationSubRole subRole in subRoles)
				{
					SubscriptionDisplayInfo currentSub = subs.Where(x => x.SubscriptionId == subRole.SubscriptionId).SingleOrDefault();
					if (currentSub != null && currentSub.SubscriptionsUsed < currentSub.NumberOfUsers)
					{
						DBHelper.Instance.DeleteInvitationSubRole(subRole.InvitationId, subRole.SubscriptionId);
					}
				}

				DBHelper.RemoveUserInvitation(invitationId);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Getst a list of the user invitations for the current organization.
		/// </summary>
		/// <returns>List of InvitationInfos of organization's user invitations.</returns>
		public IEnumerable<InvitationInfo> GetUserInvitations()
		{
			return DBHelper.GetUserInvitationsByOrgId(UserContext.ChosenOrganizationId).Select(i => InitializeInvitationInfo(i));
		}

		/// <summary>
		/// Creates a subscription role for an invitation.
		/// </summary>
		/// <param name="invitationId">Invitation id.</param>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <param name="selectedRole">Selected role.</param>
		public void CreateInvitationSubRole(int invitationId, int subscriptionId, int selectedRole)
		{
			#region Validation

			if (invitationId <= 0)
			{
				throw new ArgumentOutOfRangeException("invitationId", "Invitation Id cannot be 0 or negative.");
			}

			if (subscriptionId <= 0)
			{
				throw new ArgumentOutOfRangeException("subscriptionId", "Subscription Id cannot be 0 or negative.");
			}

			if (selectedRole <= 0)
			{ // TODO: Figure out if there is any further validation that can be done for this number.
				throw new ArgumentOutOfRangeException("selectedRole", "Selected role cannot be negative.");
			}

			#endregion Validation

			DBHelper.CreateInvitationSubRole(invitationId, subscriptionId, selectedRole);
		}

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
		public IEnumerable<OrganizationUserInfo> GetOrganizationMemberList(int orgId)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return DBHelper.GetOrganizationMemberList(orgId).Select(o => InitializeOrganizationUserInfo(o));
		}

		// TODO: Look more closely at the use of this method in UploadCsvFileAction to see if some other existing service method can be used instead, and this one retired.

		/// <summary>
		/// Gets the first name of a user in the current organization by email.
		/// </summary>
		/// <param name="email">Email address.</param>
		/// <returns>User's first name.</returns>
		public string GetOrgUserFirstName(string email)
		{
			#region Validation

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email", "Email address must have a value.");
			}
			else if (!Service.IsEmailAddressValid(email))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			#endregion Validation

			return DBHelper.GetOrgUserFirstName(UserContext.ChosenOrganizationId, email);
		}

		/// <summary>
		/// Gets a list of subscription details for the current organization.
		/// </summary>
		/// <returns><see cref="IEnumerable{SubscriptionInfo}"/></returns>
		public IEnumerable<SubscriptionInfo> GetSubscriptionDetails()
		{
			IEnumerable<SubscriptionDBEntity> subDBEList = DBHelper.GetSubscriptionDetails(UserContext.ChosenOrganizationId);
			List<SubscriptionInfo> list = new List<SubscriptionInfo>();
			foreach (SubscriptionDBEntity subDBE in subDBEList)
			{
				if (subDBE != null)
				{
					list.Add(new SubscriptionInfo
					{
						OrganizationName = subDBE.OrganizationName,
						OrganizationId = subDBE.OrganizationId,
						SubscriptionId = subDBE.SubscriptionId,
						SkuId = subDBE.SkuId,
						NumberOfUsers = subDBE.NumberOfUsers,
						Licenses = subDBE.Licenses,
						CreatedUTC = subDBE.CreatedUTC,
						IsActive = subDBE.IsActive,
						Name = subDBE.Name
					});
				}
			}

			return list;
		}

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
		/// Updates an organization User.
		/// </summary>
		/// <param name="userId">The updated user ID.</param>
		/// <param name="orgId">The updated org id.</param>
		/// <param name="orgRoleId">The updated org role id.</param>
		/// <param name="EmployeeId">The updated employee id. If left null, it will be the current employee id.</param>
		public void UpdateOrganizationUser(int userId, int orgId, int orgRoleId, string EmployeeId = null)
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

			if (!Enum.IsDefined(typeof(OrganizationRole), orgRoleId))
			{
				throw new ArgumentOutOfRangeException("orgRoleId", "Organization role value must correspond to a defined role in OrganizationRoleIdEnum.");
			}

			#endregion Validation

			DBHelper.UpdateOrganizationUser(new OrganizationUserDBEntity
			{
				UserId = userId,
				OrganizationId = orgId,
				OrgRoleId = orgRoleId,
				EmployeeId = EmployeeId == null ? this.GetEmployeeId(userId, orgId) : EmployeeId
			});
		}

		/// <summary>
		/// Sets the employee id to a given value for a user in an organization.
		/// </summary>
		/// <param name="userId">User id.</param>
		/// <param name="orgId">Organization id.</param>
		/// <param name="employeeId">New employee id.</param>
		public void SetEmployeeId(int userId, int orgId, string employeeId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			if (string.IsNullOrEmpty(employeeId))
			{
				throw new ArgumentNullException("employeeId", "Employee Id must have a value");
			}

			DBHelper.SetEmployeeId(userId, orgId, employeeId);
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
		/// <param name="isActive">True (default) to only return active projects, false to include all projects, active or not.</param>
		/// <returns>A list of project info objects based on Organization.</returns>
		public IEnumerable<CompleteProjectInfo> GetProjectsByOrganization(int orgId, bool isActive = true)
		{
			if (orgId < 0)
			{
				throw new ArgumentOutOfRangeException("orgId", "Organization Id cannot be negative.");
			}

			return DBHelper.GetProjectsByOrgId(orgId, isActive ? 1 : 0).Select(c => InitializeCompleteProjectInfo(c));
		}

		/// <summary>
		/// Gets the user roles for an organization.
		/// </summary>
		/// <returns>List of UserRolesInfos.</returns>
		public IEnumerable<UserRolesInfo> GetUserRoles()
		{
			return DBHelper.GetRoles(UserContext.ChosenOrganizationId).Select(o => InitializeUserRolesInfo(o));
		}

		#region Info-DBEntity Conversions
		/// <summary>
		/// Translates an OrganizationUserDBEntity into an OrganizationUserInfo business object.
		/// </summary>
		/// <param name="organizationUser">OrganizationUserDBEntity instance.</param>
		/// <returns>OrganizationUserInfo instance.</returns>
		public static OrganizationUserInfo InitializeOrganizationUserInfo(OrganizationUserDBEntity organizationUser)
		{
			if (organizationUser == null)
			{
				return null;
			}

			return new OrganizationUserInfo
			{
				CreatedUTC = organizationUser.CreatedUTC,
				EmployeeId = organizationUser.EmployeeId,
				OrganizationId = organizationUser.OrganizationId,
				OrgRoleId = organizationUser.OrgRoleId,
				UserId = organizationUser.UserId,
				Email = organizationUser.Email,
				FirstName = organizationUser.FirstName,
				LastName = organizationUser.LastName
			};
		}

		/// <summary>
		/// Translates an OrganizationDBEntity into an Organization business object.
		/// </summary>
		/// <param name="organization">OrganizationDBEntity instance.</param>
		/// <returns>Organization instance.</returns>
		public static Organization InitializeOrganization(OrganizationDBEntity organization)
		{
			if (organization == null)
			{
				return null;
			}

			return new Organization
			{
				Address = organization.Address,
				City = organization.City,
				Country = organization.Country,
				DateCreated = organization.CreatedUTC,
				FaxNumber = organization.FaxNumber,
				Name = organization.Name,
				OrganizationId = organization.OrganizationId,
				PhoneNumber = organization.PhoneNumber,
				SiteUrl = organization.SiteUrl,
				State = organization.State,
				Subdomain = organization.Subdomain,
				PostalCode = organization.PostalCode
			};
		}

		/// <summary>
		/// Translates an Organization business object into an OrganizationDBEntity.
		/// </summary>
		/// <param name="organization">Organization instance.</param>
		/// <returns>OrganizationDBEntity instance.</returns>
		public static OrganizationDBEntity GetDBEntityFromOrganization(Organization organization)
		{
			if (organization == null)
			{
				return null;
			}

			return new OrganizationDBEntity
			{
				Address = organization.Address,
				City = organization.City,
				Country = organization.Country,
				CreatedUTC = organization.DateCreated,
				FaxNumber = organization.FaxNumber,
				Name = organization.Name,
				OrganizationId = organization.OrganizationId,
				PhoneNumber = organization.PhoneNumber,
				SiteUrl = organization.SiteUrl,
				State = organization.State,
				Subdomain = organization.Subdomain,
				PostalCode = organization.PostalCode
			};
		}

		/// <summary>
		/// Translates an InvitationSubRoleDBEntity into an InvitationSubRole business object.
		/// </summary>
		/// <param name="invitationSubRole">InvitationSubRoleDBEntity instance.</param>
		/// <returns>InvitationSubRole instance.</returns>
		public static InvitationSubRole InitializeInvitationSubRole(InvitationSubRoleDBEntity invitationSubRole)
		{
			if (invitationSubRole == null)
			{
				return null;
			}

			return new InvitationSubRole
			{
				InvitationId = invitationSubRole.InvitationId,
				ProductRoleId = invitationSubRole.ProductRoleId,
				SubscriptionId = invitationSubRole.SubscriptionId
			};
		}

		/// <summary>
		/// Translates an InvitationDBEntity into an InvitationInfo business object.
		/// </summary>
		/// <param name="invitation">InvitationDBEntity instance.</param>
		/// <returns>InvitationInfo instance.</returns>
		public static InvitationInfo InitializeInvitationInfo(InvitationDBEntity invitation)
		{
			if (invitation == null)
			{
				return null;
			}

			return new InvitationInfo
			{
				AccessCode = invitation.AccessCode,
				DateOfBirth = invitation.DateOfBirth,
				Email = invitation.Email,
				CompressedEmail = Service.GetCompressedEmail(invitation.Email),
				FirstName = invitation.FirstName,
				InvitationId = invitation.InvitationId,
				LastName = invitation.LastName,
				OrganizationId = invitation.OrganizationId,
				OrganizationName = invitation.OrganizationName,
				OrgRole = invitation.OrgRole,
				OrgRoleName = invitation.OrgRoleName,
				ProjectId = invitation.ProjectId,
				EmployeeId = invitation.EmployeeId
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
				ProductName = product.Name
			};
		}

		/// <summary>
		/// Translates an InvitationInfo business object into an InvitationDBEntity.
		/// </summary>
		/// <param name="invitation">InvitationInfo instance.</param>
		/// <returns>InvitationDBEntity instance.</returns>
		public static InvitationDBEntity GetDBEntityFromInvitationInfo(InvitationInfo invitation)
		{
			if (invitation == null)
			{
				return null;
			}

			return new InvitationDBEntity
			{
				AccessCode = invitation.AccessCode,
				DateOfBirth = invitation.DateOfBirth,
				Email = invitation.Email,
				FirstName = invitation.FirstName,
				InvitationId = invitation.InvitationId,
				LastName = invitation.LastName,
				OrganizationId = invitation.OrganizationId,
				OrgRole = invitation.OrgRole,
				ProjectId = invitation.ProjectId,
				EmployeeId = invitation.EmployeeId
			};
		}
		#endregion
	}
}
