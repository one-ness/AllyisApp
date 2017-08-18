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
		public const int MaxDocumentNameLength = 32;
		public const int MaxDocumentLinkLength = 100;

		private int applicationDocumentId;
		private int applicationId;
		private string documentLink;
		private string documentName;

		/// <summary>
		/// Gets or sets the ApplicationDocumentId.
		/// </summary>
		public int ApplicationDocumentId
		{
			get => applicationDocumentId;
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(ApplicationDocumentId), value, nameof(ApplicationDocumentId) + " must be greater than 0.");
				}
				applicationDocumentId = value;
			}
		}

		/// <summary>
		/// Gets or sets the application's ID.
		/// </summary>
		public int ApplicationId
		{
			get => applicationId;
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(ApplicationId), value, nameof(ApplicationId) + " must be greater than 0.");
				}
				applicationId = value;
			}
		}

		/// <summary>
		/// Gets or sets the DocumentLink.
		/// </summary>
		public string DocumentLink
		{
			get => documentLink;
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException(nameof(DocumentLink), nameof(DocumentLink) + " must not be null or empty.");
				}
				if (value.Length > MaxDocumentLinkLength)
				{
					throw new ArgumentOutOfRangeException(nameof(DocumentLink), value, nameof(DocumentLink) + " length must be less than " + MaxDocumentLinkLength + ".");
				}
				documentLink = value;
			}
		}

		/// <summary>
		/// Gets or sets the DocumentName.
		/// </summary>
		public string DocumentName
		{
			get => documentName;
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException(nameof(DocumentName), nameof(DocumentName) + " must not be null or empty.");
				}
				if (value.Length > MaxDocumentNameLength)
				{
					throw new ArgumentOutOfRangeException(nameof(DocumentName), value, nameof(DocumentName) + " length must be less than " + MaxDocumentNameLength + ".");
				}
				documentName = value;
			}
		}
	}
}
