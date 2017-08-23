using AllyisApps.Services.Lookup;

namespace AllyisApps.Services
{
	public partial class AppService : BaseService
	{
		public Address getAddress(int? addressID)
		{
			if (addressID == null)
			{
				return null;
			}
			var address = DBHelper.getAddreess(addressID);
			return InitializeAddress(address);
		}
	}
}
