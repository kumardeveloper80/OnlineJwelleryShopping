using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using Zoughaibandco.Models;
using Zoughaibandco.Repository;
using Zoughaibandco.Utility;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Controllers
{
    [RoutePrefix("api/Ajaxcall")]
    public class AjaxCallController : ApiController
    {
        #region ::  GLOBAL VARIABLES
        Zoughaibandco_DBEntities _DBContext;
        CollectionRepository _CollectionRepository;
        ProductWishListRepository productWishListRepository;
        ProductCartRepository productCartRepository;
        ContactRepository contactRepository;
        EGiftCardRepository eGiftCardRepository;
        HomeRepository _homeRepository;

        #endregion

        #region :: CONSTRUCTOR

        public AjaxCallController()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        #endregion

        #region :: ACTION

        /// <summary>
        /// Function for SingUp the user
        /// </summary>
        /// <param name="Signup"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Signup")]
        public IHttpActionResult Signup([FromBody] Signup_VM Signup)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (Signup != null)
                {
                    if (!_DBContext.Clients.Where(c => c.Email.ToLower() == Signup.Email.ToLower()).Any())
                    {
                        var clientObj = Mapper.Map<Client>(Signup);
                        clientObj.AllowToUsePlateform = true;
                        clientObj.CreatedDate = DateTime.Now;
                        clientObj.Password = Helper.Encrypt(Signup.Password);
                        _DBContext.Clients.Add(clientObj);
                        commonResponse.status = _DBContext.SaveChanges();
                        commonResponse.dataenum = clientObj.Id;
                        HttpContext.Current.Session["UserId"] = clientObj.Id;
                    }
                    else
                    {
                        commonResponse.status = -2;
                    }
                }
                else
                {
                    commonResponse.status = -3;
                    commonResponse.message = "bad request";
                }
            }
            catch (System.Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }


        [HttpGet]
        [Route("CheckUser")]
        public IHttpActionResult CheckUser()
        {
            CommonResponse<Signup_VM> commonResponse = new CommonResponse<Signup_VM>();
            try
            {
                int UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                commonResponse.status = UserId;
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }


        /// <summary>
        /// Function for get User details by User Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUser")]
        public IHttpActionResult GetUser()
        {
            int UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            CommonResponse<Signup_VM> commonResponse = new CommonResponse<Signup_VM>();
            try
            {
                if (UserId > 0)
                {
                    var result = _DBContext.Clients.Where(x => x.Id == UserId && x.DeletedDate == null).FirstOrDefault();
                    if (result != null)
                    {
                        result.Password = Helper.Decrypt(result.Password);
                        var client = Mapper.Map<Signup_VM>(result);
                        commonResponse.dataenum = client;
                        commonResponse.status = 1;
                    }
                    else
                    {
                        commonResponse.status = -2;
                    }
                }
                else
                {
                    commonResponse.status = -3;
                    commonResponse.message = "bad request";
                }
            }
            catch (System.Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for update the user profile
        /// </summary>
        /// <param name="Signup"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUser")]
        public IHttpActionResult UpdateUser([FromBody] Signup_VM Signup)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (Signup != null)
                {
                    var updateObj = Mapper.Map<Client>(Signup);
                    var clientObj = _DBContext.Clients.Find(updateObj.Id);
                    if (clientObj != null)
                    {
                        updateObj.AllowToUsePlateform = true;
                        updateObj.CreatedDate = clientObj.CreatedDate;
                        updateObj.UpdatedDate = DateTime.Now;
                        updateObj.Password = Helper.Encrypt(Signup.Password);
                        _DBContext.Entry(clientObj).CurrentValues.SetValues(updateObj);
                        commonResponse.status = _DBContext.SaveChanges();
                        commonResponse.dataenum = clientObj.Id;
                        HttpContext.Current.Session["UserId"] = clientObj.Id;
                    }
                    else
                    {
                        commonResponse.status = -2;
                    }
                }
                else
                {
                    commonResponse.status = -3;
                    commonResponse.message = "bad request";
                }
            }
            catch (System.Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for save the career information.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveCareer")]
        public IHttpActionResult SaveCareer()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                string fname = string.Empty;
                string fPath = string.Empty;
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    string Folder = Convert.ToString(ConfigurationManager.AppSettings["CVUpload"]);
                    string DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                    fPath = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                    fname = fPath;
                    fPath = Path.Combine(DirectoryPath, fPath);
                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }
                    file.SaveAs(fPath);
                }

                Career_VM career_VM = new Career_VM();
                career_VM.FirstName = HttpContext.Current.Request.Form["FirstName"];
                career_VM.LastName = HttpContext.Current.Request.Form["LastName"];
                career_VM.Email = HttpContext.Current.Request.Form["Email"];
                career_VM.Phone = HttpContext.Current.Request.Form["Phone"];
                career_VM.Address = HttpContext.Current.Request.Form["Address"];
                career_VM.Position = HttpContext.Current.Request.Form["Position"];
                career_VM.AboutUs = HttpContext.Current.Request.Form["AboutUs"];
                career_VM.FileName = fname;

                if (career_VM != null)
                {
                    var careerObj = Mapper.Map<Career>(career_VM);
                    _DBContext.Careers.Add(careerObj);
                    commonResponse.status = _DBContext.SaveChanges();
                }
                else
                {
                    commonResponse.status = -3;
                    commonResponse.message = "bad request";
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for save the Need Help information
        /// </summary>
        /// <param name="contact_VM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveHelp")]
        public IHttpActionResult SaveHelp(Contact_VM contact_VM)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (contact_VM != null)
                {
                    contactRepository = new ContactRepository();
                    commonResponse.status = contactRepository.Add(contact_VM);
                }
                else
                {
                    commonResponse.status = -3;
                    commonResponse.message = "Bad request";
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for SignOut
        /// Destory session
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SignOut")]
        public IHttpActionResult SignOut()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            HttpContext.Current.Session.Abandon();
            commonResponse.status = 1;
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for User authentication
        /// </summary>
        /// <param name="login_VM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] Login_VM login_VM)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {

                if (login_VM != null)
                {
                    var client = _DBContext.Clients.Where(c => c.Email.ToLower() == login_VM.LoginUsername.ToLower() && c.AllowToUsePlateform == true && c.DeletedDate == null).SingleOrDefault();
                    if (client != null)
                    {

                        var EncryptPwd = Helper.Encrypt(login_VM.LoginPassword);
                        //var DecryptPwd = Helper.Decrypt(client.Password);
                        if (EncryptPwd == client.Password)
                        {
                            commonResponse.status = 2;
                            commonResponse.message = "password match";
                            HttpContext.Current.Session["UserId"] = client.Id;
                            HttpContext.Current.Session["GusetLogin"] = false;

                            //Copy data of GuestLogin to User Login
                            using (var transaction = _DBContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    var GuestUserId = new Guid(HttpContext.Current.Session["GuestId"].ToString());
                                    productCartRepository = new ProductCartRepository();
                                    var data = productCartRepository.CountWishListGuestCart(GuestUserId);
                                    HttpContext.Current.Session["CartCount"] = data.CartCount;
                                    HttpContext.Current.Session["WishCount"] = data.WishListCount;
                                    if (data.CartCount > 0)
                                    {
                                        var CartList = productCartRepository.GetAllCartProductDetailsGuest(GuestUserId);
                                        if (CartList != null && CartList.Count > 0)
                                        {
                                            foreach (var ls in CartList)
                                            {
                                                var Cart = _DBContext.ProductCarts.Where(x => x.UserId == (int)client.Id).FirstOrDefault();

                                                if (Cart != null)
                                                {
                                                    var CartDetails = _DBContext.ProductCartDetails.Where(x => x.ProductId == ls.ProductId && x.ProductCartId == Cart.Id).FirstOrDefault();
                                                    if (CartDetails != null)
                                                    {
                                                        productCartRepository.UpdateProductsCartDetails(CartDetails.Id, 1);
                                                    }
                                                    else
                                                    {
                                                        productCartRepository.AddProductsToCartDetails(Cart.Id, ls.ProductId, 1);
                                                    }
                                                }
                                                else
                                                {
                                                    productCartRepository.AddtoProductsCart(Convert.ToInt32(client.Id), ls.ProductId, 1);
                                                }
                                            }
                                        }
                                    }
                                    if (data.WishListCount > 0)
                                    {
                                        var WishList = productCartRepository.GetAllProductGuestWishList(GuestUserId);
                                        if (WishList != null && WishList.Count > 0)
                                        {
                                            foreach (var wlt in WishList)
                                            {
                                                var obj3 = new ProductWishList();
                                                obj3.ProductId = wlt.ProductId;
                                                obj3.UserId = client.Id;
                                                _DBContext.ProductWishLists.Add(obj3);
                                                _DBContext.SaveChanges();
                                            }
                                        }
                                    }
                                    transaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                }
                            }

                        }
                        else
                        {
                            commonResponse.status = 1;
                            commonResponse.message = "password do not match";
                        }
                    }
                    else
                    {
                        commonResponse.status = 1;
                        commonResponse.message = "password do not match";
                    }
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -1;
            }

            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for User authentication
        /// </summary>
        /// <param name="login_VM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginCheck")]
        public IHttpActionResult LoginCheck([FromBody] Login_VM login_VM)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                // 1) Allready Have Account
                // 2) Store User Email in Session  
                if (login_VM != null)
                {
                    var client = _DBContext.Clients.Where(c => c.Email.ToLower() == login_VM.LoginUsername.ToLower() && c.AllowToUsePlateform == true && c.DeletedDate == null).SingleOrDefault();
                    if (client != null)
                    {
                        commonResponse.status = 1;
                        commonResponse.message = "You have a account with same email id please Login.";
                    }
                    else
                    {
                        commonResponse.status = 2;
                        commonResponse.message = "New User";
                        HttpContext.Current.Session["GuestEmail"] = login_VM.LoginUsername;
                    }
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -1;
            }

            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for send password to email when forgot
        /// </summary>
        /// <param name="forgotPassword_VM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ForgotPassword")]
        public IHttpActionResult ForgotPassword([FromBody] ForgotPassword_VM forgotPassword_VM)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (forgotPassword_VM != null)
                {
                    string EmailBody = string.Empty;
                    var client = _DBContext.Clients.Where(c => c.Email.ToLower() == forgotPassword_VM.LoginUsername.ToLower() && c.AllowToUsePlateform == true && c.DeletedDate == null).SingleOrDefault();
                    if (client != null)
                    {
                        client.Password = Helper.Decrypt(client.Password);
                        commonResponse.status = 0;
                        EmailBody = "Please use the following username and password to access your account:<br/><br/>";
                        EmailBody += "Username:" + client.Email + "<br/>";
                        EmailBody += "Password:" + client.Password + "<br/><br/><br/>";
                        EmailBody += "Best regards,<br/>";
                        EmailBody += "<a href='http://www.loading-lb.com/'>Zouhaib</a>&nbsp;team";
                        SendForgotPasswordEmail(forgotPassword_VM.LoginUsername, EmailBody);
                        commonResponse.message = "Email Sent.";
                        HttpContext.Current.Session["UserId"] = client.Id;
                    }
                    else
                    {
                        commonResponse.status = 1;
                        commonResponse.message = "Email does not exists.";
                    }
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -1;
            }

            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for send forgorpassword email
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="EmailBody"></param>
        /// <returns></returns>
        [NonAction]
        public int SendForgotPasswordEmail(string Email, string EmailBody)
        {
            int nRet = 0;
            try
            {
                try
                {
                    var SmtpClient = Convert.ToString(ConfigurationManager.AppSettings["SmtpClient"]);
                    var Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                    var UserName = Convert.ToString(ConfigurationManager.AppSettings["UserName"]);
                    var Password = Convert.ToString(ConfigurationManager.AppSettings["Password"]);
                    var EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SSL"]);
                    var FromAddress = Convert.ToString(ConfigurationManager.AppSettings["From"]);

                    SmtpClient client = new SmtpClient(SmtpClient, Port);
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(UserName, Password);
                    if (EnableSSL)
                    {
                        client.EnableSsl = true;
                    }
                    string Message = "";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(FromAddress);
                    mailMessage.To.Add(Email);
                    mailMessage.Body = EmailBody;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = "Zoughaib password reminder";
                    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    client.Send(mailMessage);
                    nRet = 1;
                }
                catch (SmtpException mailex)
                {
                    nRet = -1;
                }
            }
            catch (Exception ex)
            {
                nRet = 0;
            }
            return nRet;
        }

        /// <summary>
        /// Function for get Collection for Header Menu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCollectionForMenu")]
        public IHttpActionResult GetCollectionForMenu()
        {
            CommonResponse<List<Collection_VM>> commonResponse = new CommonResponse<List<Collection_VM>>();
            _CollectionRepository = new CollectionRepository();
            var result = _CollectionRepository.GetAllCollections();
            if (result.Count > 0)
            {
                commonResponse.status = 1;
                commonResponse.dataenum = result;
            }

            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for get Jewellery(Main Category) for Header Menu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetJewellery")]
        public IHttpActionResult GetJewellery()
        {
            CommonResponse<List<Category>> commonResponse = new CommonResponse<List<Category>>();
            var result = _DBContext.Categories.OrderBy(x => x.CategoryName).ToList();
            if (result.Count > 0)
            {
                commonResponse.status = 1;
                commonResponse.dataenum = result;
            }
            else
            {
                commonResponse.status = 0;
                commonResponse.dataenum = null;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Add product to wish list
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AddItemToWishList")]
        public IHttpActionResult AddItemToWishList(int ProductId)
        {
            CommonResponse<CountWishListCart> commonResponse = new CommonResponse<CountWishListCart>();
            try
            {
                int UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                if (UserId > 0 && ProductId > 0)
                {
                    var WishList = _DBContext.ProductWishLists.Where(x => x.UserId == UserId && x.ProductId == ProductId).FirstOrDefault();
                    if (WishList == null)
                    {
                        ProductWishList productWishList = new ProductWishList();
                        productWishList.ProductId = ProductId;
                        productWishList.UserId = UserId;
                        _DBContext.ProductWishLists.Add(productWishList);
                        commonResponse.status = _DBContext.SaveChanges();
                    }
                    else
                    {
                        commonResponse.status = -2;
                    }
                    productCartRepository = new ProductCartRepository();
                    commonResponse.dataenum = productCartRepository.CountWishListCart(UserId);
                }
                else
                {
                    commonResponse.status = -3;
                    commonResponse.message = "bad request";
                }
            }
            catch (System.Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Add product to wish list
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AddItemToGuestWishList")]
        public IHttpActionResult AddItemToGuestWishList(int ProductId)
        {
            CommonResponse<CountWishListCart> commonResponse = new CommonResponse<CountWishListCart>();
            try
            {
                var GuestUserId = new Guid(HttpContext.Current.Session["GuestId"].ToString());
                var WishList = _DBContext.ProductWishListGuests.Where(x => x.GuestUserId == GuestUserId && x.ProductId == ProductId).FirstOrDefault();
                if (WishList == null)
                {
                    ProductWishListGuest productWishListGuest = new ProductWishListGuest();
                    productWishListGuest.ProductId = ProductId;
                    productWishListGuest.GuestUserId = GuestUserId;
                    _DBContext.ProductWishListGuests.Add(productWishListGuest);
                    commonResponse.status = _DBContext.SaveChanges();
                }
                else
                {
                    commonResponse.status = -2;
                }
                productCartRepository = new ProductCartRepository();
                commonResponse.dataenum = productCartRepository.CountWishListGuestCart(GuestUserId);
                HttpContext.Current.Session["CartCount"] = commonResponse.dataenum.CartCount;
                HttpContext.Current.Session["WishCount"] = commonResponse.dataenum.WishListCount;
            }
            catch (System.Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for get Number of product in wish list and in cart
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CountWishListCart")]
        public IHttpActionResult CountWishListCart(int UserId)
        {
            CommonResponse<CountWishListCart> commonResponse = new CommonResponse<CountWishListCart>();
            try
            {
                if (UserId > 0)
                {
                    productCartRepository = new ProductCartRepository();
                    commonResponse.status = 1;
                    commonResponse.dataenum = productCartRepository.CountWishListCart(UserId);
                }
                else
                {
                    commonResponse.status = 0;
                }
            }
            catch (System.Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for remove the all products from the wish list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CancelWishlist")]
        public IHttpActionResult CancelWishlist()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var UserId = HttpContext.Current.Session["UserId"];
                if (Convert.ToInt32(UserId) > 0)
                {
                    productWishListRepository = new ProductWishListRepository();
                    var result = productWishListRepository.CancelWishlist(Convert.ToInt32(UserId));
                    if (result > 0)
                    {
                        commonResponse.status = result;
                    }
                    else
                    {
                        commonResponse.status = 0;
                    }
                }
                else
                {
                    commonResponse.status = -2;
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);

        }

        /// <summary>
        /// Function for remove the all products from the wish list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CancelGuestWishlist")]
        public IHttpActionResult CancelGuestWishlist()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var GuestUserId = new Guid(HttpContext.Current.Session["GuestId"].ToString());
                productWishListRepository = new ProductWishListRepository();
                var result = productWishListRepository.CancelGuestWishlist(GuestUserId);
                if (result > 0)
                {
                    commonResponse.status = result;
                }
                else
                {
                    commonResponse.status = 0;
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);

        }

        /// <summary>
        /// Function for add product to cart
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AddToCart")]
        public IHttpActionResult AddToCart(int ProductId)
        {
            CommonResponse<CountWishListCart> commonResponse = new CommonResponse<CountWishListCart>();
            try
            {
                var UserId = HttpContext.Current.Session["UserId"];
                if (Convert.ToInt32(UserId) > 0)
                {
                    int result = 0;
                    productCartRepository = new ProductCartRepository();
                    var Cart = _DBContext.ProductCarts.Where(x => x.UserId == (int)UserId).FirstOrDefault();

                    if (Cart != null)
                    {
                        var CartDetails = _DBContext.ProductCartDetails.Where(x => x.ProductId == ProductId && x.ProductCartId == Cart.Id).FirstOrDefault();
                        if (CartDetails != null)
                        {
                            result = productCartRepository.UpdateProductsCartDetails(CartDetails.Id, 1);
                        }
                        else
                        {
                            result = productCartRepository.AddProductsToCartDetails(Cart.Id, ProductId, 1);
                        }
                    }
                    else
                    {
                        result = productCartRepository.AddtoProductsCart(Convert.ToInt32(UserId), ProductId, 1);
                    }
                    commonResponse.status = result;
                    commonResponse.dataenum = productCartRepository.CountWishListCart(Convert.ToInt32(UserId));
                }
                else
                {
                    commonResponse.status = -2;
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for remove all products from the Cart
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("EmptyCart")]
        public IHttpActionResult EmptyCart()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var UserId = HttpContext.Current.Session["UserId"];
                if (Convert.ToInt32(UserId) > 0)
                {
                    productCartRepository = new ProductCartRepository();
                    var result = productCartRepository.EmptyCart(Convert.ToInt32(UserId));
                    if (result > 0)
                    {
                        commonResponse.status = result;
                    }
                    else
                    {
                        commonResponse.status = 0;
                    }
                }
                else
                {
                    commonResponse.status = -2;
                    commonResponse.message = "Bad Requrest";
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for remove all products from the Cart
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("EmptyGuestCart")]
        public IHttpActionResult EmptyGuestCart()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var isGuest = Convert.ToBoolean(HttpContext.Current.Session["GusetLogin"]);
                if (isGuest)
                {
                    var GuestUserId = new Guid(HttpContext.Current.Session["GuestId"].ToString());
                    productCartRepository = new ProductCartRepository();
                    var result = productCartRepository.EmptyGuestCart(GuestUserId);
                    if (result > 0)
                    {
                        commonResponse.status = result;
                    }
                    else
                    {
                        commonResponse.status = 0;
                    }
                }
                else
                {
                    commonResponse.status = -2;
                    commonResponse.message = "Bad Requrest";
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for remove single product from the cart
        /// </summary>
        /// <param name="CartDetailsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CartProductRemove")]
        public IHttpActionResult CartProductRemove(int CartDetailsId)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var UserId = HttpContext.Current.Session["UserId"];
                if (Convert.ToInt32(UserId) > 0)
                {
                    productCartRepository = new ProductCartRepository();
                    commonResponse.status = productCartRepository.DeletetoProductsCartDetails(CartDetailsId);
                }
                else
                {
                    commonResponse.status = -2;
                    commonResponse.message = "Bad Requrest";
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for remove single product from the cart
        /// </summary>
        /// <param name="CartDetailsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GuestCartProductRemove")]
        public IHttpActionResult GuestCartProductRemove(int CartDetailsId)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var isGuest = Convert.ToBoolean(HttpContext.Current.Session["GusetLogin"]);
                if (isGuest)
                {
                    var GuestUserId = new Guid(HttpContext.Current.Session["GuestId"].ToString());
                    productCartRepository = new ProductCartRepository();
                    commonResponse.status = productCartRepository.DeleteGuesttoProductsCartDetails(CartDetailsId);
                    var data = productCartRepository.CountWishListGuestCart(GuestUserId);
                    HttpContext.Current.Session["CartCount"] = data.CartCount;
                    HttpContext.Current.Session["WishCount"] = data.WishListCount;
                }
                else
                {
                    commonResponse.status = -2;
                    commonResponse.message = "Bad Requrest";
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for remove single product from the wish list
        /// </summary>
        /// <param name="WishListId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("WishListProductToRemove")]
        public IHttpActionResult WishListProductToRemove(int WishListId)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var UserId = HttpContext.Current.Session["UserId"];
                if (Convert.ToInt32(UserId) > 0)
                {
                    productWishListRepository = new ProductWishListRepository();
                    commonResponse.status = productWishListRepository.WishListProductToRemove(WishListId);
                }
                else
                {
                    commonResponse.status = -2;
                    commonResponse.message = "Bad Requrest";
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for remove single product from the wish list
        /// </summary>
        /// <param name="WishListId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GuestWishListProductToRemove")]
        public IHttpActionResult GuestWishListProductToRemove(int WishListId)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                var GuestUserId = new Guid(HttpContext.Current.Session["GuestId"].ToString());
                productWishListRepository = new ProductWishListRepository();
                commonResponse.status = productWishListRepository.GuestWishListProductToRemove(WishListId);
                productCartRepository = new ProductCartRepository();
                var data = productCartRepository.CountWishListGuestCart(GuestUserId);
                HttpContext.Current.Session["CartCount"] = data.CartCount;
                HttpContext.Current.Session["WishCount"] = data.WishListCount;
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Update the Qty in the CartDetails
        /// </summary>
        /// <param name="CartDetailsId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateToCartQty")]
        public IHttpActionResult UpdateToCartQty(int CartDetailsId, int Qty)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (CartDetailsId > 0 && Qty > 0)
                {
                    productCartRepository = new ProductCartRepository();
                    commonResponse.status = productCartRepository.UpdateToCartQty(CartDetailsId, Qty);
                }
                else
                {
                    commonResponse.status = -2;
                }
            }
            catch (System.Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Update the Qty in the CartDetails
        /// </summary>
        /// <param name="CartDetailsId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GuestUpdateToCartQty")]
        public IHttpActionResult GuestUpdateToCartQty(int CartDetailsId, int Qty)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (CartDetailsId > 0 && Qty > 0)
                {
                    productCartRepository = new ProductCartRepository();
                    commonResponse.status = productCartRepository.UpdateGuestToCartQty(CartDetailsId, Qty);
                }
                else
                {
                    commonResponse.status = -2;
                }
            }
            catch (System.Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Save the E-Gift Card
        /// </summary>
        /// <param name="eGiftCard_VM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEGiftCard")]
        public IHttpActionResult SaveEGiftCard(EGiftCard_VM eGiftCard_VM)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                int UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                if (UserId > 0 && eGiftCard_VM != null)
                {
                    eGiftCardRepository = new EGiftCardRepository();
                    eGiftCard_VM.UserId = UserId;
                    eGiftCard_VM.ReferenceId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
                    commonResponse.status = eGiftCardRepository.AddGiftCard(eGiftCard_VM);
                    if (commonResponse.status == 1)
                    {
                        HttpContext.Current.Session["paymentFor"] = (int)Helper.PaymentRef.EGiftCard;
                        HttpContext.Current.Session["eGiftAmt"] = eGiftCard_VM.Amount;
                        HttpContext.Current.Session["eReferenceId"] = eGiftCard_VM.ReferenceId;
                    }
                }
                else
                {
                    commonResponse.status = -2;
                    commonResponse.message = "Bad request";
                }
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Order Details Save
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Checkout")]
        public IHttpActionResult Checkout(FormDataCollection formData)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            var isGuest = Convert.ToBoolean(HttpContext.Current.Session["GusetLogin"]);
            var UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            if (UserId > 0)
            {
                using (var transaction = _DBContext.Database.BeginTransaction())
                {
                    try
                    {
                        var checkout = Convert.ToInt32(HttpContext.Current.Session["CheckOutType"]);
                        var _Customer = _DBContext.Clients.Where(c => c.Id == UserId).FirstOrDefault();

                        #region :: Order

                        Order orderObj = new Order();
                        orderObj.UserId = Convert.ToInt32(UserId);
                        orderObj.IsGuest = false;
                        orderObj.EmailId = _Customer.Email;
                        int Type = Convert.ToInt32(formData["paymentMethod"]);

                        if (Type == (int)PaymentType.COD)
                        {
                            orderObj.PaymentMethod = PaymentType.COD.ToString();
                        }
                        else if (Type == (int)PaymentType.ONLINE)
                        {
                            orderObj.PaymentMethod = PaymentType.ONLINE.ToString();
                        }
                        orderObj.PaymentStatus = PaymentStatus.NOTPAID.ToString();
                        orderObj.ReferenceId = formData["reference_number"];
                        orderObj.CreatedDate = DateTime.Now;
                        orderObj = _DBContext.Orders.Add(orderObj);
                        _DBContext.SaveChanges();

                        //Send email to the zoubandco.
                        _homeRepository = new HomeRepository();                        
                        var _orders = _DBContext.Orders.Where(x => x.ReferenceId == orderObj.ReferenceId).FirstOrDefault();
                        string Emailbody = string.Empty;
                        Emailbody += "Hi Admin,</br>";
                        Emailbody += "Order with referenceId " + _orders.ReferenceId + " has been processed for customer " + string.Format("{0} {1}", Convert.ToString(_Customer.FirstName).Trim(), Convert.ToString(_Customer.LastName).Trim());
                        _homeRepository.SendOrderEmail(Emailbody, "Zoughaib Order Processed : " + _orders.ReferenceId);

                        #endregion

                        #region :: Order Delivery Detail

                        OrderDeliveryDetail orderDeliveryDetailObj = new OrderDeliveryDetail();
                        orderDeliveryDetailObj.OrderId = orderObj.Id;
                        orderDeliveryDetailObj.PrimaryAddress = formData["billingPrimaryAddress"];
                        orderDeliveryDetailObj.PostalCode = formData["billingPostalCode"];
                        orderDeliveryDetailObj.City = formData["billingCity"];
                        orderDeliveryDetailObj.Country = formData["billingCountry"];
                        orderDeliveryDetailObj.SecondaryAddress = formData["billingSecondaryAddress"];
                        orderDeliveryDetailObj.PrimaryPhoneNo = formData["billingPrimaryPhone"];
                        orderDeliveryDetailObj.SecondaryPhoneNo = formData["billingSecondaryPhone"];
                        orderDeliveryDetailObj.AdditionalInformation = formData["billingAdditionalInfo"];
                        orderDeliveryDetailObj.DeliveryDate = formData["deliveryDate"];
                        orderDeliveryDetailObj.DeliveryTime = formData["deliveryTime"];
                        orderDeliveryDetailObj.DeliveryAddress = formData["deliveryAddress"];
                        orderDeliveryDetailObj.DeliveryMessage = formData["deliveryMessage"];
                        _DBContext.OrderDeliveryDetails.Add(orderDeliveryDetailObj);
                        _DBContext.SaveChanges();

                        #endregion

                        if (checkout == (int)CheckoutTye.WISHLIST)
                        {
                            productWishListRepository = new ProductWishListRepository();
                            var checkOutWishList = productWishListRepository.GetProductsWishList(UserId);
                            if (checkOutWishList.Count > 0)
                            {
                                #region :: Order Details

                                List<OrderDetail> orderDetaillst = new List<OrderDetail>();
                                foreach (var checkOut in checkOutWishList)
                                {
                                    var orderDetailObj = new OrderDetail();
                                    orderDetailObj.OrderId = orderObj.Id;
                                    orderDetailObj.ProductName = checkOut.Title;
                                    orderDetailObj.ProductId = checkOut.ProductId;
                                    orderDetailObj.OrderQy = 1;
                                    orderDetailObj.ProductPrice = checkOut.Price;
                                    orderDetailObj.Discount = 0;
                                    orderDetaillst.Add(orderDetailObj);
                                }
                                _DBContext.OrderDetails.AddRange(orderDetaillst);
                                _DBContext.SaveChanges();



                                #endregion

                                #region :: Remove CheckList

                                var removeCheckList = productWishListRepository.CancelWishlist(UserId);

                                #endregion
                                transaction.Commit();
                                commonResponse.status = 1;
                                commonResponse.dataenum = (int)CheckoutTye.WISHLIST;
                                HttpContext.Current.Session["paymentFor"] = (int)Helper.PaymentRef.Purchase;
                            }
                            else
                            {
                                transaction.Rollback();
                                commonResponse.status = -2;
                                commonResponse.message = "No order product found";
                            }
                        }
                        else if (checkout == (int)CheckoutTye.CART)
                        {
                            productCartRepository = new ProductCartRepository();
                            var checkOutCartList = productCartRepository.GetProductsCart(UserId);
                            if (checkOutCartList != null && checkOutCartList.productCartDetails_VMs.Count > 0)
                            {
                                #region :: Order Details

                                List<OrderDetail> orderDetaillst = new List<OrderDetail>();
                                foreach (var checkOut in checkOutCartList.productCartDetails_VMs)
                                {
                                    var orderDetailObj = new OrderDetail();
                                    orderDetailObj.OrderId = orderObj.Id;
                                    orderDetailObj.ProductName = checkOut.Title;
                                    orderDetailObj.ProductId = checkOut.ProductId;
                                    orderDetailObj.OrderQy = (int)checkOut.Quantity;
                                    orderDetailObj.ProductPrice = checkOut.Price;
                                    orderDetailObj.Discount = 0;
                                    orderDetaillst.Add(orderDetailObj);
                                }
                                _DBContext.OrderDetails.AddRange(orderDetaillst);
                                _DBContext.SaveChanges();

                                #endregion

                                #region :: Remove CartList

                                var removeCartList = productCartRepository.EmptyCart(UserId);

                                #endregion

                                transaction.Commit();
                                commonResponse.status = 1;
                                commonResponse.dataenum = (int)CheckoutTye.CART;
                                HttpContext.Current.Session["paymentFor"] = (int)Helper.PaymentRef.Purchase;
                            }
                            else
                            {
                                transaction.Rollback();
                                commonResponse.status = -2;
                                commonResponse.message = "No order product found";
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            HttpContext.Current.Session.Remove("CheckOutType");
                            commonResponse.status = -1;
                        }
                        if (Type == (int)PaymentType.COD)
                        {
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
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        commonResponse.status = -3;
                        commonResponse.message = "Something went wrong";
                        throw;
                    }
                }
            }
            else if (isGuest)
            {
                var GuestUserId = new Guid(HttpContext.Current.Session["GuestId"].ToString());
                var GuestEmail = HttpContext.Current.Session["GuestEmail"].ToString();
                using (var transaction = _DBContext.Database.BeginTransaction())
                {
                    try
                    {
                        var checkout = Convert.ToInt32(HttpContext.Current.Session["CheckOutType"]);

                        #region :: Order

                        Order orderObj = new Order();
                        //orderObj.UserId = Convert.ToInt32(UserId);
                        orderObj.IsGuest = true;
                        orderObj.EmailId = GuestEmail;
                        int Type = Convert.ToInt32(formData["paymentMethod"]);

                        if (Type == (int)PaymentType.COD)
                        {
                            orderObj.PaymentMethod = PaymentType.COD.ToString();
                        }
                        else if (Type == (int)PaymentType.ONLINE)
                        {
                            orderObj.PaymentMethod = PaymentType.ONLINE.ToString();
                        }
                        orderObj.PaymentStatus = PaymentStatus.NOTPAID.ToString();
                        orderObj.ReferenceId = formData["reference_number"];
                        orderObj.CreatedDate = DateTime.Now;
                        orderObj = _DBContext.Orders.Add(orderObj);
                        _DBContext.SaveChanges();

                        //Send email to the zoubandco.
                        _homeRepository = new HomeRepository();
                        //var _Customer = _DBContext.Clients.Where(c => c.Id == UserId).FirstOrDefault();
                        var _orders = _DBContext.Orders.Where(x => x.ReferenceId == orderObj.ReferenceId).FirstOrDefault();
                        string Emailbody = string.Empty;
                        Emailbody += "Hi Admin,</br>";
                        Emailbody += "Order with referenceId " + _orders.ReferenceId;
                        Emailbody += " has been processed for Guest User ";
                        Emailbody += " Email Id "+ GuestEmail;
                        _homeRepository.SendOrderEmail(Emailbody, "Zoughaib Order Processed : " + _orders.ReferenceId);

                        #endregion

                        #region :: Order Delivery Detail

                        OrderDeliveryDetail orderDeliveryDetailObj = new OrderDeliveryDetail();
                        orderDeliveryDetailObj.OrderId = orderObj.Id;
                        orderDeliveryDetailObj.PrimaryAddress = formData["billingPrimaryAddress"];
                        orderDeliveryDetailObj.PostalCode = formData["billingPostalCode"];
                        orderDeliveryDetailObj.City = formData["billingCity"];
                        orderDeliveryDetailObj.Country = formData["billingCountry"];
                        orderDeliveryDetailObj.SecondaryAddress = formData["billingSecondaryAddress"];
                        orderDeliveryDetailObj.PrimaryPhoneNo = formData["billingPrimaryPhone"];
                        orderDeliveryDetailObj.SecondaryPhoneNo = formData["billingSecondaryPhone"];
                        orderDeliveryDetailObj.AdditionalInformation = formData["billingAdditionalInfo"];
                        orderDeliveryDetailObj.DeliveryDate = formData["deliveryDate"];
                        orderDeliveryDetailObj.DeliveryTime = formData["deliveryTime"];
                        orderDeliveryDetailObj.DeliveryAddress = formData["deliveryAddress"];
                        orderDeliveryDetailObj.DeliveryMessage = formData["deliveryMessage"];
                        _DBContext.OrderDeliveryDetails.Add(orderDeliveryDetailObj);
                        _DBContext.SaveChanges();

                        #endregion

                        if (checkout == (int)CheckoutTye.WISHLIST)
                        {
                            productWishListRepository = new ProductWishListRepository();
                            var checkOutWishList = productWishListRepository.GetProductsGuestWishList(GuestUserId);
                            if (checkOutWishList.Count > 0)
                            {
                                #region :: Order Details

                                List<OrderDetail> orderDetaillst = new List<OrderDetail>();
                                foreach (var checkOut in checkOutWishList)
                                {
                                    var orderDetailObj = new OrderDetail();
                                    orderDetailObj.OrderId = orderObj.Id;
                                    orderDetailObj.ProductName = checkOut.Title;
                                    orderDetailObj.ProductId = checkOut.ProductId;
                                    orderDetailObj.OrderQy = 1;
                                    orderDetailObj.ProductPrice = checkOut.Price;
                                    orderDetailObj.Discount = 0;
                                    orderDetaillst.Add(orderDetailObj);
                                }
                                _DBContext.OrderDetails.AddRange(orderDetaillst);
                                _DBContext.SaveChanges();



                                #endregion

                                #region :: Remove CheckList

                                var removeCheckList = productWishListRepository.CancelGuestWishlist(GuestUserId);
                                HttpContext.Current.Session["WishCount"] = 0;

                                #endregion
                                transaction.Commit();
                                commonResponse.status = 1;
                                commonResponse.dataenum = (int)CheckoutTye.WISHLIST;
                                HttpContext.Current.Session["paymentFor"] = (int)Helper.PaymentRef.Purchase;
                            }
                            else
                            {
                                transaction.Rollback();
                                commonResponse.status = -2;
                                commonResponse.message = "No order product found";
                            }
                        }
                        else if (checkout == (int)CheckoutTye.CART)
                        {
                            productCartRepository = new ProductCartRepository();
                            var checkOutCartList = productCartRepository.GetGuestProductsCart(GuestUserId);
                            if (checkOutCartList != null && checkOutCartList.productGuestCartDetails_VMs.Count > 0)
                            {
                                #region :: Order Details

                                List<OrderDetail> orderDetaillst = new List<OrderDetail>();
                                foreach (var checkOut in checkOutCartList.productGuestCartDetails_VMs)
                                {
                                    var orderDetailObj = new OrderDetail();
                                    orderDetailObj.OrderId = orderObj.Id;
                                    orderDetailObj.ProductName = checkOut.Title;
                                    orderDetailObj.ProductId = checkOut.ProductId;
                                    orderDetailObj.OrderQy = (int)checkOut.Quantity;
                                    orderDetailObj.ProductPrice = checkOut.Price;
                                    orderDetailObj.Discount = 0;
                                    orderDetaillst.Add(orderDetailObj);
                                }
                                _DBContext.OrderDetails.AddRange(orderDetaillst);
                                _DBContext.SaveChanges();

                                #endregion

                                #region :: Remove CartList

                                var removeCartList = productCartRepository.EmptyGuestCart(GuestUserId);
                                HttpContext.Current.Session["CartCount"] = 0;

                                #endregion

                                transaction.Commit();
                                commonResponse.status = 1;
                                commonResponse.dataenum = (int)CheckoutTye.CART;
                                HttpContext.Current.Session["paymentFor"] = (int)Helper.PaymentRef.Purchase;
                            }
                            else
                            {
                                transaction.Rollback();
                                commonResponse.status = -2;
                                commonResponse.message = "No order product found";
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            HttpContext.Current.Session.Remove("CheckOutType");
                            commonResponse.status = -1;
                        }
                        if (Type == (int)PaymentType.COD)
                        {
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
                            _homeRepository.SendOrderEmailToUser(orderItems, "Zoughaib Order Processed : " + _orders.ReferenceId, "Guest User", GuestEmail);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        commonResponse.status = -3;
                        commonResponse.message = "Something went wrong";
                        throw;
                    }
                }
            }
            else
            {
                HttpContext.Current.Session.Remove("CheckOutType");
                commonResponse.status = 0;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for add product to cart
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AddToGuestCart")]
        public IHttpActionResult AddToGuestCart(int ProductId)
        {
            CommonResponse<CountWishListCart> commonResponse = new CommonResponse<CountWishListCart>();
            try
            {
                var GuestUserId = new Guid(HttpContext.Current.Session["GuestId"].ToString());
                int result = 0;
                productCartRepository = new ProductCartRepository();
                var Cart = _DBContext.ProductCartGuests.Where(x => x.GuestUserId == GuestUserId).FirstOrDefault();

                if (Cart != null)
                {
                    var CartDetails = _DBContext.ProductCartDetailsGuests.Where(x => x.ProductId == ProductId && x.ProductCartGuestId == Cart.Id).FirstOrDefault();
                    if (CartDetails != null)
                    {
                        result = productCartRepository.UpdateProductsGuestCartDetails(CartDetails.Id, 1);
                    }
                    else
                    {
                        result = productCartRepository.AddProductsToGuestCartDetails(Cart.Id, ProductId, 1);
                    }
                }
                else
                {
                    result = productCartRepository.AddtoProductsGuestCart(GuestUserId, ProductId, 1);
                }
                commonResponse.status = result;
                commonResponse.dataenum = productCartRepository.CountWishListGuestCart(GuestUserId);
                HttpContext.Current.Session["CartCount"] = commonResponse.dataenum.CartCount;
                HttpContext.Current.Session["WishCount"] = commonResponse.dataenum.WishListCount;
            }
            catch (Exception ex)
            {
                commonResponse.status = -1;
                commonResponse.message = ex.ToString();
            }
            return Ok(commonResponse);
        }
        #endregion
    }
}
