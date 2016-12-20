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
        /// <param name="result">The returned ImportActionResult from a Service.Import call.</param>
        /// <returns>A two-member string array with the success and failure formatted messages.</returns>
        public static string[] FormatImportResult(ImportActionResult result)
        {
            string[] formattedResult = new string[2];

            // formattedResult[0]: Success notification
            List<string> successfulImports = new List<string>();
            if (result.CustomersImported > 0)
            {
                successfulImports.Add(string.Format("{0} customers", result.CustomersImported));
            }

            if (result.ProjectsImported > 0)
            {
                successfulImports.Add(string.Format("{0} projects", result.ProjectsImported));
            }

            if (result.UsersImported > 0)
            {
                successfulImports.Add(string.Format("{0} users", result.UsersImported));
            }

            if (result.TimeEntriesImported > 0)
            {
                successfulImports.Add(string.Format("{0} time entries", result.TimeEntriesImported));
            }

            string successMessage = null;
            int successes = successfulImports.Count;
            switch (successes)
            {
                case 0:
                    break;
                case 1:
                    successMessage = successfulImports[0];
                    break;
                case 2:
                    successMessage = successfulImports[0] + " and " + successfulImports[1];
                    break;
                default:
                    successMessage = string.Empty;
                    for (int i = 0; i < successes - 1; i++)
                    {
                        successMessage = successMessage + successfulImports[i] + ", ";
                    }

                    successMessage = successMessage + " and " + successfulImports[successes - 1];
                    break;
            }

            if (successMessage != null)
            {
                formattedResult[0] = successMessage + " imported.";
            }

            // formattedResult[1]: Fail notification - simply joins all the fail messages into one, separated by newlines
            List<string> failures = result.CustomerFailures.Union(
                result.ProjectFailures).Union(
                result.UserFailures).Union(
                result.TimeEntryFailures).Union(
                result.OrgUserFailures).Union(
                result.UserSubscriptionFailures).ToList();
            if (failures.Count > 0)
            {
                formattedResult[1] = string.Join("<br>", failures.ToArray());
            }

            return formattedResult;
        }
    }
}