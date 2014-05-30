using System;

namespace BetterCms.Module.Pages.Command.Page.GetUrlSuggestion
{
    public class GetUrlSuggestionCommandRequest
    {
        public string Title { get; set; }

        public bool TitleChanged { get; set; }

        public string ParentPageUrl { get; set; }

        public Guid ParentPageId { get; set; }

        public Guid LanguageId { get; set; }

        public Guid CategoryId { get; set; }
    }
}