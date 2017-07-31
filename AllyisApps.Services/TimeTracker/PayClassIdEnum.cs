using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Auth
{
	public enum PayClassId : int
	{
		/// <summary>
		/// Regular.
		/// </summary>
		Regular = 1,

		/// <summary>
		/// Paid Time Off.
		/// </summary>
		PaidTimeOff = 2,

		/// <summary>
		/// Unpaid Time Off.
		/// </summary>
		UnpaidTimeOff = 3,

		/// <summary>
		/// Holiday.
		/// </summary>
		Holiday = 4
	}
}
