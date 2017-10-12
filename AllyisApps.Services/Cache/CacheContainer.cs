using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Auth;
using AllyisApps.DBModel;
using AllyisApps.Services.Lookup;

namespace AllyisApps.Services.Cache
{
	/// <summary>
	/// container for all the caches for all of service layer, used as a singleton
	/// </summary>
	public static class CacheContainer
	{
		/// <summary>
		/// dictionary of all products, indexed by product id
		/// </summary>
		public static Dictionary<ProductIdEnum, Product> ProductsCache { get; private set; }

		/// <summary>
		/// dictionary of all skus, indexed by product id
		/// </summary>
		public static Dictionary<ProductIdEnum, List<Sku>> SkusCache { get; private set; }

		/// <summary>
		/// dictionary of all product roles, indexed by product id
		/// </summary>
		public static Dictionary<ProductIdEnum, List<ProductRole>> ProductRolesCache { get; private set; }

		/// <summary>
		/// dictionary of all langugages, indexed by culture name
		/// </summary>
		public static Dictionary<string, Language> LanguagesCache { get; set; }

		/// <summary>
		/// dictionary of all countries, indexed by country code
		/// </summary>
		public static Dictionary<string, Country> CountriesCache { get; set; }

		/// <summary>
		/// dictionary of all states, indexed by country code
		/// </summary>
		public static Dictionary<string, List<State>> StatesCache { get; set; }

		private static string sqlConnectionString;
		private static DBHelper DBHelper;

		/// <summary>
		/// init
		/// </summary>
		public static void Init(string connectionString)
		{
			sqlConnectionString = connectionString;
			DBHelper = new DBHelper(sqlConnectionString);

			// TODO: init all caches
		}
	}
}
