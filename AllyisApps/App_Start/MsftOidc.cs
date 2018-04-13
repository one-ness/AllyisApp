using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using System.Web.Helpers;
using System.IO;

namespace AllyisApps
{
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
	/// contains helpers for microsoft open id connect protocol
	/// described here: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-protocols-openid-connect-code
	/// </summary>
	public static class MsftOidc
	{
		static string OidcMetaDataJson;
		static dynamic OidcMetaData;

		// open id connect metadata document is here: https://login.microsoftonline.com/{tenant}/.well-known/openid-configuration
		// but, we use common metadata url since we are configured for multi-tenant
		const string metadataUrl = "https://login.microsoftonline.com/common/.well-known/openid-configuration";
		// default authorization url
		const string DefaultMsftOidcCommonAuthorizationUrl = "https://login.microsoftonline.com/{tenant}/oauth2/authorize";
		/// <summary>
		/// init the msft oidc
		/// </summary>
		public static void Init()
		{
			try
			{
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
								OidcMetaData.authorization_endpoint = DefaultMsftOidcCommonAuthorizationUrl;
							}
						}
					}
				}
			}
			catch
			{
				// something went wrong. create the metadata object with just the common authorize url
				OidcMetaData.authorization_endpoint = DefaultMsftOidcCommonAuthorizationUrl;
			}
		}

		/// <summary>
		/// get the open id authorization url
		/// </summary>
		public static string GetMsftOidcAuthorizationUrl()
		{
			return OidcMetaData.authorization_endpoint;
		}
	}
}
