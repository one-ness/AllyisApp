//------------------------------------------------------------------------------
// <copyright file="ErrorHandler.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Utilities
{
	// [AttributeUsage(AttributeTargets.All)]

	/// <summary>
	/// Error handler.
	/// </summary>
	public class ErrorHandler : HandleErrorAttribute
	{
		/// <summary>
		/// On exception.
		/// </summary>
		/// <param name="filterContext">Filter Context.</param>
		public override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
		}
	}
}
