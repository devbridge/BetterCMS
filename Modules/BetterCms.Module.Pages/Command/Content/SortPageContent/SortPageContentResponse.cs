using System.Collections.Generic;
using BetterCms.Module.Pages.ViewModels.Content;

namespace BetterCms.Module.Pages.Command.Content.SortPageContent
{

    public class SortPageContentResponse
    {
        /// <summary>
        /// Gets or sets the list of updated page content.
        /// </summary>
        /// <value>
        /// The list of updated page content.
        /// </value>
        public IList<ContentViewModel> UpdatedPageContents { get; set; }

        public SortPageContentResponse()
        {
            UpdatedPageContents = new List<ContentViewModel>();
        }
    }
}