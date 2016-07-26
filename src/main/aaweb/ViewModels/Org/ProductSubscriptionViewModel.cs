//------------------------------------------------------------------------------
// <copyright file="ProductSubscriptionViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using AllyisApps.Services.BusinessObjects;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// Represents information regarding a Product the user is attempting to subscribe to.
	/// </summary>
	[CLSCompliant(false)]
	public class ProductSubscriptionViewModel : BaseViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProductSubscriptionViewModel"/> class.
		/// </summary>
		public ProductSubscriptionViewModel()
		{
			this.Billing = new BillingViewModel();
		}

		/// <summary>
		/// Gets or sets the current Organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the current product's Id.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets all information relating to this product.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets a description of this product.
		/// </summary>
		public string ProductDescription { get; set; }

		/// <summary>
		/// Gets or sets an object representing the subscription the organization currently has with this
		/// productId.
		/// </summary>
		public SubscriptionInfo CurrentSubscription { get; set; }

		/// <summary>
		/// Gets or sets a list of the SkuS applicable to this product.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Skus", Justification = "acronym")]
		public IEnumerable<SkuInfo> Skus { get; set; }

		/// <summary>
		/// Gets or sets the ID of the Sku that is currently selected.
		/// </summary>
		public int SelectedSku { get; set; }

		/// <summary>
		/// Gets or sets the Id of the previous Sku used for this product.
		/// </summary>
		public int PreviousSku { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the model successfully populated or not.
		/// </summary>
		public bool IsValid { get; set; }

		/// <summary>
		/// Gets or sets a stripe Token. 
		/// </summary>
		public string StripeToken { get; set; }

		/// <summary>
		/// Gets or sets the Billing Model.
		/// </summary>
		public BillingViewModel Billing { get; set; }

		/// <summary>
		/// Gets or sets the number of Users for this subscription.
		/// </summary>
		public int NumberOfUsers { get; set; }

		/// <summary>
		/// Gets or sets the name of the Selected Sku.
		/// </summary>
		public string SelectedSkuName { get; set; }

		/// <summary>
		/// Gets the number of users currently attached to the organization.
		/// </summary>
		public int CurrentUsers { get; internal set; }
	}
}