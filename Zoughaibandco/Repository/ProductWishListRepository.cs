using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class ProductWishListRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public ProductWishListRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public List<ProductWishList_VM> GetProductsWishList(int UserId)
        {
            var result = (from p in _DBContext.Products
                          join w in _DBContext.ProductWishLists on p.Id equals w.ProductId
                          where w.UserId == UserId && p.DeletedDate == null && p.IsPublished == true
                          select new ProductWishList_VM
                          {
                              WishListId = w.Id,
                              ProductId = p.Id,
                              Title = p.Title,
                              Price = p.PriceDiscounted,
                              ProductImg = p.ImageName,
                          }).ToList();

            return result;
        }

        public List<ProductGuestWishList_VM> GetProductsGuestWishList(Guid GuestUserId)
        {
            var result = (from p in _DBContext.Products
                          join w in _DBContext.ProductWishListGuests on p.Id equals w.ProductId
                          where w.GuestUserId == GuestUserId && p.DeletedDate == null && p.IsPublished == true
                          select new ProductGuestWishList_VM
                          {
                              WishListId = w.Id,
                              ProductId = p.Id,
                              Title = p.Title,
                              Price = p.PriceDiscounted,
                              ProductImg = p.ImageName,
                          }).ToList();

            return result;
        }

        public int CancelWishlist(int UserId)
        {
            var wishLists = _DBContext.ProductWishLists.Where(x => x.UserId == UserId).ToList();
            if(wishLists.Count > 0)
            {
                _DBContext.ProductWishLists.RemoveRange(wishLists);
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int CancelGuestWishlist(Guid GuestUserId)
        {
            var wishLists = _DBContext.ProductWishListGuests.Where(x => x.GuestUserId == GuestUserId).ToList();
            if (wishLists.Count > 0)
            {
                _DBContext.ProductWishListGuests.RemoveRange(wishLists);
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int WishListProductToRemove(int WishListId)
        {
            var result = _DBContext.ProductWishLists.Where(x => x.Id == WishListId).FirstOrDefault();
            if (result != null)
            {
                _DBContext.ProductWishLists.Remove(result);
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        public int GuestWishListProductToRemove(int WishListId)
        {
            var result = _DBContext.ProductWishListGuests.Where(x => x.Id == WishListId).FirstOrDefault();
            if (result != null)
            {
                _DBContext.ProductWishListGuests.Remove(result);
                return _DBContext.SaveChanges();
            }
            return 0;
        }

    }
}