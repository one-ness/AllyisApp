//------------------------------------------------------------------------------
// <copyright file="EmailService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Net.Mail;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace AllyisApps.Services.Utilities
{
	/// <summary>
	/// Class which represents an e-mailer (SMTP service).
	/// </summary>
	public class EmailService : IIdentityMessageService
	{
		/// <summary>
		/// Gets the SMTP server for the application.
		/// </summary>
		public static string NoReplyEmail
		{
			get
			{
				return System.Web.Configuration.WebConfigurationManager.AppSettings["NoReplyEmail"];
			}
		}

		/// <summary>
		/// Sends an e-mail asynchronously with the specified contents.
		/// </summary>
		/// <param name="body">Body of message.</param>
		/// <param name="destination">Email address to send to.</param>
		/// <param name="subject">Subject of email.</param>
		/// <returns>Task that is returned.</returns>
		public async Task CreateMessage(string body, string destination, string subject)
		{
			IdentityMessage output = new IdentityMessage();
			output.Body = body;
			output.Destination = destination;
			output.Subject = subject;
			await this.SendAsync(output);
		}

		/// <summary>
		/// Sends a message object over email.
		/// </summary>
		/// <param name="message">Message object to send.</param>
		/// <returns>Async Task.</returns>
		public async Task SendAsync(IdentityMessage message)
		{
			if (message == null)
			{
				await Task.FromResult(0);
			}

			// Configure the client:
			SmtpClient client = new System.Net.Mail.SmtpClient();
			MailMessage mail = new MailMessage(NoReplyEmail, message.Destination, message.Subject, message.Body);

			mail.IsBodyHtml = true;
			client.EnableSsl = true;
			client.Send(mail);
		}
	}
}