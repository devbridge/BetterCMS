using System;

using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Root.Models;
using BetterModules.Core.Models;

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

        /// <summary>
        /// Gets or sets the default content text mode.
        /// </summary>
        /// <value>
        /// The default content text mode.
        /// </value>
        public virtual ContentTextMode DefaultContentTextMode { get; set; }
    }
}