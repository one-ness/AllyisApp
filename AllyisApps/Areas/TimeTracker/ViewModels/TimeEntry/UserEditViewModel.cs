﻿//------------------------------------------------------------------------------
// <copyright file="UserEditViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the UserEdit view.
	/// </summary>
	public class UserEditViewModel
	{
		/// <summary>
		/// Gets or sets the id of the user.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the subscription's Id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the subscription's Name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets all projects available to the user.
		/// </summary>
		public IEnumerable<Services.Project.Project> AllProjects { get; set; }

		/// <summary>
		/// Gets or sets the projects associated with the user.
		/// </summary>
		public IEnumerable<Services.Project.Project> UserProjects { get; set; }
	}
}