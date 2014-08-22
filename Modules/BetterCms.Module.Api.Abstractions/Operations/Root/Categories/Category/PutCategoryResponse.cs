using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Response after category saving.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutCategoryResponse : SaveResponseBase
    {
    }
}