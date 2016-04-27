using System.Web;
using System.Web.Optimization;

namespace ChatLearning
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-2.2.2.min.js",
                "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/chatLearning").Include(
                "~/Scripts/chatLearning.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/jquery-ui.min.css",
                "~/Content/jquery-ui.structure.min.css",
                "~/Content/jquery-ui.theme.min.css",
                "~/Content/bootstrap.min.css",
                "~/Content/Loader.css",
                "~/Content/Chat.css",
                "~/Content/Site.css"));
        }
    }
}
