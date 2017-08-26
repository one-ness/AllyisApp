//------------------------------------------------------------------------------
// <copyright file="SkuIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// SKUs offered by us. Must match the Ids inserted in to db.
	/// </summary>
	public enum SkuIdEnum : int
	{
		/// <summary>
		/// Id for no sku
		/// </summary>
		None = 0,

		/// <summary>
		/// Time tracker basic.
		/// </summary>
		TimeTrackerBasic = 200001,

		/// <summary>
		/// Expense tracker basic.
		/// </summary>
		ExpenseTrackerBasic = 300001,

		/// <summary>
		/// staffing manager basic.
		/// </summary>
		StaffingManagerBasic = 400001,
	}
}
