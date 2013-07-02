using System.Runtime.Serialization;

using ServiceStack.ServiceInterface.ServiceModel;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    public abstract class ResponseBase<TData> : IHasResponseStatus
    {
        [DataMember]
        public TData Data { get; set; }

        [DataMember]
        public ResponseStatus ResponseStatus
        {
            get; set;
        }
    }
}