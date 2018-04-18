//------------------------------------------------------------------------------
// <copyright file="GlobalSettings.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
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
		/// Gets or sets support email.
		/// </summary>
		public static string SupportEmail { get; set; }

		/// <summary>
		/// Gets or sets sendgrid api key.
		/// </summary>
		public static string SendGridApiKey { get; set; }

		/// <summary>
		/// gets or sets whether user has to confirm his/her email
		/// </summary>
		public static bool RequireEmailConfirmation { get; set; }

		/// <summary>
		/// Azure Active Directory Client Id to be used for msft office 365 / azure ad login
		/// </summary>
		public static Guid AadClientId { get; set; }

		const string SupportEmailDefault = "support@allyisapps.com";
		const string connectionStringKey = "DefaultConnection";
		const string supportEmailKey = "SupportEmail";
		const string sendGridApiKeyKey = "SendGridApiKey";
		const string requireEmailConfirmationKey = "RequireEmailConfirmation";
		const string aadClientIdKey = "AadClientId";

		/// <summary>
		/// Initialize Global Settings.
		/// </summary>
		public static void Init()
		{
			SqlConnectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
			if (string.IsNullOrWhiteSpace(SqlConnectionString))
			{
				throw new ArgumentException("connection string not found");
			}

			string temp = ConfigurationManager.AppSettings[supportEmailKey];
			if (!string.IsNullOrWhiteSpace(temp))
			{
				SupportEmail = temp.Trim();
			}
			else
			{
				SupportEmail = SupportEmailDefault;
			}

			temp = ConfigurationManager.AppSettings[sendGridApiKeyKey];
			if (!string.IsNullOrWhiteSpace(temp))
			{
				SendGridApiKey = temp.Trim();
			}

			temp = ConfigurationManager.AppSettings[requireEmailConfirmationKey];
			if (!string.IsNullOrWhiteSpace(temp))
			{
				temp = temp.Trim();
				if (string.Compare(temp, "true", true) == 0)
				{
					RequireEmailConfirmation = true;
				}
			}

			temp = ConfigurationManager.AppSettings[aadClientIdKey];
			if (!string.IsNullOrWhiteSpace(temp))
			{
				temp = temp.Trim();
				AadClientId = new Guid(temp);
			}
		}
	}
}