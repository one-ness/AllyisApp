﻿//------------------------------------------------------------------------------
// <copyright file="SubscriptionRoleSelectionModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Object for displaying and capturing Subscription Role selections.
	/// </summary>
	public class SubscriptionRoleSelectionModel : BaseViewModel
	{
		private IEnumerable<ProductRoleViewModel> roles;

		/// <summary>
		/// Gets or sets the subscriptionId.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the name of the product.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets the role objects associated with this subscription.
		/// </summary>
		public IEnumerable<ProductRoleViewModel> Roles
		{
			get
			{
				return roles;
			}

			internal set
			{
				foreach (ProductRoleViewModel role in value)
				{
					role.ProductRoleName = Resources.Strings.ResourceManager.GetString(role.ProductRoleName.Replace(" ", string.Empty)) ?? role.ProductRoleName;
				}

				roles = value;
			}
		}

		/// <summary>
		/// Gets or sets the selected role Id.
		/// </summary>
		public int SelectedRole { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user role can be selected.
		/// </summary>
		public bool Disabled { get; set; }
	}
}