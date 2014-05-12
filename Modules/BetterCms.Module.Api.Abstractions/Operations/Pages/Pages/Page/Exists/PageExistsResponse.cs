using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    [DataContract]
    [Serializable]
    public class PageExistsResponse : ResponseBase<PageModel>
    {
    }
}