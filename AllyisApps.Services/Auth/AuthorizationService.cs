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
		/// organizational actions
		/// </summary>
		public enum OrgAction : int
		{
			AddUser = 1,
			EditUser,
			DeleteUser,
			EditUserPermission,
			EditInvitation,
			DeleteInvitation,
			EditOrganization,
			DeleteOrganization,
			SubscribeToProduct,
			UnsubscribeFromProduct,
			EditBilling,
			DeleteBilling,
		}

		/// <summary>
		/// time tracker actions
		/// </summary>
		public enum TimeTrackerAction : int
		{
			TimeEntry = 1,
			CreateCustomer,
			EditCustomer,
			DeleteCustomer,
			ViewCustomer,
			CreateProject,
			EditProject,
			DeleteProject,
			ViewOthers,
			EditOthers,
		}

		/// <summary>
		/// check permissions for the given action in the given org for the logged in user
		/// </summary>
		public bool CheckOrgAction(OrgAction action, int orgId, bool throwException = true)
		{
			bool result = false;
			UserOrganization orgInfo = null;
			this.UserContext.UserOrganizations.TryGetValue(orgId, out orgInfo);

			if (orgInfo != null)
			{
				switch (action)
				{
					case OrgAction.AddUser:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.DeleteBilling:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.DeleteInvitation:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.DeleteOrganization:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.DeleteUser:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.EditBilling:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.EditInvitation:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.EditOrganization:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.EditUser:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.EditUserPermission:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.SubscribeToProduct:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

						break;

					case OrgAction.UnsubscribeFromProduct:
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}

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
		/// check the permissions in the org the given subscription belongs to for the given user
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
		/// check permissions for the given action in the given subscription for the logged in user
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
					case TimeTrackerAction.CreateCustomer:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;
						}

						break;

					case TimeTrackerAction.CreateProject:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;
						}

						break;

					case TimeTrackerAction.DeleteCustomer:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;
						}

						break;

					case TimeTrackerAction.DeleteProject:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;
						}

						break;

					case TimeTrackerAction.EditCustomer:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;
						}

						break;

					case TimeTrackerAction.EditOthers:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;
						}

						break;

					case TimeTrackerAction.EditProject:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;
						}

						break;

					case TimeTrackerAction.TimeEntry:
						// everyone with timetracker product id
						result = true;
						break;

					case TimeTrackerAction.ViewCustomer:
						result = true;
						break;

					case TimeTrackerAction.ViewOthers:
						switch (ttRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;
						}

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
	}
}
