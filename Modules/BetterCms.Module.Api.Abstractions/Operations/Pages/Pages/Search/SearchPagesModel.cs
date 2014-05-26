using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Search
{
    [DataContract]
    [Serializable]
    public class SearchPagesModel
    {
        /// <summary>
        /// Gets or sets the starting item number.
        /// </summary>
        /// <value>
        /// The starting item number.
        /// </value>
        [DataMember]
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of returning items.
        /// </summary>
        /// <value>
        /// The maximum count of returning items.
        /// </value>
        [DataMember]
        public int? Take { get; set; }
    }
}
