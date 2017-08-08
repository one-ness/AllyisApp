//------------------------------------------------------------------------------
// <copyright file="GlobalSettings.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Configuration;
using System;
using AllyisApps.Lib;

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
		/// support email.
		/// </summary>
		public static string SupportEmail { get; set; }

		/// <summary>
		/// sendgrid api key.
		/// </summary>
		public static string SendGridApiKey { get; set; }

		/// <summary>
		/// Initialize Global Settings.
		/// </summary>
		public static void Init(string connectionStringKey = "DefaultConnection", string supportEmailKey = "SupportEmail", string sendGridApiKey = "SendGridApiKey")
		{
			SqlConnectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
			if (string.IsNullOrWhiteSpace(SqlConnectionString))
			{
				throw new ArgumentException("connection string not found");
			}

			string email = ConfigurationManager.AppSettings[supportEmailKey];
			if (!string.IsNullOrWhiteSpace(email))
			{
				SupportEmail = email.Trim();
			}
			else
			{
				SupportEmail = "support@allyisapps.com";
			}

			string apiKey = ConfigurationManager.AppSettings[sendGridApiKey];
			if (!string.IsNullOrWhiteSpace(sendGridApiKey))
			{
				SendGridApiKey = apiKey.Trim();
			}
		}
	}
}
