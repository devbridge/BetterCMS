using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageContent : EquatableEntity<PageContent>, IPageContent
    {
        public virtual int Order { get; set; }

        public virtual ContentStatus Status { get; set; }

        public virtual Content Content { get; set; }

        public virtual Page Page { get; set; }

        public virtual Region Region { get; set; }

        public virtual IList<PageContentOption> Options { get; set; }

        public virtual IList<PageContentHistory> History { get; set; }
        
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
                return Content;
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