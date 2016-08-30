//------------------------------------------------------------------------------
// <copyright file="TimeEntryController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	[Authorize]
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TimeEntryController" /> class.
		/// </summary>
		public TimeEntryController() : base(Services.Crm.CrmService.GetProductIdByName(ProductNameKeyConstants.TimeTracker))
		{
		}
	}
}