using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Category tree creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostCategoryTreeResponse : SaveResponseBase
    {
    }
}