using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPages
{
    [DataContract]
    public class GetPagesRequest : DataOptions
    {
        [DataMember]
        public string SearchText { get; set; }
    }
}