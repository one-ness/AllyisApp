using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.DBModel.Shared
{
	/// <summary>
	/// The information about the customer and the organization that it belongs to. 
	/// </summary>

	public class CustomerOrgInfo
	{
		/// <summary>
		/// The id of the organization that the customer belongs to. 
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// The id of the customer. 
		/// </summary>
		public int CustomerId { get; set; }


		/// <summary>
		/// The name of the customer. 
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// The name of the organization. 
		/// </summary>

		public string OrganizationName { get; set; }
	}
}