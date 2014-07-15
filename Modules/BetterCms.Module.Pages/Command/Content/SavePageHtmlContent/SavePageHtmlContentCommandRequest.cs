using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Command.Content.SavePageHtmlContent
{
    public class SavePageHtmlContentCommandRequest
    {
        public PageContentViewModel Content { get; set; }

        public IList<ContentOptionValuesViewModel> ChildContentOptionValues { get; set; }
    }
}