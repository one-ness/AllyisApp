//------------------------------------------------------------------------------
// <copyright file="TimeEntryController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	[Authorize]
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TimeEntryController" /> class.
		/// </summary>
		public TimeEntryController()
		{
		}
	}
}
