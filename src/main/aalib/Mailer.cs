//------------------------------------------------------------------------------
// <copyright file="Mailer.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Lib
{
	/// <summary>
	/// Email utilties.
	/// </summary>
	public static class Mailer
	{
		/// <summary>
		/// Gets or sets the Server name.
		/// </summary>
		private static string ServerName { get; set; }

		/// <summary>
		/// Gets or sets the Port.
		/// </summary>
		private static int Port { get; set; }

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		private static string Username { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		private static string Password { get; set; }

		/// <summary>
		/// Init the mailer.
		/// </summary>
		/// <param name="serverName">Mail server name.</param>
		/// <param name="port">Mailing port.</param>
		/// <param name="username">Mailing username.</param>
		/// <param name="password">Mailing password.</param>
		public static void Init(string serverName, int port, string username, string password)
		{
			ServerName = serverName;
			Port       = port;
			Username   = username;
			Password   = password;
		}

		/// <summary>
		/// Send email.
		/// </summary>
		/// <param name="from">Who the message is from.</param>
		/// <param name="recipients">The message recipeient.</param>
		/// <param name="subject">The message subject line.</param>
		/// <param name="bodyHtml">The html body of the email.</param>
		/// <returns>The async mailing task.</returns>
		public static async Task SendEmailAsync(string from, string recipients, string subject, string bodyHtml)
		{
			await Task.Run(() =>
		   {
			   // TODO: implement using SMTP library
			   return;
		   });
		}
	}
}
