using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Page category node response.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PutNodeResponse : SaveResponseBase
    {
    }
}