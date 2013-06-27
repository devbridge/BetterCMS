using System.Runtime.Serialization;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Api.Operations.Pages.GetContentHistoryById
{
    [DataContract]
    public class HistoryContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [DataMember(Order = 10, Name = "contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the original content id.
        /// </summary>
        /// <value>
        /// The original content id.
        /// </value>
        [DataMember(Order = 20, Name = "originalContentId")]
        public System.Guid OriginalContentId { get; set; }

        /// <summary>
        /// Gets or sets the date content published on.
        /// </summary>
        /// <value>
        /// The date content published on.
        /// </value>
        [DataMember(Order = 30, Name = "publishedOn")]
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the content publisher user name.
        /// </summary>
        /// <value>
        /// The content publisher user name.
        /// </value>
        [DataMember(Order = 40, Name = "publishedByUser")]
        public string PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets the date content archived on.
        /// </summary>
        /// <value>
        /// The date content archived on.
        /// </value>
        [DataMember(Order = 50, Name = "archivedOn")]
        public System.DateTime? ArchivedOn { get; set; }

        /// <summary>
        /// Gets or sets the user name who archived the content.
        /// </summary>
        /// <value>
        /// The user name who archived the content.
        /// </value>
        [DataMember(Order = 60, Name = "archivedByUser")]
        public string ArchivedByUser { get; set; }

        /// <summary>
        /// Gets or sets the time the content was displayed for.
        /// </summary>
        /// <value>
        /// The time the content was displayed for.
        /// </value>
        [DataMember(Order = 70, Name = "displayedFor")]
        public System.TimeSpan? DisplayedFor { get; set; }

        /// <summary>
        /// Gets or sets the content history item status.
        /// </summary>
        /// <value>
        /// The content history item status.
        /// </value>
        [DataMember(Order = 80, Name = "status")]
        public ContentStatus Status { get; set; }
    }
}