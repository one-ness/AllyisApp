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
	public partial class DBHelper : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DBHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The Connection string.</param>
		public DBHelper(string connectionString)
		{
			SqlConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
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
			if (string.IsNullOrWhiteSpace(transactionName)) throw new ArgumentNullException(nameof(transactionName));

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

		#region IDisposable Support
		private bool alreadyDisposed; // To detect redundant calls

		/// <summary>
		/// Helper method to implement IDisposable.  An override with a boolean to differentiate
		/// between user calls (also delete managed objects), and finalizer calls (managed objects are already deleted).
		/// Neccessary because this class contains disposable objects SqlConnection and SqlTransaction.
		/// </summary>
		/// <param name="itIsSafeToAlsoFreeManagedObjects">Used for calling from either the finalizer or user-called.</param>
		protected virtual void Dispose(bool itIsSafeToAlsoFreeManagedObjects)
		{
			lock (lockObject)
			{
				if (alreadyDisposed) return;

				// Free managed resources here
				if (itIsSafeToAlsoFreeManagedObjects)
				{
					if (transaction != null)
					{
						transaction.Dispose();
						transaction = null;
					}

					if (connection != null)
					{
						connection.Dispose();
						connection = null;
					}
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				alreadyDisposed = true;
			}
		}

		///// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		//~DBHelper()
		//{
		//	// Do not change this code. Put cleanup code in Dispose(bool itIsSafeToAlsoFreeManagedObjects) above.
		//	Dispose(false);
		//}

		/// <summary>
		/// Implements the disposable pattern.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			//GC.SuppressFinalize(this);
		}
		#endregion
	}
}
