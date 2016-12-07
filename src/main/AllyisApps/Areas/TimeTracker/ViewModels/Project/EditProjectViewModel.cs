//------------------------------------------------------------------------------
// <copyright file="EditProjectViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels
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
			this.ProjectUsers = new List<BasicUserInfoViewModel>();
			this.SubscriptionUsers = new List<BasicUserInfoViewModel>();
			this.SelectedProjectUserIds = new string[] { };
            this.IsCreating = false;
        }

        /// <summary>
        /// Gets or sets Project Name.
        /// </summary>
        [Required]
		[DataType(DataType.Text)]
		[Display(Name = "Project Name")]
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets Organization ID or Null.
		/// </summary>
		[Display(Name = "Organization")]
		public int? OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets Customer ID.
		/// </summary>
		[Required]
		[Display(Name = "Customer")]
		public int ParentCustomerId { get; set; }

		/// <summary>
		/// Gets or sets Project ID.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets Project Pricing Type.
		/// </summary>
		[Required]
		public string PriceType { get; set; }

		/// <summary>
		/// Gets or sets Project Start Date. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public int StartDate { get; set; }

		/// <summary>
		/// Gets or sets Project End Date. Note: must be int and not DateTime for Json serialization to work correctly in different cultures.
		/// </summary>
		public int EndDate { get; set; }

		/// <summary>
		/// Gets or sets the Customer's name.
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the organization's name.
		/// </summary>
		public string OrganizationName { get; set; }

        /// <summary>
        /// Gets or sets the Project's Organization ID.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Project ID")]
        public string ProjectOrgId { get; set; }

        /// <summary>
        /// Returns true if the model is being used for creating a project, and false if otherwise (e.g. editing an existing project)
        /// </summary>
        public bool IsCreating { get; set; }

        /// <summary>
        /// Gets or sets the UserInfo collection of users assigned to this project.
        /// </summary>
        public IEnumerable<BasicUserInfoViewModel> ProjectUsers { get; set; }

		/// <summary>
		/// Gets or sets the SubscriptionUserInfo of users who are not a part of this project, but are a part of this subscription.
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
		/// Initializes a new instance of the <see cref="BasicUserInfoViewModel"/> class.
		/// </summary>
		/// <param name="firstName"> User's first name.</param>
		/// <param name="lastName">User's last name.</param>
		/// <param name="userId">User Id.</param>
		public BasicUserInfoViewModel(string firstName, string lastName, int userId)
		{
			this.Name = string.Format("{0} {1}", firstName, lastName);
			this.UserId = userId.ToString();
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