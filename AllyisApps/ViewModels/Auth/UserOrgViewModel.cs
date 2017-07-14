﻿//------------------------------------------------------------------------------
// <copyright file="OrganizationUserViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.ViewModels.Shared;
using System.Collections.Generic;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.Auth
{
    /// <summary>
    /// View Model used by the _OrgPanel
    /// </summary>
    public class UserOrgViewModel
    {
        /// <summary>
        /// Get or set UserInfor object for the model.
        /// </summary>
        public User UserInfo { get; set; }

        /// <summary>
        /// Get or set Organization subscription info.
        /// </summary>
        public OrgWithSubscriptionsForUserViewModel PanelInfo { get; set; }
    }
}