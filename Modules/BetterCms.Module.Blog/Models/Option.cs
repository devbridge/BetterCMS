using System;
using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class Option : EquatableEntity<Option>
    {
        /// <summary>
        /// Gets or sets the default layout.
        /// </summary>
        /// <value>
        /// The default layout.
        /// </value>
        public virtual Layout DefaultLayout { get; set; }

        /// <summary>
        /// Gets or sets the default master page.
        /// </summary>
        /// <value>
        /// The default master page.
        /// </value>
        public virtual Page DefaultMasterPage { get; set; }
    }
}