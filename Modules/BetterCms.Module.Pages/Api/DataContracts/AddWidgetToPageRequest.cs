using System;

using BetterCms.Module.Root.Api.Attributes;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class AddWidgetToPageRequest : CreatePageContentRequestBase
    {
        [EmptyGuidValidation(ErrorMessage = "Content Id must be set.")]
        public Guid ContentId { get; set; }
    }
}