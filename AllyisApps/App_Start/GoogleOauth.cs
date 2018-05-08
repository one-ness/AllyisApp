using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AllyisApps.Utilities;

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

		const string GoogleOAuthClientIdKey = "GoogleOAuthClientId";
		const string GoogleOAuthClientSecretKey = "GoogleOAuthClientSecret";

		static string OidcMetaDataDocumentJson;
		static dynamic OidcMetaDataDocument;

		/// <summary>
		/// google oauth init
		/// </summary>
		public static void Init()
		{
			// get the metadata document
			OidcMetaDataDocument = OidcUtility.GetOidcMetaDataDocument(GoogleOAuthMetaDataUrl, out OidcMetaDataDocumentJson);

			GoogleOAuthAuthorizationUrl = OidcMetaDataDocument.authorization_endpoint;

		}


	}
}