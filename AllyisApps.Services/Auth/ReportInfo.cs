//------------------------------------------------------------------------------
// <copyright file="InvitationInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents a report.
	/// </summary>
	public class ReportInfo
	{
		public List<Customer> Customers { get; set; }
		public List<CompleteProjectInfo> CompleteProjectInfo { get; set; }
		public List<SubscriptionUserInfo> SubscriptionUserInfo { get; set; }

		public ReportInfo(List<Customer> customers, List<CompleteProjectInfo> completeProjectInfo, List<SubscriptionUserInfo> subscriptionUserInfo)
		{
			this.Customers = customers;
			this.CompleteProjectInfo = completeProjectInfo;
			this.SubscriptionUserInfo = subscriptionUserInfo;
		}
	}
}