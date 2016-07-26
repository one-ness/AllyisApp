//------------------------------------------------------------------------------
// <copyright file="UserPermissionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;

namespace AllyisApps.BusinessObjects
{
    /// <summary>
	/// Object represnting a list of users and the actions to perform on them.
	/// </summary>
	public class UserPermissionsAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionsAction" /> class.
        /// </summary>
        public UserPermissionsAction()
        {
        }

        /// <summary>
        /// Gets or sets the list of selected users.
        /// </summary>
        public IEnumerable<TargetUser> SelectedUsers { get; set; }

        /// <summary>
        /// Gets or sets the actions to be performed.
        /// </summary>
        public PermissionsAction SelectedActions { get; set; }
    }    
}