
using AllyisApps.Core;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(AllyisApps.Owin.Startup))]
namespace AllyisApps.Owin
{
	/// <summary>
	/// startup class for Owin
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Configure OWIN to use OpenIdConnect 
		/// </summary>
		/// <param name="app"></param>
		public void Configuration(IAppBuilder app)
		{
			app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

			app.UseCookieAuthentication(new CookieAuthenticationOptions());
			app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions()
			{
				// Sets the ClientId, authority, RedirectUri as obtained from web.config
				ClientId = GlobalSettings.AadAppId.ToString(),
				Authority = GlobalSettings.MsftOidcAuthority,
				RedirectUri = string.Empty, // URL the user will come back to after successful signin, will be filled later in the controller
				PostLogoutRedirectUri = string.Empty, // PostLogoutRedirectUri is the page that users will be redirected to after sign-out, will be filled later in the controller
				Scope = "openid", // const string value, as specified in the open id connect protocol
				ResponseType = "id_token", // ResponseType is set to request the id_token - which contains basic information about the signed-in user, as specified in the open id connect protocol
				// ValidateIssuer set to false to allow personal and work accounts from any organization to sign in to your application
				// To only allow users from a single organizations, set ValidateIssuer to true and 'tenant' setting in web.config to the tenant name
				// To allow users from only a list of specific organizations, set ValidateIssuer to true and use ValidIssuers parameter 
				TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() { ValidateIssuer = false },
				// OpenIdConnectAuthenticationNotifications configures OWIN to send notification of failed authentications to OnAuthenticationFailed method
				Notifications = new OpenIdConnectAuthenticationNotifications()
				{
					AuthenticationFailed = OnAuthenticationFailed
				}
			});
		}

		/// <summary>
		/// Handle failed authentication requests by redirecting the user to the home page with an error in the query string
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
		{
			context.HandleResponse();
			context.Response.Redirect("/?errormessage=" + context.Exception.Message);
			return Task.FromResult(0);
		}
	}
}
