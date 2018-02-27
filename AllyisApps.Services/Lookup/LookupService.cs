using AllyisApps.DBModel.Lookup;
using AllyisApps.Services.Cache;
using AllyisApps.Services.Lookup;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.Services
{
	public partial class AppService : BaseService
	{
		/// <summary>
		/// get address for the given id
		/// </summary>
		public Address GetAddress(int? addressId)
		{
			Address result = null;
			if (addressId.HasValue)
			{
				var address = DBHelper.GetAddreess(addressId.Value);
				result = this.InitializeAddress(address);
			}

			return result;
		}

		/// <summary>
		/// get address from the given address db entity
		/// </summary>
		public Address InitializeAddress(AddressDBEntity address)
		{
			Address result = null;
			if (address != null)
			{
				result = new Address
				{
					AddressId = address.AddressId,
					Address1 = address.Address1,
					Address2 = address.Address2,
					City = address.City,
					CountryCode = address.CountryCode,
					CountryName = this.GetCountryName(address.CountryCode),
					PostalCode = address.PostalCode,
					StateId = address.StateId,
					StateName = address.StateId.HasValue ? this.GetStateName(address.StateId.Value) : string.Empty
				};
			}

			return result;
		}

		/// <summary>
		/// Gets the list of valid countries.
		/// </summary>
		/// <returns>A collection of valid countries.</returns>
		public Dictionary<string, Country> GetCountries()
		{
			return CacheContainer.CountriesCache;
		}

		/// <summary>
		/// get the list of states for the given country
		/// </summary>
		public List<State> GetStates(string countryCode)
		{
			List<State> result = new List<State>();
			if (!string.IsNullOrWhiteSpace(countryCode))
			{
				if (CacheContainer.StatesForCountryCache.TryGetValue(countryCode, out result))
				{
					result = result.OrderBy(x => x.StateName).ToList();
				}
			}

			return result;
		}

		/// <summary>
		/// get the country name for the given country code
		/// </summary>
		public string GetCountryName(string countryCode)
		{
			var result = string.Empty;
			if (!string.IsNullOrWhiteSpace(countryCode))
			{
				if (CacheContainer.CountriesCache.TryGetValue(countryCode, out Country temp))
				{
					result = temp.CountryName;
				}
			}

			return result;
		}

		/// <summary>
		/// get the state name for the given state id
		/// </summary>
		public string GetStateName(int stateId)
		{
			var result = string.Empty;
			if (CacheContainer.AllStatesCache.TryGetValue(stateId, out State temp))
			{
				result = temp.StateName;
			}

			return result;
		}
	}
}