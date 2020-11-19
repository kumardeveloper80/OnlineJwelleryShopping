using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class BannersRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public BannersRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public List<Banners_VM> GetBanners()
        {
            var banners = (from c in _DBContext.Banners
                               orderby c.Id
                               select new Banners_VM
                               {
                                   Id = c.Id,
                                   BannerKeyName = c.KeyName.ToUpper(),
                                   BannerImgName = c.ImgName
                               }).ToList();
            return banners;
        }

        public Banners_VM GetBannersById(int Id)
        {
            var collections = (from c in _DBContext.Banners
                               where c.Id == Id
                               orderby c.Id
                               select new Banners_VM
                               {
                                   Id = c.Id,
                                   BannerKeyName = c.KeyName.ToUpper(),
                                   BannerImgName = c.ImgName
                               }).FirstOrDefault();
            return collections;
        }

        public Banners_VM GetBannersByImgName(string fname)
        {
            var collections = (from c in _DBContext.Banners
                               where c.ImgName == fname
                               orderby c.Id
                               select new Banners_VM
                               {
                                   Id = c.Id,
                                   BannerKeyName = c.KeyName.ToUpper(),
                                   BannerImgName = c.ImgName
                               }).FirstOrDefault();
            return collections;
        }

        public Banners_VM GetBannersByKeyNameName(string fname)
        {
            var collections = (from c in _DBContext.Banners
                               where c.KeyName.ToLower() == fname.ToLower()
                               orderby c.Id
                               select new Banners_VM
                               {
                                   Id = c.Id,
                                   BannerKeyName = c.KeyName.ToUpper(),
                                   BannerImgName = c.ImgName
                               }).FirstOrDefault();
            return collections;
        }
    }
}