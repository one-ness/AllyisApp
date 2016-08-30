//------------------------------------------------------------------------------
// <copyright file="OrgRoleDBEntityCache.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Auth;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.DBModel.Cache
{
	/// <summary>
	/// Local list of all organization roles in the database.
	/// </summary>
	internal class OrgRoleDBEntityCache
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="OrgRoleDBEntityCache" /> class from being created.
		/// </summary>
		internal static readonly OrgRoleDBEntityCache Instance = new OrgRoleDBEntityCache();

		/// <summary>
		/// List of all of the organization roles in the dadtabase.
		/// </summary>
		private List<OrgRoleDBEntity> items;

		#region inits

		/// <summary>
		/// Prevents a default instance of the <see cref="OrgRoleDBEntityCache" /> class from being created.
		/// </summary>
		private OrgRoleDBEntityCache()
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

		#endregion inits

		#region gets

		/// <summary>
		/// Get items function.
		/// </summary>
		/// <returns>List of org roles.</returns>
		internal List<OrgRoleDBEntity> Items()
		{
			return this.items;
		}

		/// <summary>
		/// Gets the organization role by its id.
		/// </summary>
		/// <param name="itemId">Org role id.</param>
		/// <returns>Organization role.</returns>
		internal OrgRoleDBEntity GetItemById(int itemId)
		{
			return this.Items().Where(x => x.OrgRoleId == itemId).FirstOrDefault();
		}

		#endregion gets

		#region sets

		/// <summary>
		/// Updates the local instance with the new orgRole entity.
		/// </summary>
		/// <param name="entity">The org role to add/update.</param>
		internal void AddUpdate(OrgRoleDBEntity entity)
		{
			// Check our list for any current instance,
			// To avoid duplication remove it from the list
			OrgRoleDBEntity current = this.Items().Where(x => x.OrgRoleId == entity.OrgRoleId).FirstOrDefault();
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
		private List<OrgRoleDBEntity> Load()
		{
			return DBHelper.Instance.GetOrgRoleList();
		}

		#endregion sets
	}
}