using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// Toggles projects status between active and deactive.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="projIds">A comma seperated value of user ids for toggling.</param>
		/// <returns>Redirects to the index action.</returns>
		public async Task<ActionResult> ToggleStatus(int subscriptionId, string projIds)
		{
			string[] idStrings = projIds.Split(',');
			var ids = idStrings.Select(x => Convert.ToInt32(x)).ToList();
			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var customers = await AppService.GetCustomerList(orgId);
			var projectTest = (await AppService.GetProjectsByOrganization(orgId, false));
			var projects = projectTest.Where(x => ids.Contains(x.ProjectId)).ToList();
			string result = "";

			foreach (var project in projects)
			{
				if ((project.EndDate == null || project.EndDate.Value >= DateTime.UtcNow) && (project.StartDate == null || project.StartDate <= DateTime.UtcNow))
				{
					try
					{
						result = await AppService.DeactivateProject(project.ProjectId, subscriptionId);

						if (!string.IsNullOrEmpty(result))
						{
							Notifications.Add(new BootstrapAlert(string.Format("{0} Status was toggled sucessfully.", project.ProjectName), Variety.Success));
						}
					}
					catch (Exception e)
					{
						Notifications.Add(new BootstrapAlert(e.Message, Variety.Danger));
					}
				}
				else
				{
					var parentCustomer = customers.Where(x => x.CustomerId == project.owningCustomer.CustomerId).FirstOrDefault();
					if (parentCustomer.IsActive.Value)
					{
						var success = AppService.ReactivateProject(project.ProjectId, orgId, subscriptionId);

						if (!success)
						{
							Notifications.Add(new BootstrapAlert(Resources.Strings.DeleteUnauthorizedMessage, Variety.Warning));
							return RedirectToAction(ActionConstants.Index, ControllerConstants.Project, new { subscriptionId = subscriptionId });
						}
					}
					else
					{
						result = AppService.ReactivateCustomer(parentCustomer.CustomerId, subscriptionId, orgId);

						if (!string.IsNullOrEmpty(result))
						{
							var success = AppService.ReactivateProject(project.ProjectId, orgId, subscriptionId);

							if (!success)
							{
								Notifications.Add(new BootstrapAlert(Resources.Strings.DeleteUnauthorizedMessage, Variety.Warning));
								return RedirectToAction(ActionConstants.Index, ControllerConstants.Project, new { subscriptionId = subscriptionId });
							}
						}
					}
				}
			}

			return RedirectToAction(ActionConstants.Index, ControllerConstants.Project, new { subscriptionId = subscriptionId });
		}
	}
}