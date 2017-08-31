//------------------------------------------------------------------------------
// <copyright file="UserRolesDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// The database model for the collection of roles that a user has.
	/// </summary>
	public class UserRolesDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets User's first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets User's last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets User's Id.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets Their orgization role id.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		/// <summary>
		/// Gets or sets Name of their role.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets Their email.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets Their product role ids.
		/// </summary>
		public int? ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets Their subscription ids.
		/// </summary>
		public int? SubscriptionId { get; set; }
	}
}
