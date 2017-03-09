//------------------------------------------------------------------------------
// <copyright file="ProductDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

//using AllyisApps.DBModel.Billing;
//using System.Collections.Generic;
//using System.Linq;

//namespace AllyisApps.DBModel.Cache
//{
//	/// <summary>
//	/// Product DB Entity cache.
//	/// </summary>
//	internal class ProductDBEntityCache
//	{
//		/// <summary>
//		/// Prevents a default instance of the <see cref="ProductDBEntityCache" /> class from being created.
//		/// </summary>
//		internal static readonly ProductDBEntityCache Instance = new ProductDBEntityCache();

//		private List<ProductDBEntity> items;

//		/// <summary>
//		/// Prevents a default instance of the <see cref="ProductDBEntityCache" /> class from being created.
//		/// </summary>
//		private ProductDBEntityCache()
//		{
//			this.items = this.Load();
//		}

//		/// <summary>
//		/// Reload the items from the database.
//		/// </summary>
//		internal void Refresh()
//		{
//			this.items = this.Load();
//		}

//		/// <summary>
//		/// Get the list of items in the cache.
//		/// </summary>
//		/// <returns>A list of ProductDBEntity entities.</returns>
//		internal List<ProductDBEntity> Items()
//		{
//			return this.items;
//		}

//		/// <summary>
//		/// Get the item indicated by the item id.
//		/// </summary>
//		/// <param name="itemId">The itemId.</param>
//		/// <returns>The ProductDBEntity.</returns>
//		internal ProductDBEntity GetItemById(int itemId)
//		{
//			return this.Items().Where(x => x.ProductId == itemId).FirstOrDefault();
//		}

//		/// <summary>
//		/// Add or update the given entity to the cache.
//		/// </summary>
//		/// <param name="entity">The ProductDBEntity.</param>
//		internal void AddUpdate(ProductDBEntity entity)
//		{
//			var existing = this.items.Where(x => x.ProductId == entity.ProductId).FirstOrDefault();
//			if (existing != null)
//			{
//				this.items.Remove(existing);
//			}

//			this.items.Add(entity);
//		}

//		/// <summary>
//		/// Load the items from the database.
//		/// </summary>
//		/// <returns>A list of ProductDBEntities.</returns>
//		private List<ProductDBEntity> Load()
//		{
//			return DBHelper.Instance.GetProductList();
//		}
//	}
//}
