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
			SqlConnectionString = connectionString ?? throw new ArgumentNullException("connectionString");
			lockObject = new object();
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

			lock (lockObject)
			{
				if (!inTransaction)
				{
					this.transactionName = transactionName;
					inTransaction = true;
					connection = new SqlConnection(SqlConnectionString);
					connection.Open();
					// NOTE: default transaction level is read-committed. TODO: should we change it?
					transaction = connection.BeginTransaction(transactionName);
				}
			}
		}

		/// <summary>
		/// commit transaction
		/// </summary>
		public void CommitTransaction()
		{
			lock (lockObject)
			{
				if (inTransaction)
				{
					inTransaction = false;
					transaction.Commit();
					transaction.Dispose();
					transactionName = string.Empty;
					connection.Close();
					connection.Dispose();
				}
			}
		}

		/// <summary>
		/// rollback transaction
		/// </summary>
		public void RollbackTransaction()
		{
			lock (lockObject)
			{
				if (inTransaction)
				{
					inTransaction = false;
					transaction.Rollback(transactionName);
					transaction.Dispose();
					transactionName = string.Empty;
					connection.Close();
					connection.Dispose();
				}
			}
		}
	}
}
