using System.Collections.Generic;

using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.ViewModels
{
    public class ProjectionsViewModel
    {     
        public IEnumerable<IActionProjection> Projections { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // TODO: cannot add any key from this object:
            return string.Empty;
        }
    }
}