using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.WebHost;
using Zoughaibandco.Utility;

namespace Zoughaibandco
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance",
       System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
