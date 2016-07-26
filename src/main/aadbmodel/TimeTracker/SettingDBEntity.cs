//------------------------------------------------------------------------------
// <copyright file="SettingDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.TimeTracker
{
	/// <summary>
	/// The POCO for the Settings table.
	/// </summary>
	public class SettingDBEntity : BasePoco
	{
		private int pOrganizationId;
		private int pStartOfWeek;
		private int pOvertimeHours;
		private string pOvertimePeriod;
		private decimal pOvertimeMultiplier;

		/// <summary>
		/// Gets or sets the OrganizationId.
		/// </summary>
		public int OrganizationId
		{
			get
			{
				return pOrganizationId;
			}

			set
			{
				this.ApplyPropertyChange<SettingDBEntity, int>(ref this.pOrganizationId, (SettingDBEntity x) => x.OrganizationId, value);
			}
		}

		/// <summary>
		/// Gets or sets the StartOfWeek.
		/// </summary>
		public int StartOfWeek
		{
			get
			{
				return pStartOfWeek;
			}

			set
			{
				this.ApplyPropertyChange<SettingDBEntity, int>(ref this.pStartOfWeek, (SettingDBEntity x) => x.StartOfWeek, value);
			}
		}

		/// <summary>
		/// Gets or sets the OvertimeHours.
		/// </summary>
		public int OvertimeHours
		{
			get
			{
				return pOvertimeHours;
			}

			set
			{
				this.ApplyPropertyChange<SettingDBEntity, int>(ref this.pOvertimeHours, (SettingDBEntity x) => x.OvertimeHours, value);
			}
		}

		/// <summary>
		/// Gets or sets the OvertimePeriod.
		/// </summary>
		public string OvertimePeriod
		{
			get
			{
				return pOvertimePeriod;
			}

			set
			{
				this.ApplyPropertyChange<SettingDBEntity, string>(ref this.pOvertimePeriod, (SettingDBEntity x) => x.OvertimePeriod, value);
			}
		}

		/// <summary>
		/// Gets or sets the OvertimeMultiplier.
		/// </summary>
		public decimal OvertimeMultiplier
		{
			get
			{
				return pOvertimeMultiplier;
			}

			set
			{
				this.ApplyPropertyChange<SettingDBEntity, decimal>(ref this.pOvertimeMultiplier, (SettingDBEntity x) => x.OvertimeMultiplier, value);
			}
		}
	}
}
