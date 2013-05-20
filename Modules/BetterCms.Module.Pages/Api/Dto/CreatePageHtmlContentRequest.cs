using System;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Pages.Api.Dto
{
    public class CreatePageHtmlContentRequest : CreatePageContentRequestBase
    {
        public string Name { get; set; }

        public string Html { get; set; }

        public ContentStatus ContentStatus { get; set; }

        public DateTime? ActivationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string CustomCss { get; set; }

        public string CustomJs { get; set; }
    }
}