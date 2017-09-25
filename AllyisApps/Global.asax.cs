//------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AllyisApps
{
	/// <summary>
	/// Represents the entire web application.
	/// </summary>
	public class MvcApplication : System.Web.HttpApplication
	{
		/// <summary>
		/// Application start event handler. all app initializations should be done here.
		/// </summary>
		/// <param name="sender">The "sender".</param>
		/// <param name="e">The EventArgs.</param>
		protected void Application_Start(object sender, EventArgs e)
		{
			// register
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// intialize
			AppStartup.Init();
		}

		/// <summary>
		/// Application error event handler.
		/// </summary>
		/// <param name="sender">The "sender".</param>
		/// <param name="e">The EventArgs.</param>
		protected void Application_Error(object sender, EventArgs e)
		{
			Exception objErr = Server.GetLastError().GetBaseException();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("Error Caught in Application_Error event");
			sb.AppendLine(string.Format("Error in: {0}", Request.Url.ToString()));
			sb.AppendLine(string.Format("Error Message: {0}", objErr.Message.ToString()));
			sb.Append(string.Format("Stack Trace:{0}", objErr.StackTrace.ToString()));

			EventLog.WriteEntry("AllyisApps", sb.ToString(), EventLogEntryType.Error);
		}
	}
}