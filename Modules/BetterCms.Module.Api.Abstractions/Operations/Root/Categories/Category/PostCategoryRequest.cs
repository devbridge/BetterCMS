using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request for category update or creation.
    /// </summary>
    [Route("/categories", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostCategoryRequest : RequestBase<SaveCategoryModel>, IReturn<PostCategoryResponse>
    {
    }
}