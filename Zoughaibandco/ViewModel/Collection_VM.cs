using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zoughaibandco.Models;

namespace Zoughaibandco.ViewModel
{
    public class Collection_VM
    {
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public string RouteName { get; set; }
        public string ImgName { get; set; }
        public string Side { get; set; }
        public Nullable<int> SideId { get; set; }
        public Side Sidedata { get; set; }
        public Banner Banner { get; set; }
        public bool Active { get; set; }
        public Nullable<int> BannersId { get; set; }
        public Nullable<int> Order { get; set; }
        public string BackgroundImage { get; set; }
        public string ForegroundImage { get; set; }
        public string LogoImage { get; set; }
        public string PageImage1 { get; set; }
        public string PageImage2 { get; set; }
        public string PageImage3 { get; set; }
        public string PageImage4 { get; set; }
        public string PageImage5 { get; set; }
        public string PageImage6 { get; set; }
        public string PageImage7 { get; set; }
        public string PageImage8 { get; set; }
        public string AboutText { get; set; }
        public bool IsAdminAdded { get; set; }

        public string Breadcrumcolour { get; set; }
        public string LookBookTextcolour { get; set; }
        public string LookBookBorderColor { get; set; }
        public string ShopTheCollectionTextColor { get; set; }
    }

    public class Side
    {
        public int sId { get; set; }
        public string sName { get; set; }
    }
}