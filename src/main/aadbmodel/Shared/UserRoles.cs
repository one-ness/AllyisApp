﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Shared
{
	/// <summary>
	/// The database model for the collection of roles that a user has
	/// </summary>
	public class UserRoles : BasePoco
	{
		/// <summary>
		/// User's first anme
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// User's last name
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// User's Id
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Their orgization role id
		/// </summary>
		public int OrgRoleId { get; set; }

		/// <summary>
		/// Name of their role
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Their email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Their product role ids
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Their subscription ids
		/// </summary>
		public int SubscriptionId { get; set; }

	}
}
