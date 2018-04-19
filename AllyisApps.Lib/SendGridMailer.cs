//------------------------------------------------------------------------------
// <copyright file="Mailer.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllyisApps.Lib
{
	/// <summary>
	/// sends email using SendGrid api
	/// </summary>
	public class SendGridMailer
	{
		SendGridClient sender;

		/// <summary>
		/// constructor
		/// </summary>
		public SendGridMailer(string apiKey)
		{
			if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));
			this.sender = new SendGridClient(apiKey);
		}

		/// <summary>
		/// Sends html message to one recipient.
		/// </summary>
		/// <param name="from">Who the message is from.</param>
		/// <param name="to">The message recipient.</param>
		/// <param name="subject">The message subject line.</param>
		/// <param name="bodyHtml">The html body of the email.</param>
		/// <returns>The async mailing task.</returns>
		public async Task<bool> SendEmailAsync(string from, string to, string subject, string bodyHtml)
		{
			if (string.IsNullOrWhiteSpace(from)) throw new ArgumentNullException(nameof(from));
			if (string.IsNullOrWhiteSpace(to)) throw new ArgumentNullException(nameof(to));
			if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));
			if (string.IsNullOrWhiteSpace(bodyHtml)) throw new ArgumentNullException(nameof(bodyHtml));

			bool result = false;
			var msg = new SendGridMessage();
			msg.AddTo(to);
			msg.From = new EmailAddress(from);
			msg.Subject = subject;
			msg.HtmlContent = bodyHtml;
			var response = await this.sender.SendEmailAsync(msg);
			if (response != null && string.Compare(response.StatusCode.ToString(), "accepted", true) == 0)
			{
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Sends html message to multiple recipients.
		/// </summary>
		/// <param name="from">Who the message is from.</param>
		/// <param name="to">The message recipients.</param>
		/// <param name="subject">The message subject line.</param>
		/// <param name="bodyHtml">The html body of the email.</param>
		/// <returns>The async mailing task.</returns>
		public async Task<bool> SendEmailAsync(string from, List<string> to, string subject, string bodyHtml)
		{
			if (string.IsNullOrWhiteSpace(from)) throw new ArgumentNullException(nameof(from));
			if (to == null || to.Count <= 0) throw new ArgumentNullException(nameof(to));
			if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));
			if (string.IsNullOrWhiteSpace(bodyHtml)) throw new ArgumentNullException(nameof(bodyHtml));

			bool result = false;
			var msg = new SendGridMessage();
			var tolist = new List<EmailAddress>();
			foreach (var item in to)
			{
				tolist.Add(new EmailAddress(item));
			}

			msg.AddTos(tolist);
			msg.From = new EmailAddress(from);
			msg.Subject = subject;
			msg.HtmlContent = bodyHtml;
			var response = await this.sender.SendEmailAsync(msg);
			if (response != null && string.Compare(response.StatusCode.ToString(), "accepted", true) == 0)
			{
				result = true;
			}

			return result;
		}
	}
}