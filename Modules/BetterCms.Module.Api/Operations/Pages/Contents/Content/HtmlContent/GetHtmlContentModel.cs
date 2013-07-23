using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [DataContract]
    public class GetHtmlContentModel
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        [DataMember]
        public System.Guid ContentId { get; set; }
    }
}