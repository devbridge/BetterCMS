using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentHistory : EquatableEntity<ContentHistory>, IContent
    {
        public virtual string Name { get; set; }

        public virtual string PreviewUrl { get; set; }       

        public virtual IList<PageContentHistory> PageContentHistory { get; set; }

        public virtual IList<ContentOptionHistory> ContentOptionHistory { get; set; }        
    }
}