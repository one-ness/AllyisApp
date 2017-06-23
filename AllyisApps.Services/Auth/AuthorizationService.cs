//------------------------------------------------------------------------------
// <copyright file="authorizationService.cs" company="Allyis, Inc.">
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
		/// Check if the logged in user can perform the target core action.
		/// </summary>
		/// <param name="targetAction">The action for which to check authorization.</param>
		/// <param name="throwException">Whether or not to throw an exception based upon the check results.</param>
		/// <param name="organizationId">The organization to use for checking permissions. Defaults to the chosen organization.</param>
        /// <param name="subscriptionId">The subscription to use for checking permissions.</param>
		/// <returns>Whether or not the user has Authorization for the given action.</returns>
		public bool Can(Actions.CoreAction targetAction, bool throwException = false, int organizationId = -1, int subscriptionId = -1)
		{
			// TODO: throwException defaults to True, result defaults to True, adjust bottom return result accordingly
			bool result = false;
			UserOrganization orgInfo = null;
			UserSubscription subInfo = null;

			if (organizationId == -1) organizationId = UserContext.ChosenOrganizationId;
            if (subscriptionId == -1) subscriptionId = UserContext.ChosenSubscriptionId;

			// Get role information
			// has the user chosen an organization
			if (organizationId > 0)
			{
				// get the user role in chosen organization
				UserContext.UserOrganizations.TryGetValue(organizationId, out orgInfo);

				// Info should never be null
				if (orgInfo != null)
				{
					// Find the subscription that is relevant to the target action, if it exists.
					ProductIdEnum product = Actions.GetProductForAction(targetAction);
					if (product != ProductIdEnum.None)
					{
                        //orgInfo.UserSubscriptions.TryGetValue((int)product, out subInfo);
                        orgInfo.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);
                    }
				}
			}

			/*BEGIN PERMISSIONS*/

			// Check role permissions against action types
			// Different actions require checking different roles, so switching on Action first makes more sense
			// Note: Grouping all of the actions here makes the code look cleaner and adding more actions easier, but we can alternatively spread these out in the checks above
			switch (targetAction)
			{
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
							case ProductRoleIdEnum.TimeTrackerUser:
							case ProductRoleIdEnum.TimeTrackerManager:
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
							case ProductRoleIdEnum.TimeTrackerManager:
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
							case ProductRoleIdEnum.TimeTrackerManager:
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
							case ProductRoleIdEnum.TimeTrackerUser:
							case ProductRoleIdEnum.TimeTrackerManager:
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
							case ProductRoleIdEnum.TimeTrackerManager:
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
							case ProductRoleIdEnum.TimeTrackerUser:
							case ProductRoleIdEnum.TimeTrackerManager:
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
							case ProductRoleIdEnum.TimeTrackerManager:
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
