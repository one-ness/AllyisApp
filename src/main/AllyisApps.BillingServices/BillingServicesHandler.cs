using System;
using System.Collections.Generic;
using AllyisApps.BillingServices;
using AllyisApps.BillingServices.Common;
using AllyisApps.BillingServices.Common.Types;

namespace AllyisApps.BillingServices
{
	/// <summary>
	/// 
	/// </summary>
	public class BillingServicesHandler : IBillingServicesInterface
	{
		#region fields
		private readonly IBillingServicesInterface service;
		#endregion

		#region constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		public BillingServicesHandler(string serviceType)
		{
			if (Enum.IsDefined(typeof(BillingServicesEnum), serviceType))
			{
				BillingServicesEnum serviceTypeAsEnum = (BillingServicesEnum)Enum.Parse(typeof(BillingServicesEnum), serviceType);
				switch (serviceTypeAsEnum)
				{
					case BillingServicesEnum.Stripe:
						{
							this.service = new StripeService.StripeWrapper();
							break;
						}

					default:
						{
							throw new NotImplementedException(string.Format("Billing system, {0}, is not implemented", serviceType));
						}
				}
			}
			else
			{
				throw new NotImplementedException(string.Format("Billing system, {0}, is not implemented", serviceType));
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
			return this.service.CreatePlan(amount, interval, planName);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingPlan RetrievePlan()
		{
			return this.service.RetrievePlan();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool UpdatePlan()
		{
			return this.service.UpdatePlan();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool DeletePlan()
		{
			return this.service.DeletePlan();
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
			return this.service.CreateCustomer(email, token);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		public BillingCustomer RetrieveCustomer(BillingServicesCustomerId customerId)
		{
			return this.service.RetrieveCustomer(customerId);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<BillingCustomer> ListCustomers()
		{
			return this.service.ListCustomers();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token)
		{
			return this.service.UpdateCustomer(customerId, token);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool DeleteCustomer()
		{
			return this.service.DeleteCustomer();
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
			return this.service.CreateSubscription(amount, interval, planName, customerId);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingSubscription RetrieveSubscription()
		{
			return this.service.RetrieveSubscription();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<BillingSubscription> ListSubscriptions()
		{
			return this.service.ListSubscriptions();
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
			return this.service.UpdateSubscription(amount, interval, planName, subscriptionId, customerId);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="subscriptionId"></param>
		public void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId)
		{
			this.service.DeleteSubscription(customerId, subscriptionId);
		}

		#endregion

		#region charges
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool CreateCharge()
		{
			return this.service.CreateCharge();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillingCharge RetrieveCharge()
		{
			return this.RetrieveCharge();
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		public List<BillingCharge> ListCharges(BillingServicesCustomerId customerId)
		{
			return this.service.ListCharges(customerId);
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
			return this.service.ListInvoices(customerId);
		}
		#endregion
		#endregion
	}
}