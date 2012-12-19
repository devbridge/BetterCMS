using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Page;

namespace BetterCms.Module.Pages.Commands.GetTemplates
{
    public class GetTemplatesResponse
    {
        /// <summary>
        /// Gets or sets the list of templates view models.
        /// </summary>
        /// <value>
        /// The list of templates view models.
        /// </value>
        public IList<TemplateViewModel> Templates { get; set; }
    }
}