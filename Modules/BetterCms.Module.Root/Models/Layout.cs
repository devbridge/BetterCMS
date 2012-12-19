using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Layout : EquatableEntity<Layout>
    {
        public virtual string Name { get; set; }

        public virtual string LayoutPath { get; set; }

        public virtual Module Module { get; set; }

        public virtual IList<Page> Pages { get; set; }

        public virtual IList<LayoutRegion> LayoutRegions { get; set; }
    }
}