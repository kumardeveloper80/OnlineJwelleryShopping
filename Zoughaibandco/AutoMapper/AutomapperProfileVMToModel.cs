using AutoMapper;
using Zoughaibandco.Models;
using Zoughaibandco.ViewModel;

namespace Zoughaibandco.AutoMapper
{
    public class AutomapperProfileVMToModel : Profile
    {
        public AutomapperProfileVMToModel()
        {
            CreateMap<Contact_VM, Contact>();
            CreateMap<Career_VM, Career>();
            CreateMap<Signup_VM, Client>();
            CreateMap<Jewellery_VM,Category>();
            CreateMap<EGiftCard_VM, EGiftCard>();
        }
    }
}
