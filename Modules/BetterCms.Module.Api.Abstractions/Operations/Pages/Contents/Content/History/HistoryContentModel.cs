using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [DataContract]
    [System.Serializable]
    public class HistoryContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [DataMember]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the original content id.
        /// </summary>
        /// <value>
        /// The original content id.
        /// </value>
        [DataMember]
        public System.Guid? OriginalContentId { get; set; }

        /// <summary>
        /// Gets or sets the date content published on.
        /// </summary>
        /// <value>
        /// The date content published on.
        /// </value>
        [DataMember]
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the content publisher user name.
        /// </summary>
        /// <value>
        /// The content publisher user name.
        /// </value>
        [DataMember]
        public string PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets the date content archived on.
        /// </summary>
        /// <value>
        /// The date content archived on.
        /// </value>
        [DataMember]
        public System.DateTime? ArchivedOn { get; set; }

        /// <summary>
        /// Gets or sets the user name who archived the content.
        /// </summary>
        /// <value>
        /// The user name who archived the content.
        /// </value>
        [DataMember]
        public string ArchivedByUser { get; set; }

        /// <summary>
        /// Gets or sets the content history item status.
        /// </summary>
        /// <value>
        /// The content history item status.
        /// </value>
        [DataMember]
        public ContentStatus Status { get; set; }
    }
}