using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Category creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostCategoryResponse : ResponseBase<Guid?>
    {
    }
}