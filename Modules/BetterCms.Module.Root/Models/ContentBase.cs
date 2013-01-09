using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentBase
    {
        public virtual string Name { get; set; }        

        public virtual IList<PageContent> PageContents { get; set; }

        public virtual IList<ContentOption> ContentOptions { get; set; }

        public virtual string PreviewUrl { get; set; }

    }
}