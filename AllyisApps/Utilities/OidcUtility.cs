using System;
using System.IO;
using System.Net;
using System.Text;

namespace AllyisApps.Utilities
{
	/// <summary>
	/// utilities for open id connect
	/// </summary>
	public static class OidcUtility
	{
		/// <summary>
		/// id_token received in the post from the authorization provider
		/// </summary>
		public const string IdTokenKey = "id_token"; // we ask for id_token in response type and should get this back from the authorization provider

		/// <summary>
		/// get the metadata document from the given url
		/// </summary>
		public static dynamic GetOidcMetaDataDocument(string url, out string jsonDocument)
		{
			if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

			jsonDocument = string.Empty;
			dynamic result = null;
			try
			{
				// obtain the oidc metadata document
				var req = WebRequest.Create(url);
				using (var res = req.GetResponse())
				{
					using (var stream = res.GetResponseStream())
					{
						using (var reader = new StreamReader(stream))
						{
							// get metadata json string
							jsonDocument = reader.ReadToEnd();

							// convert to object
							result = System.Web.Helpers.Json.Decode(jsonDocument);
						}
					}
				}
			}
			catch
			{
				// something went wrong. create the metadata object with just the common authorize url
				// TODO: log
				throw;
			}

			return result;
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
					if (rem > 0)
					{
						var filler = new string('=', 4 - rem);
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