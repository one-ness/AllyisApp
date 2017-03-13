﻿//------------------------------------------------------------------------------
// <copyright file="GetProjectsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// Gets a list of all projects for a given customer and returns it.
		/// </summary>
		/// <param name="customerID">The customer ID.</param>
		/// <returns>A JsonResult of an IEnumberable of ProjectInfo's containing projects for the customer.</returns>
		public ActionResult GetProjectsOLD(int customerID)
		{
			return Json(Service.GetProjectsByCustomer(customerID));
		}
	}
}
