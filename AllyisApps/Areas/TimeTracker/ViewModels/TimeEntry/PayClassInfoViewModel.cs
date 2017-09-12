using System;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Pay Class.
	/// </summary>
	public class PayClassInfoViewModel
	{
		/// <summary>
		/// Gets or sets the pay class id.
		/// </summary>
		public int PayClassId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string PayClassName { get; set; }

		/// <summary>
		/// Gets or sets the organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Created time.
		/// </summary>
		public DateTime CreatedUtc { get; set; }
	}
}