using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Navigation.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Navigation.Command.Sitemap.GetPageLinks
{
    /// <summary>
    /// Command to get page links data.
    /// </summary>
    public class GetPageLinksCommand : CommandBase, ICommand<string, IList<PageLinkViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Sitemap root nodes.</returns>
        public IList<PageLinkViewModel> Execute(string request)
        {
            // TODO: implement.
            return new List<PageLinkViewModel> { new PageLinkViewModel { Title = "Test1", Url = "/test" } };
        }
    }
}