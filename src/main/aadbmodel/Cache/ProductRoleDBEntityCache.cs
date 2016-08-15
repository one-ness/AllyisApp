//------------------------------------------------------------------------------
// <copyright file="ProductRoleDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AllyisApps.DBModel.Auth;

namespace AllyisApps.DBModel.Cache
{
	/// <summary>
	/// Local list of all product roles.
	/// </summary>
	internal class ProductRoleDBEntityCache
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="ProductRoleDBEntityCache" /> class from being created.
		/// </summary>
		internal static readonly ProductRoleDBEntityCache Instance = new ProductRoleDBEntityCache();

		/// <summary>
		/// List of all of the product roles in the dadtabase.
		/// </summary>
		private List<ProductRoleDBEntity> items;

		//-------------------------------------------------------------------//
		//---------------------------Inits-----------------------------------//
		//-------------------------------------------------------------------//

		/// <summary>
		/// Prevents a default instance of the <see cref="ProductRoleDBEntityCache" /> class from being created.
		/// </summary>
		private ProductRoleDBEntityCache()
		{
			this.items = this.Load();
		}

		/// <summary>
		/// Reload all of the local items. Should never be called to save proccessing.
		/// </summary>
		internal void Refresh()
		{
			this.items = this.Load();
		}

		//-------------------------------------------------------------------//
		//---------------------------Gets------------------------------------//
		//-------------------------------------------------------------------//

		/// <summary>
		/// Get items function.
		/// </summary>
		/// <returns>List of product roles.</returns>
		internal List<ProductRoleDBEntity> Items()
		{
			return this.items;
		}

		/// <summary>
		/// Gets the product role by its id.
		/// </summary>
		/// <param name="itemId">Product role id.</param>
		/// <returns>Product role.</returns>
		internal ProductRoleDBEntity GetItemById(int itemId)
		{
			return this.Items().Where(x => x.ProductId == itemId).FirstOrDefault();
		}
		
		//-------------------------------------------------------------------//
		//---------------------------Sets------------------------------------//
		//-------------------------------------------------------------------//

		/// <summary>
		/// Updates the local instance with the new ProductRole entity.
		/// </summary>
		/// <param name="entity">The product role to add/update.</param>
		internal void AddUpdate(ProductRoleDBEntity entity)
		{
			// Check our list for any current instance,
			// To avoid duplication remove it from the list
			ProductRoleDBEntity current = this.Items().Where(x => x.ProductId == entity.ProductId).FirstOrDefault();
			if (current != null)
			{
				this.Items().Remove(current);
			}

			// Add incoming instance
			this.items.Add(entity);
		}

		/// <summary>
		/// Gets all of the organization roles from the database.
		/// </summary>
		/// <returns>A list of organization roles.</returns>
		private List<ProductRoleDBEntity> Load()
		{
			return DBHelper.Instance.GetProductRoleList();
		}
	}
}
