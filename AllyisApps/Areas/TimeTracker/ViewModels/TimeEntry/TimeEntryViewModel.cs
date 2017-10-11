using System;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// View Model for Time Entries.
	/// </summary>
	public class TimeEntryViewModel
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public TimeEntryViewModel() { }

		/// <summary>
		/// Initializes the view model with the timeentry service object.
		/// </summary>
		/// <param name="timeEntry">The time entry service object to convert</param>
		public TimeEntryViewModel(Services.TimeTracker.TimeEntry timeEntry)
		{
			ApprovalState = timeEntry.ApprovalState;
			Date = timeEntry.Date;
			Description = timeEntry.Description;
			Duration = timeEntry.Duration;
			Email = timeEntry.Email;
			EmployeeId = timeEntry.EmployeeId;
			FirstName = timeEntry.FirstName;
			IsLockSaved = timeEntry.IsLockSaved;
			LastName = timeEntry.LastName;
			ModSinceApproval = timeEntry.ModSinceApproval;
			PayClassId = timeEntry.PayClassId;
			PayClassName = timeEntry.PayClassName;
			ProjectId = timeEntry.ProjectId;
			TimeEntryId = timeEntry.TimeEntryId;
			UserId = timeEntry.UserId;
		}

		/// <summary>
		/// Gets or sets the TimeEntryId.
		/// </summary>
		public int TimeEntryId { get; set; }

		/// <summary>
		/// Gets or sets the UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the TimeEntryId.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the TimeEntryId.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the EmployeeId.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the Email address.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the ProjectId.
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the PayClassId.
		/// </summary>
		public int PayClassId { get; set; }

		/// <summary>
		/// Gets or sets the pay class name.
		/// </summary>
		public string PayClassName { get; set; }

		/// <summary>
		/// Gets or sets the Date.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the Duration.
		/// </summary>
		public float Duration { get; set; }

		/// <summary>
		/// Gets or sets the Description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the approval state.
		/// </summary>
		public int ApprovalState { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the saved approval lock.
		/// </summary>
		public bool IsLockSaved { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the data has changed since being approved or dissaproved.
		/// </summary>
		public bool ModSinceApproval { get; set; }

		/// <summary>
		/// Gets or sets the customer name for the time entry
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the project name for the time entry
		/// </summary>
		public string ProjectName { get; set; }
	}
}