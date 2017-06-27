//------------------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;

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
						switch(orgInfo.OrganizationRole)
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
				TimeTrackerRole ttRole = (TimeTrackerRole)subInfo.ProductRole;
				switch (action)
				{
					case TimeTrackerAction.CreateCustomer:
						switch(ttRole)
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

		/// <summary>
		/// Check if the logged in user can perform the target core action.
		/// </summary>
		/// <param name="targetAction">The action for which to check authorization.</param>
		/// <param name="throwException">Whether or not to throw an exception based upon the check results.</param>
		/// <param name="orgId">The organization to use for checking permissions. Defaults to the chosen organization.</param>
		/// <param name="subId">The subscription to use for checking permissions.</param>
		/// <returns>Whether or not the user has Authorization for the given action.</returns>
		public bool Can(Actions.CoreAction targetAction, bool throwException = true, int orgId = -1, int subId = -1)
		{
			bool result = false;

			// get org and sub info
			UserOrganization orgInfo = null;
			UserSubscription subInfo = null;
			if (this.UserContext != null)
			{
				this.UserContext.UserOrganizations.TryGetValue(orgId, out orgInfo);
				this.UserContext.UserSubscriptions.TryGetValue(subId, out subInfo);
			}

			// check permissions
			switch (targetAction)
			{
				case Actions.CoreAction.EditBilling:
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

					break;

				case Actions.CoreAction.EditCustomer:
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

					break;

				case Actions.CoreAction.EditInvitation:
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

					break;

				case Actions.CoreAction.EditOrganization:
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

					break;

				case Actions.CoreAction.EditProject:
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

					break;

				case Actions.CoreAction.TimeTrackerEditOthers:
					break;

				case Actions.CoreAction.TimeTrackerEditSelf:
					break;

				case Actions.CoreAction.TimeTrackerViewOthers:
					break;

				case Actions.CoreAction.TimeTrackerViewSelf:
					break;

				case Actions.CoreAction.ViewCustomer:
					break;

				case Actions.CoreAction.ViewOrganization:
					break;
				/*ORGANIZATIONAL PERMISSIONS (requires an organization ID)*/
				case Actions.CoreAction.ViewOrganization:
					if (orgInfo != null)
					{
						switch (orgInfo.OrganizationRole)
						{
							case OrganizationRole.Member:
							case OrganizationRole.Owner:
								result = true;
								break;

							default:
								break;
						}
					}

					break;

				case Actions.CoreAction.EditOrganization:
				case Actions.CoreAction.EditInvitation:
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

					break;

				/*BILLING PERMISSIONS (requires an organization ID)*/
				case Actions.CoreAction.EditBilling:
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

					break;

				/*SUBSCRIPTION PERMISSIONS (requires a subscription ID)*/

				// Product roles were previously used for both subscription action permissions and project, so we'll keep doing that to avoid breaking anything
				// In the future, we should consider separating those permissions out / create new permissions, especially since the roles are tied to the product (e.g. timetracker) but the actions are not
				// With this in mind, the customer actions and project actions are separated for now despite requiring the same permissions
				case Actions.CoreAction.ViewCustomer:
					if (subInfo != null)
					{
						switch (subInfo.ProductRole)
						{
							case TimeTrackerRole.User:
							case TimeTrackerRole.Manager:
								result = true;
								break;

							default:
								break;
						}
					}

					break;

				case Actions.CoreAction.EditCustomer:
					if (subInfo != null)
					{
						switch (subInfo.ProductRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;

							default:
								break;
						}
					}

					break;

				/*PROJECT PERMISSIONS (uses subscription permissions)*/

				// See notes above about subscription and project permissions
				// View Project permissions are unnecessary, since projects will be filtered to the user by project id in a project service object to be built in the future
				case Actions.CoreAction.EditProject:
					if (subInfo != null)
					{
						switch (subInfo.ProductRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;

							default:
								break;
						}
					}

					break;

				/*TIME TRACKER PERMISSIONS (require TimeTracker subscription roles)*/

				// There's already seperate roles for TimeTracker and Consulting (a future product), and TimeTracker actions were already separated from other subscription actions
				// So it makes sense to keep them separated
				case Actions.CoreAction.TimeTrackerEditSelf:
					if (subInfo != null)
					{
						switch (subInfo.ProductRole)
						{
							case TimeTrackerRole.User:
							case TimeTrackerRole.Manager:
								result = true;
								break;

							default:
								break;
						}
					}

					break;

				case Actions.CoreAction.TimeTrackerEditOthers:
					if (subInfo != null)
					{
						switch (subInfo.ProductRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;

							default:
								break;
						}
					}

					break;

				case Actions.CoreAction.TimeTrackerViewSelf:
					if (subInfo != null)
					{
						switch (subInfo.ProductRole)
						{
							case TimeTrackerRole.User:
							case TimeTrackerRole.Manager:
								result = true;
								break;

							default:
								break;
						}
					}

					break;

				case Actions.CoreAction.TimeTrackerViewOthers:
					if (subInfo != null)
					{
						switch (subInfo.ProductRole)
						{
							case TimeTrackerRole.Manager:
								result = true;
								break;

							default:
								break;
						}
					}

					break;

				default:
					break;
			}

			/*END OF PERMISSIONS*/
			if (!result && throwException)
			{
				throw new AccessViolationException();
			}

			return result;
		}
	}
}
