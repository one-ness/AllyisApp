//------------------------------------------------------------------------------
// <copyright file="SubscriptionRoleDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// Subscription Role.
	/// </summary>
	public class SubscriptionRoleDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string ProductRoleName { get; set; }

		/// <summary>
		/// Gets or sets ProductRoleId.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the ProductId.
		/// </summary>
		public int ProductId { get; set; }
	}
}
