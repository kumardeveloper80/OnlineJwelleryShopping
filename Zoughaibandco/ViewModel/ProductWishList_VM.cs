using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class ProductWishList_VM
    {
        public int ProductId { get; set; }
        public int WishListId { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string ProductImg { get; set; }
    }

    public class ProductGuestWishList_VM
    {
        public int ProductId { get; set; }
        public int WishListId { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string ProductImg { get; set; }
    }
}