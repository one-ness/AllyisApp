using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;

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
			var model = "";

			await Task.Delay(1);

			return View(model);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> CreateEmployeeType(int subscriptionId)
		{
			await Task.Delay(1);

			return RedirectToAction(ActionConstants.SettingsEmployeeType);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> DeleteEmployeeType(int subscriptionId)
		{
			await Task.Delay(1);

			return RedirectToAction(ActionConstants.SettingsEmployeeType);
		}
	}
}