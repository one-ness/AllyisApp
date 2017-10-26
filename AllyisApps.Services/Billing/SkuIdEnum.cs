//------------------------------------------------------------------------------
// <copyright file="SkuIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// SKUs offered by us. Must match the Ids inserted in to db.
	/// </summary>
	public enum SkuIdEnum
	{
		/// <summary>
		/// Id for no sku
		/// </summary>
		None = 0,

		/// <summary>
		/// Time tracker basic.
		/// </summary>
		TimeTrackerBasic = 200001,

		TimeTrackerPro = 200002,

		/// <summary>
		/// Expense tracker basic.
		/// </summary>
		ExpenseTrackerBasic = 300001,

		ExpenseTrackerPro = 300002,

		/// <summary>
		/// staffing manager basic.
		/// </summary>
		StaffingManagerBasic = 400001,

		StaffingManagerPro = 400002
	}
}