using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Region : EquatableEntity<Region>, IRegion
    {
        public virtual string RegionIdentifier { get; set; }

        public virtual IList<LayoutRegion> LayoutRegion { get; set; }
        
        public virtual IList<PageContent> PageContents { get; set; }
    }
}