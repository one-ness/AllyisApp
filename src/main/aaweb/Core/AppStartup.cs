//------------------------------------------------------------------------------
// <copyright file="AppStartup.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Security;

using AllyisApps.DBModel;

namespace AllyisApps.Core
{
	/// <summary>
	/// Application start up activities.
	/// </summary>
	public static class AppStartup
	{
		/// <summary>
		/// Application initialization.
		/// </summary>
		public static void Init()
		{
			// init global settings
			GlobalSettings.Init();

			// init auth
			FormsAuthentication.Initialize();

			// init db
			DBModelStartup.Init();

			// init stripe
			Stripe.StripeConfiguration.SetApiKey("sk_test_6Z1XooVuPiXjbn0DwndaHF8P");
		}
	}
}
