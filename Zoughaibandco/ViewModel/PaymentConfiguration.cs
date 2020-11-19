using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class PaymentConfiguration
    {
        public string access_key { get; set; }
        public string profile_id { get; set; }
        public string transaction_uuid { get; set; }
        public string signed_field_names { get; set; }
        public string unsigned_field_names { get; set; }
        public string signed_date_time { get; set; }
        public string locale { get; set; }
        public string transaction_type { get; set; }
        public string reference_number { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
    }
}