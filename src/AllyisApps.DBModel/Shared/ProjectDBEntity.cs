using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Shared
{
	/// <summary>
	/// The project table. 
	/// </summary>
	public class ProjectDBEntity : BasePoco
	{

		/// <summary>
		/// The project id. 
		/// </summary>
		public int ProjectId { get; set; }


		/// <summary>
		/// The id of the organization associated with the project.
		/// </summary>
		public int OrganizationId { get; set; }


		/// <summary>
		/// The id of the customer associated with the project.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// The project name. 
		/// </summary>
		public string Name { get; set; }
	}
}
