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
			IEnumerable<Project> projects = await appService.GetAllProjectsForOrganizationAsync(orgId);
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

					var project = projects.FirstOrDefault(p => p.ProjectCode.Equals(projectCode));
					if (project == null)
					{
						//Project Not found
						continue;
					}

					var payClassName = row[ColumnConstants.PayClassCode].ToString();
					if (payClassName.Equals("REGSAL"))
					{
						//hard code regsal to regular as per instuctions 
						payClassName = "REGULAR";
					}
					var Payclass = payclasses.FirstOrDefault(pc => pc.PayClassName.Equals(payClassName, StringComparison.CurrentCultureIgnoreCase));
					int payclassId = 0;
					if (Payclass == null)
					{
						payclassId = await appService.CreatePayClass(payClassName, orgId, subId);
					}
					else
					{
						payclassId = Payclass.PayClassId;
					}

					string dateStr = row[ColumnConstants.Date].ToString();
					DateTime Date = DateTime.ParseExact(dateStr, "M/dd/yyyy", CultureInfo.InvariantCulture);


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
						PayClassId = payclassId
					};
					//Create Time Entry

					await appService.CreateTimeEntry(timeentry);

				}
				catch(Exception e)
				{
					Console.WriteLine("Failed to upload Time Entry due to " + e.Message);
				}
			}
		}
	}
}