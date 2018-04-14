using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using System.Web.Helpers;
using System.IO;
using System.Text;

namespace AllyisApps
{
	/// <summary>
	/// contains helpers for microsoft open id connect protocol
	/// described here: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-protocols-openid-connect-code
	/// </summary>
	public static class MsftOidc
	{
		static string OidcMetaDataJson;
		static dynamic OidcMetaData;
		//static bool fullMetadataDocumentObtained;

		// open id connect metadata document is here: https://login.microsoftonline.com/{tenant}/.well-known/openid-configuration
		// but, we use common metadata url since we are configured for multi-tenant
		const string metadataUrl = "https://login.microsoftonline.com/common/.well-known/openid-configuration";
		// default authorization url is here: https://login.microsoftonline.com/{tenant}/oauth2/authorize
		// but, we use common authorization url
		const string MsftOidcCommonAuthorizationUrl = "https://login.microsoftonline.com/common/oauth2/authorize";
		/// <summary>
		/// init the msft oidc
		/// </summary>
		public static void Init()
		{
			try
			{
				// obtain the oidc metadata document
				var req = WebRequest.Create(metadataUrl);
				using (var res = req.GetResponse())
				{
					using (var stream = res.GetResponseStream())
					{
						using (var reader = new StreamReader(stream))
						{
							// get metadata json string
							OidcMetaDataJson = reader.ReadToEnd();

							// convert to object
							OidcMetaData = System.Web.Helpers.Json.Decode(OidcMetaDataJson);

							// check if authorization end point is available
							if (string.IsNullOrWhiteSpace(OidcMetaData.authorization_endpoint))
							{
								// no, create default
								OidcMetaData.authorization_endpoint = MsftOidcCommonAuthorizationUrl;
							}
							else
							{
								// yes, assume other information in the document is also available.
								//fullMetadataDocumentObtained = true;
							}
						}
					}
				}
			}
			catch
			{
				// something went wrong. create the metadata object with just the common authorize url
				OidcMetaData.authorization_endpoint = MsftOidcCommonAuthorizationUrl;
			}
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
		const string clientId = "a4d8d348-8614-4f13-b221-0863616b8d4c"; // application id of our app in our tenant (which is our azure ad in our subscription)
		const string responseType = "id_token%20code"; // id_token is mandatory for oidc signin
		const string scope = "openid%20profile%20email%20address%20phone"; // openid is mandatory for oidc signin
		/// <summary>
		/// get the open id authorization url
		/// </summary>
		public static string GetMsftOidcLoginUrl(string returnUrl)
		{
			// TODO: store this guid in the database as an open connection, then verify in the returnUrl action, mark it as complete
			// this can mitigate xsrf and replay attacks.
			var xsrfAndReplayMitigation = Guid.NewGuid().ToString();
			string url = "{0}?client_id={1}&response_type={2}&redirect_uri={3}&response_mode=form_post&scope={4}&state={5}&nonce={6}";
			return string.Format(url, MsftOidcCommonAuthorizationUrl, clientId, responseType, HttpUtility.UrlEncode(returnUrl), scope, xsrfAndReplayMitigation, xsrfAndReplayMitigation);
		}
	}
}
