//------------------------------------------------------------------------------
// <copyright file="SubscriptionRoleInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// Subscription Role obj.
	/// </summary>
	public class SubscriptionRoleInfo
	{
		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets ProductRoleId.
		/// </summary>
		public int ProductRoleId { get; set; }
	}
}
