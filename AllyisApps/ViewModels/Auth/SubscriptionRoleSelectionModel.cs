//------------------------------------------------------------------------------
// <copyright file="SubscriptionRoleSelectionModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.Billing;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Object for displaying and capturing Subscription Role selections.
	/// </summary>
	public class SubscriptionRoleSelectionModel : BaseViewModel
	{
		private IEnumerable<ProductRole> roles;

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
		public IEnumerable<ProductRole> Roles
		{
			get
			{
				return roles;
			}

			internal set
			{
				foreach (ProductRole role in value)
				{
					role.Name = Resources.Strings.ResourceManager.GetString(role.Name.Replace(" ", string.Empty)) ?? role.Name;
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
