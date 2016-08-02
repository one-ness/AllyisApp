//------------------------------------------------------------------------------
// <copyright file="StripeWrapper.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stripe;

namespace AllyisApps.BillingServices.StripeService
{
	/// <summary>
	/// Wrapper class for the Stripe library. 
	/// </summary>
	public class StripeWrapper
	{
		/// <summary>
		/// Creates a Customer from a Token.
		/// </summary>
		/// <param name="email">The Customer's email.</param>
		/// <param name="tokenid">The Token id.</param>
		/// <returns>The created StripeCustomer object.</returns>
		[CLSCompliant(false)]
		public static Stripe.StripeCustomer CreateCustomer(string email, string tokenid)
		{
			var customer = new StripeCustomerCreateOptions();
			customer.Email = email;
			customer.SourceToken = tokenid;

			var customerService = new StripeCustomerService();
			return customerService.Create(customer);
		}

		/// <summary>
		/// Static method to create a stripe card. 
		/// </summary>
		/// <param name="number">Card number.</param>
		/// <param name="expirationYear">Card expiration year.</param>
		/// <param name="expirationMonth">Card expiration month.</param>
		/// <param name="cvc">Card cvc number.</param>
		/// <param name="cust">A Customer obejct.</param>
		/// <returns>A stripe card.</returns>
		[CLSCompliant(false)]
		public static StripeCard CreateCard(string number, string expirationYear, string expirationMonth, string cvc, StripeCustomer cust)
		{
			var card = new StripeCardCreateOptions();
			card.SourceCard = new SourceCard()
			{
				Number = number,
				ExpirationYear = expirationYear,
				ExpirationMonth = expirationMonth,
				Cvc = cvc
			};

			var cardService = new StripeCardService();
			return cardService.Create(cust.Id, card);
		}

		/// <summary>
		/// Creates a charge.
		/// </summary>
		/// <param name="cust">The Customer being charged.</param>
		/// <param name="amount">The amount being charged.</param>
		/// <param name="currency">The currency being paid in.</param>
		/// <returns>A StripeCharge object.</returns>
		[CLSCompliant(false)]
		public static StripeCharge ChargeCustomer(StripeCustomer cust, int amount, string currency)
		{
			var charge = new StripeChargeCreateOptions();

			charge.Amount = amount;
			charge.Currency = currency;

			charge.CustomerId = cust.Id;

			charge.Capture = true;
			var chargeService = new StripeChargeService();
			return chargeService.Create(charge);
		}

		/// <summary>
		/// Method for adding Customer to a subscription.
		/// </summary>
		/// <param name="interval">The charge interval.</param>
		/// <param name="amount">The amount to be charged.</param>
		/// <param name="cust">A reference to the Customer object provided by Stripe.</param>
		/// <param name="planName">The name of the subscription plan, to appear on Stripe invoices.</param>
		/// <returns>Created Sstripe sub_id.</returns>
		[CLSCompliant(false)]
		public static string Subscription(string interval, int amount, StripeCustomer cust, string planName)
		{
			Random r = new Random();
			int i = r.Next(100000000);
			var plan = new StripePlanCreateOptions();
			plan.Name = planName;
			plan.StatementDescriptor = planName;
			plan.Amount = amount;           // all amounts on Stripe are in cents, pence, etc
			plan.Currency = "usd";        // "usd" only supported right now
			plan.Interval = interval;      // "month" or "year"
			plan.IntervalCount = 1;

			int trial = DaysUntilEndOfMonth();

			plan.TrialPeriodDays = trial;
			plan.Id = i.ToString();
			var planService = new StripePlanService();
			StripePlan response = planService.Create(plan);
			var subscriptionservice = new StripeSubscriptionService();
			StripeSubscription sub = subscriptionservice.Create(cust.Id, response.Id);

			return sub.Id;
		}

		/// <summary>
		/// Updates a Customer's subscription.
		/// </summary>
		/// <param name="id">The id of the subscription to be updated.</param>
		/// <param name="amount">The amount to be charged.</param>
		/// <param name="interval">The charge interval.</param>
		/// <param name="cust">The stripe Customer in question.</param>
		/// <param name="planName">Name of subscription plan, to appear on Stripe invoices.</param>
		/// <returns>Stripe subscription Id for the updated subscription.</returns>
		[CLSCompliant(false)]
		public static string SubscriptionUpdate(string id, int amount, string interval, StripeCustomer cust, string planName)
		{
			var planService = new StripePlanService();

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

			StripePlan response = planService.Create(newPlan);

			var plan = new StripePlanUpdateOptions();

			plan.Name = newPlan.Name;
			//// plan.Amount = amount; 
			//// StripePlan ponse = planService.Update(id, plan);

			var ss = new StripeSubscriptionUpdateOptions();
			ss.PlanId = newPlan.Id;

			var subscriptionService = new StripeSubscriptionService();
			StripeSubscription sub = subscriptionService.Get(cust.Id, id);
			if (sub.TrialEnd != null)
			{
				ss.TrialEnd = sub.TrialEnd;
			}

			StripeSubscription stripeSubscription = subscriptionService.Update(cust.Id, id, ss); // optional StripeSubscriptionUpdateOptions

			return stripeSubscription.Id;
		}

		/// <summary>
		/// Method for deleteing a subscription. 
		/// </summary>
		/// <param name="id">The id of the susbcription to be deleted.</param>
		/// <param name="custid">The id of the Customer who is subscribed.</param>
		[CLSCompliant(false)]
		public static void SubscriptionCancel(string id, string custid)
		{
			var subscriptionService = new StripeSubscriptionService();
			subscriptionService.Cancel(custid, id, false); // optional cancelAtPeriodEnd flag
		}

		/// <summary>
		/// Gets the stripe Customer object.
		/// </summary>
		/// <param name="id">The Customer id.</param>
		/// <returns>The Customer object.</returns>
		[CLSCompliant(false)]
		public static StripeCustomer RetrieveCustomer(string id)
		{
			try
			{
				var customerService = new StripeCustomerService();
				return customerService.Get(id);
			}
			catch (Exception e)
			{
				e.ToString();
				return null;
			}
		}

		/// <summary>
		/// Gets all of the charges for the current customer Id.
		/// </summary>
		/// <param name="customerid">The id of the customer (stripe side).</param>
		/// <returns>List of charges.</returns>
		[CLSCompliant(false)]
		public static IEnumerable<StripeCharge> GetCharges(string customerid)
		{
			var chargeService = new StripeChargeService();
			IEnumerable<StripeCharge> response = chargeService.List(); // optional StripeChargeListOptions
			List<StripeCharge> charges = new List<StripeCharge>();
			foreach (StripeCharge c in response)
			{
				if (c.CustomerId == customerid)
				{
					charges.Add(c);
				}
			}

			return charges;
		}

		/// <summary>
		/// Gets all of the invoices for the current customerId.
		/// </summary>
		/// <param name="customerid">The customer's Id (stripe side).</param>
		/// <returns>List of invoices.</returns>
		[CLSCompliant(false)]
		public static IEnumerable<StripeInvoice> GetInvoices(string customerid)
		{
			var invoiceService = new StripeInvoiceService();
			StripeInvoiceListOptions io = new StripeInvoiceListOptions();
			io.CustomerId = customerid;
			io.Limit = 10000;
			IEnumerable<StripeInvoice> response = invoiceService.List(io); // optional StripeInvoiceListOptions

			return response;
		}

		/// <summary>
		/// Create and store card data to a customer.
		/// </summary>
		/// <param name="token">The stripe token.</param>
		/// <param name="customerId">The customer's Id.</param>
		/// <returns>An object representing a credit card, stored in stripe's servers.</returns>
		[CLSCompliant(false)]
		public static StripeCard CreateCard(StripeToken token, string customerId)
		{
			var currentCard = new StripeCardCreateOptions();
			currentCard.SourceToken = token.ToString();

			var cardService = new StripeCardService();
			StripeCard stripeCard = cardService.Create(customerId, currentCard);
			return stripeCard;
		}

		/// <summary>
		/// Update the stripe customer using the stripe token.
		/// </summary>
		/// <param name="token">The stripe token.</param>
		/// <param name="customerId">The stripe customer's Id.</param>
		[CLSCompliant(false)]
		public static void UpdateStripeCustomer(StripeToken token, string customerId)
		{
			var currentCustomer = new StripeCustomerUpdateOptions();

			// setting up the card
			currentCustomer.SourceToken = token.ToString();

			var customerService = new StripeCustomerService();
			StripeCustomer stripeCustomer = customerService.Update(customerId, currentCustomer);
		}

		/// <summary>
		/// Gets all events.
		/// </summary>
		/// <returns>List of stripe events.</returns>
		[CLSCompliant(false)]
		public static IEnumerable<StripeEvent> GetEvents()
		{
			var eventService = new StripeEventService();
			StripeEventListOptions se = new StripeEventListOptions();

			IEnumerable<StripeEvent> response = eventService.List(); // optional StripeEventListOptions

			return response;
		}

		/// <summary>
		/// Returns the number of days until the end of the month.
		/// </summary>
		/// <returns>Int of days till end of month.</returns>
		private static int DaysUntilEndOfMonth()
		{
			DateTime today = DateTime.Today;

			int eom = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

			DateTime endOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, eom);

			TimeSpan days = endOfMonth.Subtract(today);

			return days.Days;
		}
	}
}