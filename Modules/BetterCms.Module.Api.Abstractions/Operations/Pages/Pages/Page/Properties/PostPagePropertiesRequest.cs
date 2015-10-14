using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Request for page creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostPagePropertiesRequest : RequestBase<SavePagePropertiesModel>
    {
    }
}
