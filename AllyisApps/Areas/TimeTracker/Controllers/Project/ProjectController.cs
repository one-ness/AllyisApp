//------------------------------------------------------------------------------
// <copyright file="ProjectController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	[Authorize]
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectController"/> class.
		/// </summary>
		public ProjectController() : base(ProductIdEnum.TimeTracker)
		{
		}
	}
}
