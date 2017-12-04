using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Hrm
{
	/// <summary>
	/// The employee type database entity.
	/// </summary>
	public class EmployeeTypeDBEntity
	{
		/// <summary>
		/// Gets or sets the employee type id.
		/// </summary>
		public int EmployeeTypeId { get; set; }

		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the employee type name.
		/// </summary>
		public string EmployeeTypeName { get; set; }
	}
}