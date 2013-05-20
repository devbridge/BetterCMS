using System;

namespace BetterCms.Module.Pages.Api.Dto
{
    public class AddWidgetToPageRequest : CreatePageContentRequestBase
    {
        public Guid ContentId { get; set; }
    }
}