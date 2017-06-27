//------------------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	[Authorize]
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomerController"/> class.
		/// </summary>
		public CustomerController()
		{
		}

    }
}
