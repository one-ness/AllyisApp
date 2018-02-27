using AllyisApps.DBModel.Lookup;
using AllyisApps.Services.Lookup;

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
				result = InitializeAddress(address);
			}

			return result;
		}

		/// <summary>
		/// get address from the given address db entity
		/// </summary>
		public static Address InitializeAddress(AddressDBEntity address)
		{
			Address result = null;
			if (address != null)
			{
				result = new Address();
				result.AddressId = address.AddressId;
				result.Address1 = address.Address1;
				result.Address2 = address.Address2;
				result.City = address.City;
				result.StateName = address.State;
				result.PostalCode = address.PostalCode;
				result.CountryCode = address.CountryCode;
				result.StateId = address.StateId;
				result.CountryName = address.Country;
			}

			return result;
		}

		public string GetStateName(int stateId)
		{
			Cache.CacheContainer.
		}
	}
}