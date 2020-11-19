using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class CareerRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public CareerRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public int Add(Career_VM career_VM)
        {
            try
            {
                var careerObj = Mapper.Map<Career>(career_VM);
                careerObj.CreatedDate = DateTime.Now;
                _DBContext.Careers.Add(careerObj);
                return _DBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool FileToSave()
        {
            return false;
        }
    }
}