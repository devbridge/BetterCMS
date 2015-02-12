using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Api.Operations.Root.Languages;
using BetterCms.Module.Api.Operations.Root.Languages.Language;
using BetterCms.Module.Api.Operations.Root.Layouts;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;
using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Version;

using ITagService = BetterCms.Module.Api.Operations.Root.Tags.Tag.ITagService;

namespace BetterCms.Module.Api.Operations.Root
{
    public class DefaultRootApiOperations : IRootApiOperations
    {
        public DefaultRootApiOperations(ITagsService tags, ITagService tag, IVersionService version, ILayoutsService layouts, ILayoutService layout,
            ICategoryTreesService categories, ICategoryTreeService category, ILanguagesService languages, ILanguageService language)
        {
            Tags = tags;
            Tag = tag;
            Categories = categories;
            Category = category;
            Version = version;
            Layouts = layouts;
            Layout = layout;
            Languages = languages;
            Language = language;
        }

        public ITagsService Tags
        {
            get; 
            private set;
        }

        public ITagService Tag
        {
            get;
            private set;
        }
        
        public ICategoryTreesService Categories
        {
            get; 
            private set;
        }

        public ICategoryTreeService Category
        {
            get;
            private set;
        }

        public ILanguagesService Languages
        {
            get; 
            private set;
        }

        public ILanguageService Language
        {
            get;
            private set;
        }

        public IVersionService Version
        {
            get;
            private set;
        }

        public ILayoutsService Layouts
        {
            get;
            private set;
        }

        public ILayoutService Layout
        {
            get;
            private set;
        }
    }
}