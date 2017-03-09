//------------------------------------------------------------------------------
// <copyright file="SubscriptionDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

//using AllyisApps.DBModel.Billing;
//using System.Collections.Generic;
//using System.Linq;

//namespace AllyisApps.DBModel.Cache
//{
//	/// <summary>
//	/// SubscriptionDBEntity cache.
//	/// </summary>
//	internal class SubscriptionDBEntityCache
//	{
//		/// <summary>
//		/// Prevents a default instance of the <see cref="SubscriptionDBEntityCache" /> class from being created.
//		/// </summary>
//		internal static readonly SubscriptionDBEntityCache Instance = new SubscriptionDBEntityCache();

//		private List<SubscriptionDBEntity> items;

//		/// <summary>
//		/// Prevents a default instance of the <see cref="SubscriptionDBEntityCache" /> class from being created.
//		/// </summary>
//		private SubscriptionDBEntityCache()
//		{
//			this.items = this.Load();
//		}

//		/// <summary>
//		/// Reload items from the database.
//		/// </summary>
//		internal void Refresh()
//		{
//			this.items = this.Load();
//		}

//		/// <summary>
//		/// Get the list of items in the cache.
//		/// </summary>
//		/// <returns>A list of SubscriptionDBEntity entities.</returns>
//		internal List<SubscriptionDBEntity> Items()
//		{
//			return this.items;
//		}

//		/// <summary>
//		/// Get the item indicated by the item id.
//		/// </summary>
//		/// <param name="itemId">The itemId.</param>
//		/// <returns>The SubscriptionDBEntity.</returns>
//		internal SubscriptionDBEntity GetItemById(int itemId)
//		{
//			return this.Items().Where(x => x.SubscriptionId == itemId).FirstOrDefault();
//		}

//		/// <summary>
//		/// Add or update the given entity to the cache.
//		/// </summary>
//		/// <param name="entity">The SubscriptionDBEntity.</param>
//		internal void AddUpdate(SubscriptionDBEntity entity)
//		{
//			var existing = this.items.Where(x => x.SubscriptionId == entity.SubscriptionId).FirstOrDefault();
//			if (existing != null)
//			{
//				this.items.Remove(existing);
//			}

//			this.items.Add(entity);
//		}

//		/// <summary>
//		/// Load items from the database.
//		/// </summary>
//		/// <returns>A list of SubscriptionDBEntity entities.</returns>
//		private List<SubscriptionDBEntity> Load()
//		{
//			return DBHelper.Instance.GetSubscriptionList();
//		}
//	}
//}
