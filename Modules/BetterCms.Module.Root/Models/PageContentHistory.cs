using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageContentHistory : EquatableEntity<PageContentHistory>, IPageContent
    {
        public virtual int Order { get; set; }

        public virtual ContentStatus Status { get; set; }

        public virtual PageContent PageContent { get; set; }

        public virtual ContentHistory ContentHistory { get; set; }

        public virtual Page Page { get; set; }

        public virtual Region Region { get; set; }

        public virtual IList<PageContentOptionHistory> Options { get; set; }

        IPage IPageContent.Page
        {
            get
            {
                return Page;
            }
        }

        IContent IPageContent.Content
        {
            get
            {
                return ContentHistory;
            }
        }

        IRegion IPageContent.Region
        {
            get
            {
                return Region;
            }
        }
    }
}