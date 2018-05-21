using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Hrm;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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

			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["SubscriptionId"] = subscriptionId;
			var subName = await AppService.GetSubscriptionName(subscriptionId);
			var employeeTypes = await AppService.GetEmployeeTypeByOrganization(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId);

			SettingsEmployeeTypeViewModel model = new SettingsEmployeeTypeViewModel()
			{
				EmployeeTypes = employeeTypes.ToList().Select(t => new EmployeeTypeViewModel()
				{
					EmployeeTypeId = t.EmployeeTypeId,
					EmployeeTypeName = t.EmployeeTypeName,
					OrganizationId = t.OrganizationId
				}).ToList(),
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
			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["SubscriptionId"] = subscriptionId;
			var payclasses = (await AppService.GetPayClassesBySubscriptionId(subscriptionId))
				.Where(x => (x.BuiltInPayClassId != BuiltinPayClassEnum.Overtime))
				.Select(x => new PayClassInfoViewModel(x));

			SettingsEditEmployeeTypeViewModel model = new SettingsEditEmployeeTypeViewModel()
			{
				EmployeeTypeName = "",
				PayClasses = payclasses.ToList(),
				CurrentPayClasses = new List<PayClassInfoViewModel>() { },
				IsEdit = false
			};
			ViewBag.SubscriptionName = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
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

			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["SubscriptionId"] = subscriptionId;
			if (!employeeTypes.Exists(x => x.EmployeeTypeId == employeeTypeId))
			{
				Notifications.Add(new BootstrapAlert("You don't have permission to delete this Employee Type.", Variety.Warning));
				return RedirectToAction(ActionConstants.SettingsEmployeeType);
			}
			//This logic no longer applies
			/*
			if (assignedPayClasses.Count > 0)
			{
				Notifications.Add(new BootstrapAlert("Cannot Delete Employee Type. There are assigned pay class.", Variety.Warning));
				return RedirectToAction(ActionConstants.EditSettingsEmployeeType, new { employeeTypeId = employeeTypeId });
			}
			*/

			if (employeeTypes.Count == 1)
			{
				Notifications.Add(new BootstrapAlert("Cannot Delete Employee Type. You cannot delete the last employee type.", Variety.Warning));
				return RedirectToAction(ActionConstants.EditSettingsEmployeeType, new { employeeTypeId = employeeTypeId });
			}

			try
			{
				await AppService.DeleteEmployeeType(subscriptionId, employeeTypeId);
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
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> EditEmployeeType(int subscriptionId, int userId = 0)
		{
			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["SubscriptionId"] = subscriptionId;

			var employeeTypes = await AppService.GetEmployeeTypeByOrganization(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId);

			if (!employeeTypes.Exists(x => x.EmployeeTypeId == userId))
			{
				Notifications.Add(new BootstrapAlert("You don't have permission to edit this Employee Type.", Variety.Warning));
				return RedirectToAction(ActionConstants.SettingsEmployeeType);
			}

			var employeeType = employeeTypes.Where(x => x.EmployeeTypeId == userId).FirstOrDefault();
			var assignedPayClassesIds = await AppService.GetAssignedPayClasses(userId);
			var payClasses = (await AppService.GetPayClassesByOrganizationId(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId));

			var unassignedPayClasses = payClasses
				.Where(x => !assignedPayClassesIds.Contains(x.PayClassId) && (x.BuiltInPayClassId != BuiltinPayClassEnum.Overtime))
				.Select(x => new PayClassInfoViewModel(x));
			var assignedPayClasses = payClasses
				.Where(x => assignedPayClassesIds.Contains(x.PayClassId) && (x.BuiltInPayClassId != BuiltinPayClassEnum.Overtime))
				.Select(x => new PayClassInfoViewModel(x));

			SettingsEditEmployeeTypeViewModel model = new SettingsEditEmployeeTypeViewModel()
			{
				EmployeeTypeId = userId,
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
			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["SubscriptionId"] = subscriptionId;

			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var payclasses = await AppService.GetPayClassesByOrganizationId(orgId);
			var overtimepayclass = payclasses.First(pc => (pc.BuiltInPayClassId == BuiltinPayClassEnum.Overtime));
			if (string.IsNullOrEmpty(model.EmployeeTypeName))
			{
				Notifications.Add(new BootstrapAlert("Employee Type Requires a name."));
				return RedirectToAction(ActionConstants.EditSettingsEmployeeType);
			}
			if (model.EmployeeTypeId > 0)
			{
				var selected = model.SelectedPayClass != null ? model.SelectedPayClass.ToList() : new List<int>();
				var assignedPayClass = (await AppService.GetAssignedPayClasses(model.EmployeeTypeId));
				if (!(selected.Contains(overtimepayclass.PayClassId)))
				{
					selected.Add(overtimepayclass.PayClassId);
				}
				List<int> payClassesToRemove = new List<int>();
				foreach (var payClass in assignedPayClass)
				{
						
					if (selected.Contains(payClass)) // Don't want to add already assigned payclasses.
					{
						selected.Remove(payClass);
					}
					else if (!selected.Contains(payClass) && payClass != overtimepayclass.PayClassId)
					{
						payClassesToRemove.Add(payClass);
					}
					
				}
				
				

				foreach (var payClassId in selected)
				{
					await AppService.AddPayClassToEmployeeType(subscriptionId, model.EmployeeTypeId, payClassId);
				}

				foreach (var payClassId in payClassesToRemove)
				{
					await AppService.RemovePayClassFromEmployeeType(subscriptionId, model.EmployeeTypeId, payClassId);
				}
			}
			else //Creating a new Employee Type
			{
				var selected = model.SelectedPayClass != null ? model.SelectedPayClass.ToList() : new List<int>();
				

				int newEmployeeTypeId = await AppService.CreateEmployeeType(orgId, model.EmployeeTypeName, overtimepayclass.PayClassId);
				foreach (var payClassId in selected)
				{
					await AppService.AddPayClassToEmployeeType(subscriptionId, newEmployeeTypeId, payClassId);
				}
				
			}

			return RedirectToAction(ActionConstants.SettingsEmployeeType);
		}
	}
}