using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Pages.Command.Layout.DeleteTemplate
{
    public class DeleteTemplateCommandRequest
    {
           /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        /// <value>
        /// The widget id.
        /// </value>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}