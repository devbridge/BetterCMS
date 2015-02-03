using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Category node creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostCategoryNodeResponse : SaveResponseBase
    {
    }
}
