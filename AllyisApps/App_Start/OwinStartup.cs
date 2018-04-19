using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin;
using Owin;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Notifications;

[assembly: OwinStartup(typeof(AllyisApps.Owin.Startup))]
namespace AllyisApps.Owin
{
	/// <summary>
	/// startup class for Owin
	/// </summary>
	public class Startup
	{
	}
}