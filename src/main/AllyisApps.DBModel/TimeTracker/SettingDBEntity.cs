﻿//------------------------------------------------------------------------------
// <copyright file="SettingDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
        private bool pLockDateUsed;
        private string pLockDatePeriod;
        private int pLockDateQuantity;

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

        /// <summary>
        /// Gets or sets a value indicating whether to use a lock date.
        /// </summary>
        public bool LockDateUsed
        {
            get
            {
                return pLockDateUsed;
            }

            set
            {
                this.ApplyPropertyChange<SettingDBEntity, bool>(ref this.pLockDateUsed, (SettingDBEntity x) => x.LockDateUsed, value);
            }
        }

        /// <summary>
        /// Gets or sets the LockDatePeriod.
        /// </summary>
        public string LockDatePeriod
        {
            get
            {
                return pLockDatePeriod;
            }

            set
            {
                this.ApplyPropertyChange<SettingDBEntity, string>(ref this.pLockDatePeriod, (SettingDBEntity x) => x.LockDatePeriod, value);
            }
        }

        /// <summary>
        /// Gets or sets the LockDateQuantity.
        /// </summary>
        public int LockDateQuantity
        {
            get
            {
                return pLockDateQuantity;
            }

            set
            {
                this.ApplyPropertyChange<SettingDBEntity, int>(ref this.pLockDateQuantity, (SettingDBEntity x) => x.LockDateQuantity, value);
            }
        }
    }
}