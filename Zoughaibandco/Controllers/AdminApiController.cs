using Spire.Xls;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Zoughaibandco.Models;
using Zoughaibandco.Repository;
using Zoughaibandco.Utility;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Controllers
{
    [RoutePrefix("api/AdminApi")]
    public class AdminApiController : ApiController
    {
        Zoughaibandco_DBEntities _DBContext;
        ClientRepository clientRepository;
        ContactRepository contactRepository;
        ProductRepository productRepository;
        ColorRepository colorRepository;
        EGiftCardRepository eGiftCardRepository;
        OrdersRepository ordersRepository;
        CollectionRepository collectionRepository;
        BannersRepository bannersRepository;

        public AdminApiController()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        /// <summary>
        /// Function for Admin authentication
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
                if (login_VM != null && login_VM.LoginPassword != null)
                {
                    var admin = _DBContext.Admins.Where(c => c.Username.ToLower() == login_VM.LoginUsername.ToLower()).SingleOrDefault();
                    if (admin != null)
                    {
                        var EncryptPwd = Helper.Encrypt(login_VM.LoginPassword);
                        //var DecryptPwd = Helper.Decrypt(admin.Password);
                        if (EncryptPwd == admin.Password)
                        {
                            commonResponse.status = 1;
                            HttpContext.Current.Session["AdminId"] = admin.Id;
                        }
                        else
                        {
                            commonResponse.status = 0;
                        }
                    }
                    else
                    {
                        commonResponse.status = 0;
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
        /// Function for get client list
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClientList")]
        public IHttpActionResult ClientList(string search)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null)
                {
                    clientRepository = new ClientRepository();
                    var result = clientRepository.GetAllClient(search);
                    if (result.Count > 0)
                    {
                        commonResponse.status = 1;
                        commonResponse.dataenum = GetRazorViewAsString(result, "~/Views/Admin/ClientList_PartialView.cshtml");
                    }
                    else
                    {
                        commonResponse.status = 0;
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
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for delete client
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteClient")]
        public IHttpActionResult DeleteClient(int ClientId)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null && ClientId > 0)
                {
                    clientRepository = new ClientRepository();
                    commonResponse.status = clientRepository.DeleteClient(ClientId);
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }


        /// <summary>
        /// Function for get Help Message List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("HelpMsgList")]
        public IHttpActionResult HelpMsgList()
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null)
                {
                    contactRepository = new ContactRepository();
                    var result = contactRepository.GetAllHelpMessage();
                    if (result.Count > 0)
                    {
                        commonResponse.status = 1;
                        commonResponse.dataenum = GetRazorViewAsString(result, "~/Views/Admin/HelpMessage_PartialView.cshtml");
                    }
                    else
                    {
                        commonResponse.status = 0;
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
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for delete Help Message
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteHelpMsg")]
        public IHttpActionResult DeleteHelpMsg(int HelpId)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null && HelpId > 0)
                {
                    contactRepository = new ContactRepository();
                    commonResponse.status = contactRepository.DeleteHelpMessage(HelpId);
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for get product list
        /// </summary>
        /// <param name="search"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ProductList")]
        public IHttpActionResult ProductList(string search, string filter)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null)
                {
                    productRepository = new ProductRepository();
                    var result = productRepository.GetAllProductsByFilter(search, filter);
                    //var result = productRepository.GetAllProducts(search);
                    if (result.Count > 0)
                    {
                        commonResponse.status = 1;
                        commonResponse.dataenum = GetRazorViewAsString(result, "~/Views/Admin/Product_PartialView.cshtml");
                    }
                    else
                    {
                        commonResponse.status = 0;
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
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for delete product
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteProduct")]
        public IHttpActionResult DeleteProduct(int ProductId)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null && ProductId > 0)
                {
                    productRepository = new ProductRepository();
                    commonResponse.status = productRepository.DeleteProduct(ProductId);
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Save/Update product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveProduct")]
        public IHttpActionResult SaveProduct()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null)
                {
                    productRepository = new ProductRepository();

                    string fname = string.Empty;
                    string fPath = string.Empty;
                    Product product = new Product();
                    
                    if (HttpContext.Current.Request.Form["ProductId"] != "")
                    {
                        product.Id = Convert.ToInt32(HttpContext.Current.Request.Form["ProductId"]);
                    }

                    product.Title = HttpContext.Current.Request.Form["Title"];
                    product.ReferenceNO = HttpContext.Current.Request.Form["ReferenceNO"];

                    //product.Price = Convert.ToDecimal(HttpContext.Current.Request.Form["Price"]);
                    //product.PriceDiscounted = Convert.ToDecimal(HttpContext.Current.Request.Form["PriceDiscounted"]);

                    if (HttpContext.Current.Request.Form["Price"] != "")
                    {
                        product.Price = Convert.ToDecimal(HttpContext.Current.Request.Form["Price"]);
                    }

                    if (HttpContext.Current.Request.Form["PriceDiscounted"] != "")
                    {
                        product.PriceDiscounted = Convert.ToDecimal(HttpContext.Current.Request.Form["PriceDiscounted"]);
                    }

                    product.Description = HttpContext.Current.Request.Form["Description"];
                    product.IsPublished = Convert.ToBoolean(HttpContext.Current.Request.Form["IsPublished"]);
                    product.CategoryId = Convert.ToInt32(HttpContext.Current.Request.Form["CategoryId"]);
                    product.SubCategoryId = Convert.ToInt32(HttpContext.Current.Request.Form["SubCategoryId"]);
                    product.CollectionId = Convert.ToInt32(HttpContext.Current.Request.Form["CollectionId"]);
                    var ColorList = HttpContext.Current.Request.Form["ColorList"].Split('#');

                    product.Materials = Convert.ToString(HttpContext.Current.Request.Form["Material"]);

                    product.GoldWeight = HttpContext.Current.Request.Form["GoldWeight"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["GoldWeight"]) : 0;

                    product.Round = HttpContext.Current.Request.Form["Round"] != "" ?
                       Convert.ToDecimal(HttpContext.Current.Request.Form["Round"]) : 0;

                    product.Marquis = HttpContext.Current.Request.Form["Marquis"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Marquis"]) : 0;

                    //product.Ruby = Convert.ToDecimal(HttpContext.Current.Request.Form["Ruby"]);
                    product.Ruby = HttpContext.Current.Request.Form["Ruby"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Ruby"]) : 0;

                   // product.Sapphire = Convert.ToDecimal(HttpContext.Current.Request.Form["Sapphire"]);
                    product.Sapphire = HttpContext.Current.Request.Form["Sapphire"] != "" ? 
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Sapphire"]):0;

                    //product.Emerald = Convert.ToDecimal(HttpContext.Current.Request.Form["Emerald"]);
                    product.Emerald = HttpContext.Current.Request.Form["Emerald"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Emerald"]) : 0;

                    //product.Fancy = Convert.ToDecimal(HttpContext.Current.Request.Form["Fancy"]);
                    product.Fancy = HttpContext.Current.Request.Form["Fancy"] != "" ? 
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Fancy"]):0;


                    //product.Princes = Convert.ToDecimal(HttpContext.Current.Request.Form["Princes"]);
                    product.Princes = HttpContext.Current.Request.Form["Princes"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Princes"]):0;



                    //product.Baguette = Convert.ToDecimal(HttpContext.Current.Request.Form["Baguette"]);
                    product.Baguette = HttpContext.Current.Request.Form["Baguette"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Baguette"]) : 0;

                    //product.Triangle = Convert.ToDecimal(HttpContext.Current.Request.Form["Triangle"]);
                    product.Triangle = HttpContext.Current.Request.Form["Triangle"] !="" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Triangle"]) : 0;

                    //product.Pear = Convert.ToDecimal(HttpContext.Current.Request.Form["Pear"]);
                    product.Pear = HttpContext.Current.Request.Form["Pear"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["Pear"]) : 0;


                    //product.black = Convert.ToDecimal(HttpContext.Current.Request.Form["black"]);
                    product.black = HttpContext.Current.Request.Form["black"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["black"]) : 0;


                    //product.semiprecious = Convert.ToDecimal(HttpContext.Current.Request.Form["semiprecious"]);
                    product.semiprecious = HttpContext.Current.Request.Form["semiprecious"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["semiprecious"]):0;

                    //product.diamond = Convert.ToDecimal(HttpContext.Current.Request.Form["diamond"]);
                    product.diamond = HttpContext.Current.Request.Form["diamond"] != "" ?
                        Convert.ToDecimal(HttpContext.Current.Request.Form["diamond"]):0;

                    product.ParentId = Convert.ToInt32(HttpContext.Current.Request.Form["ParentId"]);
                    if(product.ParentId == 0)
                    {
                        product.IsParent = true;
                    }

                    if (product != null)
                    {
                        if (HttpContext.Current.Request.Files.Count > 0) //File Upload
                        {
                            HttpPostedFile file = HttpContext.Current.Request.Files[0];
                            string Folder = Convert.ToString(ConfigurationManager.AppSettings["ProductImg"]);
                            string DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                            fPath = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                            fname = fPath;
                            fPath = Path.Combine(DirectoryPath, fPath);
                            if (!Directory.Exists(DirectoryPath))
                            {
                                Directory.CreateDirectory(DirectoryPath);
                            }
                            file.SaveAs(fPath);
                            product.ImageName = fname;
                        }
                        if (product.Id > 0) //Update Product
                        {
                            product.UpdatedDate = DateTime.Now;
                            var oldProduct = _DBContext.Products.Find(product.Id);
                            if (oldProduct != null)
                            {
                                if (HttpContext.Current.Request.Files.Count == 0)
                                {
                                    product.ImageName = oldProduct.ImageName;
                                }
                                product.CreatedDate = oldProduct.CreatedDate;
                            }

                            var hasChild = _DBContext.Products.Where(x => x.ParentId == product.Id && product.ParentId > 0).ToList();
                            if(hasChild.Count > 0)
                            {
                                commonResponse.status = -1;
                                commonResponse.message = "Product has already child, so you can not make it child for other products.";
                            } else
                            {
                                _DBContext.Products.AddOrUpdate(product);
                                commonResponse.status = _DBContext.SaveChanges();
                            }
                            
                        }
                        else // Add Product
                        {
                            var ProductNameExists = productRepository.GetAllProducts(null).Where(x => x.Title == product.Title).FirstOrDefault();

                            if (ProductNameExists != null)
                            {
                                commonResponse.status = 0;
                                commonResponse.message = "Product Name already exists";
                            }
                            else
                            {
                                product.CreatedDate = DateTime.Now;
                                product = _DBContext.Products.Add(product);
                                commonResponse.status = _DBContext.SaveChanges();
                            }
                        }

                        //add Color
                        if (commonResponse.status > 0)
                        {
                            colorRepository = new ColorRepository();
                            colorRepository.AddColor(product.Id, ColorList);
                        }
                    }
                    else
                    {
                        commonResponse.status = -2;
                        commonResponse.message = "bad request";
                    }
                }
                else
                {
                    commonResponse.status = -1;
                    commonResponse.message = "Session expire";
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
        /// Function for get E-GiftCard List
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("EGiftCardList")]
        public IHttpActionResult EGiftCardList()
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null)
                {
                    eGiftCardRepository = new EGiftCardRepository();
                    var result = eGiftCardRepository.GetEGiftCards();
                    if (result.Count > 0)
                    {
                        commonResponse.status = 1;
                        commonResponse.dataenum = GetRazorViewAsString(result, "~/Views/Admin/EGiftCardList_PartialView.cshtml");
                    }
                    else
                    {
                        commonResponse.status = 0;
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
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for delete E-GiftCard
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteEGiftCard")]
        public IHttpActionResult DeleteEGiftCard(int EGiftCardId)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null && EGiftCardId > 0)
                {
                    eGiftCardRepository = new EGiftCardRepository();
                    commonResponse.status = eGiftCardRepository.DeleteEGiftCard(EGiftCardId);
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Update E-GiftCard
        /// </summary>
        /// <param name="eGiftCard_VM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateEGiftCard")]
        public IHttpActionResult UpdateEGiftCard(EGiftCard_VM eGiftCard_VM)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null && eGiftCard_VM != null)
                {
                    eGiftCardRepository = new EGiftCardRepository();
                    commonResponse.status = eGiftCardRepository.UpdateEGiftCard(eGiftCard_VM);
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }


        /// <summary>
        /// Function for get product list
        /// </summary>
        /// <param name="paymentType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("OrderList")]
        public IHttpActionResult OrderList(string paymentType, string startDate, string endDate)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null)
                {
                    ordersRepository = new OrdersRepository();
                    var result = ordersRepository.GetAll(paymentType, startDate, endDate);
                    if (result.Any())
                    {
                        commonResponse.status = 1;
                        commonResponse.dataenum = GetRazorViewAsString(result, "~/Views/Admin/OrderList_PartialView.cshtml");
                    }
                    else
                    {
                        commonResponse.status = 0;
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
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }


        public static string GetRazorViewAsString(object model, string filePath)
        {
            var st = new StringWriter();
            var context = new HttpContextWrapper(HttpContext.Current);
            var routeData = new System.Web.Routing.RouteData();
            var controllerContext = new System.Web.Mvc.ControllerContext(new System.Web.Routing.RequestContext(context, routeData), new AdminController());
            var razor = new System.Web.Mvc.RazorView(controllerContext, filePath, null, false, null);
            razor.Render(new System.Web.Mvc.ViewContext(controllerContext, razor, new System.Web.Mvc.ViewDataDictionary(model), new System.Web.Mvc.TempDataDictionary(), st), st);
            return st.ToString();
        }

        /// <summary>
        /// Function for Import Products
        /// </summary>
        /// <param name="eGiftCard_VM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportProduct")]
        public IHttpActionResult ImportProduct()
        {
            var files = HttpContext.Current.Request.Files[0];
            var productRepository = new ProductRepository();
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromStream(files.InputStream);
                Worksheet worksheet = workbook.Worksheets[0];
                var datatable = worksheet.ExportDataTable(worksheet.AllocatedRange, false, true);
                int count = datatable.Rows.Count;
                //commonResponse.status = productRepository.AddBulkProduct_Test(datatable);
                commonResponse.status = productRepository.AddBulkProduct(datatable);
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }


        /// <summary>
        /// Function for get collection list
        /// </summary>
        /// <param name="search"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CollectionList")]
        public IHttpActionResult CollectionList(string search, string filter)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null)
                {
                    collectionRepository = new CollectionRepository();
                    var result = collectionRepository.GetAllCollectionByFilter(search, filter);
                    if (result.Count > 0)
                    {
                        commonResponse.status = 1;
                        commonResponse.dataenum = GetRazorViewAsString(result, "~/Views/Admin/Collection_PartialView.cshtml");
                    }
                    else
                    {
                        commonResponse.status = 0;
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
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for delete collection
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteCollection")]
        public IHttpActionResult DeleteCollection(int CollectionId)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null && CollectionId > 0)
                {
                    collectionRepository = new CollectionRepository();
                    commonResponse.status = collectionRepository.DeleteCollection(CollectionId);
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }

        /// <summary>
        /// Function for Save/Update Collection
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveCollection")]
        public IHttpActionResult SaveCollection()
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null)
                {
                    string fname = string.Empty;
                    string fPath = string.Empty;
                    string Folder = string.Empty;
                    string DirectoryPath = string.Empty;
                    collectionRepository = new CollectionRepository();

                    Collection collection = new Collection();
                    collection.Id = Convert.ToInt32(HttpContext.Current.Request.Form["CollectionId"]);
                    collection.Name = HttpContext.Current.Request.Form["Name"];
                    collection.Active = Convert.ToBoolean(HttpContext.Current.Request.Form["Active"]);
                    collection.IsAdminAdded = Convert.ToBoolean(HttpContext.Current.Request.Form["IsAdminAdded"]);
                    collection.RouteName = HttpContext.Current.Request.Form["RouteName"];
                    collection.AboutText = HttpContext.Current.Request.Form["AboutText"];
                    collection.Order = Convert.ToInt32(HttpContext.Current.Request.Form["Order"]);
                    collection.Side = HttpContext.Current.Request.Form["Side"];
                    collection.CreatedDate = DateTime.Now;

                    //get the colors here.
                    collection.BreadCrumColor = HttpContext.Current.Request.Form["BreadCrumColor"];
                    collection.LookBookColor = HttpContext.Current.Request.Form["LookBookTextColor"];
                    collection.LookBookBorderColor = HttpContext.Current.Request.Form["LookBookBorderColor"];
                    collection.ShopTheCollectionTextColor = HttpContext.Current.Request.Form["ShopTheCollectionTextColor"];

                    var KeyName = collection.Name + "_collection";
                    if (collection != null)
                    {
                        //CollectionPageImg
                        //CollectionBannerImg
                        if (HttpContext.Current.Request.Files.Count > 0) //File Upload
                        {
                            if (HttpContext.Current.Request.Files["BannerImage"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["BannerImage"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionBannerImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "1_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);

                                bannersRepository = new BannersRepository();
                                var bannerdata = bannersRepository.GetBannersByKeyNameName(KeyName);
                                if (bannerdata != null)
                                {
                                    Banner banner = new Banner();
                                    banner.Id = bannerdata.Id;
                                    banner.KeyName = KeyName.ToLower();
                                    banner.ImgName = fname;
                                    _DBContext.Banners.AddOrUpdate(banner);
                                    _DBContext.SaveChanges();
                                    collection.BannersId = banner.Id;
                                }
                                else
                                {
                                    Banner banner = new Banner();
                                    banner.KeyName = KeyName.ToLower();
                                    banner.ImgName = fname;
                                    _DBContext.Banners.Add(banner);
                                    _DBContext.SaveChanges();
                                    collection.BannersId = banner.Id;
                                }
                            }
                            if (HttpContext.Current.Request.Files["BackgroundImage"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["BackgroundImage"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "2_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.BackgroundImage = fname;
                            }
                            if (HttpContext.Current.Request.Files["ForegroundImage"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["ForegroundImage"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "3_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.ForegroundImage = fname;
                            }
                            if (HttpContext.Current.Request.Files["LogoImage"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["LogoImage"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionLogoImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "4_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.LogoImage = fname;
                            }
                            if (HttpContext.Current.Request.Files["PageImage1"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["PageImage1"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "5_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.PageImage1 = fname;
                            }
                            if (HttpContext.Current.Request.Files["PageImage2"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["PageImage2"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "6_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.PageImage2 = fname;
                            }
                            if (HttpContext.Current.Request.Files["PageImage3"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["PageImage3"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "7_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.PageImage3 = fname;
                            }
                            if (HttpContext.Current.Request.Files["PageImage4"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["PageImage4"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "8_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.PageImage4 = fname;
                            }
                            if (HttpContext.Current.Request.Files["PageImage5"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["PageImage5"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "9_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.PageImage5 = fname;
                            }
                            if (HttpContext.Current.Request.Files["PageImage6"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["PageImage6"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "10_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.PageImage6 = fname;
                            }
                            if (HttpContext.Current.Request.Files["PageImage7"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["PageImage7"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "11_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.PageImage7 = fname;
                            }
                            if (HttpContext.Current.Request.Files["PageImage8"] != null)
                            {
                                HttpPostedFile file = HttpContext.Current.Request.Files["PageImage8"];
                                Folder = Convert.ToString(ConfigurationManager.AppSettings["CollectionPageImg"]);
                                DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Folder + "/");
                                fPath = "12_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                                fname = fPath;
                                fPath = Path.Combine(DirectoryPath, fPath);
                                if (!Directory.Exists(DirectoryPath))
                                {
                                    Directory.CreateDirectory(DirectoryPath);
                                }
                                file.SaveAs(fPath);
                                collection.PageImage8 = fname;
                            }
                        }
                        if (collection.Id > 0) //Update collection
                        {
                            //collection.UpdatedDate = DateTime.Now;
                            var oldProduct = _DBContext.Collections.Find(collection.Id);
                            if (oldProduct != null)
                            {
                                if (HttpContext.Current.Request.Files["BackgroundImage"] == null)
                                {
                                    collection.BackgroundImage = oldProduct.BackgroundImage;
                                }
                                if (HttpContext.Current.Request.Files["ForegroundImage"] == null)
                                {
                                    collection.ForegroundImage = oldProduct.ForegroundImage;
                                }
                                if (HttpContext.Current.Request.Files["LogoImage"] == null)
                                {
                                    collection.LogoImage = oldProduct.LogoImage;
                                }
                                if (HttpContext.Current.Request.Files["PageImage1"] == null)
                                {
                                    collection.PageImage1 = oldProduct.PageImage1;
                                }
                                if (HttpContext.Current.Request.Files["PageImage2"] == null)
                                {
                                    collection.PageImage2 = oldProduct.PageImage2;
                                }
                                if (HttpContext.Current.Request.Files["PageImage3"] == null)
                                {
                                    collection.PageImage3 = oldProduct.PageImage3;
                                }
                                if (HttpContext.Current.Request.Files["PageImage4"] == null)
                                {
                                    collection.PageImage4 = oldProduct.PageImage4;
                                }
                                if (HttpContext.Current.Request.Files["PageImage5"] == null)
                                {
                                    collection.PageImage5 = oldProduct.PageImage5;
                                }
                                if (HttpContext.Current.Request.Files["PageImage6"] == null)
                                {
                                    collection.PageImage6 = oldProduct.PageImage6;
                                }
                                if (HttpContext.Current.Request.Files["PageImage7"] == null)
                                {
                                    collection.PageImage7 = oldProduct.PageImage7;
                                }
                                if (HttpContext.Current.Request.Files["PageImage8"] == null)
                                {
                                    collection.PageImage8 = oldProduct.PageImage8;
                                }
                                collection.CreatedDate = oldProduct.CreatedDate;
                            }
                            if (collection.BannersId == null)
                            {
                                if (oldProduct.BannersId != null)
                                    collection.BannersId = oldProduct.BannersId;
                            }
                            _DBContext.Collections.AddOrUpdate(collection);
                            _DBContext.SaveChanges();
                            commonResponse.status = 1;
                            commonResponse.message = "collection details updated";
                        }
                        else // Add Product
                        {
                            var collectionNameExists = collectionRepository.GetAllCollections(null).Where(x => x.CollectionName == collection.Name).FirstOrDefault();

                            if (collectionNameExists != null)
                            {
                                commonResponse.status = 0;
                                commonResponse.message = "collection Name already exists";
                            }
                            else
                            {
                                collection.IsAdminAdded = true;
                                collection.CreatedDate = DateTime.Now;
                                collection = _DBContext.Collections.Add(collection);
                                commonResponse.status = _DBContext.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        commonResponse.status = -2;
                        commonResponse.message = "bad request";
                    }
                }
                else
                {
                    commonResponse.status = -1;
                    commonResponse.message = "Session expire";
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
        /// Function for delete Collection Images
        /// </summary>
        /// <param name="CollectionId"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteImage")]
        public IHttpActionResult DeleteImage(int CollectionId, string ColumnName)
        {
            CommonResponse<string> commonResponse = new CommonResponse<string>();
            try
            {
                if (HttpContext.Current.Session["AdminId"] != null && CollectionId > 0 && ColumnName != "")
                {
                    collectionRepository = new CollectionRepository();
                    commonResponse.status = collectionRepository.DeleteCollectionImage(CollectionId, ColumnName);
                }
                else
                {
                    commonResponse.status = -1;
                }
            }
            catch (Exception ex)
            {
                commonResponse.message = ex.ToString();
                commonResponse.status = -2;
            }
            return Ok(commonResponse);
        }
    }
}
