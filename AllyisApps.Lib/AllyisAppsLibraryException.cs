//------------------------------------------------------------------------------
// <copyright file="AllyisAppsLibraryException.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Lib
{
	/// <summary>
	/// Allyis apps library exception.
	/// </summary>
	[Serializable]
	public class AllyisAppsLibraryException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AllyisAppsLibraryException"/> class.
		/// </summary>
		public AllyisAppsLibraryException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllyisAppsLibraryException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		public AllyisAppsLibraryException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllyisAppsLibraryException"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="innerException">The inner exception to pass along.</param>
		public AllyisAppsLibraryException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}