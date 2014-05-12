using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Version
{
    [DataContract]
    [Serializable]
    public class GetVersionResponse : ResponseBase<VersionModel>
    {
    }
}