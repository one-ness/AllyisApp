//------------------------------------------------------------------------------
// <copyright file="ApplicationDocument.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// An ApplicationDocument object, holding all info directly related to the ApplicationDocument.  Also performs basic field validation.
	/// </summary>
	public class ApplicationDocument
	{
		private const int MaxDocumentNameLength = 32;
		private const int MaxDocumentLinkLength = 100;

		private int applicationDocumentId;
		private int applicationId;
		private string documentLink;
		private string documentName;

		/// <summary>
		/// The constructor used when GETTING or UPDATING info from the backend (includes ApplicationDocumentId)
		/// </summary>
		/// <param name="applicationDocumentId">The application document id.</param>
		/// <param name="applicationId">The application id that the application document belongs to.</param>
		/// <param name="documentLink">The link to the actual document, wherever the document is actually stored (not in our db).</param>
		/// <param name="documentName">The name of the document</param>
		public ApplicationDocument(
			int applicationDocumentId,
			int applicationId,
			string documentLink,
			string documentName)
		{
			ApplicationDocumentId = applicationDocumentId;
			ApplicationId = applicationId;
			DocumentLink = documentLink;
			DocumentName = documentName;
		}

		/// <summary>
		/// The constructor used when INSERTING into the backend (excludes ApplicationDocumentId)
		/// </summary>
		/// <param name="applicationId">The application id that the application document belongs to.</param>
		/// <param name="documentLink">The link to the actual document, wherever the document is actually stored (not in our db).</param>
		/// <param name="documentName">The name of the document</param>
		public ApplicationDocument(
			int applicationId,
			string documentLink,
			string documentName)
		{
			ApplicationId = applicationId;
			DocumentLink = documentLink;
			DocumentName = documentName;
		}

		/// <summary>
		/// Gets or sets the ApplicationDocumentId.
		/// </summary>
		public int ApplicationDocumentId
		{
			get
			{
				return applicationDocumentId;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ApplicationDocumentId", value, "ApplicationDocumentId must be greater than 0.");
				}
				applicationDocumentId = value;
			}
		}

		/// <summary>
		/// Gets or sets the application's ID.
		/// </summary>
		public int ApplicationId
		{
			get
			{
				return applicationId;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ApplicationId", value, "ApplicationId must be greater than 0.");
				}
				applicationId = value;
			}
		}

		/// <summary>
		/// Gets or sets the DocumentLink.
		/// </summary>
		public string DocumentLink
		{
			get
			{
				return documentLink;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("DocumentLink", "DocumentLink must not be null or empty.");
				}
				else if (value.Length > MaxDocumentLinkLength)
				{
					throw new ArgumentOutOfRangeException("DocumentLink", value, "DocumentLink length must be less than " + MaxDocumentLinkLength + ".");
				}
				documentLink = value;
			}
		}

		/// <summary>
		/// Gets or sets the DocumentName.
		/// </summary>
		public string DocumentName
		{
			get
			{
				return documentName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("DocumentName", "DocumentName must not be null or empty.");
				}
				else if (value.Length > MaxDocumentNameLength)
				{
					throw new ArgumentOutOfRangeException("DocumentName", value, "DocumentName length must be less than " + MaxDocumentNameLength + ".");
				}
				documentName = value;
			}
		}
	}
}
