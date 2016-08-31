//------------------------------------------------------------------------------
// <copyright file="BillingServicesToken.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.Billing;

namespace AllyisApps.Services.Common.Types
{
	/// <summary>
	/// Container class for a Billing Services Token.
	/// </summary>
	public class BillingServicesToken
	{
		#region private fields
		private readonly string token;
		#endregion private fields

		#region constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="BillingServicesToken"/> class.
		/// </summary>
		/// <param name="token">The token for the container.</param>
		public BillingServicesToken(string token)
		{
			this.token = token;
		}
		#endregion constructor

		#region accessor properties
		/// <summary>
		/// Gets the Token.
		/// </summary>
		public string Token
		{
			get
			{
				return this.token;
			}
		}
		#endregion accessor properties
	}
}