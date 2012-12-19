using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.Mvc
{
    public static class HtmlHelpers
    {
        private const int MaxFoldersCount = 4;

        public static IHtmlString MediaPathRange(this HtmlHelper html, MediaPathViewModel model)
        {
            var cut = model.Folders.Count() > MaxFoldersCount;
            var range = cut
                            ? model.Folders.Skip(model.Folders.Count() - MaxFoldersCount - 1).Take(MaxFoldersCount + 1)
                            : model.Folders;

            StringBuilder sb = new StringBuilder();

            bool first = true;
            foreach (var folder in range)
            {
                sb.Append(string.Concat("<a class=\"bcms-breadcrumbs-root\" href=\"#nolink\" data-folder-id=\"", folder.Id, "\">\\", first ? "..." : html.Encode(folder.Name), "</a>"));
                
                if (first)
                {
                    first = false;
                }
            }                       

            return new MvcHtmlString(sb.ToString());
        }
    }
}