//------------------------------------------------------------------------------
// <copyright file="ManageController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Services;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Manage Controller.
	/// </summary>
	[Authorize]
	public partial class ManageController : BaseProductController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ManageController" /> class.
		/// </summary>
		public ManageController()
			: base(Service.GetProductIdByName(ProductNameKeyConstants.TimeTracker))
		{
		}
	}
}