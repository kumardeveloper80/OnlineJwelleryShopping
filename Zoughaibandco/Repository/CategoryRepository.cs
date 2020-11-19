using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{

    public class CategoryRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public CategoryRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }


        public List<Category_VM> getAllCategory()
        {
            var category = (from c in _DBContext.Categories
                            select new Category_VM
                            {
                                Id = c.Id,
                                CategoryName = c.Type.ToUpper() == GenderType.men.ToString().ToUpper() ? c.CategoryName +" "+ GenderType.men.ToString().ToUpper() :
                                c.CategoryName + " " + GenderType.women.ToString().ToUpper()
                            }).OrderBy(x=>x.CategoryName).ToList();
            return category;
        }
    }
}