using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [DataContract]
    [System.Serializable]
    public class GetHtmlContentModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include child contents options.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include child contents options; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeChildContentsOptions { get; set; }
    }
}
