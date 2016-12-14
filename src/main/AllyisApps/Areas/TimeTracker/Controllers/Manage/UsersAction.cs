//------------------------------------------------------------------------------
// <copyright file="UsersAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Manage;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Manage Controller.
	/// </summary>
	public partial class ManageController : BaseProductController
	{
		/// <summary>
		/// POST: Manage/Users.
		/// </summary>
		/// <param name="model">The model object created form form data.</param>
		/// <returns>Action Result.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Users(EditSubscriptionUsersViewModel model)
		{
			int subscriptionId = UserContext.ChosenSubscriptionId;

			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				if (model == null)
				{
					return this.Users();
				}

				model.InvitationCount = Service.GetInvitationSubRoles().Where(i => i.SubscriptionId == subscriptionId).Count();

				IEnumerable<SubscriptionUserInfo> users = model.Users.Where(x => int.Parse(x.ProductRoleId) != 1);
				IEnumerable<SubscriptionUserInfo> usersToRemove = model.Users.Where(x => int.Parse(x.ProductRoleId) == 1);

				if (users.Count() + model.InvitationCount <= model.MaxUsers)
				{
					foreach (SubscriptionUserInfo user in users)
					{
						Service.UpdateSubscriptionUserProductRole(int.Parse(user.ProductRoleId), subscriptionId, user.UserId);
					}

					foreach (SubscriptionUserInfo user in usersToRemove)
					{
						Service.DeleteSubscriptionUser(subscriptionId, user.UserId);
					}

					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Manage.Strings.OrganizationUserRolesUpdated, Variety.Success));
				}
				else
				{
					string notification = string.Format("You can only have {0} users subscribed to this application at a time!", model.MaxUsers);
					Notifications.Add(new BootstrapAlert(notification, Variety.Danger));
				}

				return this.Users();
			}

			// Permissions failure
			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Manage.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.TimeTracker.Controllers.Manage.Strings.ActionUnauthorizedMessage), ControllerConstants.Subscription, ActionConstants.EditUsers));
		}

		/// <summary>
		/// GET: Manage/Users.
		/// </summary>
		/// <returns>The view for managing users.</returns>
		[HttpGet]
		public ActionResult Users()
		{
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				EditSubscriptionUsersViewModel model = this.ConstructEditSubcriptionUsersViewModel();

				return model.IsValid ? this.View(model) : this.View(ViewConstants.Error, new HandleErrorInfo(new ArgumentException(@Resources.Errors.SubscriptionNonExistantMessage), ControllerConstants.Subscription, ActionConstants.EditUsers));
			}

			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Manage.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.TimeTracker.Controllers.Manage.Strings.ActionUnauthorizedMessage), ControllerConstants.Subscription, ActionConstants.EditUsers));
		}

		/// <summary>
		/// Uses services to populate an <see cref="EditSubscriptionUsersViewModel"/> and returns it.
		/// </summary>
		/// <returns>The EditSubscriptionUsersViewModel.</returns>
		public EditSubscriptionUsersViewModel ConstructEditSubcriptionUsersViewModel()
		{
			EditSubscriptionUsersViewModel result = new EditSubscriptionUsersViewModel();
			result.Details = Service.GetSubscription(UserContext.ChosenSubscriptionId);
			if (result.Details == null)
			{
				result.IsValid = false;
			}
			else
			{
				result.Details.OrganizationId = UserContext.ChosenOrganizationId;
				result.IsValid = true;
				result.MaxUsers = result.Details.NumberOfUsers;
				result.InvitationCount = Service.GetInvitationSubRoles().Where(i => i.SubscriptionId == UserContext.ChosenSubscriptionId).Count();
				result.Users = Service.GetUsers();
				result.Roles = Service.GetProductRolesFromSubscription(UserContext.ChosenSubscriptionId);
			}

			return result;
		}
	}
}