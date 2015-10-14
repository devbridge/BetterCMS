using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options
{
    [DataContract]
    [System.Serializable]
    public class GetPageContentOptionsRequest : RequestBase<DataOptions>
    {
        [DataMember]
        public System.Guid PageContentId { get; set; }
    }
}
