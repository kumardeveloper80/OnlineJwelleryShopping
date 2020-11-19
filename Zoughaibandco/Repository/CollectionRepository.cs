using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.Repository
{
    public class CollectionRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public CollectionRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public List<Collection_VM> GetAllCollections()
        {
            var collections = (from c in _DBContext.Collections
                               join b in _DBContext.Banners on c.BannersId equals b.Id
                               where c.Active == true
                               orderby c.Order
                               select new Collection_VM
                               {
                                   CollectionId = c.Id,
                                   CollectionName = c.Name.ToUpper(),
                                   Active = (bool)c.Active,
                                   RouteName = c.RouteName,
                                   BannersId = b.Id,
                                   Order = c.Order,
                                   Side = c.Side.ToLower(),
                                   ImgName = b.ImgName,
                                   BackgroundImage = c.BackgroundImage,
                                   ForegroundImage = c.ForegroundImage,
                                   LogoImage = c.LogoImage,
                                   IsAdminAdded = c.IsAdminAdded,
                                   PageImage1 = c.PageImage1,
                                   PageImage2 = c.PageImage2,
                                   PageImage3 = c.PageImage3,
                                   PageImage4 = c.PageImage4,
                                   PageImage5 = c.PageImage5,
                                   PageImage6 = c.PageImage6,
                                   PageImage7 = c.PageImage7,
                                   PageImage8 = c.PageImage8,
                                   AboutText = c.AboutText,
                               }).OrderBy(c => c.Order).ToList();
            return collections;
        }

        public List<Collection_VM> GetAllCollections(string str)
        {
            var collections = (from c in _DBContext.Collections
                               join b in _DBContext.Banners on c.BannersId equals b.Id
                               where c.Active == true
                               orderby c.Order
                               select new Collection_VM
                               {
                                   CollectionId = c.Id,
                                   CollectionName = c.Name.ToUpper(),
                                   Active = (bool)c.Active,
                                   RouteName = c.RouteName,
                                   BannersId = b.Id,
                                   Order = c.Order,
                                   Side = c.Side.ToLower(),
                                   ImgName = b.ImgName,
                                   BackgroundImage = c.BackgroundImage,
                                   ForegroundImage = c.ForegroundImage,
                                   LogoImage = c.LogoImage,
                                   IsAdminAdded = c.IsAdminAdded,
                                   PageImage1 = c.PageImage1,
                                   PageImage2 = c.PageImage2,
                                   PageImage3 = c.PageImage3,
                                   PageImage4 = c.PageImage4,
                                   PageImage5 = c.PageImage5,
                                   PageImage6 = c.PageImage6,
                                   PageImage7 = c.PageImage7,
                                   PageImage8 = c.PageImage8,
                                   AboutText = c.AboutText,
                               }).ToList();
            return collections;
        }

        public Collection_VM GetCollectionsById(int Id)
        {
            var collections = (from c in _DBContext.Collections
                               join b in _DBContext.Banners on c.BannersId equals b.Id
                               where c.Active == true && c.Id == Id
                               orderby c.Order
                               select new Collection_VM
                               {
                                   CollectionId = c.Id,
                                   CollectionName = c.Name.ToUpper(),
                                   Active = (bool)c.Active,
                                   RouteName = c.RouteName,
                                   BannersId = b.Id,
                                   Order = c.Order,
                                   Side = c.Side.ToLower(),
                                   ImgName = b.ImgName,
                                   BackgroundImage = c.BackgroundImage,
                                   ForegroundImage = c.ForegroundImage,
                                   LogoImage = c.LogoImage,
                                   IsAdminAdded = c.IsAdminAdded,
                                   PageImage1=c.PageImage1,
                                   PageImage2=c.PageImage2,
                                   PageImage3=c.PageImage3,
                                   PageImage4=c.PageImage4,
                                   PageImage5=c.PageImage5,
                                   PageImage6=c.PageImage6,
                                   PageImage7=c.PageImage7,
                                   PageImage8=c.PageImage8,
                                   AboutText = c.AboutText,
                                   Breadcrumcolour = c.BreadCrumColor,
                                   LookBookBorderColor = c.LookBookBorderColor,
                                   LookBookTextcolour = c.LookBookColor,
                                   ShopTheCollectionTextColor = c.ShopTheCollectionTextColor

                               }).FirstOrDefault();

            if (collections != null)
            {
                collections.SideId = 1;
                if (collections.Side == "left")
                {
                    collections.SideId = 2;
                }
            }

           

            //if (collections.Side == "right")
            //{
            //    collections.SideId = 1;
            //}
            //else if (collections.Side == "left")
            //{
            //    collections.SideId = 2;
            //}
            //else
            //{
            //    collections.SideId = 1;
            //}
            return collections;
        }

        //Delete Collection by Admin
        public int DeleteCollection(int CollectionId)
        {
            var collections = _DBContext.Collections.Find(CollectionId);
            if (collections != null)
            {
                collections.Active = false;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        //Delete Collection Image by Admin
        public int DeleteCollectionImage(int CollectionId,string ColumnName)
        {
            var collections = _DBContext.Collections.Find(CollectionId);
            if (collections != null)
            {
                switch (ColumnName)
                {
                    case "BackgroundImage":
                        collections.BackgroundImage = null;
                        break;
                    case "ForegroundImage":
                        collections.ForegroundImage = null;
                        break;
                    case "LogoImage":
                        collections.LogoImage = null;
                        break;
                    case "PageImage1":
                        collections.PageImage1 = null;
                        break;
                    case "PageImage2":
                        collections.PageImage2 = null;
                        break;
                    case "PageImage3":
                        collections.PageImage3 = null;
                        break;
                    case "PageImage4":
                        collections.PageImage4 = null;
                        break;
                    case "PageImage5":
                        collections.PageImage5 = null;
                        break;
                    case "PageImage6":
                        collections.PageImage6 = null;
                        break;
                    case "PageImage7":
                        collections.PageImage7 = null;
                        break;
                    case "PageImage8":
                        collections.PageImage8 = null;
                        break;
                    default:
                        // code block
                        break;
                }
                collections.Active = false;
                return _DBContext.SaveChanges();
            }
            return 0;
        }

        //get Collection by filter
        public List<Collection_VM> GetAllCollectionByFilter(string search, string filter)
        {
            var collectionsByFilter = new List<Collection_VM>();
            if (search == null)
            {
                collectionsByFilter = (from c in _DBContext.Collections
                                       join b in _DBContext.Banners on c.BannersId equals b.Id
                                       where c.Active == true
                                       orderby c.Order
                                       select new Collection_VM
                                       {
                                           CollectionId = c.Id,
                                           CollectionName = c.Name.ToUpper(),
                                           Active = (bool)c.Active,
                                           RouteName = c.RouteName,
                                           BannersId = b.Id,
                                           Order = c.Order,
                                           Side = c.Side.ToLower(),
                                           ImgName = b.ImgName,
                                           BackgroundImage = c.BackgroundImage,
                                           ForegroundImage = c.ForegroundImage,
                                           LogoImage = c.LogoImage,
                                           IsAdminAdded = c.IsAdminAdded,
                                           PageImage1 = c.PageImage1,
                                           PageImage2 = c.PageImage2,
                                           PageImage3 = c.PageImage3,
                                           PageImage4 = c.PageImage4,
                                           PageImage5 = c.PageImage5,
                                           PageImage6 = c.PageImage6,
                                           PageImage7 = c.PageImage7,
                                           PageImage8 = c.PageImage8,
                                           AboutText = c.AboutText,
                                       }).ToList();               
            }
            else
            {
                collectionsByFilter = (from c in _DBContext.Collections
                                       join b in _DBContext.Banners on c.BannersId equals b.Id
                                       where c.Active == true && c.Name.Contains(search)
                                       orderby c.Order
                                       select new Collection_VM
                                       {
                                           CollectionId = c.Id,
                                           CollectionName = c.Name.ToUpper(),
                                           Active = (bool)c.Active,
                                           RouteName = c.RouteName,
                                           BannersId = b.Id,
                                           Order = c.Order,
                                           Side = c.Side.ToLower(),
                                           ImgName = b.ImgName,
                                           BackgroundImage = c.BackgroundImage,
                                           ForegroundImage = c.ForegroundImage,
                                           LogoImage = c.LogoImage,
                                           IsAdminAdded = c.IsAdminAdded,
                                           PageImage1 = c.PageImage1,
                                           PageImage2 = c.PageImage2,
                                           PageImage3 = c.PageImage3,
                                           PageImage4 = c.PageImage4,
                                           PageImage5 = c.PageImage5,
                                           PageImage6 = c.PageImage6,
                                           PageImage7 = c.PageImage7,
                                           PageImage8 = c.PageImage8,
                                           AboutText = c.AboutText,
                                       }).ToList();
            }
            return collectionsByFilter;
        }
    }
}