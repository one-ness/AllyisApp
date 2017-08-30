//------------------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

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
			EditUser = 1,
			EditUserPermission,
			EditInvitation,
			EditOrganization,
			EditSubscription,
			EditBilling,
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
            StatusUpdate
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
                result = CheckOrgAction(action, orgId, orgInfo.OrganizationRole, throwException);
			}

			if (!result && throwException)
			{
				string message = string.Format("action {0} denied for org {1}", action.ToString(), orgId);
				throw new AccessViolationException(message);
			}

			return result;
		}

        public bool CheckOrgAction(OrgAction action,int orgId, OrganizationRole role, bool throwException = true)
        {
            bool result = false;
            switch (role)
            {
                case OrganizationRole.Owner:
                    result = true;
                    break;

                default:
                    break;
            }
            if (!result && throwException)
            {
                string message = string.Format("action {0} denied for org {1}", action.ToString(), orgId);
                throw new AccessViolationException(message);
            }
            return result;
        }

        /// <summary>
        /// check the permissions in the org the given subscription belongs to for the given user.
        /// </summary>
        public bool CheckOrgActionForSubscriptionId(OrgAction action, int subscriptionId, bool throwException = true)
		{
			int orgId = -1;
			UserSubscription subInfo = null;
			this.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);
			if (subInfo != null)
			{
				orgId = subInfo.OrganizationId;
			}

			return this.CheckOrgAction(action, orgId, throwException);
		}

		/// <summary>
		/// check permissions for the given action in the given subscription for the logged in user.
		/// </summary>
		public bool CheckTimeTrackerAction(TimeTrackerAction action, int subId, bool throwException = true)
		{
			bool result = false;
			UserSubscription subInfo = null;
			this.UserContext.UserSubscriptions.TryGetValue(subId, out subInfo);

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
        /// <param name="action">The controller action</param>
        /// <param name="subId">The subscription id.</param>
        /// <param name="throwException">Throw exception or not.</param>
        /// <returns></returns>
        public bool CheckExpenseTrackerAction(ExpenseTrackerAction action, int subId, bool throwException = true)
        {
            bool result = false;

            UserSubscription subInfo = null;
            this.UserContext.UserSubscriptions.TryGetValue(subId, out subInfo);
			if (subInfo != null)
			{
				ExpenseTrackerRole etRole = (ExpenseTrackerRole)subInfo.ProductRoleId;
				if (subInfo.ProductId == ProductIdEnum.ExpenseTracker && etRole != ExpenseTrackerRole.NotInProduct)
				{
					if (action == ExpenseTrackerAction.AdminReport || action == ExpenseTrackerAction.StatusUpdate || action == ExpenseTrackerAction.AdminExpense)
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
					else if (action == ExpenseTrackerAction.EditReport)
					{
						switch (etRole)
						{
							case ExpenseTrackerRole.User:
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

