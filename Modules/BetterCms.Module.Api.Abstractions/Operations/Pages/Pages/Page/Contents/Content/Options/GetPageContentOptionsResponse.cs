using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options
{
    [DataContract]
    [Serializable]
    public class GetPageContentOptionsResponse : ListResponseBase<OptionValueModel>
    {
    }
}
