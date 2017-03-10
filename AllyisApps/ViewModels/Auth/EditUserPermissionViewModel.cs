//------------------------------------------------------------------------------
// <copyright file="EditUserPermissionViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Model for editing the user's permissions in an organization.
	/// </summary>
	public class EditUserPermissionViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the full name of the current user.
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// Gets or sets the userId of the current user.
		/// </summary>
		[Required]
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the Organization Id of the current Row.
		/// </summary>
		[Required]
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the user's current role in the organization.
		/// </summary>
		[Display(Name = "Organization User Role")]
		public string OrgRole { get; set; }

		/// <summary>
		/// Gets or sets the available organization roles.
		/// </summary>
		public IList<SelectListItem> OrgRoles { get; set; }

		/// <summary>
		/// Gets or sets the product list information.
		/// </summary>
		public IList<SelectListItem> ProductInfo { get; set; }

		/// <summary>
		/// Gets or sets the role information for each product.
		/// </summary>
		public Dictionary<string, List<SelectListItem>> RoleInfo { get; set; }

		/// <summary>
		/// Gets or sets the Users roles for each product.
		/// </summary>
		public Dictionary<string, string> UserRoles { get; set; }
	}
}
