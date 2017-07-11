//------------------------------------------------------------------------------
// <copyright file="SkusAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /account/skus/id
		/// </summary>
		/// <param name="id">The organization id</param>
		public ActionResult Skus(int id)
		{

			this.AppService.CheckOrgAction(AppService.OrgAction.SubscribeToProduct, id);    //only org owner has permission
			//var infos = AppService.GetProductSubscriptionInfo(id, productId);

			SkusListViewModel model = new SkusListViewModel();

			return this.View("Skus", model);
		}		
	}
}
