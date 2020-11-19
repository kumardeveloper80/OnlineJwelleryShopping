using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Web.WebPages.Html;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class ProductRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public ProductRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public List<Product_VM> GetProductsByCategory(int categoryId, string priceFL, string CollectionFL, string SubCatFL)
        {
            var result = (from p in _DBContext.Products
                          where p.CategoryId == categoryId && p.IsPublished == true && p.DeletedDate == null && p.IsParent
                          orderby p.SequenceNumber ascending
                          select new Product_VM
                          {
                              ProductId = p.Id,
                              Title = p.Title,
                              Price = p.PriceDiscounted,
                              ProductImg = p.ImageName,
                              CollectionId = (int)p.CollectionId,
                              SubCategoryId = (int)p.SubCategoryId
                          }).ToList();

            if (result.Count > 0)
            {
                //For Price Filter
                if (priceFL != null)
                {
                    if (priceFL == PriceFilter.hl.ToString())
                    {
                        result = result.OrderByDescending(x => x.Price).ToList();
                    }
                    else if (priceFL == PriceFilter.lh.ToString())
                    {
                        result = result.OrderBy(x => x.Price).ToList();
                    }
                }

                //For Colletion Filter
                if (Convert.ToInt32(CollectionFL) > 0)
                {
                    result = result.Where(x => x.CollectionId == Convert.ToInt32(CollectionFL)).ToList();
                }

                //For SubCategory Filter
                if (Convert.ToInt32(SubCatFL) > 0)
                {
                    result = result.Where(x => x.SubCategoryId == Convert.ToInt32(SubCatFL)).ToList();
                }

            }
            return result;
        }

        public List<Product_VM> GetProductsByCollection(int collectionId, string priceFL)
        {
            var result = (from p in _DBContext.Products
                          where p.CollectionId == collectionId && p.IsPublished == true && p.DeletedDate == null && p.IsParent
                          orderby p.SequenceNumber ascending
                          select new Product_VM
                          {
                              ProductId = p.Id,
                              Title = p.Title,
                              Price = p.PriceDiscounted,
                              ProductImg = p.ImageName
                          }).ToList();

            if (result.Count > 0 && priceFL != null)
            {
                if (priceFL == PriceFilter.hl.ToString())
                {
                    result = result.OrderByDescending(x => x.Price).ToList();
                }
                else if (priceFL == PriceFilter.lh.ToString())
                {
                    result = result.OrderBy(x => x.Price).ToList();
                }
            }
            return result;
        }

        public Product_VM GetProductsById(int productId)
        {
            try
            {


               

                var result = (from p in _DBContext.Products
                              where p.Id == productId && p.IsPublished == true && p.DeletedDate == null
                              select new Product_VM
                              {
                                  ProductId = p.Id,
                                  Title = p.Title,
                                  Price = p.PriceDiscounted,
                                  ProductImg = p.ImageName,
                                  Description = p.Description,
                                  CollectionId = p.CollectionId,
                                  //CollectionName = (p.CollectionId == null || p.CollectionId == 0)? "" : allcollections.Where(x => x.Id == p.CollectionId).Select(x => x.Name).FirstOrDefault(),
                                  //RouteName = (p.CollectionId != null || p.CollectionId == 0)? "" : allcollections.Where(x => x.Id == p.CollectionId).Select(x => x.RouteName).FirstOrDefault(),

                                  NextProductId = _DBContext.Products.Where(x => x.CollectionId == p.CollectionId && x.Id > productId).OrderBy(x => x.Id).Select(x => x.Id).FirstOrDefault(),
                                  ProductColors = (from color in _DBContext.Colors
                                  where color.ProductId == productId
                                  select new Colors_VM
                                  {
                                      ColorId = color.Id,
                                      ColorName = color.ColorsName
                                  }).ToList(),

                                  Material = p.Materials,
                                  diamond = p.diamond,
                                  GoldWeight = p.GoldWeight,
                                  Round = p.Round,
                                  Marquis = p.Marquis,
                                  Fancy = p.Fancy,
                                  Princes = p.Princes,
                                  Baguette = p.Baguette,
                                  Triangle = p.Triangle,
                                  Pear = p.Pear,
                                  black = p.black,
                                  Ruby = p.Ruby,
                                  Sapphire = p.Sapphire,
                                  Emerald = p.Emerald,
                                  semiprecious = p.semiprecious,
                                  ChildList = (from child in _DBContext.Products
                                               where child.ParentId == productId && child.IsPublished == true 
                                               && child.DeletedDate == null

                                               select new ChildProduct_VM
                                               {
                                                   ProductId = child.Id,
                                                   Title = child.Title,
                                                   ProductImg = child.ImageName
                                               }).ToList(),
                                  IsParent = p.IsParent,
                                  ParentId = p.ParentId,
                                  ReferenceNO = p.ReferenceNO
                              }).FirstOrDefault();


                if (result != null)
                {
                    if (result.CollectionId > 0)
                    {
                        var collectionDetails = _DBContext.Collections.Where(c => c.Id == result.CollectionId).FirstOrDefault();

                        if (collectionDetails != null)
                        { 
                            result.CollectionName = collectionDetails.Name;
                            result.RouteName = collectionDetails.RouteName;
                        }
                    }
                }


               if(result != null && result.IsParent == false)
                {
                    var parent = (
                        from p in _DBContext.Products
                        where p.Id == result.ParentId
                        && p.IsPublished == true
                        && p.DeletedDate == null
                        select new ChildProduct_VM()
                        {
                            ProductId = p.Id,
                            Title = p.Title,
                            ProductImg = p.ImageName
                        }).FirstOrDefault();

                    var child = (
                        from p in _DBContext.Products
                        where p.ParentId == parent.ProductId && p.Id != productId
                        && p.IsPublished == true
                        && p.DeletedDate == null
                        select new ChildProduct_VM()
                        {
                            ProductId = p.Id,
                            Title = p.Title,
                            ProductImg = p.ImageName
                        }).ToList();

                    var parentChildList = new List<ChildProduct_VM>();
                    parentChildList.Add(parent);
                    parentChildList.AddRange(child);

                    result.ChildList = parentChildList;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //Product List for Admin
        public List<Product_VM> GetAllProducts(string search)
        {
            var productList = (from p in _DBContext.Products
                               where p.DeletedDate == null
                               select new Product_VM
                               {
                                   ProductId = p.Id,
                                   Title = p.Title,
                               }).ToList();
            if (productList.Count > 0 && search != null)
            {
                productList = productList.Where(x => x.Title.ToLower().Contains(search.ToLower())).ToList();
            }

            return productList;
        }

        //Delete Product by Admin
        public int DeleteProduct(int ProductId)
        {
            var product = _DBContext.Products.Find(ProductId);
            if (product != null)
            {
                product.DeletedDate = DateTime.Now;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        //Get Product By Id for Admin
        public Product_VM GetProductById(int productId)
        {
            try
            {
                var result = (from p in _DBContext.Products
                              where p.Id == productId && p.DeletedDate == null
                              select new Product_VM
                              {
                                  ProductId = p.Id,
                                  Title = p.Title,
                                  Price = p.Price,
                                  PriceDiscounted = p.PriceDiscounted,
                                  ProductImg = p.ImageName,
                                  Description = p.Description,
                                  CollectionId = _DBContext.Collections.Where(x => x.Id == p.CollectionId).Select(x => x.Id).FirstOrDefault(),
                                  CategoryId = p.CategoryId == null ? 0 : p.CategoryId.Value,
                                  SubCategoryId = p.SubCategoryId == null ? 0 : p.SubCategoryId.Value,
                                  IsPublished = (bool)p.IsPublished,
                                  ColorList = _DBContext.Colors.Where(x => x.ProductId == productId).Select(x => x.ColorsName).ToList(),

                                  Material = p.Materials,
                                  GoldWeight = p.GoldWeight,
                                  Round = p.Round,
                                  Marquis = p.Marquis,
                                  Fancy = p.Fancy,
                                  Princes = p.Princes,
                                  Baguette = p.Baguette,
                                  Triangle = p.Triangle,
                                  Pear = p.Pear,
                                  black = p.black,
                                  Ruby = p.Ruby,
                                  Sapphire = p.Sapphire,
                                  Emerald = p.Emerald,
                                  semiprecious = p.semiprecious,
                                  diamond = p.diamond,
                                  ParentId = p.ParentId == null ? 0 : p.ParentId.Value,
                                  ReferenceNO = p.ReferenceNO
                              }).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //get product by filter
        public List<ProductFilter_VM> GetAllProductsByFilter(string search, string filter)
        {
            var categories = _DBContext.Categories;
            var productsByFilter = (from p in _DBContext.Products
                                    join c in categories on p.CategoryId equals c.Id into cat
                                    from c in cat.DefaultIfEmpty()
                                    where p.DeletedDate == null
                                    group p by p.CategoryId into g
                                    select new ProductFilter_VM
                                    {
                                        Category = g.Key == 0 ? "None category" :
                                        categories.Where(x => x.Id == g.Key).Select(x => x.CategoryName.ToUpper() + " " + x.Type.ToUpper()).FirstOrDefault(),
                                        ProductList = (from c in g
                                                       select new Product_VM
                                                       {
                                                           ProductId = c.Id,
                                                           Title = c.Title,
                                                           ReferenceNO = c.ReferenceNO != null ? c.ReferenceNO : ""
                                                       }).OrderBy(o => o.Title).ToList(),
                                        filterType = filter
                                    }).ToList();

            if (productsByFilter.Count > 0 && search != null)
            {
                foreach (var p in productsByFilter)
                {
                    p.ProductList = p.ProductList.Where(x => x.Title.ToLower().Contains(search.ToLower())
                    || x.ReferenceNO.ToLower().Contains(search.ToLower())).ToList();
                }
            }
            return productsByFilter;
        }

        public int AddBulkProduct(DataTable datatable)
        {
            try
            {
                var collections = _DBContext.Collections.ToList();
                var categories = _DBContext.Categories.ToList();
                var subCategories = _DBContext.SubCategories.ToList();

                List<Product> productList = new List<Product>();

                for (int i = 1; i < datatable.Rows.Count; i++)
                {
                    Product product = new Product();
                    product.Title = Convert.ToString(datatable.Rows[i][0]);
                    product.CollectionId = collections.Where(c => c.Name.ToLower() == Convert.ToString(datatable.Rows[i][1]).ToLower()).Select(c => c.Id).FirstOrDefault();
                    product.Description = Convert.ToString(datatable.Rows[i][2]);
                    product.Materials = Convert.ToString(datatable.Rows[i][3]);
                    product.GoldWeight = Convert.ToDecimal(datatable.Rows[i][4]);
                    product.Round = Convert.ToDecimal(datatable.Rows[i][5]);
                    product.Marquis = Convert.ToDecimal(datatable.Rows[i][6]);
                    product.Ruby = Convert.ToDecimal(datatable.Rows[i][7]);
                    product.Sapphire = Convert.ToDecimal(datatable.Rows[i][8]);
                    product.Emerald = Convert.ToDecimal(datatable.Rows[i][9]);
                    product.Fancy = Convert.ToDecimal(datatable.Rows[i][10]);
                    product.Princes = Convert.ToDecimal(datatable.Rows[i][11]);
                    product.Baguette = Convert.ToDecimal(datatable.Rows[i][12]);
                    product.Triangle = Convert.ToDecimal(datatable.Rows[i][13]);
                    product.Pear = Convert.ToDecimal(datatable.Rows[i][14]);
                    product.black = Convert.ToDecimal(datatable.Rows[i][15]);
                    product.semiprecious = Convert.ToDecimal(datatable.Rows[i][16]);
                    product.diamond = Convert.ToDecimal(datatable.Rows[i][17]);
                    product.Price = Convert.ToDecimal(datatable.Rows[i][18]);
                    product.PriceDiscounted = Convert.ToDecimal(datatable.Rows[i][19]);
                    product.ImageName = Convert.ToString(datatable.Rows[i][20]);
                    product.IsPublished = Convert.ToBoolean(Convert.ToInt16(datatable.Rows[i][21]));
                    product.CategoryId = categories.Where(c => c.CategoryName == Convert.ToString(datatable.Rows[i][22])).Select(c => c.Id).FirstOrDefault();
                    product.SubCategoryId = subCategories.Where(c => c.SubCategoryName == Convert.ToString(datatable.Rows[i][23])).Select(c => c.Id).FirstOrDefault();
                    product.CreatedDate = DateTime.Now;
                    productList.Add(product);
                }

                _DBContext.Products.AddRange(productList);
                _DBContext.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public int AddBulkProduct_Test(DataTable datatable)
        //{
        //    try
        //    {
        //        var collections = _DBContext.Collections.ToList();
        //        var categories = _DBContext.Categories.ToList();
        //        var subCategories = _DBContext.SubCategories.ToList();

        //        List<Product_Test> productList = new List<Product_Test>();

        //        for (int i = 1; i < datatable.Rows.Count; i++)
        //        {
        //            Product_Test product = new Product_Test();
        //            product.Title = Convert.ToString(datatable.Rows[i][0]);
        //            product.CollectionId = collections.Where(c => c.Name == Convert.ToString(datatable.Rows[i][1])).Select(c => c.Id).FirstOrDefault();
        //            product.Description = Convert.ToString(datatable.Rows[i][2]);
        //            product.Materials = Convert.ToString(datatable.Rows[i][3]);
        //            product.GoldWeight = Convert.ToDecimal(datatable.Rows[i][4]);
        //            product.Round = Convert.ToDecimal(datatable.Rows[i][5]);
        //            product.Marquis = Convert.ToDecimal(datatable.Rows[i][6]);
        //            product.Ruby = Convert.ToDecimal(datatable.Rows[i][7]);
        //            product.Sapphire = Convert.ToDecimal(datatable.Rows[i][8]);
        //            product.Emerald = Convert.ToDecimal(datatable.Rows[i][9]);
        //            product.Fancy = Convert.ToDecimal(datatable.Rows[i][10]);
        //            product.Princes = Convert.ToDecimal(datatable.Rows[i][11]);
        //            product.Baguette = Convert.ToDecimal(datatable.Rows[i][12]);
        //            product.Triangle = Convert.ToDecimal(datatable.Rows[i][13]);
        //            product.Pear = Convert.ToDecimal(datatable.Rows[i][14]);
        //            product.black = Convert.ToDecimal(datatable.Rows[i][15]);
        //            product.semiprecious = Convert.ToDecimal(datatable.Rows[i][16]);
        //            product.diamond = Convert.ToDecimal(datatable.Rows[i][17]);
        //            product.Price = Convert.ToDecimal(datatable.Rows[i][18]);
        //            product.PriceDiscounted = Convert.ToDecimal(datatable.Rows[i][19]);
        //            product.ImageName = Convert.ToString(datatable.Rows[i][20]);
        //            product.IsPublished = Convert.ToBoolean(Convert.ToInt16(datatable.Rows[i][21]));
        //            product.CategoryId = categories.Where(c => c.CategoryName == Convert.ToString(datatable.Rows[i][22])).Select(c => c.Id).FirstOrDefault();
        //            product.SubCategoryId = subCategories.Where(c => c.SubCategoryName == Convert.ToString(datatable.Rows[i][23])).Select(c => c.Id).FirstOrDefault();
        //            product.CreatedDate = DateTime.Now;
        //            productList.Add(product);
        //        }
        //        _DBContext.Product_Test.AddRange(productList);
        //        _DBContext.SaveChanges();
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        // get parent product list
        public List<Product_VM> GetParentProduct(int ProductId)
        {
            var result = new List<Product_VM>();

            result = (from p in _DBContext.Products
                      where p.DeletedDate == null && p.IsParent
                      select new Product_VM
                      {
                          ProductId = p.Id,
                          Title = p.Title,
                      }).ToList();

            var noneOption = new Product_VM { ProductId = 0, Title = "None" };
            result.Add(noneOption);

            if (ProductId > 0)
            {
                result = result.Where(x => x.ProductId != ProductId).ToList();
            }
            return result.OrderBy(x => x.ProductId).ToList();
        }
    }
}