//------------------------------------------------------------------------------
// <copyright file="SubscriptionUserDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Billing;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.DBModel.Cache
{
	/// <summary>
	/// SubscriptionUserDBEntity Cache.
	/// </summary>
	internal class SubscriptionUserDBEntityCache
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="SubscriptionUserDBEntityCache" /> class from being created.
		/// </summary>
		internal static readonly SubscriptionUserDBEntityCache Instance = new SubscriptionUserDBEntityCache();

		private List<SubscriptionUserDBEntity> items;

		/// <summary>
		/// Prevents a default instance of the <see cref="SubscriptionUserDBEntityCache" /> class from being created.
		/// </summary>
		private SubscriptionUserDBEntityCache()
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
		/// <returns>A list of the SubscriptionUserDBEntity entities.</returns>
		internal List<SubscriptionUserDBEntity> Items()
		{
			return this.items;
		}

		/// <summary>
		/// Get the item indicated by the item id.
		/// </summary>
		/// <param name="itemId">The itemId.</param>
		/// <returns>The SubscriptionUserDBEntity.</returns>
		internal SubscriptionUserDBEntity GetItemById(int itemId)
		{
			return this.Items().Where(x => x.UserId == itemId).FirstOrDefault();
		}

		/// <summary>
		/// Add or update the given entity to the cache.
		/// </summary>
		/// <param name="entity">The SubscriptionUserDBEntity.</param>
		internal void AddUpdate(SubscriptionUserDBEntity entity)
		{
			var existing = this.items.Where(x => x.UserId == entity.UserId).FirstOrDefault();
			if (existing != null)
			{
				this.items.Remove(existing);
			}

			this.items.Add(entity);
		}

		/// <summary>
		/// Load items from the database.
		/// </summary>
		/// <returns>A list of the SubscriptionUserDBEntity entities.</returns>
		private List<SubscriptionUserDBEntity> Load()
		{
			return DBHelper.Instance.GetSubscriptionUserList();
		}
	}
}