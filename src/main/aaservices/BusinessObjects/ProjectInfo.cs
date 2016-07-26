//------------------------------------------------------------------------------
// <copyright file="ProjectInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// An object for keeping track of all the info related to a given project.
	/// </summary>
	public class ProjectInfo
    {
        /// <summary>
        /// Gets or sets the projects ID number.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the id of the organization associated with the project.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the id of the customer associated with the project.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the project name. 
        /// </summary>
        public string Name { get; set; }
    }
}
