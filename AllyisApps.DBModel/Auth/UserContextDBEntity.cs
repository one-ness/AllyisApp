namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// The model for rows returned by Auth.GetUserContextInfo.
	/// </summary>
	public class UserContextDBEntity
	{
		/// <summary>
		/// Gets or sets the user Id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the user's first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user's last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the user's email address.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user's language preference.
		/// </summary>
		public int? LanguagePreference { get; set; }

		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int? OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the organization name.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the id of the user's role in the organization.
		/// </summary>
		public int? OrgRoleId { get; set; }

		/// <summary>
		/// Gets or sets the subscription id.
		/// </summary>
		public int? SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the subscription name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the product id for the subscription.
		/// </summary>
		public int? ProductId { get; set; }

		/// <summary>
		/// Gets or sets the product name for the subscription.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the product role name for the subscription.
		/// </summary>
		public string ProductRoleName { get; set; }

		/// <summary>
		/// Gets or sets the sku id for the subscription.
		/// </summary>
		public int? SkuId { get; set; }

		/// <summary>
		/// Gets or sets the id of the user's role in the product subscription.
		/// </summary>
		public int? ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the product area url.
		/// </summary>
		public string AreaUrl { get; set; }
	}
}
