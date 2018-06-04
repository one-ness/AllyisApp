using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Billing
{
	[CLSCompliant(false)]
	public enum ProductStatusEnum : uint
	{
		Active = 1,
		Inactive,
		Any = uint.MaxValue,
	}
}
