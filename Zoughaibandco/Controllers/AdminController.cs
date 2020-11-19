using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Zoughaibandco.Models;
using Zoughaibandco.Repository;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Controllers
{
    public class AdminController : Controller
    {
        Zoughaibandco_DBEntities _DBContext;
        ProductRepository productRepository;
        CollectionRepository collectionRepository;
        CategoryRepository categoryRepository;
        ClientRepository clientRepository;
        ContactRepository contactRepository;
        EGiftCardRepository eGiftCardRepository;
        OrdersRepository ordersRepository;
        BannersRepository bannersRepository;

        public AdminController()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult AddEditProduct(int ProductId = 0)
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                productRepository = new ProductRepository();
                collectionRepository = new CollectionRepository();
                categoryRepository = new CategoryRepository();
                
                
                //ViewBag.Categories = new SelectList(categoryRepository.getAllCategory(), "Id", "CategoryName");
                var categoryList = categoryRepository.getAllCategory();
                categoryList.Add(new Category_VM
                {
                    Id = 0,
                    CategoryName = "---SELECT---",
                });
                categoryList = categoryList.OrderBy(x => x.Id).ToList();
                ViewBag.Categories = new SelectList(categoryList, "Id", "CategoryName");


                var collectionList = collectionRepository.GetAllCollections();
                collectionList.Add(new Collection_VM
                {
                    CollectionId = 0,
                    CollectionName = "---SELECT---"
                });
                collectionList = collectionList.OrderBy(x => x.CollectionId).ToList();
                ViewBag.CollectionsList = new SelectList(collectionList, "CollectionId", "CollectionName");
                //ViewBag.CollectionsList = new SelectList(collectionRepository.GetAllCollections(), "CollectionId", "CollectionName");

                ViewBag.SubCategories = new SelectList(_DBContext.SubCategories.ToList(), "Id", "SubCategoryName");
                ViewBag.ParentProductList = new SelectList(productRepository.GetParentProduct(ProductId), "ProductId", "Title");
                if (ProductId > 0)
                {
                    var product = productRepository.GetProductById(ProductId);
                    return View(product);
                }
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Client()
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult EditClient(int ClientId)
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                if (ClientId > 0)
                {
                    clientRepository = new ClientRepository();
                    var client = clientRepository.GetClientById(ClientId);
                    return View(client);
                }
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult SaveClient(Client_VM client_VM)
        {
            var UserId = Session["AdminId"];
            if (client_VM != null)
            {
                clientRepository = new ClientRepository();
                clientRepository.UpdateClient(client_VM);
                return RedirectToAction("Client", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult HelpMessage()
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                contactRepository = new ContactRepository();
                return View(contactRepository.GetAllHelpMessage());
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult EditHelpMessage(int HelpId)
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                contactRepository = new ContactRepository();
                return View(contactRepository.GetHelpMessage(HelpId));
            }
            return RedirectToAction("Index", "Admin");
        }

        [ValidateInput(false)]
        public ActionResult UpdateHelpMessage(Contact_VM contact_VM)
        {
            var UserId = Session["AdminId"];
            if (contact_VM != null)
            {
                contactRepository = new ContactRepository();
                contactRepository.UpdateHelpMessage(contact_VM);
                return RedirectToAction("HelpMessage", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult GiftCardRequest()
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult EditGiftCard(int EGiftCardId)
        {
            var UserId = Session["AdminId"];
            if (UserId != null && EGiftCardId > 0)
            {
                eGiftCardRepository = new EGiftCardRepository();
                return View(eGiftCardRepository.GetEGiftCardById(EGiftCardId));
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Orders()
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                //ordersRepository = new OrdersRepository();
                //return View(ordersRepository.GetAll());
                return View();
            }
            return RedirectToAction("Index", "Admin");

        }

        public ActionResult Collection()
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult AddEditCollection(int CollectionId)
        {
            var UserId = Session["AdminId"];
            if (UserId != null)
            {
                collectionRepository = new CollectionRepository();
                categoryRepository = new CategoryRepository();
                bannersRepository = new BannersRepository();

                List<Side> SideList = new List<Side>();
                Side side = new Side();
                side.sId = 1;
                side.sName = "Right";
                SideList.Add(side);
                side = new Side();
                side.sId = 2;
                side.sName = "Left";
                SideList.Add(side);

                ViewBag.SideList = new SelectList(SideList, "sId", "sName");
                ViewBag.Banners = new SelectList(bannersRepository.GetBanners(), "Id", "BannerKeyName");
                ViewBag.Categories = new SelectList(categoryRepository.getAllCategory(), "Id", "CategoryName");
                ViewBag.CollectionsList = new SelectList(collectionRepository.GetAllCollections(), "CollectionId", "CollectionName");
                ViewBag.SubCategories = new SelectList(_DBContext.SubCategories.ToList(), "Id", "SubCategoryName");

                if (CollectionId > 0)
                {
                    var collection = collectionRepository.GetCollectionsById(CollectionId);
                    return View(collection);
                }
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }

    }
}