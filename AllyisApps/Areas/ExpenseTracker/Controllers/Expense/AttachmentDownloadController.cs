using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Expense controller.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// Downloads an attachment file.
		/// </summary>
		/// <param name="reportId">The report id.</param>
		/// <param name="fileName">The file name.</param>
		/// <returns>Returns the selected file.</returns>
		async public Task<ActionResult> Download(int reportId, string fileName)
		{
			MemoryStream stream = new MemoryStream();
			string contentType = await AzureFiles.DownloadReportAttachment(reportId, fileName, stream);
			stream.Seek(0, SeekOrigin.Begin);
			return File(stream, contentType, fileName);
		}
	}
}