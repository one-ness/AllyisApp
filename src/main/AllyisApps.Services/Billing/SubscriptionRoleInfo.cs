//------------------------------------------------------------------------------
// <copyright file="SubscriptionRoleInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// Subscription Role obj.
	/// </summary>
	public class SubscriptionRoleInfo
	{
		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets ProductRoleId.
		/// </summary>
		public int ProductRoleId { get; set; }
	}
}