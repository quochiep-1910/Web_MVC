using System.Web.Optimization;

namespace Web_ASPMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //đây là jquery chính
            //bundles.Add(new ScriptBundle("~/bundles/jqueryCore").Include(
            //           "~/assets/Client/js/jquery-1.11.1.min.js",
            //            "~/assets/Client/js/jquery-ui.js",
            //            "~/assets/Client/js/move-top.js",
            //            "~/assets/Client/js/easing.js"
            //            ));

            //đây là js có thể sửa đổi
            //bundles.Add(new ScriptBundle("~/bundles/Controller").Include(
            //          "~/assets/Client/js/bootstrap.min.js",
            //          "~/assets/Client/js/minicart.js",
            //          "~/assets/Client/js/controller/SearchController.js"
            //             ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            //CSS cho trang user

            //bundles.Add(new StyleBundle("~/Content/UserCss")
            //    .Include("~/assets/Client/css/bootstrap.css", new CssRewriteUrlTransform())
            //  .Include("~/assets/Client/css/style.css", new CssRewriteUrlTransform())
            //  .Include("~/assets/Client/css/font-awesome.css", new CssRewriteUrlTransform()));

            //bundles.Add(new StyleBundle("~/Content/UserCss").Include(
            //    "~/assets/Client/css/*.css",
            //        new CssRewriteUrlTransform()));
            ////BundleTable.EnableOptimizations = true;
        }
    }
}