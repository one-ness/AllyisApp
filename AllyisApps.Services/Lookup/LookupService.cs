using AllyisApps.DBModel.Lookup;
using AllyisApps.Services.Cache;
using AllyisApps.Services.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.Services
{
	public partial class AppService : BaseService
	{
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
			try
			{
				return CacheContainer.StatesForCountryCache[countryCode].OrderBy(s => s.StateName).ToList();
			}
			catch (KeyNotFoundException)
			{
				return new List<State>();
			}
			catch (ArgumentNullException)
			{
				return new List<State>();
			}
		}

		public string GetCountryName(string countryCode)
		{
			var result = string.Empty;
			if (!string.IsNullOrWhiteSpace(countryCode))
			{
				try
				{
					var country = CacheContainer.CountriesCache[countryCode];
					if (country != null)
					{
						result = country.CountryName;
					}
				}
				catch { }
			}

			return result;
		}

		public string GetStateName(int stateId)
		{
			var result = string.Empty;
			try
			{
				var state = CacheContainer.AllStatesCache[stateId];
				if (state != null)
				{
					result = state.StateName;
				}
			}
			catch { }

			return result;
		}

		/// <summary>
		/// get address
		/// </summary>
		public Address GetAddress(int? addressId)
		{
			Address result = null;
			if (addressId.HasValue)
			{
				var address = this.DBHelper.GetAddress(addressId.Value);
				if (address != null)
				{
					result = this.InitializeAddress(address);
				}
			}

			return result;
		}

		/// <summary>
		/// Initializes a <see cref="Address"/> from a <see cref="AddressDBEntity"/>.
		/// </summary>
		public Address InitializeAddress(AddressDBEntity address)
		{
			return new Address
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
	}
}
