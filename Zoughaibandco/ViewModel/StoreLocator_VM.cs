using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class StoreLocator_VM
    {
        public StoreLocator_VM()
        {
            StoreAddresses = new List<StoreAddress_VM>();
        }

        public string CountryName { get; set; }
        public List<StoreAddress_VM> StoreAddresses { get; set; }
    }
}