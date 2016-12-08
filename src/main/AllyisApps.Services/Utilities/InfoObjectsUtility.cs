//------------------------------------------------------------------------------
// <copyright file="InfoObjectsUtility.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Crm;
using AllyisApps.DBModel.TimeTracker;
using AllyisApps.Services.Billing;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Services.Utilities
{
	/// <summary>
	/// The InfoObjectsUtility class.
	/// </summary>
	public class InfoObjectsUtility
	{
		/// <summary>
		/// Translates a UserDBEntity into a UserInfo business object.
		/// </summary>
		/// <param name="user">UserDBEntity instance.</param>
		/// <returns>UserInfo instance.</returns>
		public static UserInfo InitializeUserInfo(UserDBEntity user)
		{
			if (user == null)
			{
				return null;
			}

			return new UserInfo
			{
				AccessFailedCount = user.AccessFailedCount,
				ActiveOrganizationId = user.ActiveOrganizationId,
				Address = user.Address,
				City = user.City,
				Country = user.Country,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				EmailConfirmed = user.EmailConfirmed,
				FirstName = user.FirstName,
				LastName = user.LastName,
				LastSubscriptionId = user.LastSubscriptionId,
				LockoutEnabled = user.LockoutEnabled,
				LockoutEndDateUtc = user.LockoutEndDateUtc,
				PasswordHash = user.PasswordHash,
				PasswordResetCode = user.PasswordResetCode,
				PhoneExtension = user.PhoneExtension,
				PhoneNumber = user.PhoneNumber,
				PhoneNumberConfirmed = user.PhoneNumberConfirmed,
				State = user.State,
				TwoFactorEnabled = user.TwoFactorEnabled,
				UserId = user.UserId,
				UserName = user.UserName,
				PostalCode = user.PostalCode
			};
		}

		/// <summary>
		/// Initialized holiday info with a given HolidayDBEntity.
		/// </summary>
		/// <param name="hol">The HolidayDBEntity to use.</param>
		/// <returns>A holiday info object.</returns>
		public static HolidayInfo InitializeHolidayInfo(HolidayDBEntity hol)
		{
			return new HolidayInfo
			{
				CreatedUTC = hol.CreatedUTC,
				ModifiedUTC = hol.ModifiedUTC,
				Date = hol.Date,
				HolidayId = hol.HolidayId,
				HolidayName = hol.HolidayName,
				OrganizationId = hol.OrganizationId
			};
		}

		/// <summary>
		/// Initialized PayClassInfo object based on a given PayClassDBEntity.
		/// </summary>
		/// <param name="payClass">The PayClassDBEntity object to use.</param>
		/// <returns>The initialied PayClassInfo object.</returns>
		public static PayClassInfo InitializePayClassInfo(PayClassDBEntity payClass)
		{
			return new PayClassInfo
			{
				OrganizationId = payClass.OrganizationId,
				CreatedUTC = payClass.CreatedUTC,
				Name = payClass.Name,
				ModifiedUTC = payClass.ModifiedUTC,
				PayClassID = payClass.PayClassID
			};
		}

		/// <summary>
		/// Initialized a SettingsInfo object based on a given SettingDBEntity.
		/// </summary>
		/// <param name="settings">The SettingsDBEntity to use.</param>
		/// <returns>The initialized SettingsInfo object.</returns>
		public static SettingsInfo InitializeSettingsInfo(SettingDBEntity settings)
		{
			return new SettingsInfo
			{
				OrganizationId = settings.OrganizationId,
				OvertimeHours = settings.OvertimeHours,
				OvertimeMultiplier = settings.OvertimeMultiplier,
				OvertimePeriod = settings.OvertimePeriod,
				StartOfWeek = settings.StartOfWeek
			};
		}

		/// <summary>
		/// Initializes a TimeEntryInfo object based on a given TimeEntryDBEntity.
		/// </summary>
		/// <param name="entity">The TimeEntryDBEntity to use.</param>
		/// <returns>The initialized TimeEntryInfo object.</returns>
		public static TimeEntryInfo InitializeTimeEntryInfo(TimeEntryDBEntity entity)
		{
			return new TimeEntryInfo
			{
				ApprovalState = entity.ApprovalState,
				Date = entity.Date,
				Description = entity.Description,
				Duration = entity.Duration,
				FirstName = entity.FirstName,
				LastName = entity.LastName,
				LockSaved = entity.LockSaved,
				ModSinceApproval = entity.ModSinceApproval,
				PayClassId = entity.PayClassId,
				ProjectId = entity.ProjectId,
				TimeEntryId = entity.TimeEntryId,
				UserId = entity.UserId
			};
		}

		/// <summary>
		/// Builds a TimeEntryDBEntity based on a given TimeEntryInfo object.
		/// </summary>
		/// <param name="info">The TimeEntryInfo to use.</param>
		/// <returns>The built TimeEntryDBEntity based on the TimeEntryInfo object.</returns>
		public static TimeEntryDBEntity GetDBEntityFromTimeEntryInfo(TimeEntryInfo info)
		{
			return new TimeEntryDBEntity
			{
				ApprovalState = info.ApprovalState,
				Date = info.Date,
				Description = info.Description,
				Duration = info.Duration,
				FirstName = info.FirstName,
				LastName = info.LastName,
				LockSaved = info.LockSaved,
				ModSinceApproval = info.ModSinceApproval,
				PayClassId = info.PayClassId,
				ProjectId = info.ProjectId,
				TimeEntryId = info.TimeEntryId,
				UserId = info.UserId
			};
		}

		/// <summary>
		/// Translates a ProductRoleDBEntity into a ProductRoleInfo business object.
		/// </summary>
		/// <param name="productRole">ProductRoleDBEntity instance.</param>
		/// <returns>ProductRoleInfo instance.</returns>
		public static ProductRoleInfo InitializeProductRoleInfo(ProductRoleDBEntity productRole)
		{
			if (productRole == null)
			{
				return null;
			}

			return new ProductRoleInfo
			{
				CreatedUTC = productRole.CreatedUTC,
				ModifiedUTC = productRole.ModifiedUTC,
				ProductRoleName = productRole.Name,
				PermissionAdmin = productRole.PermissionAdmin,
				ProductId = productRole.ProductId,
				ProductRoleId = productRole.ProductRoleId
			};
		}

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
				UserId = organizationUser.UserId
			};
		}

		/// <summary>
		/// Translates an OrganizationDBEntity into an OrganizationInfo business object.
		/// </summary>
		/// <param name="organization">OrganizationDBEntity instance.</param>
		/// <returns>OrganizationInfo instance.</returns>
		public static OrganizationInfo InitializeOrganizationInfo(OrganizationDBEntity organization)
		{
			if (organization == null)
			{
				return null;
			}

			return new OrganizationInfo
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
		/// Creates a HolidayDBEntity based on a HolidayInfo object.
		/// </summary>
		/// <param name="holiday">The HolidayInfo to use to creat the DB entity.</param>
		/// <returns>The created HolidayDBEntity object.</returns>
		public static HolidayDBEntity GetDBEntityFromHolidayInfo(HolidayInfo holiday)
		{
			return new HolidayDBEntity()
			{
				CreatedUTC = holiday.CreatedUTC,
				Date = holiday.Date,
				HolidayId = holiday.HolidayId,
				HolidayName = holiday.HolidayName,
				ModifiedUTC = holiday.ModifiedUTC,
				OrganizationId = holiday.OrganizationId,
			};
		}

		/// <summary>
		/// Translates an OrganizationInfo business object into an OrganizationDBEntity.
		/// </summary>
		/// <param name="organization">OrganizationInfo instance.</param>
		/// <returns>OrganizationDBEntity instance.</returns>
		public static OrganizationDBEntity GetDBEntityFromOrganizationInfo(OrganizationInfo organization)
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
		/// Translates an InvitationSubRoleDBEntity into an InvitationSubRoleInfo business object.
		/// </summary>
		/// <param name="invitationSubRole">InvitationSubRoleDBEntity instance.</param>
		/// <returns>InvitationSubRoleInfo instance.</returns>
		public static InvitationSubRoleInfo InitializeInvitationSubRoleInfo(InvitationSubRoleDBEntity invitationSubRole)
		{
			if (invitationSubRole == null)
			{
				return null;
			}

			return new InvitationSubRoleInfo
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
				OrgRole = invitation.OrgRole,
				ProjectId = invitation.ProjectId,
                EmployeeId = invitation.EmployeeId
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

        /// <summary>
        /// Translates a <see cref="ProjectDBEntity"/> into a <see cref="ProjectInfo"/>.
        /// </summary>
        /// <param name="project">ProjectDBEntity instance.</param>
        /// <returns>ProjectInfo instance.</returns>
        public static ProjectInfo InitializeProjectInfo(ProjectDBEntity project)
        {
            if (project == null)
            {
                return null;
            }

            return new ProjectInfo
            {
                CustomerId = project.CustomerId,
                EndingDate = project.EndingDate,
                Name = project.Name,
                OrganizationId = project.OrganizationId,
                ProjectId = project.ProjectId,
                ProjectOrgId = project.ProjectOrgId,
                StartingDate = project.StartingDate,
                Type = project.Type
            };
        }

        /// <summary>
        /// Translates a <see cref="ProjectInfo"/> into a <see cref="ProjectDBEntity"/>.
        /// </summary>
        /// <param name="project">ProjectInfo instance.</param>
        /// <returns>ProjectDBEntity instance.</returns>
        public static ProjectDBEntity GetDBEntityFromProjectInfo(ProjectInfo project)
        {
            if (project == null)
            {
                return null;
            }

            return new ProjectDBEntity
            {
                CustomerId = project.CustomerId,
                EndingDate = project.EndingDate,
                Name = project.Name,
                OrganizationId = project.OrganizationId,
                ProjectId = project.ProjectId,
                ProjectOrgId = project.ProjectOrgId,
                StartingDate = project.StartingDate,
                Type = project.Type
            };
        }

		/// <summary>
		/// Translates a <see cref="CompleteProjectDBEntity"/> into a <see cref="CompleteProjectInfo"/>.
		/// </summary>
		/// <param name="completeProject">CompleteProjectDBEntity instance.</param>
		/// <returns>CompleteProjectInfo instance.</returns>
		public static CompleteProjectInfo InitializeCompleteProjectInfo(CompleteProjectDBEntity completeProject)
		{
			if (completeProject == null)
			{
				return null;
			}

			return new CompleteProjectInfo
			{
				CreatedUTC = completeProject.CreatedUTC,
				CustomerId = completeProject.CustomerId,
				CustomerName = completeProject.CustomerName,
				EndDate = completeProject.EndDate,
				IsActive = completeProject.IsActive,
				IsCustomerActive = completeProject.IsCustomerActive,
				IsUserActive = completeProject.IsUserActive,
				OrganizationId = completeProject.OrganizationId,
				OrganizationName = completeProject.OrganizationName,
				OrgRoleId = completeProject.OrgRoleId,
				PriceType = completeProject.PriceType,
				ProjectId = completeProject.ProjectId,
				ProjectName = completeProject.ProjectName,
				StartDate = completeProject.StartDate,
                ProjectOrgId = completeProject.ProjectOrgId
			};
		}

		/// <summary>
		/// Translates a <see cref="SubscriptionDisplayDBEntity"/> into a <see cref="SubscriptionDisplayInfo"/>.
		/// </summary>
		/// <param name="subscriptionDisplay">SubscriptionDisplayDBEntity instance.</param>
		/// <returns>SubscriptionDisplay instance.</returns>
		public static SubscriptionDisplayInfo InitializeSubscriptionDisplayInfo(SubscriptionDisplayDBEntity subscriptionDisplay)
		{
			if (subscriptionDisplay == null)
			{
				return null;
			}

			return new SubscriptionDisplayInfo
			{
				CreatedUTC = subscriptionDisplay.CreatedUTC,
				NumberOfUsers = subscriptionDisplay.NumberOfUsers,
				OrganizationId = subscriptionDisplay.OrganizationId,
				OrganizationName = subscriptionDisplay.OrganizationName,
				ProductId = subscriptionDisplay.ProductId,
				ProductName = subscriptionDisplay.ProductName,
				SkuId = subscriptionDisplay.SkuId,
				SkuName = subscriptionDisplay.SkuName,
				SubscriptionId = subscriptionDisplay.SubscriptionId,
				SubscriptionsUsed = subscriptionDisplay.SubscriptionsUsed,
				Tier = subscriptionDisplay.Tier
			};
		}

		/// <summary>
		/// Translates a <see cref="UserRolesDBEntity"/> into a <see cref="UserRolesInfo"/>.
		/// </summary>
		/// <param name="userRoles">UserRolesDBEntity instance.</param>
		/// <returns>UserRolesInfo instance.</returns>
		public static UserRolesInfo InitializeUserRolesInfo(UserRolesDBEntity userRoles)
		{
			if (userRoles == null)
			{
				return null;
			}

			return new UserRolesInfo
			{
				Email = userRoles.Email,
				FirstName = userRoles.FirstName,
				LastName = userRoles.LastName,
				Name = userRoles.Name,
				OrgRoleId = userRoles.OrgRoleId,
				ProductRoleId = userRoles.ProductRoleId,
				SubscriptionId = userRoles.SubscriptionId,
				UserId = userRoles.UserId
			};
		}
	}
}