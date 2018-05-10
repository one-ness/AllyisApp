﻿using System;
using System.Configuration;
using System.Web;


namespace AllyisApps
{
	/// <summary>
	/// contains helpers for microsoft open id connect protocol
	/// described here: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-protocols-openid-connect-code
	/// </summary>
	public static class MsftOidc
	{
		/// <summary>
		/// Azure Active Directory Application Id to be used for msft office 365 / azure ad login
		/// </summary>
		public static Guid MsftAadAppId { get; set; }

		/// <summary>
		/// Tenant name as registered in our Azure subscription. If we use the name "common"
		/// it will allow all office 365 / azure ad users to login to our app
		/// </summary>
		public static string MsftAadTenantName { get; set; }

		/// <summary>
		/// Authority is the URL for authority, composed by Azure Active Directory v2 endpoint and the tenant name
		/// https://login.microsoftonline.com/{tenant}/v2.0
		/// if we are multi-tenant, then tenant = "common"
		/// </summary>
		public static string MsftOidcAuthority { get; set; }

		/// <summary>
		/// msft oidc metadata url. open id connect metadata document is here: https://login.microsoftonline.com/{tenant}/.well-known/openid-configuration
		/// if we are multi-tenant, then tenant = "common"
		/// </summary>
		public static string MsftOidcMetaDataUrl { get; set; }

		/// <summary>
		/// msft oidc authorization url. default authorization url is here: https://login.microsoftonline.com/{tenant}/oauth2/authorize
		/// if we are multi-tenant, then tenant = "common"
		/// </summary>
		public static string MsftOidcAuthorizationUrl { get; set; }

		const string aadAppIdKey = "MsftAadAppId";
		const string aadTenantNameKey = "MsftAadTenantName";

		/// <summary>
		/// init the msft oidc
		/// </summary>
		public static void Init()
		{
			// get the msft oidc settings from config
			string temp = ConfigurationManager.AppSettings[aadAppIdKey];
			if (string.IsNullOrWhiteSpace(temp)) throw new ArgumentNullException(string.Format("{0} setting not found in config.", nameof(aadAppIdKey)));

			temp = temp.Trim();
			MsftAadAppId = new Guid(temp);

			// NOTE: endpoints can be obtained in the azure portal under App Registrations --> End Points
			// we can also form them using the tenant name
			temp = ConfigurationManager.AppSettings[aadTenantNameKey];
			if (string.IsNullOrWhiteSpace(temp)) throw new ArgumentNullException(string.Format("{0} setting not found in config.", nameof(aadTenantNameKey)));

			MsftAadTenantName = temp.Trim();
			MsftOidcAuthority = string.Format("https://login.microsoftonline.com/{0}/v2.0", MsftAadTenantName);
			MsftOidcMetaDataUrl = string.Format("https://login.microsoftonline.com/{0}/.well-known/openid-configuration", MsftAadTenantName);
			MsftOidcAuthorizationUrl = string.Format("https://login.microsoftonline.com/{0}/oauth2/authorize", MsftAadTenantName);
		}

		/*
		* sample login url
		GET https://login.microsoftonline.com/{tenant}/oauth2/authorize?
			client_id=6731de76-14a6-49ae-97bc-6eba6914391e
			&response_type=id_token
			&redirect_uri=http%3A%2F%2Flocalhost%2Fmyapp%2F
			&response_mode=form_post
			&scope=openid
			&state=12345
			&nonce=7362CAEA-9CA5-4B43-9BA3-34D7C303EBA7
		*/
		const string responseType = "id_token%20code"; // id_token is mandatory for oidc signin
		const string scope = "openid%20profile%20email%20address%20phone"; // openid is mandatory for oidc signin
		/// <summary>
		/// get the open id login url
		/// </summary>
		public static string GetMsftOidcLoginUrl(string returnUrl)
		{
			// TODO: store this guid in the database as an open connection, then verify in the returnUrl action, mark it as complete
			// this can mitigate xsrf and replay attacks.
			var xsrfAndReplayMitigation = Guid.NewGuid().ToString();
			string url = "{0}?client_id={1}&response_type={2}&redirect_uri={3}&response_mode=form_post&scope={4}&state={5}&nonce={6}";
			return string.Format(url, MsftOidcAuthorizationUrl, MsftAadAppId, responseType, HttpUtility.UrlEncode(returnUrl), scope, xsrfAndReplayMitigation, xsrfAndReplayMitigation);
		}
	}
}
