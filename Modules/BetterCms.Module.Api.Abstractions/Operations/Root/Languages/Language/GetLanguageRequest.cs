using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    [Route("/languages/{LanguageId}", Verbs = "GET")]
    [Route("/languages/by-code/{LanguageCode}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetLanguageRequest : IReturn<GetLanguageResponse>
    {
        [DataMember]
        public Guid? LanguageId { get; set; }

        [DataMember]
        public string LanguageCode { get; set; }
    }
}