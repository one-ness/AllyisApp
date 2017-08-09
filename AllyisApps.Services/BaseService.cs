//------------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.DBModel;

namespace AllyisApps.Services
{
	/// <summary>
	/// Base service - service layer that contains all the business logic.
	/// </summary>
	public class BaseService
	{
		/// <summary>
		/// service settings.
		/// </summary>
		public ServiceSettings ServiceSettings { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseService"/> class.
		/// </summary>
		public BaseService(ServiceSettings settings)
		{
			this.ServiceSettings = settings ?? throw new ArgumentNullException("settings");
			this.DBHelper = new DBHelper(this.ServiceSettings.SqlConnectionString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseService"/> class.
		/// </summary>
		public BaseService(ServiceSettings settings, UserContext userContext) : this(settings)
		{
			this.UserContext = userContext ?? throw new ArgumentNullException("userContext");
		}

		/// <summary>
		/// Gets the Db context for database operations.
		/// </summary>
		protected DBHelper DBHelper { get; private set; }

		/// <summary>
		/// Gets the Logged in user context.
		/// </summary>
		public UserContext UserContext { get; private set; }

		/// <summary>
		/// Sets the UserContext.
		/// </summary>
		/// <param name="userContext">The UserContext.</param>
		public void SetUserContext(UserContext userContext)
		{
			this.UserContext = userContext;
		}

		/// <summary>
		/// Given a previous alphanumeric Id, increments to the next Id.
		/// Increments numeric, then upper-case, then lower-case, then repeats.
		/// 0000 -> 0001, 009A -> 009B, 1Bjz -> 1Bk0, etc.
		/// </summary>
		/// <param name="previousId">The Id string to increment.</param>
		/// <returns>A Char[] of the previous Id, incremented by one.</returns>
		public char[] IncrementAlphanumericCharArray(char[] previousId)
		{
			// Define legal characters
			var characters = new List<char>();
			for (char c = '0'; c <= '9'; c++) characters.Add(c); // Add numeric characters first
			for (char c = 'A'; c <= 'Z'; c++) characters.Add(c); // Add upper-case next
			for (char c = 'a'; c <= 'z'; c++) characters.Add(c); // Add lower-case last

			// Increment the string
			for (int i = previousId.Length - 1; i >= 0; --i)
			{
				if (previousId[i] == characters[characters.Count - 1]) previousId[i] = characters[0]; // If last value, round it to the first one and continue the loop to the next index
				else // The value can simply be incremented, so break out of the loop
				{
					previousId[i] = characters[characters.IndexOf(previousId[i]) + 1];
					break;
				}
			}
			return previousId;
		}
	}
}
