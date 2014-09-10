using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    [DataContract]
    [Serializable]
    public class PageContentModel : ModelBase
    {
        [DataMember]
        public Guid PageId { get; set; }

        [DataMember]
        public Guid RegionId { get; set; }

        [DataMember]
        public Guid ContentId { get; set; }

        [DataMember]
        public Guid? ParentPageContentId { get; set; }

        [DataMember]
        public int Order { get; set; }
    }
}