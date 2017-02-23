//------------------------------------------------------------------------------
// <copyright file="SkuDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Billing;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.DBModel.Cache
{
	/// <summary>
	/// Sku DB Entity cache.
	/// </summary>
	internal class SkuDBEntityCache
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="SkuDBEntityCache" /> class from being created.
		/// </summary>
		internal static readonly SkuDBEntityCache Instance = new SkuDBEntityCache();

		private List<SkuDBEntity> items;

		/// <summary>
		/// Prevents a default instance of the <see cref="SkuDBEntityCache" /> class from being created.
		/// </summary>
		private SkuDBEntityCache()
		{
			this.items = this.Load();
		}

		/// <summary>
		/// Reload items from the database.
		/// </summary>
		internal void Refresh()
		{
			this.items = this.Load();
		}

		/// <summary>
		/// Get the list of items in the cache.
		/// </summary>
		/// <returns>A list of SkuDBEntity entities.</returns>
		internal List<SkuDBEntity> Items()
		{
			return this.items;
		}

		/// <summary>
		/// Get the item indicated by the item id.
		/// </summary>
		/// <param name="itemId">The itemId.</param>
		/// <returns>The SkuDBEntity.</returns>
		internal SkuDBEntity GetItemById(int itemId)
		{
			return this.Items().Where(x => x.SkuId == itemId).FirstOrDefault();
		}

		/// <summary>
		/// Add or update the given entity to the cache.
		/// </summary>
		/// <param name="entity">The SkuDBEntity.</param>
		internal void AddUpdate(SkuDBEntity entity)
		{
			var existing = this.items.Where(x => x.SkuId == entity.ProductId).FirstOrDefault();
			if (existing != null)
			{
				this.items.Remove(existing);
			}

			this.items.Add(entity);
		}

		/// <summary>
		/// Load items from the database.
		/// </summary>
		/// <returns>A list of SkuDBEntity entities.</returns>
		private List<SkuDBEntity> Load()
		{
			return DBHelper.Instance.GetSkuList().ToList();
		}
	}
}
