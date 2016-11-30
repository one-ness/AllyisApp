﻿//------------------------------------------------------------------------------
// <copyright file="Mailer.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using Newtonsoft.Json;

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
		/// Send html message to one recipient.
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

        /// <summary>
        /// Send html message to multiple recipients.
        /// </summary>
        /// <param name="from">Who the message is from.</param>
        /// <param name="to">The message recipients.</param>
        /// <param name="subject">The message subject line.</param>
        /// <param name="bodyHtml">The html body of the email.</param>
        /// <returns>The async mailing task.</returns>
        public static async Task SendEmailAsync(string from, List<string> to, string subject, string bodyHtml)
        {
            string toString = "";
            foreach(string recipient in to)
            {
                toString += "{{ 'email': '" + recipient + "' }}, ";
            }
            toString = toString.Substring(0, toString.Length - 2); // Chop off last comma and space

            string data = "{{'personalizations': [ {{ 'to': [ {0} ], 'subject': {1} }} ], 'from': {{ 'email': {2} }}, 'content': [ {{ 'type': 'text/html', 'value': {3} }} ] }}";
            object jsonData = JsonConvert.DeserializeObject<object>(String.Format(data, toString, subject, from, bodyHtml));
            await sender.client.mail.send.post(requestBody: jsonData.ToString());
        }
    }
}