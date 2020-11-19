using System.Web;
using System.Web.Optimization;

namespace Zoughaibandco
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery10.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/style.css",
                      "~/Content/sweetalert.css",
                      "~/Content/jquery-ui-timepicker-addon.css",
                      "~/Content/jquery-ui.min.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Custom").Include(
                "~/Scripts/homepage.js",
                "~/Scripts/sweetalert-dev.js",
                "~/Scripts/typed.js",
                "~/Scripts/faq.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/migrate.js",
                "~/Scripts/jquery.easing.js",
                "~/Scripts/modernizr.custom.57033.js",
                "~/Scripts/setCookie.js",
                "~/Scripts/jquery-ui-timepicker-addon.js",
                "~/Scripts/store_locator.js",
                 "~/Scripts/header.js"
                ));

            bundles.Add(new ScriptBundle("~/NeedHelp").Include(
                "~/Scripts/NeedHelp.js"));

            //bundles.Add(new ScriptBundle("~/Scripts/Header").Include(
            //   "~/Scripts/header.js"));

            bundles.Add(new ScriptBundle("~/Careers").Include(
               "~/Scripts/Careers.js"));

            bundles.Add(new ScriptBundle("~/ECard").Include(
               "~/Scripts/ECard.js"));
        }
    }
}
