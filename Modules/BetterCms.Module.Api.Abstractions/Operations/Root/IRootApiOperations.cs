using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Api.Operations.Root.Layouts;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;
using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;
using BetterCms.Module.Api.Operations.Root.Version;

namespace BetterCms.Module.Api.Operations.Root
{
    public interface IRootApiOperations
    {
        ILayoutsService Layouts { get; }

        ILayoutService Layout { get; }

        ITagsService Tags { get; }

        ITagService Tag { get; }
        
        ICategoriesService Categories { get; }

        ICategoryService Category { get; }

        IVersionService Version { get; }
    }
}
