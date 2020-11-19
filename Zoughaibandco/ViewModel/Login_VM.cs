using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zoughaibandco.ViewModel
{
    public class Login_VM
    {
        public string LoginUsername { get; set; }
        public  string LoginPassword { get; set; }
    }

    public class ForgotPassword_VM
    {
        public string LoginUsername { get; set; }
    }
}