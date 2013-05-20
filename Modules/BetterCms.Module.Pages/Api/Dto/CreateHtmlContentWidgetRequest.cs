using System;

namespace BetterCms.Module.Pages.Api.Dto
{
    public class CreateHtmlContentWidgetRequest
    {
        public string Name { get; set; }

        public string Html { get; set; }

        public Guid? CategoryId { get; set; }

        public string Css { get; set; }

        public string JavaScript { get; set; }
    }
}