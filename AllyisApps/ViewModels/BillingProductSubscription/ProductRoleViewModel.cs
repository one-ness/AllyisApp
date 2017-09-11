namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View Model For Product Role
	/// </summary>
	public class ProductRoleViewModel
	{
		/// <summary>
		/// indicates that user is not assigned this product yet.
		/// </summary>
		public const int NotInProduct = 0;

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string ProductRoleName { get; set; }

		/// <summary>
		/// Gets or sets ProductRoleId.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Product Id.
		/// </summary>
		public int ProductId { get; set; }
	}
}