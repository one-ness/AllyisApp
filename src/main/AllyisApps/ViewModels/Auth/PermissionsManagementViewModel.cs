//------------------------------------------------------------------------------
// <copyright file="PermissionsManagementViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using AllyisApps.Services;
using AllyisApps.Services.Billing;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The Permissions Management model.
	/// </summary>
	public class PermissionsManagementViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets a value representing the TimeTracker product ID.
		/// </summary>
		public int TimeTrackerId { get; internal set; }

		/// <summary>
		/// Gets the list of users.
		/// </summary>
		public IEnumerable<UserPermissionsManagement> UserPermissions { get; internal set; }

		/// <summary>
		/// Gets the list filters related to the this data set.
		/// </summary>
		public FilterDataModel Filters { get; internal set; }

		/// <summary>
		/// Gets the user metadata serialization for view javascript.
		/// </summary>
		public string UserMetaData
		{
			get
			{
				StringBuilder output = new StringBuilder();
				output.Append("{");
				UserPermissionsManagement user = this.UserPermissions.ElementAt(0);
				output.Append(string.Format("\"{0}\":{1}", user.UserId, "{")); // A little bit of curly brace funny business to make the formatter happy...
				output.Append(string.Format("name:\"{0}\",", user.UserName));
				output.Append(string.Format("search:\"{0} {1}\"", user.UserName, user.Email));
				output.Append("}");
				for (int i = 1; i < this.UserPermissions.Count(); i++)
				{
					output.Append(",");
					output.Append(string.Format("\"{0}\":{1}", this.UserPermissions.ElementAt(i).UserId, "{"));
					output.Append(string.Format("name:\"{0}\",", this.UserPermissions.ElementAt(i).UserName));
					output.Append(string.Format("search:\"{0} {1}\"", this.UserPermissions.ElementAt(i).UserName, this.UserPermissions.ElementAt(i).Email));
					output.Append("}");
				}

				output.Append("}");
				return HttpUtility.HtmlDecode(output.ToString());
			}
		}

		/// <summary>
		/// Gets the subscriptionDisplay information.
		/// </summary>
		public IEnumerable<SubscriptionDisplayInfo> Subscriptions { get; internal set; }
	}

	/// <summary>
	/// Object for viewing a single user's org and sub roles.
	/// </summary>
	public class UserPermissionsManagement
	{
		/// <summary>
		/// Gets the user's Id.
		/// </summary>
		public string UserId { get; internal set; }

		/// <summary>
		/// Gets the User's full name (first and last).
		/// </summary>
		public string UserName { get; internal set; }

		/// <summary>
		/// Gets the user's email.
		/// </summary>
		public string Email { get; internal set; }

		/// <summary>
		/// Gets the user's role in the organization.
		/// </summary>
		public int OrganizationRoleId { get; internal set; }

		/// <summary>
		/// Gets the list of Subscription roles the user has.
		/// </summary>
		public List<SubscriptionRoleInfo> SubscriptionRoles { get; internal set; }
	}

	/// <summary>
	/// The model for Filter Data.
	/// </summary>
	public class FilterDataModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FilterDataModel" /> class.
		/// </summary>
		public FilterDataModel()
		{
			this.FilterGroups = new List<FilterGroup>();
		}

		/// <summary>
		/// Gets the filter representing all users in the organization.
		/// </summary>
		public Filter AllUsers { get; internal set; }

		/// <summary>
		/// Gets the filter representing all users who do not have subscriptions in this organization.
		/// </summary>
		public Filter UnassignedUsers { get; internal set; }

		/// <summary>
		/// Gets the list of filter groups.
		/// </summary>
		public List<FilterGroup> FilterGroups { get; internal set; }

		/// <summary>
		/// Creates and returns a new FilterGroup object.
		/// </summary>
		/// <param name="name">The name of the filter group.</param>
		/// <returns>The new instance of FilterGroup.</returns>
		public FilterGroup AddNewFilterGroup(string name)
		{
			FilterGroup result = new FilterGroup(name);
			this.FilterGroups.Add(result);
			return result;
		}
	}

	/// <summary>
	/// An object representing a group of filters.
	/// </summary>
	public class FilterGroup
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FilterGroup" /> class.
		/// </summary>
		/// <param name="optGroup">Name of the group.</param>
		public FilterGroup(string optGroup)
		{
			this.OptionGroup = optGroup;
			this.Filters = new List<Filter>();
		}

		/// <summary>
		/// Gets the name of the filter group.
		/// </summary>
		public string OptionGroup { get; internal set; }

		/// <summary>
		/// Gets the list of the filters.
		/// </summary>
		public List<Filter> Filters { get; internal set; }
	}

	/// <summary>
	/// A filter object for populating filter data.
	/// </summary>
	public class Filter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Filter" /> class.
		/// </summary>
		/// <param name="name">The name of the filter.</param>
		/// <param name="users">The list of users.</param>
		public Filter(string name, IEnumerable<UserRolesInfo> users)
		{
			this.Name = name;
			this.UserIds = users.Select(x => x.UserId);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Filter" /> class.
		/// </summary>
		/// <param name="name">The name of the filter.</param>
		/// <param name="users">The list of users.</param>
		/// <param name="whereExpression">A limiting where expression for selecting a subset of users.</param>
		public Filter(string name, IEnumerable<UserRolesInfo> users, Expression<Func<UserRolesInfo, bool>> whereExpression)
		{
			this.Name = name;
			Func<UserRolesInfo, bool> whereFunction = whereExpression.Compile();
			this.UserIds = users.Where(whereFunction).Select(x => x.UserId);
		}

		/// <summary>
		/// Gets or sets the name of the filter.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the list of users who are within this filter.
		/// </summary>
		public IEnumerable<string> UserIds { get; internal set; }

		/// <summary>
		/// Gets a string output of the list of userids in this filter.
		/// </summary>
		/// <returns>Comma-separated list of userIds.</returns>
		public string UsersValue()
		{
			if (this.UserIds.Count() > 0)
			{
				string[] array = this.UserIds.ToArray();
				string output = string.Join(",", array);
				return output;
			}

			return string.Empty;
		}
	}
}