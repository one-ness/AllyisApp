using System;
using System.Collections.Generic;
using AllyisApps.BillingServices;
using AllyisApps.BillingServices.Common;
using AllyisApps.BillingServices.Common.Types;

namespace AllyisApps.BillingServices
{
	public class BillingServicesHandler : IBillingServicesInterface
	{
		private readonly IBillingServicesInterface Service;

		public BillingServicesHandler(string service)
		{
			if (Enum.IsDefined(typeof(BillingServicesEnum), service))
			{
				BillingServicesEnum ServiceType = (BillingServicesEnum)Enum.Parse(typeof(BillingServicesEnum), service);
				switch (ServiceType)
				{
					case (BillingServicesEnum.Stripe):
						{
							Service = new StripeService.StripeWrapper();
							break;
						}
					default:
						{
							throw new NotImplementedException(string.Format("Billing system, {0}, is not implemented", service));
						}
				}
			}
			else
			{
				throw new NotImplementedException(string.Format("Billing system, {0}, is not implemented", service));
			}
		}

		public bool CreateCharge()
		{
			return Service.CreateCharge();
		}

		public string CreateCustomer(string email, BillingServicesToken token)
		{
			return Service.CreateCustomer(email, token);
		}

		public bool CreatePlan(int amount, string interval, string planName)
		{
			return Service.CreatePlan(amount, interval, planName);
		}

		public bool CreateSubscription()
		{
			return Service.CreateSubscription();
		}

		public bool DeleteCustomer()
		{
			return Service.DeleteCustomer();
		}

		public bool DeletePlan()
		{
			return Service.DeletePlan();
		}

		public bool DeleteSubscription()
		{
			return Service.DeleteSubscription();
		}

		public List<BillingCustomer> ListCustomers()
		{
			return Service.ListCustomers();
		}

		public List<BillingSubscription> ListSubscriptions()
		{
			return Service.ListSubscriptions();
		}

		public BillingCharge RetrieveCharge()
		{
			return Service.RetrieveCharge();
		}

		public BillingCustomer RetrieveCustomer(string customerId)
		{
			return Service.RetrieveCustomer(customerId);
		}

		public BillingPlan RetrievePlan()
		{
			return Service.RetrievePlan();
		}

		public BillingSubscription RetrieveSubscription()
		{
			return Service.RetrieveSubscription();
		}

		public bool UpdateCustomer(string customerId, BillingServicesToken token)
		{
			return Service.UpdateCustomer(customerId, token);
		}

		public bool UpdatePlan()
		{
			return Service.UpdatePlan();
		}

		public bool UpdateSubscription(string subscriptionId, int amount, string interval, BillingCustomer customer, string planName)
		{
			return Service.UpdateSubscription(subscriptionId, amount, interval, customer, planName);
		}
	}
}
