//------------------------------------------------------------------------------
// <copyright file="DBHelper.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Configuration;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// Contains data and information that is not linked to a schema
	/// but is still required for the appllication.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="DBHelper"/> class from being created.
		/// </summary>
		public static readonly DBHelper Instance = new DBHelper();

		/// <summary>
		/// Initializes a new instance of the <see cref="DBHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The Connection string.</param>
		public DBHelper(string connectionString)
		{
			this.SqlConnectionString = connectionString;
		}

		/// <summary>
		/// Prevents a default instance of the <see cref="DBHelper"/> class from being created.
		/// </summary>
		private DBHelper()
		{
		}

		/// <summary>
		/// Gets or sets the connection string to the backing database.
		/// </summary>
		private string SqlConnectionString { get; set; }

		/// <summary>
		/// Initializes the database helper.
		/// </summary>
		/// <param name="key">The key of the connection strings configuration.</param>
		public void Init(string key)
		{
			this.SqlConnectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
		}
	}
}
