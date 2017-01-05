//------------------------------------------------------------------------------
// <copyright file="LockDateDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.TimeTracker
{
    /// <summary>
    /// Represents a lock date for an organization.
    /// </summary>
    public class LockDateDBEntity
    {
        /// <summary>
        /// Gets or sets a quantity indicating whether to use a lock date.
        /// </summary>
        public bool LockDateUsed { get; set; }

        /// <summary>
        /// Gets or sets the lock date period (Days/Weeks/Months).
        /// </summary>
        public string LockDatePeriod { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the lock date period.
        /// </summary>
        public int LockDateQuantity { get; set; }
    }
}
