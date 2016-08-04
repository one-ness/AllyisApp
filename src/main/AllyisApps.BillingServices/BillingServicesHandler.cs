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
			return Service.RetrieveCustomer(id);
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
