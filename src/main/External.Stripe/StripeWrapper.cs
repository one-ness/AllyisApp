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

		public StripeWrapper()
		{
			CustomerService = new StripeCustomerService();
			PlanService = new StripePlanService();
			SubscriptionService = new StripeSubscriptionService();
		}

		public bool CreateCharge()
		{
			throw new NotImplementedException();
		}

		public bool CreateCustomer(string email, string billingId)
		{
			StripeCustomerCreateOptions customerOptions = new StripeCustomerCreateOptions();
			customerOptions.Email = email;
			customerOptions.SourceToken = billingId;

			CustomerService.Create(customerOptions);

			return true; // needs to return false if the operation fails
		}

		public bool CreatePlan(int amount, string interval, string planName)
		{
			Random r = new Random();
			int i = r.Next(100000000);  //wtf
			var newPlan = new StripePlanCreateOptions();
			newPlan.Name = planName;
			newPlan.StatementDescriptor = planName;
			newPlan.Amount = amount;           // all amounts on Stripe are in cents, pence, etc
			newPlan.Currency = "usd";        // "usd" only supported right now
			newPlan.Interval = interval;      // "month" or "year"
			newPlan.IntervalCount = 1;

			newPlan.Id = i.ToString();

			StripePlan response = PlanService.Create(newPlan);

			return true;
		}

		public bool CreateSubscription()
		{
			throw new NotImplementedException();
		}

		public bool DeleteCustomer()
		{
			throw new NotImplementedException();
		}

		public bool DeletePlan()
		{
			throw new NotImplementedException();
		}

		public bool DeleteSubscription()
		{
			throw new NotImplementedException();
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

		public BillingCustomer RetrieveCustomer(string customerId)
		{
			// there is almost definitely some exception handling that will need to be done here.

			var customerService = new StripeCustomerService();
			StripeCustomer stripeCustomer = customerService.Get(customerId);

			// need to determine what stripeCustomer info is needed.

			BillingCustomer customer = new BillingCustomer();
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

		public bool UpdateCustomer(string customerId)
		{
			var currentCustomer = new StripeCustomerUpdateOptions();

			// setting up the card
			currentCustomer.SourceToken = token.ToString();

			StripeCustomer stripeCustomer = CustomerService.Update(customerId, currentCustomer);
		}

		public bool UpdatePlan()
		{
			throw new NotImplementedException();
		}

		public bool UpdateSubscription(string subscriptionId, int amount, string interval, BillingCustomer customer, string planName)
		{
			Random r = new Random();
			int i = r.Next(100000000);
			var newPlan = new StripePlanCreateOptions();
			newPlan.Name = planName;
			newPlan.StatementDescriptor = planName;
			newPlan.Amount = amount;           // all amounts on Stripe are in cents, pence, etc
			newPlan.Currency = "usd";        // "usd" only supported right now
			newPlan.Interval = interval;      // "month" or "year"
			newPlan.IntervalCount = 1;

			newPlan.Id = i.ToString();

			StripePlan response = PlanService.Create(newPlan);

			var plan = new StripePlanUpdateOptions();

			plan.Name = newPlan.Name;

			var ss = new StripeSubscriptionUpdateOptions();
			ss.PlanId = newPlan.Id;

			var subscriptionService = new StripeSubscriptionService();
			StripeSubscription sub = subscriptionService.Get(customer.Id, subscriptionId);
			if (sub.TrialEnd != null)
			{
				ss.TrialEnd = sub.TrialEnd;
			}

			StripeSubscription stripeSubscription = subscriptionService.Update(customer.Id, subscriptionId, ss); // optional StripeSubscriptionUpdateOptions

			return true;
		}
	}
}
