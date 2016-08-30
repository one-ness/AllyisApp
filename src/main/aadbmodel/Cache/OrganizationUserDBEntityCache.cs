//------------------------------------------------------------------------------
// <copyright file="OrganizationUserDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Auth;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.DBModel.Cache
{
	/// <summary>
	/// OrganizationUser DB Entity cache.
	/// </summary>
	internal class OrganizationUserDBEntityCache
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="OrganizationUserDBEntityCache" /> class from being created.
		/// </summary>
		internal static readonly OrganizationUserDBEntityCache Instance = new OrganizationUserDBEntityCache();

		private List<OrganizationUserDBEntity> items;

		/// <summary>
		/// Prevents a default instance of the <see cref="OrganizationUserDBEntityCache" /> class from being created.
		/// </summary>
		private OrganizationUserDBEntityCache()
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
		/// <returns>A list of OrganizationUserDBEntity entities.</returns>
		internal List<OrganizationUserDBEntity> Items()
		{
			return this.items;
		}

		/// <summary>
		/// Get the item indicated by the item id.
		/// </summary>
		/// <param name="itemId">The itemId.</param>
		/// <returns>The OrganizationUserDBEntity.</returns>
		internal OrganizationUserDBEntity GetItemById(int itemId)
		{
			return this.Items().Where(x => x.OrganizationId == itemId).FirstOrDefault();
		}

		/// <summary>
		/// Add or update the given entity to the cache.
		/// </summary>
		/// <param name="entity">The OrganizationUserDBEntity.</param>
		internal void AddUpdate(OrganizationUserDBEntity entity)
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
		/// <returns>A list of OrganizationUserDBEntity entities.</returns>
		private List<OrganizationUserDBEntity> Load()
		{
			return DBHelper.Instance.GetOrganizationUserList();
		}
	}
}