//------------------------------------------------------------------------------
// <copyright file="ManagePermissionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Extensions.IEnumerableExtensions;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET Account/ManagePermissions.
		/// </summary>
		/// <returns>Action result.</returns>
		[HttpGet]
		public ActionResult ManagePermissions()
		{
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				PermissionsManagementViewModel model = this.ConstructPermissionsManagementViewModel();
				return this.View("Permission", model);
			}

			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction("OrgIndex");
		}

		/// <summary>
		/// GET Account/ManagePermissions.
		/// </summary>
		/// <returns>Action result.</returns>
		[HttpGet]
		public ActionResult ManagePermissions2()
		{
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				var infos = Service.GetOrgAndSubRoles();
				ManagePermissionsViewModel model = new ManagePermissionsViewModel
				{
					Users = new List<UserPermissionsViewModel>(),
					Subscriptions = infos.Item2,
					SubIds = infos.Item2.Select(s => s.SubscriptionId).ToList(),
					// TODO: Get rid of this once product panes in Permissions page are genericized.
					TimeTrackerId = Service.GetProductIdByName(ProductNameKeyConstants.TimeTracker)
				};

				// This can also be axed after finding a good way to genericize products in the Permissions page.
				var ttsub = model.Subscriptions.Where(s => s.ProductId == model.TimeTrackerId).SingleOrDefault();
				if (ttsub != null)
				{
					model.TimeTrackerSubIndex = model.Subscriptions.IndexOf(ttsub);
				}
				
				foreach (UserRolesInfo role in infos.Item1)
				{
					UserPermissionsViewModel modelUser = model.Users.Where(u => u.UserId == int.Parse(role.UserId)).SingleOrDefault();
					if (modelUser == null)
					{
						modelUser = new UserPermissionsViewModel
						{
							FirstName = role.FirstName,
							LastName = role.LastName,
							UserId = int.Parse(role.UserId),
							Email = role.Email,
							OrgRoleId = role.OrgRoleId,
							ProductRoleIds = new List<int>()
						};

						// Start out with default NotInProduct role
						foreach(SubscriptionDisplayInfo sub in model.Subscriptions)
						{
							modelUser.ProductRoleIds.Add((int)ProductRole.NotInProduct);
						}
						model.Users.Add(modelUser);
					}

					if (role.SubscriptionId != -1)
					{
						int index = model.SubIds.IndexOf(role.SubscriptionId);
						if (index >= 0)
						{
							modelUser.ProductRoleIds[model.SubIds.IndexOf(role.SubscriptionId)] = role.ProductRoleId;
						}
					}
				}
				
				return this.View("Permission2", model);
			}

			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction("OrgIndex");
		}

		/// <summary>
		/// Uses services to populate a <see cref="PermissionsManagementViewModel"/> and returns it.
		/// </summary>
		/// <returns>The PermissionsManagementViewModel.</returns>
		public PermissionsManagementViewModel ConstructPermissionsManagementViewModel()
		{
			PermissionsManagementViewModel result = new PermissionsManagementViewModel()
			{
				TimeTrackerId = Service.GetProductIdByName(ProductNameKeyConstants.TimeTracker)
			};
			result.Subscriptions = Service.GetSubscriptionsDisplay();

			List<UserPermissionsManagement> permissions = new List<UserPermissionsManagement>();
			IEnumerable<UserRolesInfo> users = Service.GetUserRoles().OrderBy(u => u.UserId); // In case of multiple subscriptions, there can be multiple items per user, one for each sub role
			string currentUser = string.Empty;
			UserPermissionsManagement currentUserPerm = null;
			foreach (UserRolesInfo user in users)
			{
				if (!user.UserId.Equals(currentUser))
				{
					currentUser = user.UserId;
					currentUserPerm = new UserPermissionsManagement()
					{
						UserId = user.UserId,
						UserName = string.Format("{0} {1}", user.FirstName, user.LastName),
						OrganizationRoleId = user.OrgRoleId,
						SubscriptionRoles = new List<SubscriptionRoleInfo>()
					};
					permissions.Add(currentUserPerm);
				}

				if (user.ProductRoleId > 0)
				{
					try
					{
						currentUserPerm.SubscriptionRoles.Add(new SubscriptionRoleInfo
						{
							ProductRoleId = user.ProductRoleId,
							ProductId = result.Subscriptions.Where(s => s.SubscriptionId == user.SubscriptionId).Single().ProductId
						});
					} catch (InvalidOperationException) { } // Deleted subscription
				}

				//List<SubscriptionRoleInfo> subRoles = new List<SubscriptionRoleInfo>();
				//SubscriptionRoleInfo temp = new SubscriptionRoleInfo() { ProductRoleId = user.ProductRoleId };
				//subRoles.Add(temp);
				//permissions.Add(new UserPermissionsManagement()
				//{
				//	UserId = user.UserId,
				//	UserName = string.Format("{0} {1}", user.FirstName, user.LastName),
				//	OrganizationRoleId = user.OrgRoleId,
				//	SubscriptionRoles = subRoles
				//});
			}

			// Add in "Not in Product" roles for subscriptions each user is not assigned to
			foreach (UserPermissionsManagement permission in permissions)
			{
				foreach (SubscriptionDisplayInfo subscription in result.Subscriptions)
				{
					if (permission.SubscriptionRoles.Where(s => s.ProductId == subscription.ProductId).Count() == 0)
					{
						permission.SubscriptionRoles.Add(new SubscriptionRoleInfo
						{
							ProductId = subscription.ProductId,
							ProductRoleId = (int)ProductRole.NotInProduct
						});
					}
				}
			}

			result.UserPermissions = permissions.DistinctBy(u => u.UserId).OrderBy(u => u.UserName.Split(' ').Last()).ToList();   // UserRoles are unique via SubscriptionId and UserId, but UserPermissionsManagement does not track SubscriptionId, causing duplicate users to be stored

			result.Filters = new FilterDataModel();
			result.Filters.UnassignedUsers = new ViewModels.Auth.Filter("Unassigned", users, x => x.ProductRoleId == 0);
			result.Filters.AllUsers = new ViewModels.Auth.Filter("All Users", users);
			FilterGroup orgFilters = result.Filters.AddNewFilterGroup("Organization");
			orgFilters.Filters.Add(new ViewModels.Auth.Filter("Owner", users, x => x.OrgRoleId == (int)OrganizationRole.Owner));
			orgFilters.Filters.Add(new ViewModels.Auth.Filter("Member", users, x => x.OrgRoleId == (int)OrganizationRole.Member));

			FilterGroup timeTrackerFilters = result.Filters.AddNewFilterGroup("TimeTracker");
			timeTrackerFilters.Filters.Add(new ViewModels.Auth.Filter("Any", users, x => x.ProductRoleId != 0));
			timeTrackerFilters.Filters.Add(new ViewModels.Auth.Filter("Manager", users, u => u.ProductRoleId == (int)ProductRole.TimeTrackerManager));
			timeTrackerFilters.Filters.Add(new ViewModels.Auth.Filter("User", users, u => u.ProductRoleId == (int)ProductRole.TimeTrackerUser));
			timeTrackerFilters.Filters.Add(new ViewModels.Auth.Filter("Unassigned", users, x => x.ProductRoleId == 0));

			return result;
		}

		/// <summary>
		/// Makes changes to users' permissions in the organization.
		/// Called from Account/Permission do_it_submit().
		/// </summary>
		/// <param name="data">The JSON string of the model of actions and users.</param>
		/// <returns>A Json object representing the results of the actions.</returns>
		public ActionResult ManagePermissions(string data)
		{
			UserPermissionsAction model = JsonConvert.DeserializeObject<UserPermissionsAction>(data);
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				if (model.SelectedActions == null)
				{
					Notifications.Add(new BootstrapAlert("No actions were selected.", Variety.Danger));
					return RedirectToAction(ActionConstants.Manage);
				}

				if (model.SelectedUsers == null)
				{
					Notifications.Add(new BootstrapAlert("No users were selected.", Variety.Danger));
					return RedirectToAction(ActionConstants.Manage);
				}

				model.SelectedActions.OrganizationId = UserContext.ChosenOrganizationId;
				model.SelectedActions.TimeTrackerSubscriptionId = Service.GetSubscriptionDetails()
					.Select(x => x.SubscriptionId)
					.SingleOrDefault();

				PermissionsActionsResults result = new PermissionsActionsResults();
				if (model.SelectedActions.OrgRoleTarget != 0)
				{
					PerformOrganizationAssignmentAction(model.SelectedActions, model.SelectedUsers, result);
				}

				if (model.SelectedActions.TimeTrackerRoleTarget != 0)
				{
					PerformTimeTrackerAssignmentAction(model.SelectedActions, model.SelectedUsers, result);
				}

				// Constructing notification
				// TODO: This largely mirros the way the Permission.cshtml page used to construct the notifications, but
				//  without quite as much detail. We could either a) restore that detail to notifications here, or b) remove
				//  some of the now uneccessary structure in all the PermissionResultetc objects.
				//  Also, the logic behind what to write and what variety to use may need to be reexamined.
				int totalResults = result.Results.Count();
				Variety variety = null;
				if (totalResults == result.Results.Where(x => x.ActionStatus == "success").Count())
				{
					result.Status = "success"; // full Success case
					variety = Variety.Success;
				}
				else if (totalResults == result.Results.Where(x => x.ActionStatus == "failure").Count())
				{
					result.Status = "failure"; // full failure case
					variety = Variety.Danger;
				}
				else if (totalResults == result.Results.Where(x => x.ActionStatus == "error").Count())
				{
					result.Status = "error"; // full error case
					variety = Variety.Warning;
				}
				else
				{
					result.Status = "partial"; // partial failure case
					variety = Variety.Info;
				}

				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(string.IsNullOrEmpty(result.Result) ? string.Empty : string.Format("{0}\n\r", result.Result));
				foreach (var res in result.Results)
				{
					sb.AppendLine(string.Format("{0}/{1}: {2}", res.AffectedUserCount, res.TotalUserCount, res.ActionText));
				}

				Notifications.Add(new BootstrapAlert(sb.ToString(), variety));
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
			}

			if (model.isPermissions2) // TODO: Delete once there's only one manage permissions page (also delete the action constant)
			{
				return RedirectToAction(ActionConstants.ManagePermissions2);
			}
			return RedirectToAction(ActionConstants.ManagePermissions);
		}

		/// <summary>
		/// Attempts to perform the OrganizationAssignment action on the list of users.
		/// </summary>
		/// <param name="selectedAction">The action to perform.</param>
		/// <param name="users">The list of users.</param>
		/// <param name="result">The instance of PermissionsActionResultViewModel to store result data.</param>
		private void PerformOrganizationAssignmentAction(PermissionsAction selectedAction, IEnumerable<TargetUser> users, PermissionsActionsResults result)
		{
			selectedAction.OrganizationMembers = selectedAction.OrganizationMembers ?? new HashSet<int>(Service.GetOrganizationMemberList(selectedAction.OrganizationId).Select(x => x.UserId));
			bool containsCurrentUser = users.Select(x => x.UserId).Contains(Convert.ToInt32(UserContext.UserId));
			if (users == null || users.Count() == 0)
			{
				result.Status = "error";
				result.Message = AllyisApps.Resources.Controllers.Auth.Strings.NoUsersHaveBeenDefined;
			}
			else if (!selectedAction.OrgRoleTarget.HasValue)
			{
				result.Results.Add(new PermissionsActionResult()
				{
					ActionStatus = "error",
					ActionText = AllyisApps.Resources.Controllers.Auth.Strings.YouDidNotDefineATargetRole
				});
			}
			else if (containsCurrentUser && selectedAction.OrgRoleTarget == -1)
			{
				result.Results.Add(new PermissionsActionResult()
				{
					ActionStatus = "error",
					ActionText = AllyisApps.Resources.Controllers.Auth.Strings.YouAreUnableToRemoveYourself
				});
			}
			else if (containsCurrentUser && selectedAction.OrgRoleTarget > 0)
			{
				result.Results.Add(new PermissionsActionResult()
				{
					ActionStatus = "error",
					ActionText = AllyisApps.Resources.Controllers.Auth.Strings.YouAreUnableToChangeYourOwnRole
				});
			}
			else if (selectedAction.OrgRoleTarget.Value != 0)
			{
				PermissionsActionResult successResult = new PermissionsActionResult
				{
					ActionStatus = "success",
					ActionText = AllyisApps.Resources.Controllers.Auth.Strings.UsersChangedRolesInOrg,
					TotalUserCount = users.Count()
				};
				PermissionsActionResult failureResult = new PermissionsActionResult
				{
					TotalUserCount = users.Count()
				};
				bool successOccurred = false;
				bool failureOccurred = false;

				foreach (TargetUser user in users)
				{
					try
					{
						// if (DB Helper.GetPermissionLevel(this.OrganizationId, user.UserId).Id == (int)Role.Owner)
						// {
						//     throw new InvalidOperationException("You cannot change the roles of an organization owner.");
						// }
						if (selectedAction.OrgRoleTarget.Value == -1)
						{
							Service.RemoveOrganizationUser(selectedAction.OrganizationId, user.UserId);
							if (selectedAction.TimeTrackerSubscriptionId != 0)
							{
								Service.DeleteSubscriptionUser(selectedAction.TimeTrackerSubscriptionId, user.UserId);
							}
						}
						else if (!selectedAction.OrganizationMembers.Contains(user.UserId))
						{
							throw new InvalidOperationException("No longer a member of the organization.<span class='filter-reload'>Click <a href='javascript: history.go(0)'>here</a> to reload the member list.</span>");
						}
						else
						{
							Service.UpdateOrganizationUser(user.UserId, selectedAction.OrganizationId, selectedAction.OrgRoleTarget.Value);
						}

						successResult.AffectedUserCount += 1;
						successResult.Users.Add(user);
						successOccurred = true;
					}
					catch (Exception e)
					{
						failureResult.AffectedUserCount += 1;
						failureResult.Users.Add(user);
						failureResult.ActionText = e.Message;
						failureResult.ActionStatus = "failure";
						failureOccurred = true;
					}
				}

				if (successOccurred)
				{
					result.Results.Add(successResult);
				}

				if (failureOccurred)
				{
					result.Results.Add(failureResult);
				}
			}
		}

		/// <summary>
		/// Attempts to perform the TimeTrackerAssignment action on the list of users.
		/// </summary>
		/// <param name="selectedAction">The action to perform.</param>
		/// <param name="users">The list of users.</param>
		/// <param name="result">The instance of PermissionsActionResultViewModel to store result data.</param>
		private void PerformTimeTrackerAssignmentAction(PermissionsAction selectedAction, IEnumerable<TargetUser> users, PermissionsActionsResults result)
		{
			selectedAction.OrganizationMembers = selectedAction.OrganizationMembers ?? new HashSet<int>(Service.GetOrganizationMemberList(selectedAction.OrganizationId).Select(x => x.UserId));
			SubscriptionInfo subDetails = null;
			if (selectedAction.TimeTrackerSubscriptionId != 0)
			{
				subDetails = Service.GetSubscription(selectedAction.TimeTrackerSubscriptionId);
			}

			if (subDetails == null)
			{
				result.Status = "error";
				result.Message = AllyisApps.Resources.Controllers.Auth.Strings.YouDontHaveASubscriptionToTimeTracker;
				selectedAction.TimeTrackerSubscriptionId = 0;
			}

			IEnumerable<UserInfo> currentUsers = Service.GetUsersWithSubscriptionToProductInOrganization(selectedAction.OrganizationId, Service.GetProductIdByName(ProductNameKeyConstants.TimeTracker));
			IEnumerable<int> userIds = currentUsers.Select(user => user.UserId);

			int alteringUsers = users.Where(x => !userIds.Contains(x.UserId)).Count();

			if (users == null || users.Count() == 0)
			{
				result.Status = "error";
				result.Message = AllyisApps.Resources.Controllers.Auth.Strings.NoUsersHaveBeenDefined;
			}
			else if (!selectedAction.TimeTrackerRoleTarget.HasValue)
			{
				result.Results.Add(new PermissionsActionResult
				{
					ActionStatus = "error",
					ActionText = AllyisApps.Resources.Controllers.Auth.Strings.YouDidNotDefineATargetRole,
					TotalUserCount = users.Count(),
					AffectedUserCount = users.Count()
				});
			}
			else if (subDetails.NumberOfUsers < userIds.Count() + alteringUsers && selectedAction.TimeTrackerRoleTarget > 0)
			{
				result.Results.Add(new PermissionsActionResult
				{
					ActionStatus = "error",
					ActionText = string.Format("You are going over your TimeTracker subscription limit (currently {0}/{1}). Please remove some users.", userIds.Count(), subDetails.NumberOfUsers),
					TotalUserCount = users.Count(),
					AffectedUserCount = users.Count()
				});
			}
			else if (selectedAction.TimeTrackerRoleTarget.Value != 0)
			{
				PermissionsActionResult successResult = new PermissionsActionResult
				{
					ActionStatus = "success",
					ActionText = AllyisApps.Resources.Controllers.Auth.Strings.UsersChangedRolesInTimeTracker,
					TotalUserCount = users.Count()
				};
				PermissionsActionResult failureResult = new PermissionsActionResult
				{
					TotalUserCount = users.Count()
				};
				bool successOccurred = false;
				bool failureOccurred = false;

				foreach (TargetUser user in users)
				{
					try
					{
						// if (DB Helper.GetPermissionLevel(this.OrganizationId, user.UserId).Id == (int)Role.Owner)
						// {
						//     throw new InvalidOperationException("You cannot change the roles of an organization owner.");
						// }
						if (selectedAction.TimeTrackerRoleTarget.Value == -1)
						{
							Service.DeleteSubscriptionUser(selectedAction.TimeTrackerSubscriptionId, user.UserId);
						}
						else if (!selectedAction.OrganizationMembers.Contains(user.UserId))
						{
							throw new InvalidOperationException("No longer a member of the organization.<span class='filter-reload'>Click <a href='javascript: history.go(0)'>here</a> to reload the member list.</span>");
						}
						else
						{
							Service.UpdateSubscriptionUserProductRole(selectedAction.TimeTrackerRoleTarget.Value, selectedAction.TimeTrackerSubscriptionId, user.UserId);
						}

						successResult.AffectedUserCount += 1;
						successResult.Users.Add(user);
						successOccurred = true;
					}
					catch (Exception e)
					{
						failureResult.AffectedUserCount += 1;
						failureResult.Users.Add(user);
						failureResult.ActionText = e.Message;
						failureResult.ActionStatus = "failure";
						failureOccurred = true;
					}
				}

				if (successOccurred)
				{
					result.Results.Add(successResult);
				}

				if (failureOccurred)
				{
					result.Results.Add(failureResult);
				}
			}
		}
	}
}
