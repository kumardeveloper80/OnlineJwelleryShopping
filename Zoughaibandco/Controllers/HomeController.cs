using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Zoughaibandco.Models;
using Zoughaibandco.Repository;
using Zoughaibandco.Utility;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Controllers
{
    public class HomeController : Controller
    {
        Zoughaibandco_DBEntities _DBContext;
        StoresRepository _storesRepository;
        HomeRepository _homeRepository;


        public HomeController()
        {
            _DBContext = new Zoughaibandco_DBEntities();
            _homeRepository = new HomeRepository();
            _storesRepository = new StoresRepository();
        }

        public ActionResult Index()
        {
            Session.Remove("CheckOutType");
            if (Session["GusetLogin"] == null)
            {
                Session["GusetLogin"] = true;
                Session["GuestId"] = Guid.NewGuid();
                Session["CartCount"] = 0;
                Session["WishCount"] = 0;
                Session["GuestEmail"] = "";
            }
            if (Session["UserId"] != null)
            {
                Session["GusetLogin"] = false;
            }
            else
            {
                Session["GusetLogin"] = true;
            }
            return View();
        }

        public JsonResult PaymentCallBack()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            //FileStream fs1 = new FileStream("C:\\LogData\\Log.txt", FileMode.Append, FileAccess.Write);
            //StreamWriter writer = new StreamWriter(fs1);
            try
            {
                //writer.WriteLine("<<<<-----Start----->>>>");
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                foreach (var key in Request.Form.AllKeys)
                {
                    parameters.Add(key, Request.Params[key]);
                    //writer.WriteLine(key + ":" + Request.Params[key]);
                }
                if (parameters != null && parameters.Any())
                {
                    string refNo = parameters.Where(c => c.Key == "req_reference_number").Select(c => c.Value).SingleOrDefault();
                    string paymentStatus = parameters.Where(c => c.Key == "decision").Select(c => c.Value).SingleOrDefault();
                    var _orders = _DBContext.Orders.Where(x => x.ReferenceId == refNo).FirstOrDefault();
                    if (_orders != null)
                    {
                        commonResponse.message = "Payment form the Product Purchase";
                        //writer.WriteLine("Payment form the Product Purchase");
                        _orders.PaymentStatus = paymentStatus;
                        _DBContext.SaveChanges();
                        var _Customer = _DBContext.Clients.Where(c => c.Id == _orders.UserId).FirstOrDefault();
                        var orderItems = (from _orderDetails in _DBContext.OrderDetails
                                          join _product in _DBContext.Products on _orderDetails.ProductName equals _product.Title
                                          where _orderDetails.OrderId == _orders.Id
                                          select new OrderItems_VM
                                          {
                                              ProductName = _orderDetails.ProductName,
                                              ProductImg = _product.ImageName,
                                              Price = (decimal)_orderDetails.ProductPrice,
                                              Qty = (int)_orderDetails.OrderQy,
                                              Total = (decimal)_orderDetails.ProductPrice * (int)_orderDetails.OrderQy
                                          }).ToList();
                        _homeRepository.SendOrderEmailToUser(orderItems, "Zoughaib Order Processed : " + _orders.ReferenceId, _Customer.FirstName, _Customer.Email);
                    }
                    else
                    {
                        //writer.WriteLine("Payment form the E-Gift Card");
                        var eGifCard = _DBContext.EGiftCards.Where(x => x.Referenceid == refNo).FirstOrDefault();
                        if (eGifCard != null && paymentStatus.ToUpper() == "ACCEPT")
                        {
                            var _Customer = _DBContext.Clients.Where(c => c.Id == eGifCard.UserId).FirstOrDefault();
                            commonResponse.message = "Payment form the E-Gift Card";
                            eGifCard.IsPaid = true;
                            _DBContext.SaveChanges();
                            _homeRepository.SendVoucherEmailToUser(_Customer.Email, "Zoughaib Gift Voucher Sold", Convert.ToDecimal(eGifCard.Amount), eGifCard.ToFirstName);
                            string Emailbody = string.Empty;
                            Emailbody += "Hi Admin,</br>";
                            Emailbody += "A Gift Card of USD " + eGifCard.Amount;
                            Emailbody += " has been purchased for " + eGifCard.ToFirstName + " " + eGifCard.ToLastName;
                            Emailbody += " phone number " + eGifCard.ToPhoneNo;
                            Emailbody += " deliverable on date " + eGifCard.DeliverDate + "</br>";
                            _homeRepository.SendOrderEmail(Emailbody, "Zoughaib Gift Voucher Sold : USD " + eGifCard.Amount);
                        }
                    }
                }
                //writer.WriteLine("<<<<-----END----->>>>");
                //writer.Close();
                commonResponse.status = 1;
            }
            catch (Exception ex)
            {
                //writer.WriteLine("<<<<-----END----->>>>");
                //writer.Close();
                commonResponse.message = "Error :" + ex.Message;
                commonResponse.status = 0;
                return Json(commonResponse);
            }
            return Json(commonResponse);
        }

        public ActionResult ThankYou()
        {
            //code to update the info to DB.
            Session.Remove("CheckOutType");
            Session.Remove("eGiftAmt");
            Session.Remove("eReferenceId");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (var key in Request.Form.AllKeys)
            {
                parameters.Add(key, Request.Params[key]);
            }

            if (parameters != null && parameters.Any())
            {
                string refNo = parameters.Where(c => c.Key == "req_reference_number").Select(c => c.Value).SingleOrDefault();
                string paymentStatus = parameters.Where(c => c.Key == "decision").Select(c => c.Value).SingleOrDefault();

                //Payment form the E-Gift Card
                if (Convert.ToInt32(Session["paymentFor"]) == (int)Helper.PaymentRef.EGiftCard && paymentStatus.ToUpper() == "ACCEPT")
                {
                    var eGifCard = _DBContext.EGiftCards.Where(x => x.Referenceid == refNo).FirstOrDefault();
                    var _Customer = _DBContext.Clients.Where(c => c.Id == eGifCard.UserId).FirstOrDefault();

                    if (eGifCard != null)
                    {
                        eGifCard.IsPaid = true;
                        _DBContext.SaveChanges();

                        _homeRepository.SendVoucherEmailToUser(_Customer.Email, "Zoughaib Gift Voucher Sold", Convert.ToDecimal(eGifCard.Amount), eGifCard.ToFirstName);
                        //Send email to the zoubandco.
                        string Emailbody = string.Empty;
                        Emailbody += "Hi Admin,</br>";
                        Emailbody += "A Gift Card of USD " + eGifCard.Amount;
                        Emailbody += " has been purchased for " + eGifCard.ToFirstName + " " + eGifCard.ToLastName;
                        Emailbody += " phone number " + eGifCard.ToPhoneNo;
                        Emailbody += " deliverable on date " + eGifCard.DeliverDate + "</br>";
                        _homeRepository.SendOrderEmail(Emailbody, "Zoughaib Gift Voucher Sold : USD " + eGifCard.Amount);
                    }
                }
                //Payment form the Product Purchase
                else if (Convert.ToInt32(Session["paymentFor"]) == (int)Helper.PaymentRef.Purchase)
                {
                    var _orders = _DBContext.Orders.Where(x => x.ReferenceId == refNo).FirstOrDefault();
                    if (_orders != null)
                    {
                        _orders.PaymentStatus = paymentStatus;
                        _DBContext.SaveChanges();
                        var _Customer = _DBContext.Clients.Where(c => c.Id == _orders.UserId).FirstOrDefault();


                        //send mail to user

                        var orderItems = (from _orderDetails in _DBContext.OrderDetails
                                          join _product in _DBContext.Products on _orderDetails.ProductName equals _product.Title
                                          where _orderDetails.OrderId == _orders.Id
                                          select new OrderItems_VM
                                          {
                                              ProductName = _orderDetails.ProductName,
                                              ProductImg = _product.ImageName,
                                              Price = (decimal)_orderDetails.ProductPrice,
                                              Qty = (int)_orderDetails.OrderQy,
                                              Total = (decimal)_orderDetails.ProductPrice * (int)_orderDetails.OrderQy
                                          }).ToList();

                        _homeRepository.SendOrderEmailToUser(orderItems, "Zoughaib Order Processed : " + _orders.ReferenceId, _Customer.FirstName, _Customer.Email);

                        //Send email to the zoubandco.
                        //string Emailbody = string.Empty;
                        //Emailbody += "Hi Admin,</br>";
                        //Emailbody += "Order with referenceId " + _orders.ReferenceId + " has been processed for customer " + string.Format("{0} {1}", Convert.ToString(_Customer.FirstName).Trim(), Convert.ToString(_Customer.LastName).Trim());
                        //_homeRepository.SendOrderEmail(Emailbody, "Zoughaib Order Processed : " + _orders.ReferenceId);
                    }
                }
            }
            Session.Remove("paymentFor");
            return View("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult FAQS()
        {
            return View();
        }

        public ActionResult Stores()
        {
            var result = _storesRepository.getAllStores();
            return View(result);
        }

        public ActionResult GetStores()
        {
            return null;
        }

        public ActionResult OurStory()
        {
            return View();
        }

        public ActionResult HighEndJewellery()
        {
            return View();
        }

        public ActionResult ShippingPolicy()
        {
            return View();
        }

        public ActionResult Cookies()
        {
            return View();
        }

        public ActionResult ExchangePolicy()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Careers()
        {
            return View();
        }



        public ActionResult Help()
        {
            return View();
        }

        public ActionResult ECard()
        {
            ViewBag.Banner = _DBContext.Banners.Where(x => x.KeyName == "ecard").Select(x => x.ImgName).FirstOrDefault();
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [NonAction]
        public int SendEmail()
        {
            int nRet = 0;
            try
            {

                nRet = 1;
            }
            catch (Exception ex)
            {
                nRet = -1;
            }
            return nRet;
        }


        /// <summary>
        /// Action for EGiftCard Payment
        /// </summary>
        /// <returns></returns>
        public ActionResult EGiftPayment()
        {
            var UserId = Session["UserId"];
            if (UserId != null)
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
                _paymentConfiguration.reference_number = Convert.ToString(Session["eReferenceId"]);
                _paymentConfiguration.amount = Convert.ToDecimal(Session["eGiftAmt"]);
                Session.Remove("eGiftAmt");
                Session.Remove("eReferenceId");
                return View(_paymentConfiguration);
            }
            else
            {
                Session.Remove("eGiftAmt");
                Session.Remove("eReferenceId");
                return RedirectToAction("Index", "Home");
            }
        }


    }
}