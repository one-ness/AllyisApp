using System;
using System.Data;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Auth;
using System.Linq;
using AllyisApps.Services.Project;
using System.Collections.Generic;
using AllyisApps.Services.TimeTracker;
using System.Globalization;
using System.Threading.Tasks;

namespace UploadDataDirect
{
	internal class TimeEntryUpload
	{
		private DataTable hoursData;
		private AllyisApps.Services.AppService appService;
		private int orgId;
		private int subId;
		private Customer customerId;

		public TimeEntryUpload(DataTable hoursData, AllyisApps.Services.AppService appService, int orgId, int subId, Customer customerId)
		{
			this.hoursData = hoursData;
			this.appService = appService;
			this.orgId = orgId;
			this.subId = subId;
			this.customerId = customerId;
		}

		internal async Task uploadTimeEntries()
		{
			var orgUsers = await appService.GetOrganizationUsersAsync(orgId);
			var payclasses = await appService.GetPayClassesByOrganizationId(orgId);
			
			var gotPayClasses = new Dictionary<string, int>();
			var gotProjects = new Dictionary<string, Project>();
			IEnumerable<Project> projects = await appService.GetAllProjectsForOrganizationAsync(orgId);

			foreach(var proj in projects)
			{
				gotProjects.Add(proj.ProjectCode, proj);
			}

			foreach(var pc in payclasses)
			{
				gotPayClasses.Add(pc.PayClassName.ToUpper(), pc.PayClassId);
			}

			foreach (DataRow row in hoursData.Rows)
			{
				try
				{
					string employeid = row[ColumnConstants.Employee].ToString();
					OrganizationUser user = orgUsers.FirstOrDefault(u => u.EmployeeId.Equals(employeid));
					if (user == null)
					{
						//User is not found
						continue;
					}
					string projectCode = row[ColumnConstants.ProjectCode].ToString();

					
					if(!gotProjects.TryGetValue(projectCode, out Project project)){
						//Project not found 
						continue;
					}
					var payClassName = row[ColumnConstants.PayClassCode].ToString();

					if (payClassName.Equals("REGSAL"))
					{
						//hard code regsal to regular as per instuctions 
						payClassName = "REGULAR";
					}

			
					if (!gotPayClasses.TryGetValue(payClassName.ToUpper(),out int payclassId))
					{
						payclassId = await appService.CreatePayClass(payClassName, orgId, subId);
						gotPayClasses.Add(payClassName.ToUpper(), payclassId);
					}
					

					string dateStr = row[ColumnConstants.Date].ToString();
					DateTime Date = DateTime.Parse(dateStr);


					string dur = row[ColumnConstants.Duration].ToString();

					float? duration = appService.ParseDuration(dur);
					if (duration == null)
					{
						//Failed to parse duration
						continue;
					}


					//Ensure user is assigned to project 
					appService.CreateProjectUser(project.ProjectId, user.UserId);

					TimeEntry timeentry = new TimeEntry()
					{
						Date = Date,
						EmployeeId = user.EmployeeId,
						UserId = user.UserId,
						ProjectId = project.ProjectId,
						Duration = duration.Value,
						PayClassId = payclassId,
						TimeEntryStatusId = (int) TimeEntryStatus.Pending
					};
					//Create Time Entry

					await appService.CreateTimeEntry(timeentry);
					Console.WriteLine("Uplaoded TimeEntyr for date  " + timeentry.Date + ". For person " + user.FirstName + " " + user.LastName);

				}
				catch(Exception e)
				{
					Console.WriteLine("Failed to upload Time Entry due to " + e.Message);
				}
			}
		}
	}
}