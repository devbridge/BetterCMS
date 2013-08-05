using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PageContent : EquatableEntity<PageContent>, IPageContent, IOptions, IOptionValues
    {
        public virtual int Order { get; set; }

        public virtual Content Content { get; set; }

        public virtual Page Page { get; set; }

        public virtual Region Region { get; set; }

        public virtual IList<PageContentOption> Options { get; set; }       

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

        IEnumerable<IOption> IOptions.Options
        {
            get
            {
                return Content.ContentOptions;
            }
        }

        IEnumerable<IOption> IOptionValues.OptionValues
        {
            get
            {
                return Options;
            }
        }
    }
}