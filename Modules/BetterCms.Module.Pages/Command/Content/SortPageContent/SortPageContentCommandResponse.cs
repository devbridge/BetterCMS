using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Content;

namespace BetterCms.Module.Pages.Command.Content.SortPageContent
{
    public class SortPageContentCommandResponse
    {
        /// <summary>
        /// Gets or sets the list of page content ids.
        /// </summary>
        /// <value>
        /// The list of page content ids and versions.
        /// </value>
        public IList<ContentSortViewModel> PageContents { get; set; }
    }
}