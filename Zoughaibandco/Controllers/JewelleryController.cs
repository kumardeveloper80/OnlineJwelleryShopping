using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Controllers
{
    public class JewelleryController : Controller
    {
        Zoughaibandco_DBEntities _DBContext;
        public JewelleryController()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }
        public ActionResult Index()
        {
            var result = _DBContext.Categories.OrderBy(x => x.CategoryName).ToList();
            var category = Mapper.Map<List<Jewellery_VM>>(result);
            return View(category);
        }
    }
}