//------------------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all authorization related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		/// <summary>
		/// logical grouping of business objects in the application, the user can take action on
		/// </summary>
		public enum AppEntity : int
		{
			Billing = 10,
			Organization = 20,
			OrganizationUser = 30,
			Subscription = 40,
			SubscriptionUser = 50,
			Permission = 60,
		}

		/// <summary>
		/// list of actions a user can take
		/// </summary>
		public enum UserAction : int
		{
			Create = 10,
			Read = 20,
			Update = 30,
			Delete = 40,
		}

		/// <summary>
		/// checks if the logged in user has permission to perform the given action on the given entity, in the given organziatio or subscription
		/// </summary>
		public async Task<bool> CheckPermissionAsync(ProductIdEnum productId, UserAction userActionId, AppEntity appEntityId, int orgOrSubId, bool throwException = true)
		{
			bool result = false;
			var productRoleId = 0;
			// product id tells us if it is org or sub
			if (productId == ProductIdEnum.AllyisApps)
			{
				// it is organization id
				// check if the user is part of the organization
				UserContext.OrganizationAndRole role = null;
				if (this.UserContext.OrganizationsAndRoles.TryGetValue(orgOrSubId, out role))
				{
					productRoleId = role.OrganizationRoleId;
				}
			}
			else
			{
				// it is subscription id
				// check if the user is part of the subscription
				UserContext.SubscriptionAndRole role = null;
				if (this.UserContext.SubscriptionsAndRoles.TryGetValue(orgOrSubId, out role))
				{
					productRoleId = role.SubscriptionId;
				}
			}

			// is the user part of the subscription or organization?
			if (productRoleId > 0)
			{
				// yes, check the db for permission for this role for this action on this entity
				var entity = await this.DBHelper.GetPermissionAsync(productRoleId, (int)userActionId, (int)appEntityId);
				// is there a permission entry?
				if (entity != null)
				{
					// yes
					result = !entity.IsDenied;
				}
			}

			// does the caller want to throw exception?
			if (!result && throwException)
			{
				// yes
				throw new AccessViolationException(string.Format("Access denied to perform action: {0} on entity: {1}.", userActionId, appEntityId));
			}

			return result;
		}

		/// <summary>
		/// organizational actions.
		/// </summary>
		public enum OrgAction
		{
			CreateSubscription = 100000,
			CreateBilling,

			ReadBilling = 200000,
			ReadInvitationsList,
			ReadOrganization,
			ReadSubscription,
			ReadPermissions, // view list of users and their permissions
			ReadSubscriptions, // view list of subscriptions
			ReadUser, // view other users (view self must always be allowed)
			ReadUsersList, // view list of users

			EditUser = 300000, // edit other users (edit self must always be allowed)
			EditUserPermission,
			EditOrganization,
			EditSubscription,
			EditSubscriptionUser,
			EditBilling,
			AddUserToOrganization, // same as create invitation
			AddUserToSubscription,
			ChangePassword, // change password for others (change for self must always be allowed)
			ResendInvitation,

			DeleteUserFromOrganization = 400000,
			DeleteUserFromSubscription,
			DeleteInvitation,
			DeleteOrganization,
			DeleteSubscritpion,
			DeleteBilling,
		}

		public enum StaffingManagerAction
		{
			EditCustomer,
			ViewCustomer,
			EditProject
		}

		/// <summary>
		/// time tracker actions.
		/// </summary>
		public enum TimeTrackerAction
		{
			TimeEntry = 1,
			EditCustomer,
			ViewCustomer,
			EditProject,
			ViewOthers,
			EditOthers,
		}

		/// <summary>
		/// Expense Tracker Actions.
		/// </summary>
		public enum ExpenseTrackerAction
		{
			Unmanaged = 0,
			EditReport,
			AdminReport,
			AdminExpense,
			StatusUpdate,
			Pending,
			UpdateReport,
			CreateReport,
			UserSettings,
			Accounts
		}

		/// <summary>
		/// staffing actions.
		/// </summary>
		public enum StaffingAction
		{
			Index = 1,
			EditPosition,
			ViewPosition,
			EditApplicant,
			ViewApplicant,
			EditApplication,
			ViewApplication,
			ViewOthers,
			EditOthers,
		}

		public bool CheckStaffingManagerAction(StaffingManagerAction action, int subId, bool throwException = true)
		{
			bool result = false;
			UserContext.SubscriptionsAndRoles.TryGetValue(subId, out UserContext.SubscriptionAndRole subInfo);
			if (subInfo?.ProductId == ProductIdEnum.StaffingManager)
			{
				StaffingManagerRole smRole = (StaffingManagerRole)subInfo.ProductRoleId;
				switch (action)
				{
					case StaffingManagerAction.EditCustomer:
						result = true;
						break;
					default:
						result = true;
						break;
				}
			}

			if (!result && throwException)
			{
				string message = string.Format("action {0} denied for subscription {1}", action, subId);
				throw new AccessViolationException(message);
			}

			return result;
		}

		/// <summary>
		/// check permissions for the given action in the given subscription for the logged in user.
		/// </summary>
		public bool CheckTimeTrackerAction(TimeTrackerAction action, int subId, bool throwException = true)
		{
			bool result = false;
			UserContext.SubscriptionsAndRoles.TryGetValue(subId, out UserContext.SubscriptionAndRole subInfo);
			if (subInfo?.ProductId == ProductIdEnum.TimeTracker)
			{
				TimeTrackerRole ttRole = (TimeTrackerRole)subInfo.ProductRoleId;
				switch (action)
				{
					case TimeTrackerAction.TimeEntry:
						switch (ttRole)
						{
							case TimeTrackerRole.Admin:
							case TimeTrackerRole.User:
								result = true;
								break;
						}
						break;

					default:
						switch (ttRole)
						{
							case TimeTrackerRole.Admin:
								result = true;
								break;
						}
						break;
				}
			}

			if (!result && throwException)
			{
				string message = string.Format("action {0} denied for subscription {1}", action, subId);
				throw new AccessViolationException(message);
			}

			return result;
		}

		/// <summary>
		/// Checks if an action is allowed for the current user.
		/// </summary>
		/// <param name="action">The controller action.</param>
		/// <param name="subId">The subscription id.</param>
		/// <param name="throwException">Throw exception or not.</param>
		/// <returns></returns>
		public bool CheckExpenseTrackerAction(ExpenseTrackerAction action, int subId, bool throwException = true)
		{
			bool result = false;

			if (UserContext.SubscriptionsAndRoles.TryGetValue(subId, out UserContext.SubscriptionAndRole subInfo) && subInfo.ProductId == ProductIdEnum.ExpenseTracker)
			{
				ExpenseTrackerRole etRole = (ExpenseTrackerRole)subInfo.ProductRoleId;
				if (subInfo.ProductId == ProductIdEnum.ExpenseTracker && etRole != ExpenseTrackerRole.NotInProduct)
				{
					if (action == ExpenseTrackerAction.AdminReport
						|| action == ExpenseTrackerAction.StatusUpdate
						|| action == ExpenseTrackerAction.AdminExpense
						|| action == ExpenseTrackerAction.UserSettings
						|| action == ExpenseTrackerAction.Accounts)
					{
						switch (etRole)
						{
							case ExpenseTrackerRole.Manager:
								result = true;
								break;
						}
					}
					else if (action == ExpenseTrackerAction.Pending)
					{
						switch (etRole)
						{
							case ExpenseTrackerRole.Manager:
								result = true;
								break;

							case ExpenseTrackerRole.Admin:
								result = true;
								break;
						}
					}
					else
					{
						result = true;
					}
				}
			}

			if (result || !throwException) return result;

			string message = string.Format("action {0} denied for subscription {1}", action, subId);
			throw new AccessViolationException(message);
		}
	}
}
