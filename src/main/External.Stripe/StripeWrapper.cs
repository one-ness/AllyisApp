using System;
using System.Collections.Generic;
using AllyisApps.BillingServices.Common;
using AllyisApps.BillingServices.Common.Types;
using Stripe;

namespace AllyisApps.BillingServices.StripeService
{
	public class StripeWrapper : IBillingServicesInterface
	{
		private readonly StripeCustomerService CustomerService;
		public StripeWrapper()
		{
			CustomerService = new StripeCustomerService();
		}

		public bool CreateCharge()
		{
			throw new NotImplementedException();
		}

		public BillingCustomer CreateCustomer(string email, string billingId)
		{
			StripeCustomerCreateOptions customerOptions = new StripeCustomerCreateOptions();
			customerOptions.Email = email;
			customerOptions.SourceToken = billingId;

			StripeCustomer stripeCustomer = CustomerService.Create(customerOptions);

			// need to determine the glue that will bind a stripe customer into a BillingCustomer.

			BillingCustomer customer = new BillingCustomer();

			return customer;
		}

		public bool CreatePlan()
		{
			throw new NotImplementedException();
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

		public BillingCustomer RetrieveCustomer(string id)
		{
			// there is almost definitely some exception handling that will need to be done here.

			var customerService = new StripeCustomerService();
			StripeCustomer stripeCustomer = customerService.Get(id);

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

		public bool UpdateCustomer()
		{
			throw new NotImplementedException();
		}

		public bool UpdatePlan()
		{
			throw new NotImplementedException();
		}

		public bool UpdateSubscription()
		{
			throw new NotImplementedException();
		}
	}
}
