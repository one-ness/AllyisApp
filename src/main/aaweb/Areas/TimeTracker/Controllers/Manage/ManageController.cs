//------------------------------------------------------------------------------
// <copyright file="ManageController.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using System.Web.Mvc;

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
			: base(Services.Crm.CrmService.GetProductIdByName(ProductNameKeyConstants.TimeTracker))
		{
		}
	}
}