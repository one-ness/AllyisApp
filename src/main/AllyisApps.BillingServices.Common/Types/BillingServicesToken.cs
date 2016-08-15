//------------------------------------------------------------------------------
// <copyright file="BillingServicesToken.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.BillingServices.Common.Types
{
	/// <summary>
	/// Container class for a Billing Services Token.
	/// </summary>
	public class BillingServicesToken
	{
		#region private fields
		private readonly string token;
		#endregion

		#region constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="token"></param>
		public BillingServicesToken(string token)
		{
			this.token = token;
		}
		#endregion

		#region accessor properties
		/// <summary>
		/// 
		/// </summary>
		public string Token
		{
			get
			{
				return this.token;
			}
		}
		#endregion
	}
}
