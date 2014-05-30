using System;

namespace BetterCms.Module.Pages.Helpers
{
    public class PageUrlGenerationRequest
    {
        public string Title { get; set; }

        public Guid ParentPageId { get; set; }

        public Guid LanguageId { get; set; }

        public Guid CategoryId { get; set; }
    }

    public static class UrlHelper
    {
        public static Func<PageUrlGenerationRequest, string> GeneratePageUrl { get; set; }
    }
}