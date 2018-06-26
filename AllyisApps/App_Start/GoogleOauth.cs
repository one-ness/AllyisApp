using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AllyisApps.Utilities;
using System.Configuration;

namespace AllyisApps
{
	/// <summary>
	/// google oauth connect information (also for open id)
	/// </summary>
	public static class GoogleOAuth
	{
		/// <summary>
		/// oauth cleint id
		/// </summary>
		public static string GoogleOAuthClientId { get; set; }

		/// <summary>
		/// oauth client secret
		/// </summary>
		public static string GoogleOAuthClientSecret { get; set; }

		/// <summary>
		/// metadata url that is hardcoded as per google instruction
		/// </summary>
		public static string GoogleOAuthMetaDataUrl = "https://accounts.google.com/.well-known/openid-configuration";

		/// <summary>
		/// authorization end point
		/// </summary>
		public static string GoogleOAuthAuthorizationUrl { get; set; }

		private const string googleOAuthClientIdKey = "GoogleOAuthClientId";
		private const string googleOAuthClientSecretKey = "GoogleOAuthClientSecret";

		private static string oidcMetaDataDocumentJson;
		private static dynamic oidcMetaDataDocument;

		/// <summary>
		/// google oauth init
		/// </summary>
		public static void Init()
		{
			// get the msft oidc settings from config
			string temp = ConfigurationManager.AppSettings[googleOAuthClientIdKey];
			if (string.IsNullOrWhiteSpace(temp)) throw new ArgumentNullException(string.Format("{0} setting not found in config.", nameof(googleOAuthClientIdKey)));

			GoogleOAuthClientId = temp.Trim();

			// NOTE: endpoints can be obtained in the azure portal under App Registrations --> End Points
			// we can also form them using the tenant name
			temp = ConfigurationManager.AppSettings[googleOAuthClientSecretKey];
			if (string.IsNullOrWhiteSpace(temp)) throw new ArgumentNullException(string.Format("{0} setting not found in config.", nameof(googleOAuthClientSecretKey)));

			GoogleOAuthClientSecret = temp.Trim();

			// get the metadata document
			try
			{
				oidcMetaDataDocument = OidcUtility.GetOidcMetaDataDocument(GoogleOAuthMetaDataUrl, out oidcMetaDataDocumentJson);
				GoogleOAuthAuthorizationUrl = oidcMetaDataDocument.authorization_endpoint;
			}
			catch (Exception ex)
			{
				throw new Exception("Unexpected error: Unable to initialize for Google Login.", ex);
			}
		}

		/*
		* sample login url
		GET https://accounts.google.com/o/oauth2/v2/auth?
			client_id=424911365001.apps.googleusercontent.com&
			response_type=code&
			scope=openid%20email%20profile&
			redirect_uri=https://oauth2-login-demo.example.com/code&
			state=security_token%3D138r5719ru3e1%26url%3Dhttps://oauth2-login-demo.example.com/myHome&
			login_hint=jsmith@example.com&
			openid.realm=example.com&
			nonce=0394852-3190485-2490358&
			hd=example.com
		 */
		private const string ResponseType = "id_token%20code%20token"; // id_token is mandatory for oidc signin
		private const string Scope = "openid%20profile%20email%20address%20phone"; // openid is mandatory for oidc signin
		/// <summary>
		/// get the open id login url
		/// </summary>
		public static string GetGoogleOAuthLoginUrl(string returnUrl)
		{
			// TODO: store this guid in the database as an open connection, then verify in the returnUrl action, mark it as complete
			// this can mitigate xsrf and replay attacks.
			var xsrfAndReplayMitigation = Guid.NewGuid().ToString();
			string url = "{0}?client_id={1}&response_type={2}&redirect_uri={3}&response_mode=form_post&scope={4}&state={5}&nonce={6}&hd=*";
			return string.Format(url, GoogleOAuthAuthorizationUrl, GoogleOAuthClientId, ResponseType, returnUrl, Scope, xsrfAndReplayMitigation, xsrfAndReplayMitigation);
		}
	}
}