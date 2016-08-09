﻿using System;
using System.Collections.Generic;
using AllyisApps.BillingServices.Common;
using AllyisApps.BillingServices.Common.Types;
using Stripe;

namespace AllyisApps.BillingServices.StripeService
{
	public class StripeWrapper : IBillingServicesInterface
	{
		private readonly StripeCustomerService CustomerService;
		private readonly StripePlanService PlanService;
		private readonly StripeSubscriptionService SubscriptionService;
		private readonly StripeTokenService TokenService;
		private readonly StripeInvoiceService InvoiceService;
		private readonly StripeChargeService ChargeService;

		public StripeWrapper()
		{
			Stripe.StripeConfiguration.SetApiKey("sk_test_6Z1XooVuPiXjbn0DwndaHF8P");

			CustomerService = new StripeCustomerService();
			PlanService = new StripePlanService();
			SubscriptionService = new StripeSubscriptionService();
			TokenService = new StripeTokenService();
			InvoiceService = new StripeInvoiceService();
			ChargeService = new StripeChargeService();
		}

		public bool CreateCharge()
		{
			throw new NotImplementedException();
		}

		public BillingServicesCustomerId CreateCustomer(string email, BillingServicesToken token)
		{
			StripeCustomerCreateOptions customerOptions = new StripeCustomerCreateOptions();
			customerOptions.Email = email;
			customerOptions.SourceToken = this.GenerateStripeToken(token.Token).ToString();

			StripeCustomer customer = CustomerService.Create(customerOptions);

			return new BillingServicesCustomerId(customer.Id);
		}

		public bool CreatePlan(int amount, string interval, string planName)
		{
			CreateStripePlan(amount, interval, planName);

			return true;
		}

		public string CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId)
		{
			StripePlan newPlan = CreateStripePlan(amount, interval, planName);

			StripeSubscription sub = SubscriptionService.Create(customerId.Id, newPlan.Id);

			return sub.Id;
		}

		public bool DeleteCustomer()
		{
			throw new NotImplementedException();
		}

		public bool DeletePlan()
		{
			throw new NotImplementedException();
		}

		public void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId)
		{
			SubscriptionService.Cancel(customerId.Id, subscriptionId);
		}

		public List<BillingCustomer> ListCustomers()
		{
			throw new NotImplementedException();
		}

		public List<BillingSubscription> ListSubscriptions()
		{
			throw new NotImplementedException();
		}

		public BillingCharge RetrieveCharge()
		{
			throw new NotImplementedException();
		}

		public BillingCustomer RetrieveCustomer(BillingServicesCustomerId customerId)
		{
			// there is almost definitely some exception handling that will need to be done here.

			StripeCustomerService customerService = new StripeCustomerService();
			StripeCustomer stripeCustomer = customerService.Get(customerId.Id);

			BillingCustomer customer = new BillingCustomer(
				customerId,
				last4: stripeCustomer.SourceList.Data[0].Last4);
			return customer;
		}

		public BillingPlan RetrievePlan()
		{
			throw new NotImplementedException();
		}

		public BillingSubscription RetrieveSubscription()
		{
			throw new NotImplementedException();
		}

		public bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token)
		{
			StripeCustomerUpdateOptions currentCustomer = new StripeCustomerUpdateOptions();

			currentCustomer.SourceToken = this.GenerateStripeToken(token.Token).ToString();

			CustomerService.Update(customerId.Id, currentCustomer);

			return true;  // need to return false if the operation fails.
		}

		public bool UpdatePlan()
		{
			throw new NotImplementedException();
		}

		public bool UpdateSubscription(int amount, string interval, string planName, string subscriptionId, BillingServicesCustomerId customerId)
		{
			StripePlan newPlan = CreateStripePlan(amount, interval, planName);

			StripePlanUpdateOptions planUpdateOptions = new StripePlanUpdateOptions();

			planUpdateOptions.Name = newPlan.Name;

			StripeSubscriptionUpdateOptions subUpdateOptions = new StripeSubscriptionUpdateOptions();
			subUpdateOptions.PlanId = newPlan.Id;

			StripeSubscriptionService subscriptionService = new StripeSubscriptionService();
			StripeSubscription sub = subscriptionService.Get(customerId.Id, subscriptionId);
			if (sub.TrialEnd != null)
			{
				subUpdateOptions.TrialEnd = sub.TrialEnd;
			}

			StripeSubscription stripeSubscription = subscriptionService.Update(customerId.Id, subscriptionId, subUpdateOptions); // optional StripeSubscriptionUpdateOptions

			return true;
		}

		private StripeToken GenerateStripeToken(string billingServicesToken)
		{
			return TokenService.Get(billingServicesToken);
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

			return PlanService.Create(newPlan);
		}

		public List<BillingInvoice> ListInvoices(BillingServicesCustomerId customerId)
		{
			StripeInvoiceListOptions invoiceListOptions = new StripeInvoiceListOptions();
			invoiceListOptions.CustomerId = customerId.Id;
			invoiceListOptions.Limit = 10000;
			IEnumerable<StripeInvoice> stripeInvoices = InvoiceService.List(invoiceListOptions); // optional StripeInvoiceListOptions

			List<BillingInvoice> invoiceList = new List<BillingInvoice>();
			foreach(StripeInvoice stripeInvoice in stripeInvoices)
			{
				invoiceList.Add(new BillingInvoice());
			}

			return invoiceList;
		}

		public List<BillingCharge> ListCharges(BillingServicesCustomerId customerId)
		{
			var chargeService = new StripeChargeService();
			IEnumerable<StripeCharge> stripeCharges = chargeService.List(); // optional StripeChargeListOptions
			List<BillingCharge> billingCharges = new List<BillingCharge>();
			foreach (StripeCharge stripeCharge in stripeCharges)
			{
				if (stripeCharge.CustomerId == customerId.Id)
				{
					billingCharges.Add(new BillingCharge());
				}
			}

			return billingCharges;
		}
	}
}
