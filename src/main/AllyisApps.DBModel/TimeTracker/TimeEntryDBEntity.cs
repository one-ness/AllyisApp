//------------------------------------------------------------------------------
// <copyright file="TimeEntryDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.TimeTracker
{
	/// <summary>
	/// Represents an entry of time data for TimeTracker.
	/// </summary>
	public class TimeEntryDBEntity : BasePoco
	{
		private int pTimeEntryId;
		private int pUserId;
		private string pFirstName;
		private string pLastName;
        private string pEmail;
        private string pEmployeeId;
		private int pProjectId;
		private int pPayClassId;
        private string pPayClassName;
		private DateTime pDate;
		private float pDuration;
		private string pDescription;
		private int pApprovalState;
		private bool pLockSaved;
		private bool pModSinceApproval;

		/// <summary>
		/// Gets or sets the TimeEntryId.
		/// </summary>
		public int TimeEntryId
		{
			get
			{
				return this.pTimeEntryId;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, int>(ref this.pTimeEntryId, (TimeEntryDBEntity x) => x.TimeEntryId, value);
			}
		}

		/// <summary>
		/// Gets or sets the UserId.
		/// </summary>
		public int UserId
		{
			get
			{
				return this.pUserId;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, int>(ref this.pUserId, (TimeEntryDBEntity x) => x.UserId, value);
			}
		}

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		public string FirstName
		{
			get
			{
				return this.pFirstName;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, string>(ref this.pFirstName, (TimeEntryDBEntity x) => x.FirstName, value);
			}
		}

		/// <summary>
		/// Gets or sets the Last name.
		/// </summary>
		public string LastName
		{
			get
			{
				return this.pLastName;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, string>(ref this.pLastName, (TimeEntryDBEntity x) => x.LastName, value);
			}
        }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public string Email
        {
            get
            {
                return this.pEmail;
            }

            set
            {
                this.ApplyPropertyChange<TimeEntryDBEntity, string>(ref this.pEmail, (TimeEntryDBEntity x) => x.Email, value);
            }
        }

        /// <summary>
        /// Gets or sets the EmployeeId.
        /// </summary>
        public string EmployeeId
        {
            get
            {
                return this.pEmployeeId;
            }

            set
            {
                this.ApplyPropertyChange<TimeEntryDBEntity, string>(ref this.pEmployeeId, (TimeEntryDBEntity x) => x.EmployeeId, value);
            }
        }

        /// <summary>
        /// Gets or sets the ProjectId.
        /// </summary>
        public int ProjectId
		{
			get
			{
				return this.pProjectId;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, int>(ref this.pProjectId, (TimeEntryDBEntity x) => x.ProjectId, value);
			}
		}

		/// <summary>
		/// Gets or sets the PayClassId.
		/// </summary>
		public int PayClassId
		{
			get
			{
				return this.pPayClassId;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, int>(ref this.pPayClassId, (TimeEntryDBEntity x) => x.pPayClassId, value);
			}
        }

        /// <summary>
        /// Gets or sets the pay class name.
        /// </summary>
        public string PayClassName
        {
            get
            {
                return this.pPayClassName;
            }

            set
            {
                this.ApplyPropertyChange<TimeEntryDBEntity, string>(ref this.pPayClassName, (TimeEntryDBEntity x) => x.PayClassName, value);
            }
        }

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        public DateTime Date
		{
			get
			{
				return this.pDate;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, DateTime>(ref this.pDate, (TimeEntryDBEntity x) => x.Date, value);
			}
		}

		/// <summary>
		/// Gets or sets the Duration.
		/// </summary>
		public float Duration
		{
			get
			{
				return this.pDuration;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, float>(ref this.pDuration, (TimeEntryDBEntity x) => x.Duration, value);
			}
		}

		/// <summary>
		/// Gets or sets the Description.
		/// </summary>
		public string Description
		{
			get
			{
				return this.pDescription;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, string>(ref this.pDescription, (TimeEntryDBEntity x) => x.Description, value);
			}
		}

		/// <summary>
		/// Gets or sets the approval state.
		/// </summary>
		public int ApprovalState
		{
			get
			{
				return this.pApprovalState;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, int>(ref this.pApprovalState, (TimeEntryDBEntity x) => x.ApprovalState, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the saved approval lock.
		/// </summary>
		public bool LockSaved
		{
			get
			{
				return this.pLockSaved;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, bool>(ref this.pLockSaved, (TimeEntryDBEntity x) => x.LockSaved, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the data has changed since being approved or dissaproved.
		/// </summary>
		public bool ModSinceApproval
		{
			get
			{
				return this.pModSinceApproval;
			}

			set
			{
				this.ApplyPropertyChange<TimeEntryDBEntity, bool>(ref this.pModSinceApproval, (TimeEntryDBEntity x) => x.ModSinceApproval, value);
			}
		}
	}
}