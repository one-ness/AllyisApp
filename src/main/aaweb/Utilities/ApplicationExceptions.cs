//-----------------------------------------------------------------------------
// <copyright file="ApplicationExceptions.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AllyisApps.Utilities
{
	/// <summary>
	/// Exception thrown by the Crypto object. Use this exception if
	/// there is only one error condition in a method.
	/// </summary>
	[DataContract]
	[Serializable]
	public sealed class AllyisAppsException : BaseException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AllyisAppsException"/> class.
		/// </summary>
		public AllyisAppsException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllyisAppsException"/> class. Overload 1.
		/// </summary>
		/// <param name="msg">Exception message.</param>
		public AllyisAppsException(string msg) : base(msg)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllyisAppsException"/> class. Overload 2.
		/// </summary>
		/// <param name="msg">Exception message.</param>
		/// <param name="innerException">Inner exception.</param>
		public AllyisAppsException(string msg, Exception innerException) : base(msg, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllyisAppsException"/> class. Overload 3.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private AllyisAppsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	/// <summary>
	/// Base exception class for the whole application.
	/// </summary>
	[DataContract]
	[Serializable]
	public class BaseException : System.Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseException"/> class. Default constructor.
		/// </summary>
		public BaseException() : base()
		{
		}

		/// <summary>
		///  Initializes a new instance of the <see cref="BaseException"/> class. Overload 1.
		/// </summary>
		/// <param name="message">The message.</param>
		public BaseException(string message) : base(message)
		{
		}

		/// <summary>
		///  Initializes a new instance of the <see cref="BaseException"/> class. Overload 2.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public BaseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		///  Initializes a new instance of the <see cref="BaseException"/> class. Overload 3.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Gets or sets the Custom error code that can be set by the application.
		/// </summary>
		public int ErrorCode { get; set; }

		/// <summary>
		/// Adds information about the exception to SerializationInfo. We are
		/// overriding this because we have added the following custom properties
		/// to the exception object:-
		/// 1. ErrorCode
		/// that needs to be serialized.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// call base.
			base.GetObjectData(info, context);

			// add ErrorCode to info store.
			info.AddValue("ErrorCode", this.ErrorCode);
		}
	}
}
