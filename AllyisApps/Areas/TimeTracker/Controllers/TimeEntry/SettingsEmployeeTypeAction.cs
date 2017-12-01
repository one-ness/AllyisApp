using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Returns the settings holiday view.
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> SettingsEmployeeType(int subscriptionId)
		{
			var subName = await AppService.GetSubscriptionName(subscriptionId);
			var employeeTypes = await AppService.GetEmployeeTypeByOrganization(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId);
			SettingsEmployeeTypeViewModel model = new SettingsEmployeeTypeViewModel()
			{
				EmployeeTypes = employeeTypes.ToList(),
				SubscriptionId = subscriptionId,
				SubscriptionName = subName,
				UserId = AppService.UserContext.UserId
			};

			return View(model);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> CreateEmployeeType(int subscriptionId)
		{
			var payclasses = (await AppService.GetPayClassesBySubscriptionId(subscriptionId)).Select(x => new PayClassInfo() { PayClassId = x.PayClassId, PayClassName = x.PayClassName });

			SettingsEditEmployeeTypeViewModel model = new SettingsEditEmployeeTypeViewModel()
			{
				EmployeeTypeName = "",
				PayClasses = payclasses.ToList(),
				CurrentPayClasses = new List<PayClassInfo>() { },
				IsEdit = false
			};

			return View("SettingsEditEmployeeType", model);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <param name="employeeTypeId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> DeleteEmployeeType(int subscriptionId, int employeeTypeId)
		{
			var assignedPayClasses = await AppService.GetAssignedPayClasses(employeeTypeId);
			var employeeTypes = await AppService.GetEmployeeTypeByOrganization(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId);

			if (assignedPayClasses.Count > 0)
			{
				Notifications.Add(new BootstrapAlert("Cannot Delete Employee Type. There are assigned pay class.", Variety.Warning));
				return RedirectToAction(ActionConstants.EditSettingsEmployeeType, new { employeeTypeId = employeeTypeId });
			}
			if (employeeTypes.Count == 1)
			{
				Notifications.Add(new BootstrapAlert("Cannot Delete Employee Type. You cannot delete the last employee type.", Variety.Warning));
				return RedirectToAction(ActionConstants.EditSettingsEmployeeType, new { employeeTypeId = employeeTypeId });
			}

			try
			{
				await AppService.DeleteEmployeeType(employeeTypeId);
			}
			catch
			{
				Notifications.Add(new BootstrapAlert("Cannot Delete Employee Type. Users are still assigned to the employee type.", Variety.Warning));
			}

			return RedirectToAction(ActionConstants.SettingsEmployeeType);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <param name="employeetypeId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> EditEmployeeType(int subscriptionId, int employeetypeId = 0)
		{
			var employeeType = await AppService.GetEmployeeType(employeetypeId);
			var assignedPayClassesIds = await AppService.GetAssignedPayClasses(employeetypeId);
			var payClasses = (await AppService.GetPayClassesByOrganizationId(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId));

			var unassignedPayClasses = payClasses.Where(x => !assignedPayClassesIds.Contains(x.PayClassId)).Select(x => new PayClassInfo() { PayClassId = x.PayClassId, PayClassName = x.PayClassName });
			var assignedPayClasses = payClasses.Where(x => assignedPayClassesIds.Contains(x.PayClassId)).Select(x => new PayClassInfo() { PayClassId = x.PayClassId, PayClassName = x.PayClassName });
			SettingsEditEmployeeTypeViewModel model = new SettingsEditEmployeeTypeViewModel()
			{
				EmployeeTypeId = employeetypeId,
				EmployeeTypeName = employeeType.EmployeeTypeName,
				PayClasses = unassignedPayClasses.ToList(),
				CurrentPayClasses = assignedPayClasses.ToList(),
				IsEdit = true
			};

			return View("SettingsEditEmployeeType", model);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> EditEmployeeType(int subscriptionId, SettingsEditEmployeeTypeViewModel model)
		{
			if (string.IsNullOrEmpty(model.EmployeeTypeName))
			{
				Notifications.Add(new BootstrapAlert("Employee Type Requires a name."));
				return RedirectToAction(ActionConstants.EditSettingsEmployeeType);
			}
			if (model.EmployeeTypeId > 0)
			{
				var selected = model.SelectedPayClass != null ? model.SelectedPayClass.ToList() : new List<int>();
				var assignedPayClass = (await AppService.GetAssignedPayClasses(model.EmployeeTypeId));

				List<int> payClassesToRemove = new List<int>();
				foreach (var payClass in assignedPayClass)
				{
					if (selected.Contains(payClass)) // Don't want to add already assigned payclasses.
					{
						selected.Remove(payClass);
					}
					else if (!selected.Contains(payClass))
					{
						payClassesToRemove.Add(payClass);
					}
				}

				foreach (var payClassId in selected)
				{
					await AppService.AddPayClassToEmployeeType(model.EmployeeTypeId, payClassId);
				}

				foreach (var payClassId in payClassesToRemove)
				{
					await AppService.RemovePayClassFromEmployeeType(model.EmployeeTypeId, payClassId);
				}
			}
			else //Creating a new Employee Type
			{
				var selected = model.SelectedPayClass != null ? model.SelectedPayClass.ToList() : new List<int>();
				var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

				int newEmployeeTypeId = await AppService.CreateEmployeeType(orgId, model.EmployeeTypeName);
				foreach (var payClassId in selected)
				{
					await AppService.AddPayClassToEmployeeType(newEmployeeTypeId, payClassId);
				}
			}

			return RedirectToAction(ActionConstants.SettingsEmployeeType);
		}
	}
}