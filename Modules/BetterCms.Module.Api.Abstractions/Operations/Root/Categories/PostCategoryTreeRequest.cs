using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Request for category tree creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostCategoryTreeRequest : RequestBase<SaveCategoryTreeModel>
    {
    }
}