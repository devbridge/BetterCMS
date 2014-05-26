using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    [DataContract]
    [Serializable]
    public class GetPageContentModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include page content options.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include page content options; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeOptions { get; set; }
    }
}
