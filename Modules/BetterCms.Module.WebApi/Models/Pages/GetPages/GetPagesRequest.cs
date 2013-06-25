using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPages
{
    [DataContract]
    public class GetPagesRequest : ListRequestBase
    {
        [DataMember]
        public string SearchText { get; set; }
    }
}