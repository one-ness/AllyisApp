//------------------------------------------------------------------------------
// <copyright file="GlobalSettings.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Configuration;

namespace AllyisApps.Core
{
	/// <summary>
	/// Global / application-wide settings.
	/// </summary>
	public static class GlobalSettings
	{
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
			SqlConnectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
		}
	}
}
