//------------------------------------------------------------------------------
// <copyright file="IBillingServicesInterface.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

using AllyisApps.Services.Common.Types;

namespace AllyisApps.Services.Common
{
	/// <summary>
	/// An interface describing the basic functions a billing service must be able to implement in a generalized sense
	/// in order for the rest of the website functions to be able to make use of that service.
	/// </summary>
	public interface IBillingServicesInterface
	{
		#region plans
		/// <summary>
		/// Creates a billing services plan.  Plans are per offering, i.e. Time Tracker, Consulting, and etc.
		/// </summary>
		/// <param name="amount">The plan amount.</param>
		/// <param name="interval">The plan billing interval.</param>
		/// <param name="planName">The plan name.</param>
		/// <returns>A bool representing the success state of the plan creation.</returns>
		bool CreatePlan(int amount, string interval, string planName);

		/// <summary>
		/// Retrieves a billing services plan.
		/// </summary>
		/// <returns>An info object for the Billing Services plan.</returns>
		BillingServicesPlan RetrievePlan();

		/// <summary>
		/// Updates the Billing Services Plan.
		/// </summary>
		/// <returns>A bool representing the success state of updating the plan.</returns>
		bool UpdatePlan();

		/// <summary>
		/// Deletes a Billing Services plan.
		/// </summary>
		/// <returns>A bool representing the success state of deleting the plan.</returns>
		bool DeletePlan();
		#endregion plans

		#region customers
		/// <summary>
		/// Creates a billing services customer.
		/// </summary>
		/// <param name="email">The email to be associated with this customer.</param>
		/// <param name="token">A billiong services token to be associated with this customer.</param>
		/// <returns>The id of the newly created billing services customer.</returns>
		BillingServicesCustomerId CreateCustomer(string email, BillingServicesToken token);

		/// <summary>
		/// Retrieves a billing services customer.
		/// </summary>
		/// <param name="customerId">The id of the customer to retrieve.</param>
		/// <returns>An info object for the billing services customer.</returns>
		BillingServicesCustomer RetrieveCustomer(BillingServicesCustomerId customerId);

		/// <summary>
		/// Lists the customers for a billing service.
		/// </summary>
		/// <returns>A list of info objects for billins services customers for the service.</returns>
		List<BillingServicesCustomer> ListCustomers();

		/// <summary>
		/// Updates a billing services customer.
		/// </summary>
		/// <param name="customerId">The customer id of the customer to update.</param>
		/// <param name="token">The billings services customer's token.</param>
		/// <returns>A bool representing the success state of updating the customer.</returns>
		bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token);

		/// <summary>
		/// Deletes a billing services customer.
		/// </summary>
		/// <returns>A bool representing the success state of deleting the customer.</returns>
		bool DeleteCustomer();
		#endregion customers

		#region subscriptions
		/// <summary>
		/// Creates a billing services subscription.
		/// </summary>
		/// <param name="amount">The amount for the new billing services subscription.</param>
		/// <param name="interval">The billing interval for the new billing services subscription.</param>
		/// <param name="planName">The name of the plan associated with the new billing services subscription.</param>
		/// <param name="customerId">The billing services customer id of the customer subscribing to a new billing service.</param>
		/// <returns>The id of the newly created billing services subscription.</returns>
		BillingServicesSubscriptionId CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId);

		/// <summary>
		/// Retrieves a billing services subscription.
		/// </summary>
		/// <returns>An info object for the requested billing services subscription.</returns>
		BillingServicesSubscription RetrieveSubscription();

		/// <summary>
		/// Lists the billing services subscriptions for a billing service.
		/// </summary>
		/// <returns>A list of info objects for Billing services subsriptions for a billing service.</returns>
		List<BillingServicesSubscription> ListSubscriptions();

		/// <summary>
		/// Updates a billing services subscription.
		/// </summary>
		/// <param name="amount">The new amount for the billing services subscription.</param>
		/// <param name="interval">The new billing interval for the billing services subscription.</param>
		/// <param name="planName">The new plan name for the billing services subscription.</param>
		/// <param name="subscriptionId">The id of the billing services subscription to ID.</param>
		/// <param name="customerId">The id of the customer whom is associated witht the billing subscription being updated.</param>
		/// <returns>A bool representing the success state of updating the subscription.</returns>
		bool UpdateSubscription(int amount, string interval, string planName, string subscriptionId, BillingServicesCustomerId customerId);

		/// <summary>
		/// Deletes a Billing services subscription.
		/// </summary>
		/// <param name="customerId">The billing services customer id associated with the subscription to delete.</param>
		/// <param name="subscriptionId">The id of the billing services subscription to delete.</param>
		void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId);
		#endregion subscriptions

		#region charges
		/// <summary>
		/// Creates a billing services charge.
		/// </summary>
		/// <returns>A bool representing the success state of creating the charge.</returns>
		bool CreateCharge();

		/// <summary>
		/// Retrieves a billing services charge.
		/// </summary>
		/// <returns>An info object for a billing services charge.</returns>
		BillingServicesCharge RetrieveCharge();

		/// <summary>
		/// Lists the billing service charges for a billing service customer.
		/// </summary>
		/// <param name="customerId">The billing services customer id of the customer for which the charges shall be listed.</param>
		/// <returns>A list of info objects for billing services charges.</returns>
		List<BillingServicesCharge> ListCharges(BillingServicesCustomerId customerId);
		#endregion charges

		#region invoices
		/// <summary>
		/// Lists the billing services invoices for a billing services customer.
		/// </summary>
		/// <param name="customerId">The billing services customer id of the customer for which the invoices shall be listed.</param>
		/// <returns>A list of info objects for billing services invoices.</returns>
		List<BillingServicesInvoice> ListInvoices(BillingServicesCustomerId customerId);
		#endregion invoices
	}
}