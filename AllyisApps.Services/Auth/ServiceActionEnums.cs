namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// update employee id and org role
	/// </summary>
	public enum UpdateEmployeeIdAndOrgRoleResult : int
	{
		Success = 0,
		CannotSelfUpdateOrgRole,
		EmployeeIdNotUnique,
	}
}
