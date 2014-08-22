using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Response for tag delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteTagResponse : DeleteResponseBase
    {
    }
}