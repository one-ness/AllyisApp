using System.Data;
using System;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using System.Threading.Tasks;

namespace UploadDataDirect
{
	internal class ProjectUpload
	{
		private AppService appService;
		DataTable projects;
		private int orgId;
		private int subId;
		private Customer customer;

		public ProjectUpload(DataTable projects, AppService appService, int orgId, int subId, Customer customer)
		{
			this.projects = projects;
			this.appService = appService;
			this.orgId = orgId;
			this.subId = subId;
			this.customer = customer;
		}

		internal async Task projectUpload()
		{
			foreach(DataRow row in projects.Rows)
			{
				string projectCode = row[0].ToString();
				string projectName = row[1].ToString();
				if (string.IsNullOrEmpty(projectCode))
				{
					//reahed end of table?
					continue;
				}
				AllyisApps.Services.Project.Project project = new AllyisApps.Services.Project.Project()
				{
					ProjectName = projectName,
					IsHourly = true,
					ProjectCode = projectCode,
					OrganizationId = orgId,
					OwningCustomer = customer
				};
				await appService.CreateProject(project);
				Console.WriteLine("Created project " + projectCode + " " + projectName);
			}

		}
	}
}