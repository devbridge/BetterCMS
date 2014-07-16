using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    [Serializable]
    public class ChildContentOptionValuesModel
    {
        /// <summary>
        /// Gets or sets the asssignment identifier.
        /// </summary>
        /// <value>
        /// The asssignment identifier.
        /// </value>
        [DataMember]
        public Guid AssignmentIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the list of page content options.
        /// </summary>
        /// <value>
        /// The list of page content options.
        /// </value>
        [DataMember]
        public IList<OptionValueModel> OptionValues { get; set; }
    }
}
