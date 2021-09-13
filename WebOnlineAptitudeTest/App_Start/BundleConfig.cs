using System.Web;
using System.Web.Optimization;

namespace WebOnlineAptitudeTest
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));



            bundles.Add(new ScriptBundle("~/bundles/backend/js").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Content/assets/backend/plugins/assets/libs/popper.js/dist/umd/popper.min.js",
                    "~/Content/assets/backend/plugins/assets/libs/bootstrap/dist/js/bootstrap.min.js",
                    "~/Content/assets/backend/plugins/dist/js/summernote-bs4.js",
                    "~/Content/assets/backend/plugins/dist/js/app-style-switcher.js",
                    "~/Content/assets/backend/plugins/dist/js/feather.min.js",
                    "~/Content/assets/backend/plugins/assets/libs/perfect-scrollbar/dist/perfect-scrollbar.jquery.min.js",
                    "~/Content/assets/backend/plugins/dist/js/sidebarmenu.js",
                    "~/Content/assets/backend/plugins/dist/js/custom.min.js",
                    "~/Content/assets/backend/plugins/assets/extra-libs/c3/d3.min.js",
                    "~/Content/assets/backend/plugins/assets/extra-libs/c3/c3.min.js",
                    "~/Content/assets/backend/plugins/assets/libs/chartist/dist/chartist.min.js",
                    "~/Content/assets/backend/plugins/assets/libs/chartist-plugin-tooltips/dist/chartist-plugin-tooltip.min.js",
                    "~/Content/assets/backend/plugins/assets/extra-libs/jvector/jquery-jvectormap-2.0.2.min.js",
                    "~/Content/assets/backend/plugins/assets/extra-libs/jvector/jquery-jvectormap-world-mill-en.js",
                    "~/Content/assets/backend/plugins/assets/libs/chart.js/dist/Chart.min.js",
                    "~/Scripts/toastr.min.js",
                    "~/Content/assets/backend/dist/js/config-toastrJs.js",
                    //"~/Content/assets/backend/plugins/dist/js/pages/dashboards/dashboard1.min.js",
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/plugins/changePassAdmin.js",
                    "~/Scripts/plugins/logoutAdmin.js")
                    );

            bundles.Add(new StyleBundle("~/bundles/backend/css").Include(
                      "~/Content/assets/backend/plugins/assets/extra-libs/c3/c3.min.css",
                      "~/Content/assets/backend/plugins/assets/libs/chartist/dist/chartist.min.css",
                      "~/Content/assets/backend/plugins/assets/extra-libs/jvector/jquery-jvectormap-2.0.2.css"
                      //"~/Content/assets/backend/plugins/assets/libs/summernote/summernote-bs4.css",
                      //"~/Content/assets/backend/plugins/dist/css/style.min.css"
                      //"~/Content/toastr.min.css"
                      //"~/Content/SiteBackend.css"
          ));
            bundles.Add(new ScriptBundle("~/bundles/frontend/js").Include(
                    "~/Content/assets/backend/dist/js/app.js"));

            bundles.Add(new StyleBundle("~/bundles/frontend/css").Include(
                     "~/Content/assets/backend/dist/css/app.css",
                     "~/Content/Site.css"
         ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
