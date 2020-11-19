using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class Order_VM
    {
        public string OrderRef { get; set; }
        public string ClientName { get; set; }
        public DateTime CheoutDate { get; set; }
        public decimal GrandTotal { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string BillingAddress { get; set; }
        public string Phone { get; set; }
        public string ShippingAddress { get; set; }
        public string AdditionalInformation { get; set; }
        public string DeliveryMessage { get; set; }
        public string Email { get; set; }
        public bool IsGuest { get; set; }
        public List<OrderItems_VM> OrderItems { get; set; }

        public Order_VM()
        {
            OrderItems = new List<OrderItems_VM>();
        }

    }

    public class OrderItems_VM
    {
        public string ProductName { get; set; }
        public string ProductImg { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal Total { get; set; }
    }
}