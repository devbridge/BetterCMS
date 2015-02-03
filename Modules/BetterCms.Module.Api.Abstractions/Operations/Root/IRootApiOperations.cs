using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Api.Operations.Root.Languages;
using BetterCms.Module.Api.Operations.Root.Languages.Language;
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
        
        ICategoryTreesService Categories { get; }

        ICategoryTreeService Category { get; }
        
        ILanguagesService Languages { get; }

        ILanguageService Language { get; }

        IVersionService Version { get; }
    }
}
