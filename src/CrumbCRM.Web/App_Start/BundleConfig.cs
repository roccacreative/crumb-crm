using System.Web;
using System.Web.Optimization;

namespace CrumbCRM.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725 
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/styles")
                .Include("~/content/css/reset.css",
                        "~/content/css/jquery-ui-1.9.2.custom.css",
                        "~/content/css/jquery.qtip.css",
                        "~/content/css/icons.css",
                        "~/content/css/jquery.morris.css",
                        "~/content/css/master.css",
                        "~/content/css/mobile.css",
                        "~/Scripts/plugins/fullcalendar/fullcalendar.css",
                        "~/Content/css/token-input.css"));
            /*
            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include("~/Scripts/modernizr.js",
                        "~/Scripts/jquery.qtip.min.js",
                        "~/Scripts/raphael.min.js",
                        "~/Scripts/morris.min.js",
                        "~/Scripts/jquery.customselect.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.highlight.js",
                        "~/Scripts/jquery.blockui.js",
                        "~/Scripts/jquery.triggeronview.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.tokeninput.js"));
            */
        }
    }
}