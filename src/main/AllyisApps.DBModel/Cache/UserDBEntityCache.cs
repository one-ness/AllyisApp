//------------------------------------------------------------------------------
// <copyright file="UserDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

//using AllyisApps.DBModel.Auth;
//using System.Collections.Generic;
//using System.Linq;

//namespace AllyisApps.DBModel.Cache
//{
//	/// <summary>
//	/// UserDBEntity cache.
//	/// </summary>
//	internal class UserDBEntityCache
//	{
//		/// <summary>
//		/// Prevents a default instance of the <see cref="UserDBEntityCache" /> class from being created.
//		/// </summary>
//		internal static readonly UserDBEntityCache Instance = new UserDBEntityCache();

//		/// <summary>
//		/// List of all of the users in the dadtabase.
//		/// </summary>
//		private List<UserDBEntity> items;

//		#region inits

//		/// <summary>
//		/// Prevents a default instance of the <see cref="UserDBEntityCache" /> class from being created.
//		/// </summary>
//		private UserDBEntityCache()
//		{
//			this.items = this.Load();
//		}

//		/// <summary>
//		/// Reload all of the local items. Should never be called to save proccessing.
//		/// </summary>
//		internal void Refresh()
//		{
//			this.items = this.Load();
//		}

//		#endregion inits

//		#region gets

//		/// <summary>
//		/// Get items function.
//		/// </summary>
//		/// <returns>List of users.</returns>
//		internal List<UserDBEntity> Items()
//		{
//			return this.items;
//		}

//		/// <summary>
//		/// Get user by id.
//		/// </summary>
//		/// <param name="itemId">The users id.</param>
//		/// <returns>The given user or null if none.</returns>
//		internal UserDBEntity GetItemById(int itemId)
//		{
//			return this.Items().Where(x => x.UserId == itemId).FirstOrDefault();
//		}

//		/// <summary>
//		/// Gets UserDBEntity by email address.
//		/// </summary>
//		/// <param name="email">Email of the user.</param>
//		/// <returns>The UserDBEntity.</returns>
//		internal UserDBEntity GetUserByEmail(string email)
//		{
//			email.Trim();
//			return this.Items().Where(x => x.Email.Equals(email)).FirstOrDefault();
//		}

//		#endregion gets

//		#region sets

//		/// <summary>
//		/// Updates the local instance with the new user entity.
//		/// </summary>
//		/// <param name="entity">The user to add/update.</param>
//		internal void AddUpdate(UserDBEntity entity)
//		{
//			// Check our list for any current instance,
//			// To avoid duplication remove it from the list
//			UserDBEntity current = this.Items().Where(x => x.UserId == entity.UserId).FirstOrDefault();
//			if (current != null)
//			{
//				this.Items().Remove(current);
//			}

//			// Add incoming instance
//			this.items.Add(entity);
//		}

//		/// <summary>
//		/// Gets all of the users from the database.
//		/// </summary>
//		/// <returns>A list of users.</returns>
//		private List<UserDBEntity> Load()
//		{
//			return DBHelper.Instance.GetUserList();
//		}

//		#endregion sets
//	}
//}
