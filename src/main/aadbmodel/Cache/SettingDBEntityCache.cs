//------------------------------------------------------------------------------
// <copyright file="SettingDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.TimeTracker;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.DBModel.Cache
{
	/// <summary>
	/// Setting DB Entity cache.
	/// </summary>
	internal class SettingDBEntityCache
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="SettingDBEntityCache" /> class from being created.
		/// </summary>
		internal static readonly SettingDBEntityCache Instance = new SettingDBEntityCache();

		private List<SettingDBEntity> items;

		/// <summary>
		/// Prevents a default instance of the <see cref="SettingDBEntityCache" /> class from being created.
		/// </summary>
		private SettingDBEntityCache()
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
		/// <returns>A list of SettingDBEntity entities.</returns>
		internal List<SettingDBEntity> Items()
		{
			return this.items;
		}

		/// <summary>
		/// Get the item indicated by the organization id.
		/// </summary>
		/// <param name="itemId">The itemId.</param>
		/// <returns>The SettingDBEntity.</returns>
		internal SettingDBEntity GetItemById(int itemId)
		{
			return this.Items().Where(x => x.OrganizationId == itemId).FirstOrDefault();
		}

		/// <summary>
		/// Add or update the given entity to the cache.
		/// </summary>
		/// <param name="entity">The SettingDBEntity.</param>
		internal void AddUpdate(SettingDBEntity entity)
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
		/// <returns>A list of SettingDBEntity entities.</returns>
		private List<SettingDBEntity> Load()
		{
			return DBHelper.Instance.GetSettingList();
		}
	}
}