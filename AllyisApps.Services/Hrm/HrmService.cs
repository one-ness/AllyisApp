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
		/// create the given payclasses for the given organization
		/// </summary>
		public async Task CreatePayClassesAsync(int orgId, List<Tuple<int, string>> payClassIdsAndNames)
		{
			// TODO: delete the following line and create the payclasses in a transaction or use a stored procedure
			await this.DBHelper.CreateDefaultPayClassesAsync(orgId);
		}
	}
}
