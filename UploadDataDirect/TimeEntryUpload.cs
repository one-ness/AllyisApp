using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Project;
using AllyisApps.Services.TimeTracker;

namespace UploadDataDirect
{
	internal class TimeEntryUpload
	{
		private DataTable hoursData;
		private AllyisApps.Services.AppService appService;
		private int orgId;
		private int subId;
		private Customer customerId;
		private bool isNew;

		public TimeEntryUpload(DataTable hoursData, AllyisApps.Services.AppService appService, int orgId, int subId, Customer customerId, bool isNew = true)
		{
			this.hoursData = hoursData;
			this.appService = appService;
			this.orgId = orgId;
			this.subId = subId;
			this.customerId = customerId;
			this.isNew = isNew;
		}

		internal async Task uploadTimeEntries()
		{
			var orgUsers = await appService.GetOrganizationUsersAsync(orgId);

			var orgPayclasses = await appService.GetPayClassesByOrganizationId(orgId);

			var gotPayClasses = new Dictionary<string, int>();
			var gotProjects = new Dictionary<string, Project>();

			var gotEmployeePayClasses = new Dictionary<int, List<int>>();
			IEnumerable<Project> projects = await appService.GetAllProjectsForOrganizationAsync(orgId);

			foreach (var proj in projects)
			{
				gotProjects.Add(proj.ProjectCode, proj);
			}

			foreach (var pc in orgPayclasses)
			{
				gotPayClasses.Add(pc.PayClassName.ToUpper(), pc.PayClassId);
			}

			var gotTimeEntryTotalDuration = new Dictionary<Tuple<DateTime, int>, float>();

			foreach (DataRow row in hoursData.Rows)
			{
				try
				{
					string employeid = row[ColumnConstants.Employee].ToString();
					OrganizationUser user = orgUsers.FirstOrDefault(u => u.EmployeeId.Equals(employeid));
					if (user == null)
					{
						//User is not found
						throw new Exception("No user exists for employeeId " + employeid);
					}
					string projectCode = row[ColumnConstants.ProjectCode].ToString();

					if (!gotProjects.TryGetValue(projectCode, out Project project))
					{
						throw new Exception("No project exists for projectCode" + projectCode);
					}
					var payClassName = row[ColumnConstants.PayClassCode].ToString();

					if (payClassName.Equals("REGSAL"))
					{
						//hard code regsal to regular as per instuctions
						payClassName = "REGULAR";
					}

					int inputedPayClassId = await InitializePayClasses(gotPayClasses, gotEmployeePayClasses, user, payClassName, subId);

					string dateStr = row[ColumnConstants.Date].ToString();
					DateTime timeEntryDate = DateTime.Parse(dateStr);

					var timeentryIdentify = new Tuple<DateTime, int>(timeEntryDate, user.UserId);
					if (!gotTimeEntryTotalDuration.TryGetValue(timeentryIdentify, out float timeEntryTime))
					{
						//if we know that no results will come then skip this query
						if (isNew)
						{
							timeEntryTime = 0.0F;
						}
						else
						{
							var userList = new List<int>();
							userList.Add(user.UserId);
							var timeEntries = (await appService.GetTimeEntriesByUsersOverDateRange(userList, timeEntryDate, timeEntryDate, orgId)).ToList();
							timeEntryTime = 0.0F;
							timeEntries.Select(te => timeEntryTime += te.Duration);
							gotTimeEntryTotalDuration.Add(new Tuple<DateTime, int>(timeEntryDate, user.UserId), timeEntryTime);
						}
					}

					string dur = row[ColumnConstants.Duration].ToString();

					float? duration = appService.ParseDuration(dur);
					if (duration == null)
					{
						//Failed to parse duration
						continue;
					}

					if (timeEntryTime + duration >= 24.0F)
					{
						throw new Exception("Cannot add more then 24 hours to a day");
					}
					//Ensure user is assigned to project
					appService.CreateProjectUser(project.ProjectId, user.UserId);

					TimeEntry timeentry = new TimeEntry()
					{
						Date = timeEntryDate,
						EmployeeId = user.EmployeeId,
						UserId = user.UserId,
						ProjectId = project.ProjectId,
						Duration = duration.Value,
						PayClassId = inputedPayClassId,
						TimeEntryStatusId = (int)TimeEntryStatus.Pending
					};
					//Create Time Entry

					await appService.CreateTimeEntry(timeentry);
					Console.WriteLine("Uplaoded TimeEntyr for date  " + timeentry.Date + ". For person " + user.FirstName + " " + user.LastName);
					timeEntryTime += timeentry.Duration;
				}
				catch (Exception e)
				{
					Console.WriteLine("Failed to upload Time Entry due to " + e.Message);
				}
			}
		}

		private async Task<int> InitializePayClasses(Dictionary<string, int> gotPayClasses, Dictionary<int, List<int>> gotEmployeePayClasses, OrganizationUser user, string payClassName, int subId)
		{
			if (!gotPayClasses.TryGetValue(payClassName.ToUpper(), out int inputedPayClassId))
			{
				inputedPayClassId = await appService.CreatePayClass(payClassName, orgId, subId);
				gotPayClasses.Add(payClassName.ToUpper(), inputedPayClassId);
			}

			if (!gotEmployeePayClasses.TryGetValue(user.EmployeeTypeId, out List<int> possiblepayClassIds))
			{
				possiblepayClassIds = await appService.GetAssignedPayClasses(user.EmployeeTypeId);
				gotEmployeePayClasses.Add(user.EmployeeTypeId, possiblepayClassIds);
			}

			//If pay class about ot be imported is not assinged to user then assign it to user
			if (!possiblepayClassIds.Exists(pcId => pcId == inputedPayClassId))
			{
				await appService.AddPayClassToEmployeeType(subId, user.EmployeeTypeId, inputedPayClassId);
				gotEmployeePayClasses[user.EmployeeTypeId].Add(inputedPayClassId);
			}

			return inputedPayClassId;
		}
	}
}