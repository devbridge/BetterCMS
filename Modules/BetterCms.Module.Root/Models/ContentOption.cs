using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentOption : EquatableEntity<ContentOption>, IContentOption
    {
        public virtual Content Content { get; set; }

        public virtual string Key { get; set; }

        public virtual ContentOptionType Type { get; set; }

        public virtual string DefaultValue { get; set; }

        public virtual IList<PageContentOption> PageContentOptions { get; set; }

        IContent IContentOption.Content
        {
            get
            {
                return Content;
            }
        }
    }
}