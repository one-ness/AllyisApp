using System.Collections.Generic;
using AllyisApps.BillingServices.Common.Types;

namespace AllyisApps.BillingServices.Common
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBillingServicesInterface
	{
		#region plans
		// Plans are per offering, ie Time Tracker, Consulting, etc

		/// <summary>
		/// 
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="interval"></param>
		/// <param name="planName"></param>
		/// <returns></returns>
		bool CreatePlan(int amount, string interval, string planName);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		BillingPlan RetrievePlan();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		bool UpdatePlan();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		bool DeletePlan();
		#endregion

		#region customers
		/// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		BillingServicesCustomerId CreateCustomer(string email, BillingServicesToken token);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		BillingCustomer RetrieveCustomer(BillingServicesCustomerId customerId);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		List<BillingCustomer> ListCustomers();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		bool DeleteCustomer();
		#endregion

		#region subscriptions
		/// <summary>
		/// 
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="interval"></param>
		/// <param name="planName"></param>
		/// <param name="customerId"></param>
		/// <returns></returns>
		BillingServicesSubscriptionId CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		BillingSubscription RetrieveSubscription();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		List<BillingSubscription> ListSubscriptions();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="interval"></param>
		/// <param name="planName"></param>
		/// <param name="subscriptionId"></param>
		/// <param name="customerId"></param>
		/// <returns></returns>
		bool UpdateSubscription(int amount, string interval, string planName, string subscriptionId, BillingServicesCustomerId customerId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="subscriptionId"></param>
		void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId);
		#endregion

		#region charges
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		bool CreateCharge();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		BillingCharge RetrieveCharge();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		List<BillingCharge> ListCharges(BillingServicesCustomerId customerId);
		#endregion

		#region invoices
		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		List<BillingInvoice> ListInvoices(BillingServicesCustomerId customerId);
		#endregion
	}
}