using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zoughaibandco.Repository;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Controllers
{
    public class ProductController : Controller
    {
        ProductRepository productRepository;
        ProductWishListRepository productWishListRepository;
        ProductCartRepository productCartRepository;
        public ProductController()
        {

        }

        public ActionResult Index(int Id)
        {
            if (Id > 0)
            {
                productRepository = new ProductRepository();
                var product = productRepository.GetProductsById(Id);
                return View(product);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Wishlist()
        {
            var UserId = Session["UserId"];
            if (UserId != null)
            {
                Session["CheckOutType"] = (int)CheckoutTye.WISHLIST;
                productWishListRepository = new ProductWishListRepository();
                var wishList = productWishListRepository.GetProductsWishList(Convert.ToInt32(UserId));
                if (wishList.Count > 0)
                {
                    return View(wishList);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GuestWishlist()
        {

            try
            {
                var GuestUserId = new Guid(Session["GuestId"].ToString());
                Session["CheckOutType"] = (int)CheckoutTye.WISHLIST;
                productWishListRepository = new ProductWishListRepository();
                var wishList = productWishListRepository.GetProductsGuestWishList(GuestUserId);
                if (wishList.Count > 0)
                {
                    return View(wishList);
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        public ActionResult Cart()
        {
            var UserId = Session["UserId"];
            if (UserId != null)
            {
                Session["CheckOutType"] = (int)CheckoutTye.CART;
                decimal? GrandTotal = 0;
                productCartRepository = new ProductCartRepository();
                var result = productCartRepository.GetProductsCart(Convert.ToInt32(UserId));

                if (result != null && result.productCartDetails_VMs.Count > 0)
                {
                    GrandTotal = result.productCartDetails_VMs.Sum(x => x.TotalPrice);
                }
                ViewBag.GrandTotal = GrandTotal;
                return View(result);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GuestCart()
        {
            try
            {
                var GuestUserId = new Guid(Session["GuestId"].ToString());
                Session["CheckOutType"] = (int)CheckoutTye.CART;
                decimal? GrandTotal = 0;
                productCartRepository = new ProductCartRepository();
                var result = productCartRepository.GetGuestProductsCart(GuestUserId);

                if (result != null && result.productGuestCartDetails_VMs.Count > 0)
                {
                    GrandTotal = result.productGuestCartDetails_VMs.Sum(x => x.TotalPrice);
                }
                ViewBag.GrandTotal = GrandTotal;
                return View(result);
            }
            catch (FormatException)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

    }
}