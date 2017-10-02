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
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Lookup;

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
		public int SetupOrganization(string employeeId, string organizationName, string phoneNumber, string faxNumber, string siteUrl, string subDomainName, string address1, string city, int? stateId, string postalCode, string countryCode)
		{
			if (string.IsNullOrWhiteSpace(employeeId)) throw new ArgumentNullException("employeeId");
			if (string.IsNullOrWhiteSpace(organizationName)) throw new ArgumentNullException("organizationName");

			return this.DBHelper.SetupOrganization(this.UserContext.UserId, (int)OrganizationRole.Owner, employeeId, organizationName, phoneNumber, faxNumber, siteUrl, subDomainName, address1, city, stateId, postalCode, countryCode);
		}

		/// <summary>
		/// Gets an <see cref="Organization"/>.
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>The Organization.</returns>
		public Organization GetOrganization(int orgId)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException("orgId");
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
		/// and the next recommended employee id by invitations.
		/// </summary>
		/// <param name="orgId">The Organization Id.</param>
		/// <returns>.</returns>
		public Organization GetAddMemberInfo(int orgId)
		{
			var spResults = DBHelper.GetAddMemberInfo(orgId);
			Organization org = new Organization()
			{
				NextEmpolyeeID = string.Compare(spResults.Item1, spResults.Item4) > 0 ? spResults.Item1 : spResults.Item4,
				Subscriptions = spResults.Item2.Select(sddb => InitializeSubscription(sddb)).ToList()
			};
			var productRoles = spResults.Item3.Select(srdb => InitializeSubscriptionRoleInfo(srdb)).ToList();

			foreach (Subscription sub in org.Subscriptions)
			{
				sub.ProductRoles = productRoles.Where(pr => sub.ProductId == pr.ProductId).ToList();
			}
			return org;
		}

		/// <summary>
		/// Gets a list of UserRolesInfos for users in the current organization and their roles/subscription roles,
		/// and a list of Subscriptions (with only SubscriptionId, ProductId, and ProductName populated) for
		/// all subscriptions in the current organization.
		/// TODO: Redisign to populate Organization Service object and child objects.
		/// </summary>
		/// <param name="orgId">The Organization Id.</param>
		/// <returns>.</returns>
		public OrganizaionPermissions GetOrgAndSubRoles(int orgId)
		{
			var spResults = DBHelper.GetOrgAndSubRoles(orgId);

			return new OrganizaionPermissions()
			{
				UserRoles = spResults.Item1.Select(urdb => InitializeUserRole(urdb)).ToList(),
				Subscriptions = spResults.Item2.Select(sddb => InitializeSubscription(sddb)).ToList()
			};
		}

		/// <summary>
		/// Updates an organization chosen by the current user.
		/// </summary>
		public bool UpdateOrganization(int organizationId, string organizationName, string siteUrl, int? addressId, string address1, string city, int? stateId, string countryCode, string postalCode, string phoneNumber, string faxNumber, string subDomain)
		{
			if (organizationId <= 0) throw new ArgumentOutOfRangeException("organizationId");
			if (string.IsNullOrWhiteSpace(organizationName)) throw new ArgumentNullException("organizationName");

			this.CheckOrgAction(OrgAction.EditOrganization, organizationId);

			return this.DBHelper.UpdateOrganization(organizationId, organizationName, siteUrl, addressId, address1, city, stateId, countryCode, postalCode, phoneNumber, faxNumber, subDomain) > 0;
		}

		/// <summary>
		/// Deletes the user's current chosen organization.
		/// </summary>
		/// <returns>Returns false if permissions fail.</returns>
		public void DeleteOrganization(int orgId)
		{
			if (orgId <= 0) throw new ArgumentOutOfRangeException("orgId");
			this.CheckOrgAction(OrgAction.DeleteOrganization, orgId);
			this.DBHelper.DeleteOrganization(orgId);
		}

		/// <summary>
		/// Creates an invitation for a new user in the database, and also sends an email to the new user with their access code.
		/// </summary>
		public int InviteUser(string url, string email, string firstName, string lastName, int organizationId, OrganizationRole organizationRoleId, string employeedId)
		{
			if (organizationId <= 0) throw new ArgumentOutOfRangeException("organizationId");
			this.CheckOrgAction(OrgAction.AddUserToOrganization, organizationId);
			if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException("url");
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");
			if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException("firstName");
			if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException("lastName");
			if (string.IsNullOrWhiteSpace(employeedId)) throw new ArgumentNullException("employeedId");

			// Creation of invitation
			var result = DBHelper.CreateInvitation(email, firstName, lastName, organizationId, (int)organizationRoleId, employeedId);

			if (result == -1)
			{
				throw new DuplicateNameException("User is already a member of the organization.");
			}

			if (result == -2)
			{
				throw new InvalidOperationException("Employee Id is already taken.");
			}

			// Send invitation email
			string orgName = string.Empty;
			string htmlbody = string.Format(
				"{0} {1} has requested you join their organization on Allyis Apps{2}!<br /> Click <a href={3}>Here</a> to create an account and join!",
				this.UserContext.FirstName,
				this.UserContext.LastName,
				orgName,
				url);

			string msgbody = new System.Web.HtmlString(htmlbody).ToString();
			var task = Mailer.SendEmailAsync(
				this.ServiceSettings.SupportEmail,
				email,
				"Join Allyis Apps!",
				msgbody);
			// TODO: how to indicate there was an error sending the email? how to send the invite email again in that case?
			var mailSuccess = task.Result;

			// Return invitation id
			return result;
		}

		public async void NotifyInviteAcceptAsync(int inviteId)
		{
			InvitationDBEntity invitation = DBHelper.GetUserInvitationByInviteId(inviteId);
			IEnumerable<dynamic> owners = DBHelper.GetOrgOwnerEmails(invitation.OrganizationId);

			string htmlbody = string.Format(
				"{0} has joined the organization {1} on Allyis Apps.",
				invitation.FirstName + " " + invitation.LastName,
				invitation.OrganizationName);

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
			IEnumerable<dynamic> owners = DBHelper.GetOrgOwnerEmails(invitation.OrganizationId);

			string htmlbody = string.Format(
				"{0} has rejected joining the organization {1} on Allyis Apps.",
				invitation.FirstName + " " + invitation.LastName,
				invitation.OrganizationName);

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
		/// <param name="entity"></param>
		/// <param name="loadAddress"></param>
		/// <returns></returns>
		public Organization InitializeOrganization(OrganizationDBEntity entity, bool loadAddress = true)
		{
			return new Organization
			{
				CreatedUtc = entity.CreatedUtc,
				FaxNumber = entity.FaxNumber,
				OrganizationName = entity.OrganizationName,
				OrganizationId = entity.OrganizationId,
				PhoneNumber = entity.PhoneNumber,
				SiteUrl = entity.SiteUrl,
				Subdomain = entity.Subdomain,
				Address = loadAddress ? getAddress(entity.AddressId) : null
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
				ProductId = (ProductIdEnum)product.ProductId,
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