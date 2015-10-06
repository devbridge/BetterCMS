using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page save request for REST.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutPagePropertiesRequest : PutRequestBase<SavePagePropertiesModel>
    {
    }
}