using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// 
	/// </summary>
	public class IndexAndOrgsViewModel
	{
		/// <summary>
		/// Gets or sets the view model for user profile information.
		/// </summary>
		public UserInfoViewModel UserModel { get; set; }

		/// <summary>
		/// Gets or sets the view model for organizations information.
		/// </summary>
		public AccountOrgsViewModel OrgModel { get; set; }
	}
}