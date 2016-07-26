//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
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
	public partial class ProjectController : BaseProductController
	{
		/// <summary>
		/// GET: Project/Index.
		/// </summary>
		/// <returns>The index view.</returns>
		public ActionResult Index()
		{
			// This action is no longer necessary, but projects are everywhere. I think all of the things that
			// used to redirect here now instead redirect to the proper Customer/Index action, but just in case we
			// missed something, we'll throw this error for now. Eventually this action will be gone completely.
			throw new System.Exception("GET OUT OF MY ROOM, MOM!"); 
		}
	}
}
