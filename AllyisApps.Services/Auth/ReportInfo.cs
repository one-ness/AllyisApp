﻿//------------------------------------------------------------------------------
// <copyright file="InvitationInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents a report.
	/// </summary>
	public class ReportInfo
	{
		public List<Customer> Customers { get; set; }
		public List<CompleteProject> CompleteProjectInfo { get; set; }
		public List<SubscriptionUser> SubscriptionUserInfo { get; set; }

		public ReportInfo(List<Customer> customers, List<CompleteProject> completeProjectInfo, List<SubscriptionUser> subscriptionUserInfo)
		{
			this.Customers = customers;
			this.CompleteProjectInfo = completeProjectInfo;
			this.SubscriptionUserInfo = subscriptionUserInfo;
		}
	}
}