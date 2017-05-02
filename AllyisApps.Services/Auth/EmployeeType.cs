//------------------------------------------------------------------------------
// <copyright file="EmployeeType.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
    /// <summary>
    /// Type of the employee of an organization.
    /// </summary>
    public enum EmployeeType : int
    {
        /// <summary>
        /// Salaried employee.
        /// </summary>
        Salaried = 1,

        /// <summary>
        /// Hourly employee.
        /// </summary>
        Hourly = 2
    }
}
