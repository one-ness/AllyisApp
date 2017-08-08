namespace AllyisApps.Services
{
	/// <summary>
	/// Represents the user information serialized to the forms authentication cookie.
	/// </summary>
	public class CookieData
	{
		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// constructor.
		/// </summary>
		public CookieData(int userId)
		{
			this.UserId = userId;
		}
	}
}
