using AllyisApps.Services.Lookup;

namespace AllyisApps.Services
{
	public partial class AppService : BaseService
	{
		public Address GetAddress(int? addressId)
		{
			if (addressId == null)
			{
				return null;
			}
			var address = DBHelper.getAddreess(addressId);
			return InitializeAddress(address);
		}
	}
}