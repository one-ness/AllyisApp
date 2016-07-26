//------------------------------------------------------------------------------
// <copyright file="ProductRoleDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// The Product Role Table Poco.
	/// </summary>
	public class ProductRoleDBEntity : BasePoco
	{
		private int pProductRoleId;
		private int pProductId;
		private string pName;
		private bool pPermissionAdmin;
		private DateTime pCreatedUTC;
		private DateTime pModifiedUTC;

		/// <summary>
		/// Gets or sets the ProductRoleId.
		/// </summary>
		public int ProductRoleId
		{
			get
			{
				return pProductRoleId;
			}

			set
			{
				this.ApplyPropertyChange<ProductRoleDBEntity, int>(ref this.pProductRoleId, (ProductRoleDBEntity x) => x.ProductRoleId, value);
			}
		}

		/// <summary>
		/// Gets or sets the ProductId.
		/// </summary>
		public int ProductId
		{
			get
			{
				return pProductId;
			}

			set
			{
				this.ApplyPropertyChange<ProductRoleDBEntity, int>(ref this.pProductId, (ProductRoleDBEntity x) => x.ProductId, value);
			}
		}

		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		public string Name
		{
			get
			{
				return pName;
			}

			set
			{
				this.ApplyPropertyChange<ProductRoleDBEntity, string>(ref this.pName, (ProductRoleDBEntity x) => x.Name, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the PermissionAdmin bit.
		/// </summary>
		public bool PermissionAdmin
		{
			get
			{
				return pPermissionAdmin;
			}

			set
			{
				this.ApplyPropertyChange<ProductRoleDBEntity, bool>(ref this.pPermissionAdmin, (ProductRoleDBEntity x) => x.PermissionAdmin, value);
			}
		}

		/// <summary>
		/// Gets or sets the CreatedUTC.
		/// </summary>
		public DateTime CreatedUTC
		{
			get
			{
				return pCreatedUTC;
			}

			set
			{
				this.ApplyPropertyChange<ProductRoleDBEntity, DateTime>(ref this.pCreatedUTC, (ProductRoleDBEntity x) => x.CreatedUTC, value);
			}
		}

		/// <summary>
		/// Gets or sets the ModifiedUTC.
		/// </summary>
		public DateTime ModifiedUTC
		{
			get
			{
				return pModifiedUTC;
			}

			set
			{
				this.ApplyPropertyChange<ProductRoleDBEntity, DateTime>(ref this.pModifiedUTC, (ProductRoleDBEntity x) => x.ModifiedUTC, value);
			}
		}
	}
}
