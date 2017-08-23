//------------------------------------------------------------------------------
// <copyright file="NotificationFilter.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using System;
using System.Web.Mvc;

namespace AllyisApps.Filters
{
	/// <summary>
	/// Filter for adding notifications to all controller actions.
	/// </summary>
	[AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true,
	AllowMultiple = false)]
	public class NotificationFilterAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Copies the notifications to the ViewBag.
		/// </summary>
		/// <param name="filterContext">The context where the filter executed.</param>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			filterContext.Controller.ViewBag.Alerts = ((BaseController)filterContext.Controller).Notifications;
		}
	}
}
