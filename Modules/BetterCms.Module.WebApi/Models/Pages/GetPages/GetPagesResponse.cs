using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BetterCms.Module.WebApi.Models.Pages.GetPages
{
    [DataContract]
    public class GetPagesResponse : ResponseBase<List<PageModel>>
    {
    }
}