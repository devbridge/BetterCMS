using System;
using System.Runtime.Serialization;


namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    [DataContract]
    [Serializable]
    public class PageExistsRequest
    {
        [DataMember]
        public string PageUrl { get; set; }
    }
}