using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/categories/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutCategoryRequest : PutRequestBase<SaveCategoryModel>, IReturn<PutCategoryResponse>
    {
    }
}