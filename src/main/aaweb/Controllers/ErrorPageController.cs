using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for errors.
	/// </summary>
	public class ErrorPageController : Controller
	{
		/// <summary>
		/// Passes the status code to the error page view to display.
		/// </summary>
		/// <param name="id">The error status code.</param>
		/// <returns>An action result.</returns>
		public ActionResult Oops(int id)
		{
			Response.StatusCode = id;

			return View();
		}
	}
}