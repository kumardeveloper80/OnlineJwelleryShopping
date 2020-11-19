using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zoughaibandco.Repository;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Controllers
{
    public class CheckoutController : Controller
    {
        ProductWishListRepository _productWishListRepository;
        ProductCartRepository _productCartRepository;

        public ActionResult Index()
        {
            try
            {
                var UserId = Session["UserId"];
                var isGuest = Convert.ToBoolean(Session["GusetLogin"]);
                
                if (UserId != null)
                {
                    decimal grandTotal = 0;
                    var checkout = Convert.ToInt32(Session["CheckOutType"]);
                    if (checkout == 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        PaymentConfiguration _paymentConfiguration = new PaymentConfiguration();
                        _paymentConfiguration.access_key = System.Configuration.ConfigurationManager.AppSettings["access_key"];
                        _paymentConfiguration.profile_id = System.Configuration.ConfigurationManager.AppSettings["profile_id"];
                        _paymentConfiguration.transaction_uuid = Guid.NewGuid().ToString();
                        _paymentConfiguration.signed_field_names = System.Configuration.ConfigurationManager.AppSettings["signed_field_names"];
                        _paymentConfiguration.unsigned_field_names = System.Configuration.ConfigurationManager.AppSettings["unsigned_field_names"];
                        _paymentConfiguration.signed_date_time = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
                        _paymentConfiguration.locale = System.Configuration.ConfigurationManager.AppSettings["locale"];
                        _paymentConfiguration.transaction_type = System.Configuration.ConfigurationManager.AppSettings["transaction_type"];
                        _paymentConfiguration.currency = System.Configuration.ConfigurationManager.AppSettings["currency"];
                        _paymentConfiguration.reference_number = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");

                        _paymentConfiguration.amount = 0;
                        if (checkout == (int)CheckoutTye.WISHLIST)
                        {
                            _productWishListRepository = new ProductWishListRepository();
                            grandTotal = (decimal)_productWishListRepository.GetProductsWishList(Convert.ToInt32(UserId)).ToList().Sum(x => x.Price);
                            _paymentConfiguration.amount = grandTotal;
                        }
                        else if (checkout == (int)CheckoutTye.CART)
                        {
                            _productCartRepository = new ProductCartRepository();
                            grandTotal = (decimal)_productCartRepository.GetProductsCart(Convert.ToInt32(UserId)).productCartDetails_VMs.ToList().Sum(x => x.TotalPrice);
                            _paymentConfiguration.amount = grandTotal;
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return View(_paymentConfiguration);
                    }
                }
                else if (isGuest)
                {
                    var GuestUserId = new Guid(Session["GuestId"].ToString());
                    decimal grandTotal = 0;
                    var checkout = Convert.ToInt32(Session["CheckOutType"]);
                    if (checkout == 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        PaymentConfiguration _paymentConfiguration = new PaymentConfiguration();
                        _paymentConfiguration.access_key = System.Configuration.ConfigurationManager.AppSettings["access_key"];
                        _paymentConfiguration.profile_id = System.Configuration.ConfigurationManager.AppSettings["profile_id"];
                        _paymentConfiguration.transaction_uuid = Guid.NewGuid().ToString();
                        _paymentConfiguration.signed_field_names = System.Configuration.ConfigurationManager.AppSettings["signed_field_names"];
                        _paymentConfiguration.unsigned_field_names = System.Configuration.ConfigurationManager.AppSettings["unsigned_field_names"];
                        _paymentConfiguration.signed_date_time = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
                        _paymentConfiguration.locale = System.Configuration.ConfigurationManager.AppSettings["locale"];
                        _paymentConfiguration.transaction_type = System.Configuration.ConfigurationManager.AppSettings["transaction_type"];
                        _paymentConfiguration.currency = System.Configuration.ConfigurationManager.AppSettings["currency"];
                        _paymentConfiguration.reference_number = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");

                        _paymentConfiguration.amount = 0;
                        if (checkout == (int)CheckoutTye.WISHLIST)
                        {
                            _productWishListRepository = new ProductWishListRepository();
                            grandTotal = (decimal)_productWishListRepository.GetProductsGuestWishList(GuestUserId).ToList().Sum(x => x.Price);
                            _paymentConfiguration.amount = grandTotal;
                        }
                        else if (checkout == (int)CheckoutTye.CART)
                        {
                            _productCartRepository = new ProductCartRepository();
                            grandTotal = (decimal)_productCartRepository.GetGuestProductsCart(GuestUserId).productGuestCartDetails_VMs.ToList().Sum(x => x.TotalPrice);
                            _paymentConfiguration.amount = grandTotal;
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return View(_paymentConfiguration);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
