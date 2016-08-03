using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.BillingServices
{
	interface BillingSystemInterface
	{
		// Plans are per offering, ie Time Tracker, Consulting, etc
		#region plans
		bool CreatePlan();
		bool DeletePlan();
		bool UpdatePlan();
		bool RetrievePlan();
		#endregion

		#region customers
		bool CreateCustomer();
		bool DeleteCustomer();
		bool UpdateCustomer();
		bool RetrieveCustomer();
		List<bool> ListCustomers();
		#endregion

		#region subscriptions
		bool CreateSubscription();
		bool DeleteSubscription();
		bool UpdateSubscription();
		bool RetrieveSubscription();
		List<bool> ListSubscriptions();
		#endregion

		#region charges
		bool CreateCharge();
		bool RetrieveCharge();
		#endregion
	}
}
