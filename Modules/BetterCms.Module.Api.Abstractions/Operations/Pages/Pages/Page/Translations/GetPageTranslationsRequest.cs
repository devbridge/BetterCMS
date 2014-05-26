using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations
{
    [Route("/pages/{PageId}/translations")]
    [Route("/pages/translations/by-url/{PageUrl}")]
    [DataContract]
    [Serializable]
    public class GetPageTranslationsRequest : RequestBase<DataOptions>, IReturn<GetPageTranslationsResponse>
    {
        [DataMember]
        public Guid? PageId { get; set; }
        
        [DataMember]
        public string PageUrl { get; set; }
    }
}