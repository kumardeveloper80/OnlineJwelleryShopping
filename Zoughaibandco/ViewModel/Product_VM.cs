using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class Product_VM
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceDiscounted { get; set; }
        public string Description { get; set; }
        public string ProductImg { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int? CollectionId { get; set; }
        public string CollectionName { get; set; }
        public string RouteName { get; set; }
        public int NextProductId { get; set; }
        public bool IsPublished { get; set; }

        //added for extra fields
        public string Material { get; set; }
        public Nullable<decimal> GoldWeight { get; set; }
        public Nullable<decimal> Round { get; set; }
        public Nullable<decimal> Marquis { get; set; }
        public Nullable<decimal> Ruby { get; set; }
        public Nullable<decimal> Sapphire { get; set; }
        public Nullable<decimal> Emerald { get; set; }
        public Nullable<decimal> Fancy { get; set; }
        public Nullable<decimal> Princes { get; set; }
        public Nullable<decimal> Baguette { get; set; }
        public Nullable<decimal> Triangle { get; set; }
        public Nullable<decimal> Pear { get; set; }
        public Nullable<decimal> black { get; set; }
        public Nullable<decimal> semiprecious { get; set; }
        public Nullable<decimal> diamond { get; set; }

        public string ReferenceNO { get; set; }

        public List<string> ColorList { get; set; }
        public List<Colors_VM> ProductColors { get; set; }
        public int? ParentId { get; set; }

        public List<ChildProduct_VM> ChildList { get; set; }

        public bool IsParent { get; set; }

        public Product_VM()
        {
            ColorList = new List<string>();
            ProductColors = new List<Colors_VM>();
            ChildList = new List<ChildProduct_VM>();
        }
    }

    public class Colors_VM
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
    }

    public class ProductFilter_VM
    {
        public List<Product_VM> ProductList { get; set; }
        //public List<string> Category { get; set; }
        public string Category { get; set; }
        public string filterType { get; set; }

        public ProductFilter_VM()
        {
            ProductList = new List<Product_VM>();
            //Category = new List<string>();
        }
    }

    public class ChildProduct_VM
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ProductImg { get; set; }
    }
}