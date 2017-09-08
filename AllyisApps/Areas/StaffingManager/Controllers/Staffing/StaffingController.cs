//------------------------------------------------------------------------------
// <copyright file="StaffingController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;

namespace AllyisApps.Areas.StaffingManager.Controllers.Staffing
{
	/// <summary>
	/// Staffing controller.
	/// </summary>
	[Authorize]
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StaffingController" /> class.
		/// </summary>
		public StaffingController()
		{
		}
	}
}
