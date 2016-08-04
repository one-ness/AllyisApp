using System;
using System.Collections.Generic;
using AllyisApps.BillingServices;
using AllyisApps.BillingServices.Common;
using AllyisApps.BillingServices.Common.Types;

namespace AllyisApps.BillingServices
{
	public class BillingServicesHandler : IBillingServicesInterface
	{
		public bool CreateCharge()
		{
			throw new NotImplementedException();
		}

		public bool CreateCustomer()
		{
			throw new NotImplementedException();
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
			BillingServicesEnum service = GetUsersBillingSystem();
			switch (service)
			{
				case BillingServicesEnum.Stripe:
					{
						return StripeService.StripeWrapper.RetrieveCustomer(id);
					}
				default:
					{
						throw new NotImplementedException(string.Format("Billing system {0} is not implemented", service.ToString()));
					}
			}
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

		private BillingServicesEnum GetUsersBillingSystem()
		{
			return BillingServicesEnum.Stripe;
		}
	}
}
