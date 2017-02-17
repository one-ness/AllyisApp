//------------------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	[Authorize]
	public partial class CustomerController: BaseController
	{
		private static readonly int TimeTrackerID = Service.GetProductIdByName(ProductNameKeyConstants.TimeTracker);

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomerController"/> class.
		/// </summary>
		public CustomerController() : base(TimeTrackerID)
		{
		}
	}
}