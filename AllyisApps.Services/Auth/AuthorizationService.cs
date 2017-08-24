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
			UserOrganization orgInfo = null;
			this.UserContext.UserOrganizations.TryGetValue(orgId, out orgInfo);

			if (orgInfo != null)
			{
				switch (orgInfo.OrganizationRole)
				{
					case OrganizationRole.Owner:
						result = true;
						break;

					default:
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
	}
}
