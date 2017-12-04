using System;
using AllyisApps.Services;
using System.Threading.Tasks;

namespace UploadDataDirect
{
	internal class OrganizaionUploader
	{
		private AppService appService;

		public OrganizaionUploader(AppService appService)
		{
			this.appService = appService;
		}

		internal async Task<int> CreateOrganizaion()
		{
			return await appService.SetupOrganization("1_OWNER", "AllyisAppsInstall", null, null, null, null, null, null, null, null, null);
		}
	}
}