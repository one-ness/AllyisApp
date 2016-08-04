using System.Collections.Generic;
using AllyisApps.BillingServices.Common.Types;

namespace AllyisApps.BillingServices.Common
{
	public interface IBillingServicesInterface
	{
		// Plans are per offering, ie Time Tracker, Consulting, etc
		#region plans
		bool CreatePlan();

		bool DeletePlan();

		bool UpdatePlan();

		BillingPlan RetrievePlan();
		#endregion

		#region customers
		bool CreateCustomer();

		bool DeleteCustomer();

		bool UpdateCustomer();

		BillingCustomer RetrieveCustomer(string id);

		List<BillingCustomer> ListCustomers();
		#endregion

		#region subscriptions
		bool CreateSubscription();

		bool DeleteSubscription();

		bool UpdateSubscription();

		BillingSubscription RetrieveSubscription();

		List<BillingSubscription> ListSubscriptions();
		#endregion

		#region charges
		bool CreateCharge();

		BillingCharge RetrieveCharge();
		#endregion
	}
}
