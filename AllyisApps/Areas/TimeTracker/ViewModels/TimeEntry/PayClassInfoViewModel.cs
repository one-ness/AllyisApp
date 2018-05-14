using System;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Pay Class.
	/// </summary>
	public class PayClassInfoViewModel
	{
		/// <summary>
		/// Initializes new pay class view model with a service object
		/// </summary>
		/// <param name="payClass"></param>
		public PayClassInfoViewModel(Services.TimeTracker.PayClass payClass)
		{
			OrganizationId = payClass.OrganizationId;
			PayClassId = payClass.PayClassId;
			PayClassName = payClass.PayClassName;
			PayClassBuiltInId = payClass.BuiltInPayClassId;
		}

		/// <summary>
		/// Gets or sets the pay class id.
		/// </summary>
		public int PayClassId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string PayClassName { get; set; }
		/// <summary>
		/// if is builtINid
		/// </summary>
		public int PayClassBuiltInId { get; }

		/// <summary>
		/// Gets or sets the organization Id.
		/// </summary>
		public int OrganizationId { get; set; }
	}
}