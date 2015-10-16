using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Layout : EquatableEntity<Layout>, IOptionContainer<Layout>
    {
        public virtual string Name { get; set; }

        public virtual string LayoutPath { get; set; }

        public virtual string PreviewUrl { get; set; }

        public virtual Module Module { get; set; }

        public virtual IList<Page> Pages { get; set; }

        public virtual IList<LayoutRegion> LayoutRegions { get; set; }

        public virtual IList<LayoutOption> LayoutOptions { get; set; }

        IEnumerable<IDeletableOption<Layout>> IOptionContainer<Layout>.Options
        {
            get
            {
                return LayoutOptions;
            }
            set
            {
                LayoutOptions = value.Cast<LayoutOption>().ToList();
            }
        }
    }
}