using AutoMapper;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.AutoMapper
{
    public class AutomapperProfileModelToVM : Profile
    {
        public AutomapperProfileModelToVM()
        {
            CreateMap<Contact,Contact_VM>();
            CreateMap<Career, Career_VM>();
            CreateMap<Category, Jewellery_VM>();
            CreateMap<EGiftCard, EGiftCard_VM>();
        }
    }
}
