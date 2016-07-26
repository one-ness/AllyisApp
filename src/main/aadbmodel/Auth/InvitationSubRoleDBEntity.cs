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
	public class InvitationSubRoleDBEntity : BasePoco
	{
		private int pInvitationId;
		private int pSubscriptionId;
		private int pProductRoleId;

        /// <summary>
        /// Gets or sets the id of the invitation. 
        /// </summary>
        public int InvitationId
		{
			get
			{
				return this.pInvitationId;
			}

			set
			{
				this.ApplyPropertyChange<InvitationSubRoleDBEntity, int>(ref this.pInvitationId, (InvitationSubRoleDBEntity x) => x.InvitationId, value);
			}
		}

        /// <summary>
        /// Gets or sets the subscription id. 
        /// </summary>
        public int SubscriptionId
		{
			get
			{
				return this.pSubscriptionId;
			}

			set
			{
				this.ApplyPropertyChange<InvitationSubRoleDBEntity, int>(ref this.pSubscriptionId, (InvitationSubRoleDBEntity x) => x.SubscriptionId, value);
			}
		}

        /// <summary>
        /// Gets or sets the product role id. 
        /// </summary>
        public int ProductRoleId
		{
			get
			{
				return this.pProductRoleId;
			}

			set
			{
				this.ApplyPropertyChange<InvitationSubRoleDBEntity, int>(ref this.pProductRoleId, (InvitationSubRoleDBEntity x) => x.ProductRoleId, value);
			}
		}
	}
}
