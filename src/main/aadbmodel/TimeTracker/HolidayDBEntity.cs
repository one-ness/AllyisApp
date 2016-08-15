//------------------------------------------------------------------------------
// <copyright file="HolidayDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.TimeTracker
{
	/// <summary>
	/// Represents a holiday for an organization.
	/// </summary>
	public class HolidayDBEntity : BasePoco
	{
		private int pHolidayId;
		private string pHolidayName;
		private int pOrganizationId;
		private DateTime pDate;
		private DateTime pCreatedUTC;
		private DateTime pModifiedUTC;

		/// <summary>
		/// Gets or sets the HolidayId.
		/// </summary>
		public int HolidayId
		{
			get
			{
				return this.pHolidayId;
			}

			set
			{
				this.ApplyPropertyChange<HolidayDBEntity, int>(ref this.pHolidayId, (HolidayDBEntity x) => x.HolidayId, value);
			}
		}

		/// <summary>
		/// Gets or sets the HolidayName.
		/// </summary>
		public string HolidayName
		{
			get
			{
				return pHolidayName;
			}

			set
			{
				this.ApplyPropertyChange<HolidayDBEntity, string>(ref this.pHolidayName, (HolidayDBEntity x) => x.HolidayName, value);
			}
		}

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
				this.ApplyPropertyChange<HolidayDBEntity, int>(ref this.pOrganizationId, (HolidayDBEntity x) => x.OrganizationId, value);
			}
		}

		/// <summary>
		/// Gets or sets the Date.
		/// </summary>
		public DateTime Date
		{
			get
			{
				return pDate;
			}

			set
			{
				this.ApplyPropertyChange<HolidayDBEntity, DateTime>(ref this.pDate, (HolidayDBEntity x) => x.Date, value);
			}
		}

		/// <summary>
		/// Gets or sets the CreatedUTC.
		/// </summary>
		public DateTime CreatedUTC
		{
			get
			{
				return pCreatedUTC;
			}

			set
			{
				this.ApplyPropertyChange<HolidayDBEntity, DateTime>(ref this.pCreatedUTC, (HolidayDBEntity x) => x.CreatedUTC, value);
			}
		}

		/// <summary>
		/// Gets or sets the ModifiedUTC.
		/// </summary>
		public DateTime ModifiedUTC
		{
			get
			{
				return pModifiedUTC;
			}

			set
			{
				this.ApplyPropertyChange<HolidayDBEntity, DateTime>(ref this.pModifiedUTC, (HolidayDBEntity x) => x.ModifiedUTC, value);
			}
		}
	}
}
