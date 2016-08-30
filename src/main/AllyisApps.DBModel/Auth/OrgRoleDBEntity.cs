//------------------------------------------------------------------------------
// <copyright file="OrgRoleDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Represents the OrganizationRole table in the Database.
	/// </summary>
	public class OrgRoleDBEntity : BasePoco
	{
		private int pId = 0;
		private string pName;

		/// <summary>
		/// Gets or sets the OrgRoleID.
		/// </summary>
		public int OrgRoleId
		{
			get
			{
				Trace.WriteLine("GetTableOrgRoleId");
				return this.pId;
			}

			set
			{
				Trace.WriteLine("SetTableOrgRoleId");
				this.ApplyPropertyChange<OrgRoleDBEntity, int>(ref this.pId, (OrgRoleDBEntity x) => x.pId, value);
			}
		}

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		public string Name
		{
			get
			{
				Trace.WriteLine("GetTableOrgRoleName");
				return this.pName;
			}

			set
			{
				Trace.WriteLine("SetTableOrgRoleName");
				this.ApplyPropertyChange<OrgRoleDBEntity, string>(ref this.pName, (OrgRoleDBEntity x) => x.Name, value);
			}
		}
	}
}