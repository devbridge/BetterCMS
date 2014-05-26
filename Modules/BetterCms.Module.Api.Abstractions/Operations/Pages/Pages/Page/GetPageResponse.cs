using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    [DataContract]
    [Serializable]
    public class GetPageResponse : ResponseBase<PageModel>
    {
    }
}