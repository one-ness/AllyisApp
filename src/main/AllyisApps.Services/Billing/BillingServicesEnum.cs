﻿//------------------------------------------------------------------------------
// <copyright file="BillingServicesEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// This enum is the be-all-end-all location for keeping track of what services we support.  If it isn't here, the code won't try.
	/// </summary>
	internal enum BillingServicesEnum
	{
		/// <summary>
		/// I mean, this is pretty self-explanatory, no?.
		/// </summary>
		Stripe
	}
}