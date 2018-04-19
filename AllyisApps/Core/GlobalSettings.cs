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
		/// Azure Active Directory Application Id to be used for msft office 365 / azure ad login
		/// </summary>
		public static Guid AadAppId { get; set; }

		/// <summary>
		/// Tenant name as registered in our Azure subscription. If we use the name "common"
		/// it will allow all office 365 / azure ad users to login to our app
		/// </summary>
		public static string AadTenantName { get; set; }

		/// <summary>
		/// Authority is the URL for authority, composed by Azure Active Directory v2 endpoint and the tenant name
		/// </summary>
		public static string MsftOidcAuthority { get; set; }

		const string SupportEmailDefault = "support@allyisapps.com";
		const string connectionStringKey = "DefaultConnection";
		const string supportEmailKey = "SupportEmail";
		const string sendGridApiKeyKey = "SendGridApiKey";
		const string requireEmailConfirmationKey = "RequireEmailConfirmation";
		const string aadAppIdKey = "AadAppId";
		const string aadTenantNameKey = "AadTenantName";

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

			temp = ConfigurationManager.AppSettings[aadAppIdKey];
			if (!string.IsNullOrWhiteSpace(temp))
			{
				temp = temp.Trim();
				AadAppId = new Guid(temp);
			}

			temp = ConfigurationManager.AppSettings[aadTenantNameKey];
			if (!string.IsNullOrWhiteSpace(temp))
			{
				AadTenantName = temp.Trim();
				MsftOidcAuthority = string.Format("https://login.microsoftonline.com/{0}/v2.0", AadTenantName);
			}
		}
	}
}