//-----------------------------------------------------------------------------
// <copyright file="BaseProductController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Filters;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Core
{
	/// <summary>
	/// Common controller base class for products.
	/// </summary>
	[NotificationFilter]
	public abstract class BaseProductController : BaseController
	{
		private readonly int cProductId;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseProductController" /> class.
		/// </summary>
		/// <param name="productId">The id of the current product.</param>
		protected BaseProductController(int productId) : base()
		{
			this.cProductId = productId;
		}

		///// <summary>
		///// Gets or sets the Time Tracker service.
		///// </summary>
		//protected TimeTrackerService TimeTrackerService { get; set; }

		/// <summary>
		/// Override method for updating chosen subscription id for user context.
		/// </summary>
		/// <param name="filterContext">The ActionExecutingContext.</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			int? subId = null;
			SkuInfo subscription = this.Service.GetSubscriptionByOrgAndProduct((int)this.cProductId);
			if (subscription != null)
			{
				subId = subscription.SubscriptionId;
			}

			// Null subscription means this organization does not have this product, so no subscription exists and subId should stay 0
			// We want to update subId even if it is 0 to prevent inappropriately accessing subscriptions across companies
			// (e.g. prevent the following: login to company with timetracker, then swap companies to one without timetracker, then login to timetracker from the wrong company)
			if (UserContext.ChosenSubscriptionId != subId)
			{
				if (subId.HasValue)
				{
					this.UserContext.ChosenSubscriptionId = subId.Value;
				}
				else
				{
					this.UserContext.ChosenSubscriptionId = 0;
				}

				this.Service.UpdateActiveSubscription(subId); // Active subscription column in the database won't allow a subId of 0, but it will allow NULL
			}

			// Init product services with latest user info
			this.TimeTrackerService = new TimeTrackerService(GlobalSettings.SqlConnectionString, this.UserContext);
            this.TimeTrackerService.SetService(this.Service);
		}
	}
}