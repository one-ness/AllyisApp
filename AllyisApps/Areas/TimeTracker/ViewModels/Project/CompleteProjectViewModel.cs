using System;
using AllyisApps.Services.Crm;

namespace AllyisApps.ViewModels.TimeTracker.Project
{
	/// <summary>
	/// View Model for ProjectInfomation.
	/// </summary>
	public class CompleteProjectViewModel
	{
		/// <summary>
		/// Gets or sets the Project Id.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the project is active.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the Customer Id that the project belongs to.
		/// </summary>
		public int? CustomerId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the Customer is active.
		/// </summary>
		public bool IsCustomerActive { get; set; }

		/// <summary>
		/// Gets or sets the Oorganization Id that the customer belongs to.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Date/time of creation.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Name of the project.
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets the Name of the customer.
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the Name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a user is active on the project, if that project was fetched via userId.
		/// </summary>
		public bool IsUserActive { get; set; }

		/// <summary>
		/// Gets or sets the Project Pricing Type.
		/// </summary>
		public string PriceType { get; set; }

		/// <summary>
		/// Gets or sets the Project start date.
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the Project end date.
		/// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the current user can edit the project.
		/// </summary>
		public bool CanEditProject { get; set; }

		/// <summary>
		/// Gets or sets the Id of the project to be used by the organization.
		/// </summary>
		public string ProjectCode { get; set; }

		/// <summary>
		/// Gets or sets the Id of the customer to be used by the organization.
		/// </summary>
		public string CustomerCode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user is a user of this project, in some contexts.
		/// </summary>
		public bool? IsProjectUser { get; set; }

		/// <summary>
		/// Initializes a instance of see <see cref="CompleteProjectViewModel"/>
		/// </summary>
		public CompleteProjectViewModel()
		{
		}

		/// <summary>
		/// Initializes a instance of see <see cref="CompleteProjectViewModel"/>
		/// </summary>
		public CompleteProjectViewModel(CompleteProject proj)
		{
			CanEditProject = proj.CanEditProject;
			CreatedUtc = proj.CreatedUtc;
			CustomerId = proj.OwningCustomer?.CustomerId;
			CustomerName = proj.OwningCustomer?.CustomerName;
			CustomerCode = proj.OwningCustomer?.CustomerCode;
			EndDate = proj.EndDate;
			IsActive = proj.IsActive;
			IsCustomerActive = proj.IsCustomerActive;
			IsProjectUser = proj.IsProjectUser;
			IsUserActive = proj.IsUserActive;
			OrganizationId = proj.OrganizationId;
			OrganizationName = proj.OrganizationName;
			PriceType = proj.PriceType;
			ProjectId = proj.ProjectId;
			ProjectName = proj.ProjectName;
			ProjectCode = proj.ProjectCode;
			StartDate = proj.StartDate;
		}
	}
}