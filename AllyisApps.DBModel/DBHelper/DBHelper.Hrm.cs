using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Lookup;
using AllyisApps.DBModel.Hrm;
using Dapper;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// dbhelper - for hrm
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// creates the given list of payclasses for the given org
		/// </summary>
		public async Task CreatePayClassesAsync(int orgId, List<PayClassDBEntity> list)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				await con.ExecuteAsync("[Hrm].[CreatePayClass] @BuiltInPayClassId, @PayClassName, @OrganizationId", list);
			}
		}

		/// <summary>
		/// create the given employee type for the given org
		/// </summary>
		public async Task<int> CreateEmployeeType(int orgId, string employeeTypeName)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QueryFirstOrDefaultAsync<int>("[Hrm].[CreateEmployeeType] @a, @b", new { a = orgId, b = employeeTypeName });
			}
		}

		/// <summary>
		/// add all of the org payclasses to the given employee type
		/// </summary>
		public async Task AddOrgPayClassesToEmployeeType(int orgId, int employeeTypeId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				await con.ExecuteAsync("[Hrm].[AddOrgPayClassesToEmployeeType] @a, @b", new { a = employeeTypeId, b = orgId });
			}
		}
	}
}
