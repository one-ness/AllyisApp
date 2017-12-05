using System;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using System.Threading.Tasks;

namespace UploadDataDirect
{
	internal class CustomerUpload
	{
		private AppService appService;
		private int orgId;
		private int subId;

		public CustomerUpload(AppService appService, int orgId, int subId)
		{
			this.appService = appService;
			this.orgId = orgId;
			this.subId = subId;
		}

		internal async Task<Customer> uploadDefaltCustomer()
		{
			var org = await appService.GetOrganization(orgId);
			
			Customer customer = new Customer()
			{
				CustomerCode = org.OrganizationName+"_Defalt",
				OrganizationId = orgId,
				IsActive = true,
				CustomerName = org.OrganizationName,
			};
			int custId = await appService.CreateCustomerAsync(customer, subId);
			return appService.GetCustomer(custId);
		}
	}
}