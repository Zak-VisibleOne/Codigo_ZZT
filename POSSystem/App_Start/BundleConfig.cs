using System.Web;
using System.Web.Optimization;

namespace POSSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/bootstrap.bundle.min.js",
                        "~/Scripts/metisMenu.min.js",
                        "~/Scripts/jquery.slimscroll.js",
                        "~/Scripts/waves.min.js",
                        "~/plugins/jquery-sparkline/jquery.sparkline.min.js"
                        /*"~/Scripts/datatables.min.js"*/));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            ////Added for toaster
            //bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
            //           "~/Scripts/toastr.js*",
            //           "~/Scripts/toastr.min.js"));

            ////Modify for toastr
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/toastr.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/metismenu.min.css",
                      "~/Content/icons.css",
                      "~/Content/style.css"));
        }
    }
}
