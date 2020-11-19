using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Zoughaibandco.Models;
using Zoughaibandco.Repository;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Controllers
{
    public class CategoryController : Controller
    {
        Zoughaibandco_DBEntities _DBContext;
        CollectionRepository collectionRepository;
        ProductRepository productRepository;

        public CategoryController()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }
        public ActionResult Index(int Id, string Menu, string Price, string Collection, string SubCat)
        {
            if(Id > 0 && !string.IsNullOrEmpty(Menu))
            {
                collectionRepository = new CollectionRepository();
                productRepository = new ProductRepository();

                if (Menu == MenuName.Collection.ToString())
                {
                    ViewBag.ProductList = productRepository.GetProductsByCollection(Id, Price);
                    var result = collectionRepository.GetCollectionsById(Id);
                    return View(result);
                }
                else if (Menu == MenuName.Jewellery.ToString())
                {
                    ViewBag.CollectionsList = new SelectList(collectionRepository.GetAllCollections(), "CollectionId", "CollectionName");
                    ViewBag.SubCategories = new SelectList(_DBContext.SubCategories.ToList(), "Id", "SubCategoryName");

                    ViewBag.ProductList = productRepository.GetProductsByCategory(Id, Price, Collection, SubCat);
                    var result = (from j in _DBContext.Categories
                                  where j.Id == Id
                                  select new Collection_VM
                                  {
                                      CollectionName = j.Type.ToLower() == GenderType.men.ToString() ? j.CategoryName + " " + GenderType.men.ToString().ToUpper() : j.CategoryName + " " + GenderType.women.ToString().ToUpper(),
                                      ImgName = j.ImagePath
                                  }).FirstOrDefault();
                    return View(result);
                }

                

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}