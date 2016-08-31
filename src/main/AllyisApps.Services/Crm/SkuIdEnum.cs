//------------------------------------------------------------------------------
// <copyright file="SkuIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Crm
{
	/// <summary>
	/// An enum of ints of possible SKUs.
	/// </summary>
	public enum SkuIdEnum : int
	{
		/// <summary>
		/// TimeTracker SKU.
		/// </summary>
		TimeTracker = 1,

		/// <summary>
		/// TimeTrackerBasic SKU.
		/// </summary>
		TimeTrackerBasic,

		/// <summary>
		/// Consulting SKU.
		/// </summary>
		Consulting,

		/// <summary>
		/// ConsultingBasic SKU.
		/// </summary>
		ConsultingBasic,
	}
}