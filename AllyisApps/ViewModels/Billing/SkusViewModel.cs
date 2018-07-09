using System.Collections.Generic;

namespace AllyisApps.ViewModels.Billing
{
	/// <summary>
	/// skus view model
	/// </summary>
	public class SkusViewModel : BaseViewModel
	{
		/// <summary>
		/// organization id
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// has permission to subscribe?
		/// </summary>
		public bool CanSubscribe { get; set; }

		/// <summary>
		/// list of products
		/// </summary>
		public List<ProductItemViewModel> Products { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public SkusViewModel()
		{
			Products = new List<ProductItemViewModel>();
		}

		/// <summary>
		/// product
		/// </summary>
		public class ProductItemViewModel
		{
			/// <summary>
			/// list of skus in the product
			/// </summary>
			public List<SkuItemViewModel> Skus { get; set; }

			/// <summary>
			/// product name
			/// </summary>
			public string ProductName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public int ProductID { get; set; }

			/// <summary>
			/// constructor
			/// </summary>
			public ProductItemViewModel()
			{
				Skus = new List<SkuItemViewModel>();
			}

			/// <summary>
			/// sku
			/// </summary>
			public class SkuItemViewModel
			{
				/// <summary>
				/// sku id
				/// </summary>
				public int SkuId { get; set; }

				/// <summary>
				/// sku name
				/// </summary>
				public string SkuName { get; set; }

				/// <summary>
				/// description
				/// </summary>
				public string SkuDescription { get; set; }

				/// <summary>
				/// sku icon url
				/// </summary>
				public string SkuIconUrl { get; set; }

				/// <summary>
				/// price
				/// </summary>
				public decimal Price { get; set; }
			}
		}
	}
}
