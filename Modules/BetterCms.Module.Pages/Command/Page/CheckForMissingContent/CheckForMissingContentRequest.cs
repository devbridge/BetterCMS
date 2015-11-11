using System;

namespace BetterCms.Module.Pages.Command.Page.CheckForMissingContent
{
    public class CheckForMissingContentRequest
    {
        public Guid PageId { get; set; }
        public Guid? TemplateId { get; set; }
        public Guid? MasterPageId { get; set; }
    }
}