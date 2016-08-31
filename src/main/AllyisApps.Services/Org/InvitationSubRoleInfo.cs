//------------------------------------------------------------------------------
// <copyright file="InvitationSubRoleInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Org
{
	/// <summary>
	/// Represents an invitation sub role.
	/// </summary>
	public class InvitationSubRoleInfo
	{
		/// <summary>
		/// Gets or sets the Invitation Id.
		/// </summary>
		public int InvitationId { get; set; }

		/// <summary>
		/// Gets or sets the Subscription Id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the Product role id.
		/// </summary>
		public int ProductRoleId { get; set; }
	}
}