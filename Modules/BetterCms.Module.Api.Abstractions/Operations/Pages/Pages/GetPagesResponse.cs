using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [DataContract]
    [Serializable]
    public class GetPagesResponse : ListResponseBase<PageModel>
    {
    }
}