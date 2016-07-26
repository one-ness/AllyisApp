//------------------------------------------------------------------------------
// <copyright file="TargetUser.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.Services.BusinessObjects;

namespace AllyisApps.BusinessObjects
{	
	/// <summary>
	/// A user to perform actions on.
	/// </summary>
	public class TargetUser
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TargetUser" /> class.
		/// </summary>
		public TargetUser()
		{
			this.Message = string.Empty;
		}

		/// <summary>
		/// Gets or sets the user's Id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the user's Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a return message about the user.
		/// </summary>
		public string Message { get; set; }
	}	
}