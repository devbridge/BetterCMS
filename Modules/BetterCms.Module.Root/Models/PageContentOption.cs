using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageContentOption : EquatableEntity<PageContentOption>, IPageContentOption
    {
        public virtual ContentOption ContentOption { get; set; }

        public virtual PageContent PageContent { get; set; }

        public virtual string Value { get; set; }

        IPageContent IPageContentOption.PageContent
        {
            get
            {
                return PageContent;
            }
        }

        IContentOption IPageContentOption.ContentOption
        {
            get
            {
                return ContentOption;
            }
        }
    }
}