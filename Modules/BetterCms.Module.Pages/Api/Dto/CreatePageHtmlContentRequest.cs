using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Module.Root.Api.Attributes;

namespace BetterCms.Module.Pages.Api.Dto
{
    public class CreatePageHtmlContentRequest : CreatePageContentRequestBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Content name is required.")]
        [StringLength(MaxLength.Name, ErrorMessage = "Maximum length of content name cannot exceed {1} symbols.")]
        public string Name { get; set; }

        public string Html { get; set; }

        [AllowableValuesValidation(new object[] { ContentStatus.Preview, ContentStatus .Archived}, false, ErrorMessage = "Cannot create content in Archived/Preview status.")]
        public ContentStatus ContentStatus { get; set; }

        public DateTime? ActivationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string CustomCss { get; set; }

        public string CustomJs { get; set; }
    }
}