﻿//------------------------------------------------------------------------------
// <copyright file="DBModelStartup.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// Startup class for DB Model.
	/// </summary>
	public static class DBModelStartup
	{
		/// <summary>
		/// Initialize the DB Model.
		/// </summary>
		/// <param name="connectionStringKey">The connection string key.</param>
		public static void Init(string connectionStringKey = "DefaultConnection")
		{
			DBHelper.Instance.Init(connectionStringKey);
		}
	}
}
