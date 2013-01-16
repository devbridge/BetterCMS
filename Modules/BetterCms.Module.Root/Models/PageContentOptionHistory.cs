using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageContentOptionHistory : EquatableEntity<PageContentOptionHistory>, IPageContentOption
    {
        public virtual ContentOptionHistory ContentOptionHistory { get; set; }

        public virtual PageContentHistory PageContentHistory { get; set; }

        public virtual string Value { get; set; }

        IPageContent IPageContentOption.PageContent
        {
            get
            {
                return PageContentHistory;
            }
        }

        IContentOption IPageContentOption.ContentOption
        {
            get
            {
                return ContentOptionHistory;
            }
        }
    }
}