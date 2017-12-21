using System;
using System.Data;
using System.Linq;
using AllyisApps.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;

namespace UploadDataDirect
{
	internal class UserUploader
	{
		private DataTable hoursData;
		private AppService appService;
		private int organizaionID;
		

		public UserUploader(DataTable hoursData, AppService appService, int orgId)
		{
			this.hoursData = hoursData;
			this.appService = appService;
			this.organizaionID = orgId;
			
		}

		internal async Task uploadUsers()
		{
			string emailFormat = "Test{0}@testing.com";
			var project = appService.GetDefaultProject(organizaionID);
			bool hasEmployeeId = hoursData.Columns.Contains(ColumnConstants.Employee);
			bool hasUserName = hoursData.Columns.Contains(ColumnConstants.FirstName) && hoursData.Columns.Contains(ColumnConstants.LastName);
			//TODO: GET EMAIL ADDRESSES OF USERS
			//bool hasemailAddess==
			//FOR NOW DEFALUT PERHAPS ALLOW CHANGE OF EMAIL ADDRESS
			HashSet<string> createdEmployyeeIds = new HashSet<string>();
			List<int> createdUsers = new List<int>();

			if(!hasEmployeeId || !hasUserName)
			{
				throw new UploadExcepiton("Insuffiecnt Data: missing employyeeID and User Name");
			}
			var beforeorgUsers = await appService.GetOrganizationUsersAsync(organizaionID);
			int i = 1;
			
			foreach (DataRow row in hoursData.Rows)
			{
				
				string employeeId = row[ColumnConstants.Employee].ToString();
				string firstname = row[ColumnConstants.FirstName].ToString();
				string lastName = row[ColumnConstants.LastName].ToString();
				string testEmail = String.Format(emailFormat, i);
				if(!createdEmployyeeIds.Contains(employeeId) && !beforeorgUsers.Exists(user => user.EmployeeId.Equals(employeeId)) )
				{
					var user = await appService.AddUserToOrganizaion(testEmail, firstname, lastName, organizaionID, 
						AllyisApps.Services.Auth.OrganizationRoleEnum.Member, employeeId,null);
					createdEmployyeeIds.Add(employeeId);
					Console.WriteLine("Added user " + firstname + " " + lastName);
					
					//Users added this way may have time entries and should be member
					if(!beforeorgUsers.Exists(us => us.EmployeeId.Equals(employeeId)))
					{
						createdUsers.Add(user.UserId);
					}
					await appService.UpdateProjectUser(project.Id, user.UserId, true);
				}
				
				i++;
			}
			await appService.UpdateSubscriptionUsersRoles(createdUsers, organizaionID, (int)TimeTrackerRole.User, (int)ProductIdEnum.TimeTracker);
			
		}
	}
}