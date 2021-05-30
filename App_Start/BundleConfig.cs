using System.Web.Optimization;

namespace DecorDutys
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
           // bundles.Add(new ScriptBundle("~/bundles/BundleJquery").Include(
           //         "~/Content/js/jquery-3.3.1.min.js",
           //        "~/Content/js/jquery.validate.min.js",
           //        "~/Content/js/jquery.validate.unobtrusive.js"
           // ));

           // bundles.Add(new ScriptBundle("~/bundles/BundleJqueryValidation").Include(
           //        "~/Content/js/jquery.validate.mvcfoolproof.min.js",
           //        "~/Content/js/jquery.validate.mvcfoolproof.unobtrusive.min.js",
           //         "~/Content/js/CustomValidators.js"
           //));

           // bundles.Add(new ScriptBundle("~/bundles/BundleCustomJavascript").Include(
           //         "~/Content/js/owl.carousel.min.js",
           //         "~/Content/js/custom.js"
           // ));
                
           // bundles.Add(new StyleBundle("~/bundles/BundleCSS").Include(
           //        "~/Content/css/owl.carousel.min.css",
           //         "~/Content/css/style.css",
           //        "~/Content/css/model.css",
           //        "~/Content/css/responsive.css"
           // ));

           // bundles.Add(new StyleBundle("~/bundles/BundleStaticCSS").Include(
           //        "~/Content/css/style.css"
           //));

            BundleTable.EnableOptimizations = true;
        }
    }
}