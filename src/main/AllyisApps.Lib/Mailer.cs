//------------------------------------------------------------------------------
// <copyright file="Mailer.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace AllyisApps.Lib
{
	/// <summary>
	/// Email utilties.
	/// </summary>
	public static class Mailer
	{
        static SendGridAPIClient sender
        {
            get
            {
                if (senderBack == null)
                {
                    senderBack = new SendGridAPIClient(Environment.GetEnvironmentVariable("SENDGRID_APIKEY"));
                }
                return senderBack;
            }
        }
        private static SendGridAPIClient senderBack;

		/// <summary>
		/// Send email.
		/// </summary>
		/// <param name="from">Who the message is from.</param>
		/// <param name="to">The message recipient.</param>
		/// <param name="subject">The message subject line.</param>
		/// <param name="bodyHtml">The html body of the email.</param>
		/// <returns>The async mailing task.</returns>
		public static async Task SendEmailAsync(string from, string to, string subject, string bodyHtml)
		{
            Mail mail = new Mail(
                new Email(from),
                subject,
                new Email(to),
                new Content("text/html", bodyHtml)
                );
            await sender.client.mail.send.post(requestBody: mail.Get());
		}
	}
}