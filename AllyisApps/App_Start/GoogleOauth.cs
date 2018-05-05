using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

		const string GoogleOAuthClientIdKey = "GoogleOAuthClientId";
		const string GoogleOAuthClientSecretKey = "GoogleOAuthClientSecret";

		/// <summary>
		/// google oauth init
		/// </summary>
		public static void Init()
		{
		}
	}
}