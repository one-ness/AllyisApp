//------------------------------------------------------------------------------
// <copyright file="UserRolesInfo.cs" company="Allyis, Inc.">
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
	/// Represents all the roles held by a user.
	/// </summary>
	public class UserRolesInfo
	{
		/// <summary>
		/// Gets or sets the First name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the Last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the User Id.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the Orgization role Id.
		/// </summary>
		public int OrgRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Name of role.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Email address.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Product role Id.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Subscription Id.
		/// </summary>
		public int SubscriptionId { get; set; }
	}
}
