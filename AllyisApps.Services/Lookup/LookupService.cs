using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllyisApps.Services.Lookup;
using AllyisApps.DBModel.Lookup;

namespace AllyisApps.Services
{
	public partial class AppService : BaseService
	{
		/// <summary>
		/// get address
		/// </summary>
		public Address GetAddress(int addressID)
		{
			var address = DBHelper.getAddreess(addressID);
			return this.InitializeAddress(address);
		}

		/// <summary>
		/// Initializes a <see cref="Address"/> from a <see cref="AddressDBEntity"/>.
		/// </summary>
		public Address InitializeAddress(AddressDBEntity address)
		{
			if (address == null)
			{
				return null;
			}

			return new Address
			{
				AddressId = address.AddressId,
				Address1 = address.Address1,
				Address2 = address.Address2,
				City = address.City,
				StateName = address.State,
				PostalCode = address.PostalCode,
				CountryCode = address.CountryCode,
				StateId = address.StateId,
				CountryName = address.Country
			};
		}
	}
}
