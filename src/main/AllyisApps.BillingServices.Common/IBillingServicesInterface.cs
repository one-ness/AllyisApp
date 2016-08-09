using System.Collections.Generic;
using AllyisApps.BillingServices.Common.Types;

namespace AllyisApps.BillingServices.Common
{
	public interface IBillingServicesInterface
	{
		// Plans are per offering, ie Time Tracker, Consulting, etc
		#region plans
		bool CreatePlan(int amount, string interval, string planName);

		BillingPlan RetrievePlan();

		bool UpdatePlan();

		bool DeletePlan();
		#endregion

		#region customers
		BillingServicesCustomerId CreateCustomer(string email, BillingServicesToken token);

		BillingCustomer RetrieveCustomer(BillingServicesCustomerId customerId);

		List<BillingCustomer> ListCustomers();

		bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token);

		bool DeleteCustomer();
		#endregion

		#region subscriptions
		string CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId);

		BillingSubscription RetrieveSubscription();

		List<BillingSubscription> ListSubscriptions();

		bool UpdateSubscription(int amount, string interval, string planName, string subscriptionId, BillingServicesCustomerId customerId);

		void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId);
		#endregion

		#region charges
		bool CreateCharge();

		BillingCharge RetrieveCharge();

		List<BillingCharge> ListCharges(BillingServicesCustomerId customerId);
		#endregion

		#region invoices
		List<BillingInvoice> ListInvoices(BillingServicesCustomerId customerId);
		#endregion
	}
}
