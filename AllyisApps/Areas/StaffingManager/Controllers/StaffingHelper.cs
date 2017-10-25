//------------------------------------------------------------------------------
// <copyright file="StaffingHelper.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Staffing controller.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Sets the information for the nav viewdata.
		/// </summary>
		/// <param name="subscriptionId"></param>
		public void SetNavData(int subscriptionId)
		{
			ViewData["subscriptionId"] = subscriptionId;
			ViewData["subscriptionName"] = AppService.GetSubscriptionName(subscriptionId);
		}
	}
}