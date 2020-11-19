using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class ProductCart_VM
    {
        public int CartId { get; set; }
        public decimal? GrandTotal { get; set; }
        public List<ProductCartDetails_VM> productCartDetails_VMs;

        public ProductCart_VM()
        {
            productCartDetails_VMs = new List<ProductCartDetails_VM>();
        }
    }

    public class ProductCartDetails_VM
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

    public partial class ProductCartList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}