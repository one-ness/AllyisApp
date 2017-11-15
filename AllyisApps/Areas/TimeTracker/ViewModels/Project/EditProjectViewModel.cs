//------------------------------------------------------------------------------
// <copyright file="EditProjectViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AllyisApps.ViewModels.TimeTracker.Project
{
	/// <summary>
	/// View Model for Project Editing.
	/// </summary>
	public class EditProjectViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EditProjectViewModel" /> class. For MVC.
		/// </summary>
		public EditProjectViewModel()
		{
			ProjectUsers = new List<BasicUserInfoViewModel>();
			SubscriptionUsers = new List<BasicUserInfoViewModel>();
			SelectedProjectUserIds = new string[] { };
			IsCreating = false;
		}

		/// <summary>
		/// Gets or sets Project Name.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Project Name")]
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets Organization Id or Null.
		/// </summary>
		[Display(Name = "Organization")]
		public int? OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets Customer Id.
		/// </summary>
		[Required]
		[Display(Name = "Customer")]
		public int ParentCustomerId { get; set; }

		/// <summary>
		/// Gets or sets Project Id.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets Project Start Date. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets Project End Date. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Gets or sets a select list of customers.
		/// </summary>
		public List<SelectListItem> Customers { get; set; }

		/// <summary>
		/// Gets or sets the Customer's name.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the organization's name.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets SubscriptionId.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the subscription's name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the User's Id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the Project's Organization Id.
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Project Id")]
		public string ProjectCode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the model is being used for creating a project (true), or editing an existing project (false).
		/// </summary>
		public bool IsCreating { get; set; }

		/// <summary>
		/// Gets or sets the UserInfo collection of users assigned to this project.
		/// </summary>
		public IEnumerable<BasicUserInfoViewModel> ProjectUsers { get; set; }

		/// <summary>
		/// Gets or sets the SubscriptionUser of users who are not a part of this project, but are a part of this subscription.
		/// </summary>
		public IEnumerable<BasicUserInfoViewModel> SubscriptionUsers { get; set; }

		/// <summary>
		/// Gets or sets The collection of users to be assigned to the project on an update.
		/// </summary>
		public string[] SelectedProjectUserIds { get; set; }
	}

	/// <summary>
	/// A simple object that contians the name and user id of a user.
	/// For use with select items.
	/// </summary>
	public class BasicUserInfoViewModel
	{
		/// <summary>
		/// Default constructor for use with automatic assignment helper methods.
		/// </summary>
		public BasicUserInfoViewModel() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicUserInfoViewModel"/> class.
		/// </summary>
		/// <param name="firstName"> User's first name.</param>
		/// <param name="lastName">User's last name.</param>
		/// <param name="userId">User Id.</param>
		public BasicUserInfoViewModel(string firstName, string lastName, int userId)
		{
			Name = $"{firstName} {lastName}";
			UserId = userId.ToString();
		}

		/// <summary>
		/// Gets or sets The full name of the user.
		/// The text of the option.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets The UserId of the user.
		/// The value of the option.
		/// </summary>
		public string UserId { get; set; }
	}
}