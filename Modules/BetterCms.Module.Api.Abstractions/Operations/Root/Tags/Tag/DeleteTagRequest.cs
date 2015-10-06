using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Request for tag delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteTagRequest : DeleteRequestBase
    {
    }
}