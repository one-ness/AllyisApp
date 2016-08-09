using System.Collections.Generic;
using AllyisApps.BillingServices.Common.Types;

namespace AllyisApps.BillingServices.Common
{
	public interface IBillingServicesInterface
	{
		// Plans are per offering, ie Time Tracker, Consulting, etc
		#region plans
		bool CreatePlan(int amount, string interval, string planName);

		bool DeletePlan();

		bool UpdatePlan();

		BillingPlan RetrievePlan();
		#endregion

		#region customers
		BillingServicesCustomerId CreateCustomer(string email, BillingServicesToken token);

		bool DeleteCustomer();

		bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token);

		BillingCustomer RetrieveCustomer(BillingServicesCustomerId customerId);

		List<BillingCustomer> ListCustomers();
		#endregion

		#region subscriptions
		string CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId);

		void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId);

		bool UpdateSubscription(int amount, string interval, string planName, string subscriptionId, BillingServicesCustomerId customerId);

		BillingSubscription RetrieveSubscription();

		List<BillingSubscription> ListSubscriptions();
		#endregion

		#region charges
		bool CreateCharge();

		BillingCharge RetrieveCharge();
		#endregion
	}
}
