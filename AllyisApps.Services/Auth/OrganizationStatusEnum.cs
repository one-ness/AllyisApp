using System;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// status of an organization
	/// </summary>
	[CLSCompliant(false)]
	public enum OrganizationStatusEnum : uint
	{
		Active = 1,
		Inactive = 2,
		Any = uint.MaxValue,
	}
}
