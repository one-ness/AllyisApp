using System.Data;
using System;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UploadDataDirect
{
	internal class ProjectUpload
	{
		private AppService appService;
		DataTable projects;
		private int orgId;
		private int subId;
		private Dictionary<string,Customer> customers;

		public ProjectUpload(DataTable projects, AppService appService, int orgId, int subId, Dictionary<string,Customer> customers)
		{
			this.projects = projects;
			this.appService = appService;
			this.orgId = orgId;
			this.subId = subId;
			this.customers = customers;
		}

		internal async Task projectUpload()
		{
			foreach(DataRow row in projects.Rows)
			{
				string projectCode = row[0].ToString();
				string projectName = row[1].ToString();
				string customerName = row[2].ToString();
				if(!customers.TryGetValue(customerName, out Customer customer))
				{
					Console.WriteLine($"Failed to create  project {projectCode} {projectName} missing customer {customerName}");
					continue;
				}
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
				Console.WriteLine($"Created project {projectCode} {projectName}");
			}

		}
	}
}