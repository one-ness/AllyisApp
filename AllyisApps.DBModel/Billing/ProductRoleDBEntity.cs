#pragma warning disable 1591

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// product role db entity
	/// </summary>
	public class ProductRoleDBEntity : BaseDBEntity
	{
		public int ProductRoleId { get; set; }
		public int ProductId { get; set; }
		public int? OrganizationId { get; set; }
		public string ProductRoleName { get; set; }
	}
}
