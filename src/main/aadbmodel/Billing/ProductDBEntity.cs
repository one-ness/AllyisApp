//------------------------------------------------------------------------------
// <copyright file="ProductDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// Represents the Product table in the database.
	/// </summary>
	public class ProductDBEntity : BasePoco
	{
		private int pProductId;

		/// <summary>
		/// Gets or sets Description.
		/// </summary>
		public string Description { get; set; }

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
				this.ApplyPropertyChange<ProductDBEntity, int>(ref this.pProductId, (ProductDBEntity x) => x.ProductId, value);
			}
		}

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string Name { get; set; }
		/*{
            get
            {
                return this.pName;
            }
            
            set
            {
                this.ApplyPropertyChange<ProductDBEntity, string>(ref this.pName, (ProductDBEntity x) => x.Name, value);
            }
        }*/
	}
}
