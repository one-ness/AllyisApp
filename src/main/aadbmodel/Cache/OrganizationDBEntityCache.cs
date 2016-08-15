//------------------------------------------------------------------------------
// <copyright file="OrganizationDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using AllyisApps.DBModel.Auth;

namespace AllyisApps.DBModel.Cache
{
	/// <summary>
	/// Organization DB Entity cache.
	/// </summary>
	internal class OrganizationDBEntityCache
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="OrganizationDBEntityCache" /> class from being created.
		/// </summary>
		internal static readonly OrganizationDBEntityCache Instance = new OrganizationDBEntityCache();

		private List<OrganizationDBEntity> items;

		/// <summary>
		/// Prevents a default instance of the <see cref="OrganizationDBEntityCache" /> class from being created.
		/// </summary>
		private OrganizationDBEntityCache()
		{
			this.items = this.Load();
		}

		/// <summary>
		/// Load the items from the database again.
		/// </summary>
		internal void Refresh()
		{
			this.items = this.Load();
		}

		/// <summary>
		/// Get the list of items in the cache.
		/// </summary>
		/// <returns>A list of OrganizationDBEntity entities.</returns>
		internal List<OrganizationDBEntity> Items()
		{
			return this.items;
		}

		/// <summary>
		/// Get the item indicated by the item id.
		/// </summary>
		/// <param name="itemId">The itemId.</param>
		/// <returns>The OrganizationDBEntity.</returns>
		internal OrganizationDBEntity GetItemById(int itemId)
		{
			return this.Items().Where(x => x.OrganizationId == itemId).FirstOrDefault();
		}

		/// <summary>
		/// Add or update the given entity to the cache.
		/// </summary>
		/// <param name="entity">The OrganizationDBEntity.</param>
		internal void AddUpdate(OrganizationDBEntity entity)
		{
			var existing = this.items.Where(x => x.OrganizationId == entity.OrganizationId).FirstOrDefault();
			if (existing != null)
			{
				this.items.Remove(existing);
			}

			this.items.Add(entity);
		}

		/// <summary>
		/// Load from db.
		/// </summary>
		/// <returns>A list of OrganizationDBEntity entities.</returns>
		private List<OrganizationDBEntity> Load()
		{
			return DBHelper.Instance.GetOrganizationList();
		}
	}
}
