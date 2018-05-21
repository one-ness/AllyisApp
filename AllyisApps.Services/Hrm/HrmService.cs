using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	/// <summary>
	/// app service - for hrm
	/// </summary>
	public partial class AppService : BaseService
	{
		/// <summary>
		/// create the default payclasses (for all the builtinpayclassenums)
		/// </summary>
		public async Task CreateDefaultPayClassesAsync(List<Tuple<int, string>> payClassIdsAndNames)
		{
			// TODO: either change this to multiple calls in a transaction, or change stored procedure to accept multiple payclass ids and names
			await this.DBHelper.CreateDefaultPayClassesAsync();
		}

		/// <summary>
		/// add all payclasses of the organization to the given employee type
		/// </summary>
		public async Task AddAllPayClassesToEmployeeType(int orgId, int employeeTypeId)
		{

		}
	}
}
