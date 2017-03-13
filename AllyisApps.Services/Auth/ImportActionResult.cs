using System.Collections.Generic;

namespace AllyisApps.Services
{
	public class ImportActionResult
	{
		public int CustomersImported { get; set; }
		public int ProjectsImported { get; set; }
		public int UsersImported { get; set; }
		public int UsersAddedToOrganization { get; set; }
		public int UsersAddedToSubscription { get; set; }
		public int TimeEntriesImported { get; set; }

		public List<string> CustomerFailures;
		public List<string> ProjectFailures;
		public List<string> UserFailures;
		public List<string> OrgUserFailures;
		public List<string> UserSubscriptionFailures;
		public List<string> TimeEntryFailures;

		public ImportActionResult()
		{
			CustomerFailures = new List<string>();
			ProjectFailures = new List<string>();
			UserFailures = new List<string>();
			OrgUserFailures = new List<string>();
			UserSubscriptionFailures = new List<string>();
			TimeEntryFailures = new List<string>();
		}
	}
}
