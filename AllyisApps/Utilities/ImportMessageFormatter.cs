using System.Collections.Generic;
using System.Linq;
using AllyisApps.Services;

namespace AllyisApps.Utilities
{
	/// <summary>
	/// A simple utility for summarizing the results of an Import action into reader-friendly notification messages.
	/// </summary>
	public class ImportMessageFormatter
	{
		/// <summary>
		/// Takes an ImportActionResult and produces two formatted strings: one for successfull imports and one for failures.
		/// Note: the failure string will contain br tags, and any BootstrapAlert's containing it should have their IsHtmlString value set to true.
		/// </summary>
		/// <param name="result">The returned ImportActionResult from a AppService.Import call.</param>
		/// <returns>A two-member string array with the success and failure formatted messages.</returns>
		public static string[] FormatImportResult(ImportActionResult result)
		{
			var formattedResult = new string[2];

			// formattedResult[0]: Success notification
			var successfulImports = new List<string>();
			if (result.CustomersImported > 0)
			{
				successfulImports.Add($"{result.CustomersImported} customers");
			}

			if (result.ProjectsImported > 0)
			{
				successfulImports.Add($"{result.ProjectsImported} projects");
			}

			if (result.UsersImported > 0)
			{
				successfulImports.Add($"{result.UsersImported} users");
			}

			if (result.TimeEntriesImported > 0)
			{
				successfulImports.Add($"{result.TimeEntriesImported} time entries");
			}

			string successMessage = Helpers.ReplaceLastOccurrence(string.Join(", ", successfulImports), ",", " and");

			if (!string.IsNullOrEmpty(successMessage))
			{
				formattedResult[0] = successMessage + " imported.<br>"; // LANGUAGE Update to use resource file to change message language
			}

			if (result.UsersAddedToOrganization > 0)
			{
				int difference = result.UsersAddedToOrganization - result.UsersImported;
				if (difference > 0)
				{
					formattedResult[0] = $"{formattedResult[0]}{difference} existing users added to organization.<br>"; // LANGUAGE Update to use resource file to change message language
				}
			}

			formattedResult[0] += $"Total Imports: {result.TotalImports()}";

			// formattedResult[1]: Fail notification - simply joins all the fail messages into one, separated by newlines
			List<string> failures =
				result.GeneralFailures
				.Union(result.CustomerFailures)
				.Union(result.ProjectFailures)
				.Union(result.UserFailures)
				.Union(result.TimeEntryFailures)
				.Union(result.OrgUserFailures)
				.Union(result.UserSubscriptionFailures)
				.ToList();
			if (failures.Count > 0)
			{
				formattedResult[1] = $"Total Errors: {failures.Count}<br>{string.Join("<br>", failures)}";
			}

			return formattedResult;
		}
	}
}