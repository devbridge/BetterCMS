using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Response for category delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteCategoryResponse : DeleteResponseBase
    {
    }
}