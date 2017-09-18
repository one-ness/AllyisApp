namespace AllyisApps.Services
{
	/// <summary>
	/// Represents the user information serialized to the forms authentication cookie.
	/// </summary>
	public class CookieData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CookieData"/> class.
		/// </summary>
		/// <param name="userId">User id.</param>
		public CookieData(int userId)
		{
			this.UserId = userId;
		}

		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		public int UserId { get; set; }
	}
}