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

		public bool CreateCustomer()
		{
			return Service.CreateCustomer();
		}

		public bool CreatePlan()
		{
			return Service.CreatePlan();
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

		public BillingCustomer RetrieveCustomer(string id)
		{
			return Service.RetrieveCustomer(id);
		}

		public BillingPlan RetrievePlan()
		{
			return Service.RetrievePlan();
		}

		public BillingSubscription RetrieveSubscription()
		{
			return Service.RetrieveSubscription();
		}

		public bool UpdateCustomer()
		{
			return Service.UpdateCustomer();
		}

		public bool UpdatePlan()
		{
			return Service.UpdatePlan();
		}

		public bool UpdateSubscription()
		{
			return Service.UpdateSubscription();
		}
	}
}
