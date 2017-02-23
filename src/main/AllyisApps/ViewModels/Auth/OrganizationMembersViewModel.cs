﻿//------------------------------------------------------------------------------
// <copyright file="OrganizationMembersViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the members list for a given organization.
	/// </summary>
	public class OrganizationMembersViewModel : BaseViewModel
	{
		private const int PageUserLimit = 25;

		/// <summary>
		/// Gets a value indicating the number of pages that should be displayed.
		/// </summary>
		public int PageCount
		{
			get
			{
				return (int)Math.Ceiling(this.TotalUsers / (double)PageUserLimit);
			}
		}

		/// <summary>
		/// Gets a value indicating the size of pages that should be displayed.
		/// </summary>
		public int PageSize
		{
			get
			{
				return PageUserLimit;
			}
		}

		/// <summary>
		/// Gets the ID of the current user.
		/// </summary>
		public int CurrentUserId { get; internal set; }

		/// <summary>
		/// Gets the total number of users.
		/// </summary>
		public int TotalUsers { get; internal set; }

		/// <summary>
		/// Gets a list of the users.
		/// </summary>
		public IEnumerable<OrganizationUserViewModel> DisplayUsers { get; internal set; }

		/// <summary>
		/// Gets a list of pending user invitations.
		/// </summary>
		public IEnumerable<InvitationInfo> PendingInvitation { get; internal set; }

		/// <summary>
		/// Gets OrganizationId.
		/// </summary>
		public int OrganizationId { get; internal set; }

		/// <summary>
		/// Gets Organization name.
		/// </summary>
		public string OrganizationName { get; internal set; }

		/// <summary>
		/// Gets Access code.
		/// </summary>
		[Display(Name = "Access Code")]
		public string AccessCode { get; internal set; }
	}
}
