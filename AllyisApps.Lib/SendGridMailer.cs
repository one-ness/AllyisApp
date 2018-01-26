//------------------------------------------------------------------------------
// <copyright file="Mailer.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SendGrid;
using System.Text;
using SendGrid.Helpers.Mail;

namespace AllyisApps.Lib
{
	/// <summary>
	/// sends email using SendGrid api
	/// </summary>
	public class SendGridMailer
	{
		SendGridAPIClient sender;

		/// <summary>
		/// constructor
		/// </summary>
		public SendGridMailer(string apiKey)
		{
			if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));
			this.sender = new SendGridAPIClient(apiKey);
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
			Mail mail = new Mail(new Email(from), subject, new Email(to), new Content("text/html", bodyHtml));
			dynamic response = await this.sender.client.mail.send.post(requestBody: mail.Get());
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
		public async Task<dynamic> SendEmailAsync(string from, List<string> to, string subject, string bodyHtml)
		{
			if (string.IsNullOrWhiteSpace(from)) throw new ArgumentNullException(nameof(from));
			if (to == null || to.Count <= 0) throw new ArgumentNullException(nameof(to));
			if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));
			if (string.IsNullOrWhiteSpace(bodyHtml)) throw new ArgumentNullException(nameof(bodyHtml));

			// construct the multiple recipient addresses
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < to.Count - 1; i++)
			{
				sb.Append("{ 'email': '");
				sb.Append(to[i]);
				sb.Append("' }, ");
			}

			// add the last recipient address
			sb.Append("{ 'email': '");
			sb.Append(to[to.Count - 1]);
			sb.Append("' }, ");

			string data = "{{'personalizations': [ {{ 'to': [ {0} ], 'subject': '{1}' }} ], 'from': {{ 'email': '{2}' }}, 'content': [ {{ 'type': 'text/html', 'value': '{3}' }} ] }}";
			data = string.Format(data, sb.ToString(), subject, from, bodyHtml);
			object jsonData = JsonConvert.DeserializeObject<object>(data);
			return await this.sender.client.mail.send.post(requestBody: jsonData.ToString());
		}
	}
}