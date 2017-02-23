namespace AllyisApps.Services
{
	public partial class Service : BaseService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Service"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public Service(string connectionString) : base(connectionString) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Service"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="userContext">The user context.</param>
		public Service(string connectionString, UserContext userContext) : base(connectionString, userContext) { }
	}
}
