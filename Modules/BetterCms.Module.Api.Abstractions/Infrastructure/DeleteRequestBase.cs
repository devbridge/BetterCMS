using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
    public abstract class DeleteRequestBase : RequestBase<RequestDeleteModel>
    {
    }
}