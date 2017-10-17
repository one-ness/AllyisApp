//------------------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
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
		/// organizational actions.
		/// </summary>
		public enum OrgAction : int
		{
			CreateSubscription = 100000,
			CreateBilling,

			ReadBilling = 200000,
			ReadInvitationsList,
			ReadOrganization,
			ReadSubscription,
			ReadPermissionsList, // view list of users and their permissions
			ReadSubscriptionsList, // view list of subscriptions
			ReadUser, // view other users (view self must always be allowed)
			ReadUsersList, // view list of users

			EditUser = 300000, // edit other users (edit self must always be allowed)
			EditUserPermission,
			EditOrganization,
			EditSubscription,
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

		public enum StaffingManagerAction : int
		{
			EditCustomer
		}

		/// <summary>
		/// time tracker actions.
		/// </summary>
		public enum TimeTrackerAction : int
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
		public enum ExpenseTrackerAction : int
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
		public enum StaffingAction : int
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

		/// <summary>
		/// check permissions for the given action in the given org for the logged in user.
		/// </summary>
		public bool CheckOrgAction(OrgAction action, int orgId, bool throwException = true)
		{
			bool result = false;
			UserContext.OrganizationAndRole orgInfo = null;
			this.UserContext.OrganizationsAndRoles.TryGetValue(orgId, out orgInfo);

			if (orgInfo != null)
			{
				switch (orgInfo.OrganizationRole)
				{
					case OrganizationRole.Owner:
						result = true;
						break;

					default:
						switch (action)
						{
							case OrgAction.ReadOrganization:
								// all members can read organization details
								result = true;
								break;

							case OrgAction.ReadUsersList:
								// all members can read other user list
								result = true;
								break;

							default:
								break;
						}

						break;
				}
			}

			if (!result && throwException)
			{
				string message = string.Format("action {0} denied for org {1}", action.ToString(), orgId);
				throw new AccessViolationException(message);
			}

			return result;
		}

		public bool CheckStaffingManagerAction(StaffingManagerAction action, int subId, bool throwException = true)
		{
			bool result = false;
			UserContext.SubscriptionAndRole subInfo = null;
			this.UserContext.SubscriptionsAndRoles.TryGetValue(subId, out subInfo);

			if (subInfo != null && subInfo.ProductId == ProductIdEnum.StaffingManager)
			{
				StaffingManagerRole smRole = (StaffingManagerRole)subInfo.ProductRoleId;
				switch (action)
				{
					case StaffingManagerAction.EditCustomer:
						result = true;
						break;
					default:
						break;
				}
			}

			if (!result && throwException)
			{
				string message = string.Format("action {0} denied for subscription {1}", action.ToString(), subId);
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
			UserContext.SubscriptionAndRole subInfo = null;
			this.UserContext.SubscriptionsAndRoles.TryGetValue(subId, out subInfo);

			if (subInfo != null && subInfo.ProductId == ProductIdEnum.TimeTracker)
			{
				TimeTrackerRole ttRole = (TimeTrackerRole)subInfo.ProductRoleId;
				switch (action)
				{
					case TimeTrackerAction.TimeEntry:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
							case TimeTrackerRole.User:
								result = true;
								break;

							default:
								break;
						}
						break;

					default:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;

							default:
								break;
						}
						break;
				}
			}

			if (!result && throwException)
			{
				string message = string.Format("action {0} denied for subscription {1}", action.ToString(), subId);
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

			UserContext.SubscriptionAndRole subInfo = null;
			this.UserContext.SubscriptionsAndRoles.TryGetValue(subId, out subInfo);
			if (subInfo != null)
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

							default:
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

							case ExpenseTrackerRole.SuperUser:
								result = true;
								break;

							default:
								break;
						}
					}
					else
					{
						result = true;
					}
				}
			}

			if (!result && throwException)
			{
				string message = string.Format("action {0} denied for subscription {1}", action.ToString(), subId);
				throw new AccessViolationException(message);
			}

			return result;
		}
	}
}