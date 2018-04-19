using AllyisApps.Services;
using System.Threading.Tasks;
using ExcelDataReader;
using System.IO;
using System.Data;
using System;
using AllyisApps.Services.Crm;
using System.Collections.Generic;

namespace UploadDataDirect
{
	internal class DataUploader
	{
		private string file;
		private AppService appService;
		private int orgId;
		private int subId;
		private bool isNew;
		public DataUploader(string file, AppService appService, int orgId, int subId, bool isNew)
		{
			this.file = file;
			this.appService = appService;
			this.orgId = orgId;
			this.subId = subId;
			this.isNew = isNew;
		}

		public async Task UploadData()
		{
			using (FileStream fileS = File.OpenRead(file))
			{
				var reader = ExcelReaderFactory.CreateOpenXmlReader(fileS);
				//reader.IsFirstRowAsColumnNames = true;
				//DataSet data = reader.AsDataSet();
				DataSet data = new DataSet();
				reader.Close();

				DataTable hoursData = null;
				DataTable Projects = null;
				foreach (DataTable table in data.Tables)
				{
					switch (table.TableName)
					{
						case "ADP Hours Data":
							hoursData = table;
							break;
						case "Project name with code":
							Projects = table;
							break;
						default:
							break;
					}
				}
				await Import(hoursData, Projects);
			}
		}

		private async Task Import(DataTable hoursData, DataTable projects)
		{
			//Import Users
			UserUploader userUploader = new UserUploader(hoursData, appService,orgId);
			await userUploader.uploadUsers();
			//Import Customer

			CustomerUpload customerUpload = new CustomerUpload(appService, orgId, subId, projects);

			var customers = await customerUpload.UploadCustomers();
			//Import Projects
			ProjectUpload projectUploader = new ProjectUpload(projects, appService ,orgId,subId, customers);
			await projectUploader.projectUpload();

			//Import TimeEntry

			TimeEntryUpload timeEntryUploader = new TimeEntryUpload(hoursData, appService, orgId, subId, isNew);
			await timeEntryUploader.uploadTimeEntries();
		}
	}
}