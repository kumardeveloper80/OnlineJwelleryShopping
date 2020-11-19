using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class ProductCartGuest_VM
    {
        public int CartId { get; set; }
        public decimal? GrandTotal { get; set; }
        public List<ProductCartDetailsGuest_VM> productGuestCartDetails_VMs;

        public ProductCartGuest_VM()
        {
            productGuestCartDetails_VMs = new List<ProductCartDetailsGuest_VM>();
        }
    }

    public class ProductCartDetailsGuest_VM
    {
        public int Id { get; set; }
        public int CartDetailsId { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public string Title { get; set; }
        public string ProductImage { get; set; }
        public decimal? TotalPrice { get; set; }

        public int ProductId { get; set; }
    }

    public partial class ProductCartGuestList
    {
        public int Id { get; set; }
        public System.Guid GuestUserId { get; set; }
    }

    public partial class ProductCartDetailsGuestList
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ProductCartGuestId { get; set; }
    }

    public partial class ProductWishListGuestList
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public System.Guid GuestUserId { get; set; }
    }
}