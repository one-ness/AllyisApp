//------------------------------------------------------------------------------
// <copyright file="PayClassDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.TimeTracker
{
	/// <summary>
	/// Pay class.
	/// </summary>
	public class PayClassDBEntity : BasePoco
    {
        private int pPayClassID;
        private string pName;
        private int pOrganizationId;
        private DateTime pCreatedUTC;
        private DateTime pModifiedUTC;

        /// <summary>
        /// Gets or sets.
        /// </summary>
        public int PayClassID
        {
            get
            {
                return pPayClassID;
            }

            set
            {
                this.ApplyPropertyChange<PayClassDBEntity, int>(ref this.pPayClassID, (PayClassDBEntity x) => x.pPayClassID, value);
            }
        }

        /// <summary>
        /// Gets or sets.
        /// </summary>
        public string Name
        {
            get
            {
                return pName;
            }

            set
            {
                this.ApplyPropertyChange<PayClassDBEntity, string>(ref this.pName, (PayClassDBEntity x) => x.Name, value);
            }
        }

        /// <summary>
        /// Gets or sets.
        /// </summary>
        public int OrganizationId
        {
            get
            {
                return pOrganizationId;
            }

            set
            {
                this.ApplyPropertyChange<PayClassDBEntity, int>(ref this.pOrganizationId, (PayClassDBEntity x) => x.pOrganizationId, value);
            }
        }

        /// <summary>
        /// Gets or sets.
        /// </summary>
        public DateTime CreatedUTC
        {
            get
            {
                return pCreatedUTC;
            }

            set
            {
                this.ApplyPropertyChange<PayClassDBEntity, DateTime>(ref this.pCreatedUTC, (PayClassDBEntity x) => x.pCreatedUTC, value);
            }
        }

        /// <summary>
        /// Gets or sets.
        /// </summary>
        public DateTime ModifiedUTC
        {
            get
            {
                return pModifiedUTC;
            }

            set
            {
                this.ApplyPropertyChange<PayClassDBEntity, DateTime>(ref this.pModifiedUTC, (PayClassDBEntity x) => x.pModifiedUTC, value);
            }
        }
    }
}
