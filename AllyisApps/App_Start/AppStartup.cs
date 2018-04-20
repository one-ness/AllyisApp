//------------------------------------------------------------------------------
// <copyright file="AppStartup.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Security;
using AllyisApps.Core;
using AllyisApps.Services.Cache;

namespace AllyisApps
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

			// init services cache
			CacheContainer.Init(GlobalSettings.SqlConnectionString);

			// init msft oidc
			MsftOidc.Init();
		}
	}
}