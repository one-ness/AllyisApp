//------------------------------------------------------------------------------
// <copyright file="OrganizationAddMembersViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AllyisApps.Services.Org;
using AllyisApps.Services.Project;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// OrganizationAddMembersModel class.
	/// </summary>
	public class OrganizationAddMembersViewModel : BaseViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrganizationAddMembersViewModel"/> class.
		/// </summary>
		public OrganizationAddMembersViewModel()
		{
			this.AddedUsers = new HashSet<string>();
			this.UsersAlreadyExisting = new HashSet<string>();
			this.EmailedUsers = new HashSet<string>();
		}

		/// <summary>
		/// Gets or sets user first name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Org.Strings)), ErrorMessageResourceName = "FirstNameValidation")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets user last name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Org.Strings)), ErrorMessageResourceName = "LastNameValidation")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets UserInput.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Org.Strings)), ErrorMessageResourceName = "EmailValidation")]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the code to access an organizaiton.
		/// </summary>
		public string AccessCode { get; set; }

		/// <summary>
		/// Gets or sets the organization Role to give the user once the invitation is consumed.
		/// </summary>
		public int OrgRole { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to add the user as an Owner or not.
		/// </summary>
		public bool AddAsOwner { get; set; }

		/// <summary>
		/// Gets or sets the list of subscription roles.
		/// </summary>
		public IEnumerable<SubscriptionRoleSelectionModel> SubscriptionRoles { get; set; }

		/// <summary>
		/// Gets or sets the Subscription Project Id to assign to the user once the invitation is consumed.
		/// </summary>
		public int? SubscriptionProjectId { get; set; }

		/// <summary>
		/// Gets organization.
		/// </summary>
		public OrganizationInfo Organization { get; internal set; }

		/// <summary>
		/// Gets AddedUsers.
		/// </summary>
		public HashSet<string> AddedUsers { get; internal set; }

		/// <summary>
		/// Gets UsersAlreadyExisting.
		/// </summary>
		public HashSet<string> UsersAlreadyExisting { get; internal set; }

		/// <summary>
		/// Gets EmailedUsers.
		/// </summary>
		public HashSet<string> EmailedUsers { get; internal set; }

		/// <summary>
		/// Gets projects in this organizaiton.
		/// </summary>
		public IEnumerable<CompleteProjectInfo> OrganizationProjects { get; internal set; }
	}
}