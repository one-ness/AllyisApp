using System;

namespace AllyisApps.DBModel.Finance
{
	/// <summary>
	/// Represents the Expense Item table in the database.
	/// </summary>
	public class ExpenseUserDBEntity
	{
		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the employee id.
		/// </summary>
		public int EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the role of the user.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		/// <summary>
		/// Gets or sets the date the user was added to the organization.
		/// </summary>
		public DateTime OrganizationUserCreatedUtc { get; set; }
	}
}
