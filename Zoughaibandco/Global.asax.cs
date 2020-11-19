using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Zoughaibandco.AutoMapper;
using System.Web.Http;
using System;
using System.Web;

namespace Zoughaibandco
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfiguration.Configure();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Redirect("~/Home/Index");
        }
    }
}
