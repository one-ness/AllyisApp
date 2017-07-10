//------------------------------------------------------------------------------
// <copyright file="SkuIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// SKUs offered by us. Must match the IDs inserted in to db.
	/// </summary>
	public enum SkuIdEnum : short
	{
		/// <summary>
		/// TimeTracker SKU.
		/// </summary>
		TimeTrackerBasic = 1,

		/// <summary>
		/// ConsultingBasic SKU.
		/// </summary>
		ExpenseTrackerBasic = 2,
	}
}
