//------------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;
using AllyisApps.Services.Account;

namespace AllyisApps.Services
{
	/// <summary>
	/// Base service - service layer that contains all the business logic.
	/// </summary>
	public class BaseService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseService"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public BaseService(string connectionString)
		{
			this.DBHelper = new DBHelper(connectionString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseService"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="userContext">The user context.</param>
		public BaseService(string connectionString, UserContext userContext)
		{
			this.DBHelper = new DBHelper(connectionString);
			this.UserContext = userContext;
		}

		/// <summary>
		/// Gets the Db context for database operations.
		/// </summary>
		protected DBHelper DBHelper { get; private set; }

		/// <summary>
		/// Gets the Logged in user context.
		/// </summary>
		protected UserContext UserContext { get; private set; }

		/// <summary>
		/// Sets the UserContext.
		/// </summary>
		/// <param name="userContext">The UserContext.</param>
		public void SetUserContext(UserContext userContext)
		{
			this.UserContext = userContext;
		}
	}
}
