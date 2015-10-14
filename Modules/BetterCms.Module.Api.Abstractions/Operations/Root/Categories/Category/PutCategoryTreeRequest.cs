using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request to save category.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PutCategoryTreeRequest : PutRequestBase<SaveCategoryTreeModel>
    {
    }
}