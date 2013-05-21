using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Api.Dto
{
    public class CreateHtmlContentWidgetRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Content name is required")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessage = "Maximum length of content name cannot exceed {1} symbols.")]
        public string Name { get; set; }

        public string Html { get; set; }

        public Guid? CategoryId { get; set; }

        public string Css { get; set; }

        public string JavaScript { get; set; }
    }
}