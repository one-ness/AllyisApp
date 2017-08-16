using AllyisApps.Services.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
    public partial class AppService : BaseService
    {
        public Address getAddress(int? addressID)
        {
            var address = DBHelper.getAddreess(addressID);
            return InitializeAddress(address);
        } 
    }
}
