//------------------------------------------------------------------------------
// <copyright file="BillingServicesHandler.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services.Common;
using AllyisApps.Services.Common.Types;

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// Interfaces between the CrmService and the actual billing service implementation wrapper so that the CrmService
	/// need not care beyond informing the handler which Service the user has selected.  Builds the appropriate wrapper that implements
	/// the billing service interface and forwards the call along to it.
	/// </summary>
	public class BillingServicesHandler : IBillingServicesInterface
	{
		#region fields

		private readonly IBillingServicesInterface service;

		#endregion fields

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BillingServicesHandler"/> class.
		/// Given the input of which service the user has chosen, builds the appropriate BillingServiceInterface implementing wrapper object.
		/// </summary>
		/// <param name="serviceType">The service as a string.</param>
		public BillingServicesHandler(string serviceType)
		{
			// The passed in service name should exactly match the entries in the BillingServiceEnum as it is the record keeper of which services are supported.
			if (Enum.IsDefined(typeof(BillingServicesEnum), serviceType))
			{
				BillingServicesEnum serviceTypeAsEnum = (BillingServicesEnum)Enum.Parse(typeof(BillingServicesEnum), serviceType);
				switch (serviceTypeAsEnum)
				{
					case BillingServicesEnum.Stripe:
						{
							this.service = new BillingServices.StripeService.StripeWrapper();
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

		#endregion constructor

		#region IBillingServicesInterface implementation

		#region plans

		/// <summary>
		/// Calls the CreatePlan method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="amount">The cost of the new plan.</param>
		/// <param name="interval">The new plan's billing interval.</param>
		/// <param name="planName">The name of the new plan.</param>
		/// <returns>A bool representing the success or failure state of the action.</returns>
		public bool CreatePlan(int amount, string interval, string planName)
		{
			return this.service.CreatePlan(amount, interval, planName);
		}

		/// <summary>
		/// Calls the RetrievePlan method in the appropriate BillingServicesInterface implementing object.
		/// Currently not supported.
		/// </summary>
		/// <returns>The requested BillingPlan.</returns>
		[CLSCompliant(false)]
		public BillingServicesPlan RetrievePlan()
		{
			return this.service.RetrievePlan();
		}

		/// <summary>
		/// Calls the UpdatePlan method in the appropriate BillingServicesInterface implementing object.
		/// Currently not supported.
		/// </summary>
		/// <returns>A bool representing the success or failure state of the action.</returns>
		public bool UpdatePlan()
		{
			return this.service.UpdatePlan();
		}

		/// <summary>
		/// Calls the DeletePlan method in the appropriate BillingServicesInterface implementing object.
		/// Currently not supported.
		/// </summary>
		/// <returns>A bool representing the success or failure state of the action.</returns>
		public bool DeletePlan()
		{
			return this.service.DeletePlan();
		}

		#endregion plans

		#region customers

		/// <summary>
		/// Calls the CreateCustomer method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="email">The user's email address being used with the billing service.</param>
		/// <param name="token">The billing service token to use with this customer.</param>
		/// <returns>The customer id assigned to the new customer.</returns>
		[CLSCompliant(false)]
		public BillingServicesCustomerId CreateCustomer(string email, BillingServicesToken token)
		{
			return this.service.CreateCustomer(email, token);
		}

		/// <summary>
		/// Calls the RetrieveCustomer method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="customerId">The customer id associated with the customer you are trying to retrieve.</param>
		/// <returns>The billing customer object.</returns>
		[CLSCompliant(false)]
		public BillingServicesCustomer RetrieveCustomer(BillingServicesCustomerId customerId)
		{
			return this.service.RetrieveCustomer(customerId);
		}

		/// <summary>
		/// Calls the ListCustomers method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <returns>A list of BillingCustomer objects.</returns>
		[CLSCompliant(false)]
		public List<BillingServicesCustomer> ListCustomers()
		{
			return this.service.ListCustomers();
		}

		/// <summary>
		/// Calls the UpdateCustomer method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="customerId">The id of the customer being updated.</param>
		/// <param name="token">The token for the customer being updated.</param>
		/// <returns>A bool representing the success or failure state of the action.</returns>
		[CLSCompliant(false)]
		public bool UpdateCustomer(BillingServicesCustomerId customerId, BillingServicesToken token)
		{
			return this.service.UpdateCustomer(customerId, token);
		}

		/// <summary>
		/// Calls the DeleteCustomer method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <returns>A bool representing the success or failure state of the action.</returns>
		public bool DeleteCustomer()
		{
			return this.service.DeleteCustomer();
		}

		#endregion customers

		#region subscriptions

		/// <summary>
		/// Calls the CreateSubscription method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="amount">The subscription cost.</param>
		/// <param name="interval">The subscription billing interval.</param>
		/// <param name="planName">The plan name for the subscription.</param>
		/// <param name="customerId">The customer id the subscription should be associated with.</param>
		/// <returns>The subscription id of the newly screated subscription.</returns>
		[CLSCompliant(false)]
		public BillingServicesSubscriptionId CreateSubscription(int amount, string interval, string planName, BillingServicesCustomerId customerId)
		{
			return this.service.CreateSubscription(amount, interval, planName, customerId);
		}

		/// <summary>
		/// Calls the RetrieveSubscription method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <returns>The billing subscription object.</returns>
		[CLSCompliant(false)]
		public BillingServicesSubscription RetrieveSubscription()
		{
			return this.service.RetrieveSubscription();
		}

		/// <summary>
		/// Calls the ListSubscriptions method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <returns>A list of billing subscription objects.</returns>
		[CLSCompliant(false)]
		public List<BillingServicesSubscription> ListSubscriptions()
		{
			return this.service.ListSubscriptions();
		}

		/// <summary>
		/// Calls the UpdateSubscription method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="amount">The cost of the subscription.</param>
		/// <param name="interval">The billing interval of the subscription.</param>
		/// <param name="planName">The name of the plan asscociated with the subscription.</param>
		/// <param name="subscriptionId">The id of the subscription to be updated.</param>
		/// <param name="customerId">The customer id associated with the subscription.</param>
		/// <returns>A bool representing the success or failure state of the action.</returns>
		[CLSCompliant(false)]
		public bool UpdateSubscription(int amount, string interval, string planName, string subscriptionId, BillingServicesCustomerId customerId)
		{
			return this.service.UpdateSubscription(amount, interval, planName, subscriptionId, customerId);
		}

		/// <summary>
		/// Calls the DeleteSubscription method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="customerId">The customer id associcated with the subscription.</param>
		/// <param name="subscriptionId">The id of the subscription to delete.</param>
		[CLSCompliant(false)]
		public void DeleteSubscription(BillingServicesCustomerId customerId, string subscriptionId)
		{
			this.service.DeleteSubscription(customerId, subscriptionId);
		}

		#endregion subscriptions

		#region charges

		/// <summary>
		/// Calls the CreateCharge method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <returns>A bool representing the success or failure state of the action.</returns>
		public bool CreateCharge()
		{
			return this.service.CreateCharge();
		}

		/// <summary>
		/// Calls the RetrieveCharge method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <returns>The billing charge.</returns>
		[CLSCompliant(false)]
		public BillingServicesCharge RetrieveCharge()
		{
			return this.RetrieveCharge();
		}

		/// <summary>
		/// Calls the ListCharges method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="customerId">The customer id for which the charges shall be listed.</param>
		/// <returns>A list of Billing charge objects.</returns>
		[CLSCompliant(false)]
		public List<BillingServicesCharge> ListCharges(BillingServicesCustomerId customerId)
		{
			return this.service.ListCharges(customerId);
		}

		#endregion charges

		#region invoices

		/// <summary>
		/// Calls the ListInvoices method in the appropriate BillingServicesInterface implementing object.
		/// </summary>
		/// <param name="customerId">The customer Id for the customer whose invoices are being requested.</param>
		/// <returns>A list of billing invoice objects.</returns>
		[CLSCompliant(false)]
		public List<BillingServicesInvoice> ListInvoices(BillingServicesCustomerId customerId)
		{
			return this.service.ListInvoices(customerId);
		}

		#endregion invoices

		#endregion IBillingServicesInterface implementation
	}
}