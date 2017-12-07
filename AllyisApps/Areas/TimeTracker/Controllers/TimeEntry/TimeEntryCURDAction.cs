using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Lib;
using AllyisApps.Resources;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using Newtonsoft.Json;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Contains actions for new TimeTracker Index Design
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Receives a json list of time entries to either create, update, or delete for each entry.
		/// </summary>
		/// <returns>Redirect to time entry page</returns>
		[HttpPost]
		public async Task<ActionResult> TimeEntryCURD(string data)
		{
			var items = JsonConvert.DeserializeObject<TimeEntryCRUDModel>(data);

			foreach (EditTimeEntryViewModel model in items.Entries.OrderBy(item => item.Date))
			{
				if (!model.IsCreated && !model.IsDeleted && !model.IsEdited) continue;

				// DateTime dateGet = model.Date != 0 ? modelDate : DateTime.Now; // do we need this??
				DateTime modelDate = Utility.GetDateTimeFromDays(model.Date);
				float modelDuration = ParseDuration(model.Duration) ?? throw new ArgumentException(Strings.DurationFormat);

				var entry = new TimeEntry
				{
					TimeEntryId = model.TimeEntryId ?? -1, //-1 for if time entry is being created
					UserId = model.UserId,
					ProjectId = model.ProjectId,
					PayClassId = model.PayClassId,
					Date = modelDate,
					Duration = modelDuration,
					Description = model.Description
				};

				var result = CreateUpdateTimeEntryResult.InvalidAction;
				if (model.TimeEntryId.HasValue)
				{
					if (model.IsDeleted)
					{
						result = await DeleteTimeEntry(model.SubscriptionId, entry);
					}
					else if (model.IsEdited)
					{
						result = await EditTimeEntry(model.SubscriptionId, entry);
					}
				}
				else if (model.IsCreated && !model.IsDeleted)
				{
					result = await CreateTimeEntry(model.SubscriptionId, entry);
				}

				if (result != CreateUpdateTimeEntryResult.Success)
				{
					Notifications.Add(new BootstrapAlert(result.GetResultMessage(entry.Date), Variety.Danger));
				}
			}

			//Recalcuate overtime after all changes have been made
			if (items.Entries?[0]?.UserId != null)
			{
				int organizationId = AppService.UserContext.SubscriptionsAndRoles[items.SubscriptionId].OrganizationId;
				var editedEntryDates = items.Entries.Where(e => e.IsCreated || e.IsDeleted || e.IsEdited).Select(e => e.Date).OrderBy(d => d).ToList();
				var settings = await AppService.GetSettingsByOrganizationId(organizationId);
				DateTime startDate = Utility.GetDateTimeFromDays(editedEntryDates.First());
				DateTime endDate = Utility.GetDateTimeFromDays(editedEntryDates.Last());

				await AppService.RecalculateOvertimeOverDateRange(organizationId, new DateRange(startDate, endDate), items.Entries[0].UserId);
			}

			return RedirectToRoute(
				RouteNameConstants.TimeEntryIndexUserTimeSheet,
				new
				{
					controller = ControllerConstants.TimeEntry,
					action = ActionConstants.Index,
					subscriptionId = items.SubscriptionId,
					startDate = items.StartingDate,
					endDate = items.EndingDate,
					userId = items.Entries?[0]?.UserId
				});
		}

		/// <summary>
		/// Parses the input duration for either HH.HH or HH:MM format.
		/// </summary>
		/// <param name="duration">Duration in either format.</param>
		/// <returns>Parsed duration or null.</returns>
		public float? ParseDuration(string duration)
		{
			if (string.IsNullOrWhiteSpace(duration)) return null;

			float? durationOut = null;
			Match theMatch;
			if ((theMatch = Regex.Match(duration, HourMinutePattern)).Success)
			{
				float minutes = int.Parse(theMatch.Groups[2].Value) / MinutesInHour;
				durationOut = float.Parse(theMatch.Groups[1].Value) + minutes;
			}
			else if (Regex.Match(duration, DecimalPattern).Success)
			{
				durationOut = float.Parse(duration);
			}

			return durationOut;
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
		public int StartingDate;
		/// <summary>
		/// End Date 
		/// </summary>
		public int EndingDate;

		/// <summary>
		/// Subscription ID 
		/// </summary>
		public int SubscriptionId;
	}
}