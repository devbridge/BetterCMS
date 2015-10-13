using System.Collections.Generic;

using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Blog.ViewModels.Setting;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Blog.Commands.GetBlogSettings
{
    public class GetBlogSettingsCommandResponse : SearchableGridViewModel<SettingItemViewModel>
    {
        public IList<BlogTemplateViewModel> Templates { get; set; }
    }
}