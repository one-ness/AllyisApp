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
	public class ProductDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets Description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets ProductId.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets is active.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets area url.
		/// </summary>
		public string AreaUrl { get; set; }
	}
}
