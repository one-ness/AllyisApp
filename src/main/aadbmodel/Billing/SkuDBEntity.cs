//------------------------------------------------------------------------------
// <copyright file="SkuDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// A class representing the Sku table in the database.
	/// </summary>
	public class SkuDBEntity : BasePoco
	{
		private int pSkuId;
		private int pProductId;
		private string pName;
		private decimal pPrice;
		private int pUserLimit;
		private string pBillingFrequency;

		/// <summary>
		/// Gets or sets SubscriptionId.
		/// </summary>
		public int SubscriptionId { get; set; }

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
				this.ApplyPropertyChange<SkuDBEntity, int>(ref this.pSkuId, (SkuDBEntity x) => x.SkuId, value);
			}
		}

		/// <summary>
		/// Gets or sets ProductId.
		/// </summary>
		public int ProductId
		{
			get
			{
				return this.pProductId;
			}

			set
			{
				this.ApplyPropertyChange<SkuDBEntity, int>(ref this.pProductId, (SkuDBEntity x) => x.ProductId, value);
			}
		}

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string Name
		{
			get
			{
				return this.pName;
			}

			set
			{
				this.ApplyPropertyChange<SkuDBEntity, string>(ref this.pName, (SkuDBEntity x) => x.Name, value);
			}
		}

		/// <summary>
		/// Gets or sets Price.
		/// </summary>
		public decimal Price
		{
			get
			{
				return this.pPrice;
			}

			set
			{
				this.ApplyPropertyChange<SkuDBEntity, decimal>(ref this.pPrice, (SkuDBEntity x) => x.Price, value);
			}
		}

		/// <summary>
		/// Gets or sets UserLimit.
		/// </summary>
		public int UserLimit
		{
			get
			{
				return this.pUserLimit;
			}

			set
			{
				this.ApplyPropertyChange<SkuDBEntity, int>(ref this.pUserLimit, (SkuDBEntity x) => x.UserLimit, value);
			}
		}

		/// <summary>
		/// Gets or sets BillingFrequency.
		/// </summary>
		public string BillingFrequency
		{
			get
			{
				return this.pBillingFrequency;
			}

			set
			{
				this.ApplyPropertyChange<SkuDBEntity, string>(ref this.pBillingFrequency, (SkuDBEntity x) => x.BillingFrequency, value);
			}
		}
	}
}