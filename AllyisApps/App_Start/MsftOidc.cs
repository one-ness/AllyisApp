using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using System.Web.Helpers;
using System.IO;
using System.Text;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


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
		public static Guid AadAppId { get; set; }

		/// <summary>
		/// Tenant name as registered in our Azure subscription. If we use the name "common"
		/// it will allow all office 365 / azure ad users to login to our app
		/// </summary>
		public static string AadTenantName { get; set; }

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

		static string OidcMetaDataJson;
		const string aadAppIdKey = "AadAppId";
		const string aadTenantNameKey = "AadTenantName";

		/// <summary>
		/// init the msft oidc
		/// </summary>
		public static void Init()
		{
			// get the msft oidc settings from config
			string temp = ConfigurationManager.AppSettings[aadAppIdKey];
			if (string.IsNullOrWhiteSpace(temp)) throw new ArgumentNullException("AadAppId setting not found in config.");

			temp = temp.Trim();
			AadAppId = new Guid(temp);

			// NOTE: endpoints can be obtained in the azure portal under App Registrations --> End Points
			// we can also form them using the tenant name
			temp = ConfigurationManager.AppSettings[aadTenantNameKey];
			if (string.IsNullOrWhiteSpace(temp)) throw new ArgumentNullException("AadTenantName setting not found in config.");

			AadTenantName = temp.Trim();
			MsftOidcAuthority = string.Format("https://login.microsoftonline.com/{0}/v2.0", AadTenantName);
			MsftOidcMetaDataUrl = string.Format("https://login.microsoftonline.com/{0}/.well-known/openid-configuration", AadTenantName);
			MsftOidcAuthorizationUrl = string.Format("https://login.microsoftonline.com/{0}/oauth2/authorize", AadTenantName);
		}

		/// <summary>
		/// some method
		/// </summary>
		public static dynamic GetMsftOidcMetaDataDocument()
		{
			dynamic result = new object();
			try
			{
				// obtain the oidc metadata document
				var req = WebRequest.Create(MsftOidcMetaDataUrl);
				using (var res = req.GetResponse())
				{
					using (var stream = res.GetResponseStream())
					{
						using (var reader = new StreamReader(stream))
						{
							// get metadata json string
							OidcMetaDataJson = reader.ReadToEnd();

							// convert to object
							result = System.Web.Helpers.Json.Decode(OidcMetaDataJson);
						}
					}
				}
			}
			catch
			{
				// something went wrong. create the metadata object with just the common authorize url
				result.authorization_endpoint = MsftOidcAuthorizationUrl;
			}

			return result;
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
		/// <summary>
		/// id_token received in the post from the authorization provider
		/// </summary>
		public const string IdTokenKey = "id_token"; // we ask for id_token in response type and should get this back from the authorization provider
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
			return string.Format(url, MsftOidcAuthorizationUrl, AadAppId, responseType, HttpUtility.UrlEncode(returnUrl), scope, xsrfAndReplayMitigation, xsrfAndReplayMitigation);
		}

		/// <summary>
		/// decodes the given id token. id token is of the form base64(header + "." + claims + "." + signature)
		/// signature is formed by base64(HashHMAC(publickey, header + "." + claims)
		/// this is based on jwt spec
		/// </summary>
		public static string DecodeIdToken(string idtoken)
		{
			if (string.IsNullOrWhiteSpace(idtoken)) throw new ArgumentNullException(nameof(idtoken));

			var resultJson = string.Empty;
			// split on .
			var tokens = idtoken.Split('.');
			if (tokens.Length == 3)
			{
				var claimsstr = tokens[1];
				if (!string.IsNullOrWhiteSpace(claimsstr))
				{
					// base64 string should always be a multiple of 4
					int rem = claimsstr.Length % 4;
					if (rem != 0)
					{
						var filler = new string('=', rem);
						StringBuilder sb = new StringBuilder();
						sb.Append(claimsstr);
						sb.Append(filler);
						claimsstr = sb.ToString();
					}

					var jsonBytes = Convert.FromBase64String(claimsstr);
					resultJson = Encoding.UTF8.GetString(jsonBytes);
				}
			}

			return resultJson;
		}
	}
}
