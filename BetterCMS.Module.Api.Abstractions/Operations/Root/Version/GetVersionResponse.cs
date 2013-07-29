using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Version
{
    [DataContract]
    public class GetVersionResponse : ResponseBase<VersionModel>
    {
    }
}