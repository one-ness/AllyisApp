//------------------------------------------------------------------------------
// <copyright file="DBHelper.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Data.SqlClient;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// Contains data and information that is not linked to a schema
	/// but is still required for the appllication.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DBHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The Connection string.</param>
		public DBHelper(string connectionString)
		{
			this.SqlConnectionString = connectionString ?? throw new ArgumentNullException("connectionString");
			this.lockObject = new object();
		}

		private string SqlConnectionString { get; set; }
		private bool inTransaction;
		private SqlConnection connection;
		private SqlTransaction transaction;
		private object lockObject;
		private string transactionName;

		/// <summary>
		/// begin transaction
		/// </summary>
		public void BeginTransaction(string transactionName)
		{
			if (string.IsNullOrWhiteSpace(transactionName)) throw new ArgumentNullException("transactionName");

			lock (this.lockObject)
			{
				if (!this.inTransaction)
				{
					this.transactionName = transactionName;
					this.inTransaction = true;
					this.connection = new SqlConnection(this.SqlConnectionString);
					this.connection.Open();
					// NOTE: default transaction level is read-committed. TODO: should we change it?
					this.transaction = this.connection.BeginTransaction(transactionName);
				}
			}
		}

		/// <summary>
		/// commit transaction
		/// </summary>
		public void CommitTransaction()
		{
			lock (this.lockObject)
			{
				if (this.inTransaction)
				{
					this.inTransaction = false;
					this.transaction.Commit();
					this.transaction.Dispose();
					this.transactionName = string.Empty;
					this.connection.Close();
					this.connection.Dispose();
				}
			}
		}

		/// <summary>
		/// rollback transaction
		/// </summary>
		public void RollbackTransaction()
		{
			lock (this.lockObject)
			{
				if (this.inTransaction)
				{
					this.inTransaction = false;
					this.transaction.Rollback(this.transactionName);
					this.transaction.Dispose();
					this.transactionName = string.Empty;
					this.connection.Close();
					this.connection.Dispose();
				}
			}
		}
	}
}
