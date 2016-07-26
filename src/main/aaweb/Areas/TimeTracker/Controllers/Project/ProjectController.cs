//------------------------------------------------------------------------------
// <copyright file="ProjectController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	[Authorize]
	public partial class ProjectController : BaseProductController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectController"/> class.
		/// </summary>
		public ProjectController() : base(Services.Crm.CrmService.GetProductIdByName("TimeTracker"))
		{
		}
	}
}
