﻿using System;
using System.Collections.Generic;
using AllyisApps.BillingServices.Common;
using AllyisApps.BillingServices.Common.Types;
using Stripe;

namespace AllyisApps.BillingServices.StripeService
{
	/// <summary>
	/// 
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
		#endregion

		#region constructor
		/// <summary>
		/// 
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
		#endregion

		#region IBillingServicesInterface implementation
		#region plans
		/// <summary>
		/// 
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="interval"></param>
		/// <param name="planName"></param>
		/// <returns></returns>
		public bool CreatePlan(int amount, string interval, string planName)
		{
			this.CreateStripePlan(amount, interval, planName);

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingServicesPlan RetrievePlan()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool UpdatePlan()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool DeletePlan()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region customers
		/// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public BillingServicesCustomerId CreateCustomer(string email, BillingServicesToken token)
		{
			StripeCustomerCreateOptions customerOptions = new StripeCustomerCreateOptions();
			customerOptions.Email = email;
			customerOptions.SourceToken = this.GenerateStripeToken(token.Token).ToString();

			StripeCustomer customer = this.customerService.Create(customerOptions);

			return new BillingServicesCustomerId(customer.Id);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		public BillingServicesCustomer RetrieveCustomer(BillingServicesCustomerId customerId)
		{
			// there is almost definitely some exception handling that will need to be done here.
			StripeCustomer stripeCustomer = this.customerService.Get(customerId.Id);

			BillingServicesCustomer customer = new BillingServicesCustomer(
				customerId,
				last4: stripeCustomer.SourceList.Data[0].Last4);
			return customer;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<BillingServicesCustomer> ListCustomers()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token)
		{
			StripeCustomerUpdateOptions currentCustomer = new StripeCustomerUpdateOptions();

			currentCustomer.SourceToken = this.GenerateStripeToken(token.Token).ToString();

			this.customerService.Update(customerId.Id, currentCustomer);

			return true;  // need to return false if the operation fails.
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool DeleteCustomer()
		{
			throw new NotImplementedException();
		}
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
		public BillingServicesSubscriptionId CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId)
		{
			StripePlan newPlan = this.CreateStripePlan(amount, interval, planName);

			StripeSubscription sub = this.subscriptionService.Create(customerId.Id, newPlan.Id);

			return new BillingServicesSubscriptionId(sub.Id);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingServicesSubscription RetrieveSubscription()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<BillingServicesSubscription> ListSubscriptions()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="interval"></param>
		/// <param name="planName"></param>
		/// <param name="subscriptionId"></param>
		/// <param name="customerId"></param>
		/// <returns></returns>
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
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="subscriptionId"></param>
		public void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId)
		{
			this.subscriptionService.Cancel(customerId.Id, subscriptionId);
		}
		#endregion

		#region charges
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool CreateCharge()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingServicesCharge RetrieveCharge()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
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
		#endregion

		#region invoices
		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
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
		#endregion
		#endregion

		#region helper methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="billingServicesToken"></param>
		/// <returns></returns>
		private StripeToken GenerateStripeToken(string billingServicesToken)
		{
			return this.tokenService.Get(billingServicesToken);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="interval"></param>
		/// <param name="planName"></param>
		/// <returns></returns>
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
		#endregion
	}
}
