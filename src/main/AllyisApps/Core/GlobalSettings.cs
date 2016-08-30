//------------------------------------------------------------------------------
// <copyright file="GlobalSettings.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Configuration;
using AllyisApps.Utilities;

namespace AllyisApps.Core
{
	/// <summary>
	/// Global / application-wide settings.
	/// </summary>
	public static class GlobalSettings
	{
		/// <summary>
		/// Gets or sets the host name of the application.
		/// </summary>
		public static string HostName { get; set; }

		/// <summary>
		/// Gets or sets the web root of the application.
		/// </summary>
		public static string WebRoot { get; set; }

		/// <summary>
		/// Gets or sets the domain the cookie is set for.
		/// </summary>
		public static string CookieDomain { get; set; }

		/// <summary>
		/// Gets or sets the sql connection string.
		/// </summary>
		public static string SqlConnectionString { get; set; }

		/// <summary>
		/// Initialize Global Settings.
		/// </summary>
		/// <param name="connectionStringKey">The connection string key.</param>
		public static void Init(string connectionStringKey = "DefaultConnection")
		{
			HostName = Helpers.ReadAppSetting("hostname");
			WebRoot = Helpers.ReadAppSetting("webroot");
			CookieDomain = WebRoot;

			SqlConnectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
		}
	}
}