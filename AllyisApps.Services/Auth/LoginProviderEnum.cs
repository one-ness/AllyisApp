using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// list of login providers
	/// </summary>
	public enum LoginProviderEnum : int
	{
		AllyisApps = 1,
		Microsoft = 2,
		Google = 3,
	}
}
