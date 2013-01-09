using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageContentOptionHistory : EquatableEntity<PageContentOptionHistory>, IPageContentOption
    {
        public virtual ContentOptionHistory ContentOption { get; set; }

        public virtual PageContentHistory PageContent { get; set; }

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