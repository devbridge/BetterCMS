using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Search
{
    [Route("/pages/search/{SearchString*}")]
    [DataContract]
    public class SearchPagesRequest : IReturn<SearchPagesResponse>
    {
        [DataMember]
        public string SearchString { get; set; }
    }
}