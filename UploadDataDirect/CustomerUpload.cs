using System;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
namespace UploadDataDirect
{
	internal class CustomerUpload
	{
		private AppService appService;
		private int orgId;
		private int subId;
		private DataTable customers;

		public CustomerUpload(AppService appService, int orgId, int subId, DataTable projectInfomation)
		{
			this.appService = appService;
			this.orgId = orgId;
			this.subId = subId;
			customers = projectInfomation;
		}

		internal async Task<Customer> uploadDefaltCustomer()
		{
			var org = await appService.GetOrganization(orgId);
		
			Customer customer = new Customer()
			{
				CustomerCode = org.OrganizationName+"_Upload",
				OrganizationId = orgId,
				IsActive = true,
				CustomerName = org.OrganizationName +"_Upload",
			};
			int custId = await appService.CreateCustomerAsync(customer, subId);
			return appService.GetCustomer(custId);
		}

		internal async Task<Dictionary<string, Customer>> UploadCustomers()
		{
			var startCustomerCode = "import_{0}";
			var curCustoemr = 1;
			var rows = customers.Rows;
			var createdCustomers = new Dictionary<string, Customer>();
			
			foreach(DataRow row in rows)
			{
				var custName = row[2]?.ToString();
				if (string.IsNullOrEmpty(custName))
				{
					continue;
				}

				if(!createdCustomers.TryGetValue(custName,out Customer v)){
					Customer customer = new Customer()
					{
						CustomerCode = String.Format(startCustomerCode, curCustoemr),
						OrganizationId = orgId,
						IsActive = true,
						CustomerName = custName,
						
					};
					curCustoemr++;
					customer.CustomerId = await appService.CreateCustomerAsync(customer, subId);
					createdCustomers.Add(custName, customer);
					Console.WriteLine($"Customer created {customer.CustomerName} "  );
				}
			}
			return createdCustomers;
		}
	}
}