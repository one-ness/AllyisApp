//------------------------------------------------------------------------------
// <copyright file="SubscriptionDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// Represents an organization's product subscription.
	/// </summary>
	public class SubscriptionDBEntity : BasePoco
	{
		private int pSubscriptionId;
		private int pOrganizationId;
		private int pSkuId;
		private int pNumberOfUsers;
		private int pLicenses;
		private DateTime pCreatedDate;
		private bool pIsActive;

		/// <summary>
		/// Gets or sets OrganizationName.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets SubscriptionId.
		/// </summary>
		public int SubscriptionId
		{
			get
			{
				return this.pSubscriptionId;
			}

			set
			{
				this.ApplyPropertyChange<SubscriptionDBEntity, int>(ref this.pSubscriptionId, (SubscriptionDBEntity x) => x.SubscriptionId, value);
			}
		}

		/// <summary>
		/// Gets or sets OrganizationId.
		/// </summary>
		public int OrganizationId
		{
			get
			{
				return this.pOrganizationId;
			}

			set
			{
				this.ApplyPropertyChange<SubscriptionDBEntity, int>(ref this.pOrganizationId, (SubscriptionDBEntity x) => x.OrganizationId, value);
			}
		}

		/// <summary>
		/// Gets or sets SkuId.
		/// </summary>
		public int SkuId
		{
			get
			{
				return this.pSkuId;
			}

			set
			{
				this.ApplyPropertyChange<SubscriptionDBEntity, int>(ref this.pSkuId, (SubscriptionDBEntity x) => x.SkuId, value);
			}
		}

		/// <summary>
		/// Gets or sets SkuId.
		/// </summary>
		public int NumberOfUsers
		{
			get
			{
				return this.pNumberOfUsers;
			}

			set
			{
				this.ApplyPropertyChange<SubscriptionDBEntity, int>(ref this.pNumberOfUsers, (SubscriptionDBEntity x) => x.NumberOfUsers, value);
			}
		}

		/// <summary>
		/// Gets or sets Licenses.
		/// </summary>
		public int Licenses
		{
			get
			{
				return this.pLicenses;
			}

			set
			{
				this.ApplyPropertyChange<SubscriptionDBEntity, int>(ref this.pLicenses, (SubscriptionDBEntity x) => x.Licenses, value);
			}
		}

		/// <summary>
		/// Gets or sets CreatedDate.
		/// </summary>
		public DateTime CreatedUTC
		{
			get
			{
				return this.pCreatedDate;
			}

			set
			{
				this.ApplyPropertyChange<SubscriptionDBEntity, DateTime>(ref this.pCreatedDate, (SubscriptionDBEntity x) => x.CreatedUTC, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this subscription is active.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return this.pIsActive;
			}

			set
			{
				this.ApplyPropertyChange<SubscriptionDBEntity, bool>(ref this.pIsActive, (SubscriptionDBEntity x) => x.IsActive, value);
			}
		}

		/// <summary>
		///  Gets or sets the name of the Sku.
		/// </summary>
		public string Name { get; set; }
	}
}