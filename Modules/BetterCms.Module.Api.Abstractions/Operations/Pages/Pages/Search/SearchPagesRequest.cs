using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Search
{
    [Route("/pages/search/{SearchString*}")]
    [DataContract]
    [Serializable]
    public class SearchPagesRequest : RequestBase<SearchPagesModel>, IReturn<SearchPagesResponse>
    {
        [DataMember]
        public string SearchString { get; set; }
    }
}