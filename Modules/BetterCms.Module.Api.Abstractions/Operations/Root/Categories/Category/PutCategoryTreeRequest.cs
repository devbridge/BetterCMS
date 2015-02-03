using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request to save category.
    /// </summary>
    [Route("/categorytrees/{Id}", Verbs = "PUT")]
    [Serializable]
    [DataContract]
    public class PutCategoryTreeRequest : PutRequestBase<SaveCategoryTreeModel>, IReturn<PutCategoryTreeResponse>
    {
    }
}