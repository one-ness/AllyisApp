using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Lookup;
using Dapper;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// dbhelper - for hrm
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// creates a set of default payclasses
		/// </summary>
		public async Task CreateDefaultPayClassesAsync()
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				await con.ExecuteAsync("[Hrm].[CreateDefaultPayClasses]");
			}
		}

		/// <summary>
		/// create the given employee type for the given org
		/// </summary>
		public async Task<int> CreateEmployeeType(int orgId, string employeeTypeName)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QueryFirstOrDefaultAsync("[Hrm].[CreateEmployeeType] @a, @b", new { a = orgId, b = employeeTypeName });
			}
		}
	}
}
