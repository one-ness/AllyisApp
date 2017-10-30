//------------------------------------------------------------------------------
// <copyright file="LogOnPartialViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Text;

namespace AllyisApps.ViewModels.Shared
{
	/// <summary>
	/// LogOnPartial View Model.
	/// </summary>
	public class LogOnPartialViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets the Name of current user.
		/// </summary>
		public string UserName
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("{0} {1}", FirstName, LastName);
				return sb.ToString();
			}
		}

		/// <summary>
		/// Gets or sets the First Name of the user.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the Last Name of the user.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the name of the organziation the user is working in currently.
		/// </summary>
		public string CurrentOrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the name of the subscription the user is working in currently.
		/// </summary>
		public string CurrentSubscriptionName { get; set; }
	}
}