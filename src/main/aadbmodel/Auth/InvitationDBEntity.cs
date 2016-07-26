//------------------------------------------------------------------------------
// <copyright file="InvitationDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// The model for the invitations table. 
	/// </summary>
	public class InvitationDBEntity : BasePoco
	{
		private int pInvitationId;
		private string pEmail;
		private string pFirstName;
		private string pLastName;
		private DateTime pDateOfBirth;
		private int pOrganizationId;
		private string pAccessCode;
		private int pOrgRole;
		private int? pProjectId;

		/// <summary>
		/// Gets or sets the id of the invitation. 
		/// </summary>
		public int InvitationId
		{
			get
			{
				return this.pInvitationId;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, int>(ref this.pInvitationId, (InvitationDBEntity x) => x.InvitationId, value);
			}
		}

		/// <summary>
		/// Gets or sets the email address that the invitation is being sent to. 
		/// </summary>
		public string Email
		{
			get
			{
				return this.pEmail;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, string>(ref this.pEmail, (InvitationDBEntity x) => x.Email, value);
			}
		}

		/// <summary>
		/// Gets compressed email.
		/// </summary>
		public string CompressedEmail
		{
			get
			{
				if (!string.IsNullOrEmpty(this.pEmail) && this.pEmail.Length > 50)
				{
					string cemail = string.Format("{0}...{1}", this.pEmail.Substring(0, 20), this.pEmail.Substring(pEmail.Length - 15));
					return cemail;
				}
				else
				{
					return this.pEmail;
				}
			}
		}

		/// <summary>
		/// Gets or sets the first name of the recipiant. 
		/// </summary>
		public string FirstName
		{
			get
			{
				return this.pFirstName;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, string>(ref this.pFirstName, (InvitationDBEntity x) => x.FirstName, value);
			}
		}

		/// <summary>
		/// Gets or sets the last name of the recipiant. 
		/// </summary>
		public string LastName
		{
			get
			{
				return this.pLastName;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, string>(ref this.pLastName, (InvitationDBEntity x) => x.LastName, value);
			}
		}

		/// <summary>
		/// Gets or sets the birthday of the recipiant. 
		/// </summary>
		public DateTime DateOfBirth
		{
			get
			{
				return this.pDateOfBirth;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, DateTime>(ref this.pDateOfBirth, (InvitationDBEntity x) => x.DateOfBirth, value);
			}
		}

		/// <summary>
		/// Gets or sets the id of the inviting organization. 
		/// </summary>
		public int OrganizationId
		{
			get
			{
				return this.pOrganizationId;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, int>(ref this.pOrganizationId, (InvitationDBEntity x) => x.OrganizationId, value);
			}
		}

		/// <summary>
		/// Gets or sets the access code associated with the invitation. 
		/// </summary>
		public string AccessCode
		{
			get
			{
				return this.pAccessCode;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, string>(ref this.pAccessCode, (InvitationDBEntity x) => x.AccessCode, value);
			}
		}

		/// <summary>
		/// Gets or sets the Project for which an user is assigned.
		/// </summary>
		public int? ProjectId
		{
			get
			{
				return this.pProjectId;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, int?>(ref this.pProjectId, (InvitationDBEntity x) => x.ProjectId, value);
			}
		}

		/// <summary>
		/// Gets or sets the id of the org role the user will be assigned. 
		/// </summary>
		public int OrgRole
		{
			get
			{
				return this.pOrgRole;
			}

			set
			{
				this.ApplyPropertyChange<InvitationDBEntity, int>(ref this.pOrgRole, (InvitationDBEntity x) => x.OrgRole, value);
			}
		}
	}
}
