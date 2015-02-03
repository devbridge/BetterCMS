using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Request for category creation.
    /// </summary>
    [Route("/categories", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostCategoryRequest : RequestBase<SaveCategoryModel>, IReturn<PostCategoryResponse>
    {
    }
}