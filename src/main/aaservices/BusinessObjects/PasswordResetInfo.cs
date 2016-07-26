//------------------------------------------------------------------------------
// <copyright file="PasswordResetInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// Password reset information.
	/// </summary>
	public class PasswordResetInfo
	{
		/// <summary>
		/// Gets or sets the User id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the GUID Code.
		/// </summary>
		public Guid Code { get; set; }
	}
}
