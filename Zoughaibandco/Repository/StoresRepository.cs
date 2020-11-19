using System.Collections.Generic;
using System.Linq;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class StoresRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public StoresRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public List<StoreLocator_VM> getAllStores()
        {
            var storeInformation = (from sl in _DBContext.StoreLocators.Include("StoreAddresses")
                                    select new StoreLocator_VM
                                    {
                                        CountryName = sl.CountryName,
                                        StoreAddresses = (from x in sl.StoreAddresses
                                                          select new StoreAddress_VM
                                                          {
                                                              StoreName = x.StoreName,
                                                              Address = x.Address,
                                                              Email = x.Email,
                                                              Mobile = x.Mobile,
                                                              Telephone = x.Telephone,
                                                              Location = x.StoreLocatorID == 1? "lebanon": "kuwait"
                                                          }).ToList()
                                    }).ToList();
            return storeInformation;
        }
    }
}