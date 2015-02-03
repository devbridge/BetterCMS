using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Page save response.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PutCategoryTreeResponse : SaveResponseBase
    {
    }
}