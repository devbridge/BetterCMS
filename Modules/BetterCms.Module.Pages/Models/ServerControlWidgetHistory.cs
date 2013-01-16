using System;
using System.Linq;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class ServerControlWidgetHistory : WidgetHistory, IServerControlWidget
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public virtual string Url { get; set; }
    }
}