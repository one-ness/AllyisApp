using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using Newtonsoft.Json;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Contains actions for new TimeTracker Index Design
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		JsonResult jsonResult = new JsonResult();
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> TimeEntryCURD(string data)
		{

			var items = JsonConvert.DeserializeObject<TimeEntryCRUDModel>(data);

			//var list = ConvertToEditTimeEntry(items);


			foreach (EditTimeEntryViewModel entry in items.Entries.OrderBy(item => item.Date))
			{

				JsonResult res = null;
				if (entry.TimeEntryId.HasValue)
				{
					if (entry.IsDeleted)
					{
						res = (JsonResult)await DeleteTimeEntryJson(entry);
					}
					else if (entry.IsEdited)
					{
						res = (JsonResult)await EditTimeEntryJson(entry);
					}
				}
				else if (entry.IsCreated && !entry.IsDeleted)
				{
					res = (JsonResult)await CreateTimeEntryJson(entry);
				}
			}

			return RedirectToRoute(
				RouteNameConstants.TimeEntryIndexUserTimeSheet,
				new
				{
					controller = ControllerConstants.TimeEntry,
					action = ActionConstants.Index,
					subscriptionId = items.SubscriptionId,
					startDate = items.StartingDate,
					endDate = items.EndingDate
				});
		}
	}

	/// <summary>
	/// Time Entry Model for Action Triggred above
	/// Creted by Index2.js with setup form allyis-timeentry-index2.js
	/// </summary>
	public class TimeEntryCRUDModel
	{

		/// <summary>
		/// Entries for edit time view model
		/// </summary>
		public List<EditTimeEntryViewModel> Entries;
		/// <summary>
		/// Start Date 
		/// </summary>
		public long StartingDate;
		/// <summary>
		/// End Date 
		/// </summary>
		public long EndingDate;

		/// <summary>
		/// Subscription ID 
		/// </summary>
		public int SubscriptionId;
	}
}