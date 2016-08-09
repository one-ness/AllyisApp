using System;
using System.Collections.Generic;
using AllyisApps.BillingServices;
using AllyisApps.BillingServices.Common;
using AllyisApps.BillingServices.Common.Types;

namespace AllyisApps.BillingServices
{
	public class BillingServicesHandler : IBillingServicesInterface
	{
		#region fields
		private readonly IBillingServicesInterface Service;
		#endregion

		#region constructor
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
			return Service.CreatePlan(amount, interval, planName);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingPlan RetrievePlan()
		{
			return Service.RetrievePlan();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool UpdatePlan()
		{
			return Service.UpdatePlan();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool DeletePlan()
		{
			return Service.DeletePlan();
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
			return Service.CreateCustomer(email, token);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		public BillingCustomer RetrieveCustomer(BillingServicesCustomerId customerId)
		{
			return Service.RetrieveCustomer(customerId);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<BillingCustomer> ListCustomers()
		{
			return Service.ListCustomers();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token)
		{
			return Service.UpdateCustomer(customerId, token);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool DeleteCustomer()
		{
			return Service.DeleteCustomer();
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
		public string CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId)
		{
			return Service.CreateSubscription(amount, interval, planName, customerId);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingSubscription RetrieveSubscription()
		{
			return Service.RetrieveSubscription();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<BillingSubscription> ListSubscriptions()
		{
			return Service.ListSubscriptions();
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
			return Service.UpdateSubscription(amount, interval, planName, subscriptionId, customerId);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="subscriptionId"></param>
		public void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId)
		{
			Service.DeleteSubscription(customerId, subscriptionId);
		}

		#endregion

		#region charges
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool CreateCharge()
		{
			return Service.CreateCharge();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingCharge RetrieveCharge()
		{
			return Service.RetrieveCharge();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		public List<BillingCharge> ListCharges(BillingServicesCustomerId customerId)
		{
			return Service.ListCharges(customerId);
		}
		#endregion

		#region invoices
		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		public List<BillingInvoice> ListInvoices(BillingServicesCustomerId customerId)
		{
			return Service.ListInvoices(customerId);
		}
		#endregion
		#endregion
	}
}