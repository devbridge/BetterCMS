using System.Runtime.Serialization;

using ServiceStack.ServiceInterface.ServiceModel;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    public abstract class ResponseBase<TData> : IHasResponseStatus
    {
        [DataMember(Order = 1, Name = "data")]
        public TData Data { get; set; }

        [DataMember(Order = 2, Name = "responseStatus")]
        public ResponseStatus ResponseStatus
        {
            get; set;
        }
    }
}