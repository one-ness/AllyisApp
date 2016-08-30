//------------------------------------------------------------------------------
// <copyright file="TablePermissions.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// Table that contains the user permissions for a given subscription.
	/// </summary>
	public class TableSubscriptionPermissions
	{
		/// <summary>
		/// Gets or sets The user's id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets The id of the subscription.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets The id of the organization.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets The id of the Application.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets The id of the product role.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets The name of the product role.
		/// </summary>
		public string ProductRoleName { get; set; }

		/// <summary>
		/// Gets or sets The id of the project.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the user has permission admin rights.
		/// </summary>
		public bool PermissionAdmin { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the user has product permission admin rights.
		/// </summary>
		public bool PermissionProductAdmin { get; set; }
	}

	/// <summary>
	/// Table that contains the user permissions for a given organizaiton.
	/// </summary>
	public class TableOrganizationPermissions
	{
		/// <summary>
		/// Gets or sets The id of the user.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets The id of the organization.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets The id of the organization role.
		/// </summary>
		public int OrgRoleId { get; set; }

		/// <summary>
		/// Gets or sets The name of the organization role.
		/// </summary>
		public string OrgRoleName { get; set; }
	}
}