//------------------------------------------------------------------------------
// <copyright file="LastUsed.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// LastUsedDBEntity obj for getting the last subscription used by user.  
	/// </summary>
	public class LastUsed : BasePoco
	{
		/// <summary>
		/// Gets or sets Organization.
		/// </summary>
		public string Organization { get; set; }

		/// <summary>
		/// Gets or sets OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets Product.
		/// </summary>
		public string Product { get; set; }

		/// <summary>
		/// Gets or sets ProductId.
		/// </summary>
		public int ProductId { get; set; }
	}
}
