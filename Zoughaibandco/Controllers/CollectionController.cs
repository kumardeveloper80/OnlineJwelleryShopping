using System.Linq;
using System.Web.Mvc;
using Zoughaibandco.Models;
using Zoughaibandco.Repository;

namespace Zoughaibandco.Controllers
{
    public class CollectionController : Controller
    {
        Zoughaibandco_DBEntities _DBContext;
        CollectionRepository _CollectionRepository;
        public CollectionController()
        {
            _DBContext = new Zoughaibandco_DBEntities();
            _CollectionRepository = new CollectionRepository();
        }

        public ActionResult Index()
        {
            return View(_CollectionRepository.GetAllCollections());
        }

        public ActionResult Collection(int id)
        {
            _CollectionRepository = new CollectionRepository();
            var data = _CollectionRepository.GetCollectionsById(id);
            if (data != null)
            {
                return View(data);
            }
            return Index();
        }

        public ActionResult Arrow()
        {
            NextPrevious("Arrow");
            return View();
        }

        public ActionResult Classic()
        {
            NextPrevious("Classic");
            return View();
        }

        public ActionResult Crown()
        {
            NextPrevious("Crown");
            return View();
        }

        public ActionResult Emblem()
        {
            NextPrevious("Emblem");
            return View();
        }

        public ActionResult Tarsboush()
        {
            NextPrevious("Tarsboush");
            return View();
        }

        public ActionResult EmblemLove()
        {
            NextPrevious("EmblemLove");
            return View();
        }

        public ActionResult Leaves()
        {
            NextPrevious("Leaves");
            return View();
        }

        public ActionResult Lotus()
        {
            NextPrevious("Lotus");
            return View();
        }

        public ActionResult Bridal()
        {
            NextPrevious("Bridal");
            return View();
        }

        public ActionResult Fathers()
        {
            NextPrevious("Fathers");
            return View();
        }

        public ActionResult Damedice()
        {
            NextPrevious("Damedice");
            return View();
        }

        [NonAction]
        public void NextPrevious(string viewName)
        {
            var result = (from c in _DBContext.Collections
                          where c.RouteName == viewName
                          select c).FirstOrDefault();

            if (result != null)
            {
                ViewBag.Next = _DBContext.Collections.Where(x => x.Order > result.Order).OrderBy(x => x.Order).Select(x => x.RouteName).FirstOrDefault();
                ViewBag.Previous = _DBContext.Collections.Where(x => x.Order < result.Order).OrderByDescending(x => x.Order).Select(x => x.RouteName).FirstOrDefault();
                ViewBag.Id = result.Id;
            }
        }
    }
}