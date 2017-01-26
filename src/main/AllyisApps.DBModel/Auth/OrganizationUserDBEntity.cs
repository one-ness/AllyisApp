//------------------------------------------------------------------------------
// <copyright file="OrganizationUserDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Represents the OrganizationUser Table.
	/// </summary>
	public class OrganizationUserDBEntity : BasePoco
	{
		private int pUserId;
		private int pOrganizationId;
		private int pOrgRoleId;
		private DateTime pCreatedUTC;
		private string pEmployeeId;
        private string pEmail;

		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId
		{
			get
			{
				return this.pUserId;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationUserDBEntity, int>(ref this.pUserId, (OrganizationUserDBEntity x) => x.UserId, value);
			}
		}

		/// <summary>
		/// Gets or sets OrganizationId.
		/// </summary>
		public int OrganizationId
		{
			get
			{
				return this.pOrganizationId;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationUserDBEntity, int>(ref this.pOrganizationId, (OrganizationUserDBEntity x) => x.OrganizationId, value);
			}
		}

		/// <summary>
		/// Gets or sets OrgRoleId.
		/// </summary>
		public int OrgRoleId
		{
			get
			{
				return this.pOrgRoleId;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationUserDBEntity, int>(ref this.pOrgRoleId, (OrganizationUserDBEntity x) => x.OrgRoleId, value);
			}
		}

		/// <summary>
		/// Gets the OrganizationName associated with this orguser.
		/// </summary>
		public string OrganizationName
		{
			get
			{
				OrganizationDBEntity org = DBHelper.Instance.GetOrganization(this.OrganizationId);
				return org.Name;
			}
		}

		/// <summary>
		/// Gets or sets the date this user was added to the organization.
		/// </summary>
		public DateTime CreatedUTC
		{
			get
			{
				return this.pCreatedUTC;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationUserDBEntity, DateTime>(ref this.pCreatedUTC, (OrganizationUserDBEntity x) => x.CreatedUTC, value);
			}
		}

		/// <summary>
		/// Gets or sets the employee id for this user.
		/// </summary>
		public string EmployeeId
		{
			get
			{
				return this.pEmployeeId;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationUserDBEntity, string>(ref this.pEmployeeId, (OrganizationUserDBEntity x) => x.pEmployeeId, value);
			}
        }

        /// <summary>
        /// Gets or sets the email for this user.
        /// </summary>
        public string Email
        {
            get
            {
                return this.pEmail;
            }

            set
            {
                this.ApplyPropertyChange<OrganizationUserDBEntity, string>(ref this.pEmail, (OrganizationUserDBEntity x) => x.pEmail, value);
            }
        }
    }
}