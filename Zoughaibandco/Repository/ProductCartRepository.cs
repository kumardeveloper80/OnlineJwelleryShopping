using System;
using System.Collections.Generic;
using System.Linq;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class ProductCartRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public ProductCartRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public ProductCart_VM GetProductsCart(int UserId)
        {
            var result = (from pc in _DBContext.ProductCarts
                          join pcd in _DBContext.ProductCartDetails on pc.Id equals pcd.ProductCartId
                          where pc.UserId == UserId
                          select new ProductCart_VM
                          {
                              CartId = pc.Id,
                              productCartDetails_VMs = (from pcd in _DBContext.ProductCartDetails
                                                        join p in _DBContext.Products on pcd.ProductId equals p.Id
                                                        where pcd.ProductCartId == pc.Id && p.DeletedDate == null && p.IsPublished == true
                                                        select new ProductCartDetails_VM
                                                        {
                                                            Price = p.PriceDiscounted,
                                                            CartDetailsId = pcd.Id,
                                                            Quantity = pcd.Quantity,
                                                            Title = p.Title,
                                                            ProductImage = p.ImageName,
                                                            TotalPrice = pcd.Quantity * p.PriceDiscounted,
                                                            ProductId =  p.Id
                                                        }).ToList(),
                          }).FirstOrDefault();
            return result;
        }

        public ProductCartGuest_VM GetGuestProductsCart(Guid GuestUserId)
        {
            var result = (from pc in _DBContext.ProductCartGuests
                          join pcd in _DBContext.ProductCartDetailsGuests on pc.Id equals pcd.ProductCartGuestId
                          where pc.GuestUserId == GuestUserId
                          select new ProductCartGuest_VM
                          {
                              CartId = pc.Id,
                              productGuestCartDetails_VMs = (from pcd in _DBContext.ProductCartDetailsGuests
                                                        join p in _DBContext.Products on pcd.ProductId equals p.Id
                                                        where pcd.ProductCartGuestId == pc.Id && p.DeletedDate == null && p.IsPublished == true
                                                        select new ProductCartDetailsGuest_VM
                                                        {
                                                            Price = p.PriceDiscounted,
                                                            CartDetailsId = pcd.Id,
                                                            Quantity = pcd.Quantity,
                                                            Title = p.Title,
                                                            ProductImage = p.ImageName,
                                                            TotalPrice = pcd.Quantity * p.PriceDiscounted,
                                                            ProductId = p.Id
                                                        }).ToList(),
                          }).FirstOrDefault();
            return result;
        }

        public List<ProductCartGuestList> GetAllGuestProductsCart(Guid GuestUserId)
        {
            var result = (from pc in _DBContext.ProductCartGuests
                          where pc.GuestUserId == GuestUserId
                          select new ProductCartGuestList
                          {
                              Id=pc.Id,
                              GuestUserId=pc.GuestUserId,
                          }).ToList();
            return result;
        }

        public List<ProductCartDetailsGuestList> GetAllCartProductDetailsGuest(Guid GuestUserId)
        {
            var result = (from pcd in _DBContext.ProductCartDetailsGuests
                          join p in _DBContext.Products on pcd.ProductId equals p.Id
                          join pc in _DBContext.ProductCartGuests on pcd.ProductCartGuestId equals pc.Id
                          where pc.GuestUserId==GuestUserId && p.DeletedDate == null && p.IsPublished == true
                          select new ProductCartDetailsGuestList
                          {
                             ProductId=pcd.ProductId,
                             Quantity=pcd.Quantity,
                             ProductCartGuestId=pcd.ProductCartGuestId
                          }).ToList();
            return result;
        }

        public List<ProductWishListGuestList> GetAllProductGuestWishList(Guid GuestUserId)
        {
            var result = (from pw in _DBContext.ProductWishListGuests
                          where pw.GuestUserId == GuestUserId
                          select new ProductWishListGuestList
                          {
                              Id = pw.Id,
                              ProductId = pw.ProductId,
                              GuestUserId = pw.GuestUserId,
                          }).ToList();
            return result;
        }

        public int AddtoProductsCart(int UserId, int ProductId, int Qty)
        {
            ProductCart productCartObj = new ProductCart();
            productCartObj.UserId = UserId;
            productCartObj = _DBContext.ProductCarts.Add(productCartObj);
            if (_DBContext.SaveChanges() > 0)
            {
                return AddProductsToCartDetails(productCartObj.Id, ProductId, Qty);
            }
            return 0;
        }

        public int AddtoProductsGuestCart(Guid GuestUserId, int ProductId, int Qty)
        {
            ProductCartGuest productCartObj = new ProductCartGuest();
            productCartObj.GuestUserId = GuestUserId;
            productCartObj = _DBContext.ProductCartGuests.Add(productCartObj);
            if (_DBContext.SaveChanges() > 0)
            {
                return AddProductsToGuestCartDetails(productCartObj.Id, ProductId, Qty);
            }
            return 0;
        }

        public int AddProductsToCartDetails(int CartId, int ProductId, int Qty)
        {
            ProductCartDetail productCartDetailObj = new ProductCartDetail();
            productCartDetailObj.ProductCartId = CartId;
            productCartDetailObj.ProductId = ProductId;
            productCartDetailObj.Quantity = Qty;
            _DBContext.ProductCartDetails.Add(productCartDetailObj);
            return _DBContext.SaveChanges();
        }

        public int AddProductsToGuestCartDetails(int CartId, int ProductId, int Qty)
        {
            ProductCartDetailsGuest productCartDetailObj = new ProductCartDetailsGuest();
            productCartDetailObj.ProductCartGuestId = CartId;
            productCartDetailObj.ProductId = ProductId;
            productCartDetailObj.Quantity = Qty;
            _DBContext.ProductCartDetailsGuests.Add(productCartDetailObj);
            return _DBContext.SaveChanges();
        }

        public int UpdateProductsCartDetails(int CartDetailsId, int Qty)
        {
            var result = _DBContext.ProductCartDetails.Where(x => x.Id == CartDetailsId).FirstOrDefault();
            if (result != null)
            {
                result.Quantity += Qty;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int UpdateProductsGuestCartDetails(int CartDetailsId, int Qty)
        {
            var result = _DBContext.ProductCartDetailsGuests.Where(x => x.Id == CartDetailsId).FirstOrDefault();
            if (result != null)
            {
                result.Quantity += Qty;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int DeletetoProductsCartDetails(int CartDetailsId)
        {
            var result = _DBContext.ProductCartDetails.Where(x => x.Id == CartDetailsId).FirstOrDefault();
            if (result != null)
            {
                _DBContext.ProductCartDetails.Remove(result);
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int DeleteGuesttoProductsCartDetails(int CartDetailsId)
        {
            var result = _DBContext.ProductCartDetailsGuests.Where(x => x.Id == CartDetailsId).FirstOrDefault();
            if (result != null)
            {
                _DBContext.ProductCartDetailsGuests.Remove(result);
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int EmptyCart(int UserId)
        {
            var Cart = _DBContext.ProductCarts.Where(x => x.UserId == UserId).FirstOrDefault();
            if (Cart != null)
            {
                var CartsDeatils = _DBContext.ProductCartDetails.Where(x => x.ProductCartId == Cart.Id).ToList();
                if (CartsDeatils.Count > 0)
                {
                    _DBContext.ProductCartDetails.RemoveRange(CartsDeatils);
                    _DBContext.ProductCarts.Remove(Cart);
                }

                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int EmptyGuestCart(Guid GuestUserId)
        {
            var Cart = _DBContext.ProductCartGuests.Where(x => x.GuestUserId == GuestUserId).FirstOrDefault();
            if (Cart != null)
            {
                var CartsDeatils = _DBContext.ProductCartDetailsGuests.Where(x => x.ProductCartGuestId == Cart.Id).ToList();
                if (CartsDeatils.Count > 0)
                {
                    _DBContext.ProductCartDetailsGuests.RemoveRange(CartsDeatils);
                    _DBContext.ProductCartGuests.Remove(Cart);
                }
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int UpdateToCartQty(int CartDetailsId, int Qty)
        {
            var result = _DBContext.ProductCartDetails.Where(x => x.Id == CartDetailsId).FirstOrDefault();
            if (result != null)
            {
                result.Quantity = Qty;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int UpdateGuestToCartQty(int CartDetailsId, int Qty)
        {
            var result = _DBContext.ProductCartDetailsGuests.Where(x => x.Id == CartDetailsId).FirstOrDefault();
            if (result != null)
            {
                result.Quantity = Qty;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public CountWishListCart CountWishListCart(int UserId)
        {
            CountWishListCart countWishListCart = new CountWishListCart();

            countWishListCart.WishListCount = _DBContext.ProductWishLists
                        .Join(_DBContext.Products, pw => pw.ProductId, p => p.Id, (pw, p) => new { WishList = pw, Product = p })
                       .Where(x => x.Product.DeletedDate == null && x.Product.IsPublished == true && x.WishList.UserId == UserId).Count();


            var CartProduct = (from pc in _DBContext.ProductCarts
                               join pcd in _DBContext.ProductCartDetails on pc.Id equals pcd.ProductCartId
                               join product in _DBContext.Products on pcd.ProductId equals product.Id
                               where pc.UserId == UserId && pcd.Quantity > 0 && product.DeletedDate == null && product.IsPublished == true
                               group pcd by pcd.ProductCartId into g
                               select new
                               {
                                   Count = g.Count()
                               }).FirstOrDefault();

            if(CartProduct !=null)
            {
                countWishListCart.CartCount = CartProduct.Count;
            }
            return countWishListCart;
        }

        public CountWishListCart CountWishListGuestCart(Guid GuestUserId)
        {
            CountWishListCart countWishListGuestCart = new CountWishListCart();

            countWishListGuestCart.WishListCount = _DBContext.ProductWishListGuests
                        .Join(_DBContext.Products, pw => pw.ProductId, p => p.Id, (pw, p) => new { WishList = pw, Product = p })
                       .Where(x => x.Product.DeletedDate == null && x.Product.IsPublished == true && x.WishList.GuestUserId == GuestUserId).Count();


            var CartProduct = (from pc in _DBContext.ProductCartGuests
                               join pcd in _DBContext.ProductCartDetailsGuests on pc.Id equals pcd.ProductCartGuestId
                               join product in _DBContext.Products on pcd.ProductId equals product.Id
                               where pc.GuestUserId == GuestUserId && pcd.Quantity > 0 && product.DeletedDate == null && product.IsPublished == true
                               group pcd by pcd.ProductCartGuestId into g
                               select new
                               {
                                   Count = g.Count()
                               }).FirstOrDefault();

            if (CartProduct != null)
            {
                countWishListGuestCart.CartCount = CartProduct.Count;
            }
            return countWishListGuestCart;
        }
    }
}