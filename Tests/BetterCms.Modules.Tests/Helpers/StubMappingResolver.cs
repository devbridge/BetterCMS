using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Models.Maps;
using BetterCms.Module.MediaManager.Models.Maps;
using BetterCms.Module.Newsletter.Models.Maps;
using BetterCms.Module.Pages.Models.Maps;
using BetterCms.Module.Root.Models.Maps;
using BetterCms.Module.Users.Models.Maps;

using FluentNHibernate.Cfg;

namespace BetterCms.Test.Module.Helpers
{
    public class StubMappingResolver : IMappingResolver
    {
        public void AddAvailableMappings(FluentConfiguration fluentConfiguration)
        {
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<PagePropertiesMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<PageMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<MediaMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<BlogPostMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<RoleMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<SubscriberMap>());
        }
    }
}
