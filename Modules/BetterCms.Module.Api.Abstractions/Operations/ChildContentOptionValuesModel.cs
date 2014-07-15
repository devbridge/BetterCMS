using System;
using System.Collections.Generic;

namespace BetterCms.Module.Api.Operations
{
    public class ChildContentOptionValuesModel
    {
        /// <summary>
        /// Gets or sets the asssignment identifier.
        /// </summary>
        /// <value>
        /// The asssignment identifier.
        /// </value>
        public Guid AssignmentIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the list of page content options.
        /// </summary>
        /// <value>
        /// The list of page content options.
        /// </value>
        public IList<OptionValueModel> OptionValues { get; set; }
    }
}
