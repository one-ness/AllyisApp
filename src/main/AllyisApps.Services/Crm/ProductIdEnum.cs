//------------------------------------------------------------------------------
// <copyright file="ProductIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// List of products offered by us. These should match the product ids in the database.
	/// </summary>
	public enum ProductIdEnum : int
	{
        /// <summary>
        /// ID for no product.
        /// </summary>
        None = 0,

		/// <summary>
		/// ID for TimeTracker.
		/// </summary>
		TimeTracker = 1,

		/// <summary>
		/// ID for Consulting.
		/// </summary>
		Consulting,
	}
}