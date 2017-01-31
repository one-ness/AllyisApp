//------------------------------------------------------------------------------
// <copyright file="InvitationSubRoleDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Invitation sub role.
	/// </summary>
	public class InvitationSubRoleDBEntity
	{
		/// <summary>
		/// Gets or sets the id of the invitation.
		/// </summary>
		public int InvitationId { get; set; }

		/// <summary>
		/// Gets or sets the subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the product role id.
		/// </summary>
		public int ProductRoleId { get; set; }
	}
}