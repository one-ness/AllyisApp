//------------------------------------------------------------------------------
// <copyright file="StripeWrapper.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services.Common;
using AllyisApps.Services.Common.Types;
using Stripe;

namespace BillingServices.StripeService
{
	/// <summary>
	/// The Stripe Billing Service implementation of the IBillingServicesInterface.
	/// </summary>
	public class StripeWrapper : IBillingServicesInterface
	{
		#region fields
		private readonly StripeCustomerService customerService;
		private readonly StripePlanService planService;
		private readonly StripeSubscriptionService subscriptionService;
		private readonly StripeTokenService tokenService;
		private readonly StripeInvoiceService invoiceService;
		private readonly StripeChargeService chargeService;
		#endregion fields

		#region constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="StripeWrapper"/> class.
		/// </summary>
		public StripeWrapper()
		{
			Stripe.StripeConfiguration.SetApiKey("sk_test_6Z1XooVuPiXjbn0DwndaHF8P");

			this.customerService = new StripeCustomerService();
			this.planService = new StripePlanService();
			this.subscriptionService = new StripeSubscriptionService();
			this.tokenService = new StripeTokenService();
			this.invoiceService = new StripeInvoiceService();
			this.chargeService = new StripeChargeService();
		}
		#endregion constructor

		#region IBillingServicesInterface implementation
		#region plans
		/// <summary>
		/// Creates a stripe plan.  Plans are per offering, i.e. Time Tracker, Consulting, and etc.
		/// </summary>
		/// <param name="amount">The plan amount.</param>
		/// <param name="interval">The plan billing interval.</param>
		/// <param name="planName">The plan name.</param>
		/// <returns>A bool representing the success state of the plan creation.</returns>
		public bool CreatePlan(int amount, string interval, string planName)
		{
			this.CreateStripePlan(amount, interval, planName);

			return true;
		}

		/// <summary>
		/// Retrieves a stripe plan.
		/// </summary>
		/// <returns>An info object for the stripe plan.</returns>
		public BillingServicesPlan RetrievePlan()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates the stripe Plan.
		/// </summary>
		/// <returns>A bool representing the success state of updating the plan.</returns>
		public bool UpdatePlan()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Deletes a stripe plan.
		/// </summary>
		/// <returns>A bool representing the success state of deleting the plan.</returns>
		public bool DeletePlan()
		{
			throw new NotImplementedException();
		}
		#endregion plans

		#region customers
		/// <summary>
		/// Creates a stripe customer.
		/// </summary>
		/// <param name="email">The email to be associated with this customer.</param>
		/// <param name="token">A billiong services token to be associated with this customer.</param>
		/// <returns>The id of the newly created stripe customer.</returns>
		public BillingServicesCustomerId CreateCustomer(string email, BillingServicesToken token)
		{
			StripeCustomerCreateOptions customerOptions = new StripeCustomerCreateOptions();
			customerOptions.Email = email;
			customerOptions.SourceToken = this.GenerateStripeToken(token.Token).ToString();

			StripeCustomer customer = this.customerService.Create(customerOptions);

			return new BillingServicesCustomerId(customer.Id);
		}

		/// <summary>
		/// Retrieves a stripe customer.
		/// </summary>
		/// <param name="customerId">The id of the customer to retrieve.</param>
		/// <returns>An info object for the stripe customer.</returns>
		public BillingServicesCustomer RetrieveCustomer(BillingServicesCustomerId customerId)
		{
			// there is almost definitely some exception handling that will need to be done here.
			StripeCustomer stripeCustomer = this.customerService.Get(customerId.Id);

			BillingServicesCustomer customer = new BillingServicesCustomer(
				customerId,
				stripeCustomer.Email,
				last4: stripeCustomer.SourceList.Data[0].Last4);
			return customer;
		}

		/// <summary>
		/// Lists the customers for stripe.
		/// </summary>
		/// <returns>A list of info objects for stripe customers.</returns>
		public List<BillingServicesCustomer> ListCustomers()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a stripe customer.
		/// </summary>
		/// <param name="customerId">The customer id of the customer to update.</param>
		/// <param name="token">The stripe customer's token.</param>
		/// <returns>A bool representing the success state of updating the customer.</returns>
		public bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token)
		{
			StripeCustomerUpdateOptions currentCustomer = new StripeCustomerUpdateOptions();

			currentCustomer.SourceToken = this.GenerateStripeToken(token.Token).ToString();

			this.customerService.Update(customerId.Id, currentCustomer);

			return true;  // need to return false if the operation fails.
		}

		/// <summary>
		/// Deletes a stripe customer.
		/// </summary>
		/// <returns>A bool representing the success state of deleting the customer.</returns>
		public bool DeleteCustomer()
		{
			throw new NotImplementedException();
		}
		#endregion customers

		#region subscriptions
		/// <summary>
		/// Creates a stripe subscription.
		/// </summary>
		/// <param name="amount">The amount for the new stripe subscription.</param>
		/// <param name="interval">The billing interval for the new stripe subscription.</param>
		/// <param name="planName">The name of the plan associated with the new stripe subscription.</param>
		/// <param name="customerId">The stripe customer id of the customer subscribing to a new billing service.</param>
		/// <returns>The id of the newly created stripe subscription.</returns>
		public BillingServicesSubscriptionId CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId)
		{
			StripePlan newPlan = this.CreateStripePlan(amount, interval, planName);

			StripeSubscription sub = this.subscriptionService.Create(customerId.Id, newPlan.Id);

			return new BillingServicesSubscriptionId(sub.Id);
		}

		/// <summary>
		/// Retrieves a stripe subscription.
		/// </summary>
		/// <returns>An info object for the requested stripe subscription.</returns>
		public BillingServicesSubscription RetrieveSubscription()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Lists the stripe subscriptions for stripe.
		/// </summary>
		/// <returns>A list of info objects for stripe subsriptions for stripe.</returns>
		public List<BillingServicesSubscription> ListSubscriptions()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a stripe subscription.
		/// </summary>
		/// <param name="amount">The new amount for the stripe subscription.</param>
		/// <param name="interval">The new billing interval for the stripe subscription.</param>
		/// <param name="planName">The new plan name for the stripe subscription.</param>
		/// <param name="subscriptionId">The id of the stripe subscription to ID.</param>
		/// <param name="customerId">The id of the customer whom is associated witht the billing subscription being updated.</param>
		/// <returns>A bool representing the success state of updating the subscription.</returns>
		public bool UpdateSubscription(int amount, string interval, string planName, string subscriptionId, BillingServicesCustomerId customerId)
		{
			StripePlan newPlan = this.CreateStripePlan(amount, interval, planName);

			StripePlanUpdateOptions planUpdateOptions = new StripePlanUpdateOptions();

			planUpdateOptions.Name = newPlan.Name;

			StripeSubscriptionUpdateOptions subUpdateOptions = new StripeSubscriptionUpdateOptions();
			subUpdateOptions.PlanId = newPlan.Id;

			StripeSubscription sub = this.subscriptionService.Get(customerId.Id, subscriptionId);
			if (sub.TrialEnd != null)
			{
				subUpdateOptions.TrialEnd = sub.TrialEnd;
			}

			StripeSubscription stripeSubscription = this.subscriptionService.Update(customerId.Id, subscriptionId, subUpdateOptions); // optional StripeSubscriptionUpdateOptions

			return true;
		}

		/// <summary>
		/// Deletes a stripe subscription.
		/// </summary>
		/// <param name="customerId">The stripe customer id associated with the subscription to delete.</param>
		/// <param name="subscriptionId">The id of the stripe subscription to delete.</param>
		public void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId)
		{
			this.subscriptionService.Cancel(customerId.Id, subscriptionId);
		}
		#endregion subscriptions

		#region charges
		/// <summary>
		/// Creates a stripe charge.
		/// </summary>
		/// <returns>A bool representing the success state of creating the charge.</returns>
		public bool CreateCharge()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a stripe charge.
		/// </summary>
		/// <returns>An info object for a stripe charge.</returns>
		public BillingServicesCharge RetrieveCharge()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Lists the stripe charges for a stripe customer.
		/// </summary>
		/// <param name="customerId">The stripe customer id of the customer for which the charges shall be listed.</param>
		/// <returns>A list of info objects for stripe charges.</returns>
		public List<BillingServicesCharge> ListCharges(BillingServicesCustomerId customerId)
		{
			var chargeService = new StripeChargeService();
			IEnumerable<StripeCharge> stripeCharges = this.chargeService.List(); // optional StripeChargeListOptions
			List<BillingServicesCharge> billingCharges = new List<BillingServicesCharge>();
			foreach (StripeCharge stripeCharge in stripeCharges)
			{
				if (stripeCharge.CustomerId == customerId.Id)
				{
					billingCharges.Add(new BillingServicesCharge(stripeCharge.Amount, stripeCharge.Created, stripeCharge.Id, stripeCharge.StatementDescriptor));
				}
			}

			return billingCharges;
		}
		#endregion charges

		#region invoices
		/// <summary>
		/// Lists the stripe invoices for a stripe customer.
		/// </summary>
		/// <param name="customerId">The stripe customer id of the customer for which the invoices shall be listed.</param>
		/// <returns>A list of info objects for stripe invoices.</returns>
		public List<BillingServicesInvoice> ListInvoices(BillingServicesCustomerId customerId)
		{
			StripeInvoiceListOptions invoiceListOptions = new StripeInvoiceListOptions();
			invoiceListOptions.CustomerId = customerId.Id;
			invoiceListOptions.Limit = 10000;
			IEnumerable<StripeInvoice> stripeInvoices = this.invoiceService.List(invoiceListOptions); // optional StripeInvoiceListOptions

			List<BillingServicesInvoice> invoiceList = new List<BillingServicesInvoice>();
			foreach (StripeInvoice stripeInvoice in stripeInvoices)
			{
				invoiceList.Add(new BillingServicesInvoice(stripeInvoice.AmountDue, stripeInvoice.Date, stripeInvoice.Id, stripeInvoice.StripeInvoiceLineItems.Data[0].Plan.Name, "Stripe"));
			}

			return invoiceList;
		}
		#endregion invoices
		#endregion IBillingServicesInterface implementation

		#region helper methods
		private StripeToken GenerateStripeToken(string billingServicesToken)
		{
			return this.tokenService.Get(billingServicesToken);
		}

		private StripePlan CreateStripePlan(int amount, string interval, string planName)
		{
			Random r = new Random();
			int i = r.Next(100000000);
			StripePlanCreateOptions newPlan = new StripePlanCreateOptions();
			newPlan.Name = planName;
			newPlan.StatementDescriptor = planName;
			newPlan.Amount = amount;           // all amounts on Stripe are in cents, pence, etc
			newPlan.Currency = "usd";        // "usd" only supported right now
			newPlan.Interval = interval;      // "month" or "year"
			newPlan.IntervalCount = 1;

			newPlan.Id = i.ToString();

			return this.planService.Create(newPlan);
		}
		#endregion helper methods
	}
}