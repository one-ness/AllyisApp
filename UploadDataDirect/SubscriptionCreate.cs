using System;
using AllyisApps.Services;
using System.Threading.Tasks;

namespace UploadDataDirect
{
	internal class SubscriptionCreate
	{
		private AppService appService;
		private int organizaionId;
		public SubscriptionCreate(AppService appService, int organizaionId)
		{
			this.appService = appService;
			this.organizaionId = organizaionId;
		}

		internal async Task<int> CreateTimeTrackerSubscription()
		{
			return await appService.Subscribe(organizaionId, AllyisApps.Services.Billing.SkuIdEnum.TimeTrackerBasic, "Time Tracker");
		}
	}
}