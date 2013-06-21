using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models
{
    [DataContract]
    public abstract class ResponseBase<TData>
    {
        [DataMember(Order = 0, Name = "status")]
        public string Status { get; set; }

        [DataMember(Order = 1, Name = "data")]
        public TData Data { get; set; }
    }
}