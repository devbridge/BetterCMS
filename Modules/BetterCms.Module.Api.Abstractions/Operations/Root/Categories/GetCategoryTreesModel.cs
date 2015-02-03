using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Data model for getting category trees list.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetCategoryTreesModel : DataOptions
    {
    }
}