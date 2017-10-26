using System;
using AllyisApps.Lib;

namespace AllyisApps.Services
{
	/// <summary>
	/// service settings
	/// </summary>
	public class ServiceSettings
	{
		/// <summary>
		/// Gets or sets the sql connection string.
		/// </summary>
		public string SqlConnectionString { get; set; }

		/// <summary>
		/// support email
		/// </summary>
		public string SupportEmail { get; set; }

		/// <summary>
		/// sendgrid api key
		/// </summary>
		public string SendGridApiKey { get; set; }

		public ServiceSettings(string sqlConnectionString, string supportEmail, string sendGridApiKey)
		{
			if (string.IsNullOrWhiteSpace(sqlConnectionString)) throw new ArgumentNullException(nameof(sqlConnectionString));
			SqlConnectionString = sqlConnectionString;
			SupportEmail = supportEmail;
			SendGridApiKey = sendGridApiKey;
			Mailer.Init(SendGridApiKey);
		}
	}
}