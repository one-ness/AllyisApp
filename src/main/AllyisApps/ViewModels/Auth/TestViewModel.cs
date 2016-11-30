﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
    /// <summary>
    /// A simple view model for passing desired information to the view for testing.
    /// </summary>
    public class TestViewModel
    {
        /// <summary>
        /// The value of the SENDGRID_APIKEY environment variable in the current deployment.
        /// </summary>
        public string apiKey { get; set; }
    }
}