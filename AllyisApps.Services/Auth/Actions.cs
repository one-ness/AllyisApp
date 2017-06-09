//------------------------------------------------------------------------------
// <copyright file="Actions.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

#pragma warning disable 1591

namespace AllyisApps.Services
{
	/// <summary>
	/// List of actions that can be taken by a user.
	/// </summary>
	public partial class Actions
	{
		// Actions that do not need permissions include logging on and off, registering, email validation, password and profile editing, and organization creating

		/// <summary>
		/// Core actions taken by the user. These require an organization Id.
		/// </summary>
		public enum CoreAction : int
		{
			// We might want to seperate some of these actions into other categories (i.e. not in Core Action) in the future

			// OrgActions only require an orgID

			/// <summary>
			/// Edit organization info, add/remove user, modify user permissions, add/remove subscriptions, adjust billing info; no reason to be able to do one of these but not another.
			/// </summary>
			EditOrganization = 1,

			/// <summary>
			/// View organization and org details; should not be available to all users, only to members of the organization.
			/// </summary>
			ViewOrganization = 2,

			// EditOrganizationBilling = x, Possible separation of responsibilities if new financial organization user role is created, for now not necessary.

			// Subscription Actions additionally require a subscription Id

			/// <summary>
			/// Actions that involve viewing customer information.
			/// </summary>
			ViewCustomer = 3,

			/// <summary>
			/// Actions that involve creating, editing, or deleting customers.
			/// </summary>
			EditCustomer = 4,

			// Project Actions also require a subscription id .
			// Product roles were previously used for both subscription action permissions and project, so we'll keep doing that to avoid breaking anything.
			// Project actions use to require ids, but now the ids will be filtered separately from permissions with some sort of project filter service object.

			/// <summary>
			/// Permissions for editing details of a project, assigning users to the project, deleting the project, etc.
			/// </summary>
			EditProject = 5,

			// TimeTracker actions (these use the time tracker subscription roles)
			// Possibly move these from core actions?

			/// <summary>
			/// Allow all actions for one's own time tracker entries.
			/// </summary>
			TimeTrackerEditSelf = 6,

			/// <summary>
			/// Allow all actions for viewing, editing, etc. the entries of others.
			/// </summary>
			TimeTrackerEditOthers = 7,

            /// <summary>
            /// Allow user to view their organization's Time Tracker information
            /// </summary>
            TimeTrackerViewSelf = 8,

            /// <summary>
            /// Allow managers, accountants, etc. to view other user's Time Tracker information within the organization
            /// </summary>
            TimeTrackerViewOthers = 9,

            /// <summary>
            /// Allow all actions for editing, removing, etc. an invitation to an organization
            /// </summary>
            EditInvitation = 10,

            /// <summary>
            /// Allow all actions for editing, removing, etc. billing for an organization
            /// </summary>
            EditBilling = 11,
		}

		/// <summary>
		/// Returns a ProductIdEnum for the product matching a given action, or ProductIdEnum.None if
		/// the action is not for a product.
		/// </summary>
		/// <param name="action">The action to check.</param>
		/// <returns>The ProductIdEnum for the product matching the given action.</returns>
		public static ProductIdEnum GetProductForAction(CoreAction action)
		{
			if (action == CoreAction.EditCustomer ||
				action == CoreAction.ViewCustomer ||
				action == CoreAction.EditProject ||
				action == CoreAction.TimeTrackerEditSelf ||
				action == CoreAction.TimeTrackerEditOthers ||
                action == CoreAction.TimeTrackerViewSelf ||
                action == CoreAction.TimeTrackerViewOthers)
			{
				return ProductIdEnum.TimeTracker;
			}

			return ProductIdEnum.None;
		}
	}
}
