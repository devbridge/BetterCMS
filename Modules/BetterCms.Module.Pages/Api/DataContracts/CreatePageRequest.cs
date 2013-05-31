using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Mvc.Attributes;
using BetterCms.Module.Root.Api.Attributes;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class CreatePageRequest
    {
        [CustomPageUrlValidation(ErrorMessage = "Ivalid Page Url")]
        [StringLength(MaxLength.Url, MinimumLength = 1, ErrorMessage = "Maximum length of page title cannot exceed {1} symbols.")]
        public string PageUrl { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Page title is required")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessage = "Maximum length of page title cannot exceed {1} symbols.")]
        public string Title { get; set; }

        [AllowableValuesValidation(new object[] {PageStatus.Preview, PageStatus.Draft}, false, ErrorMessage = "Cannot create page in Preview/Draft Status.")]
        public PageStatus Status { get; set; }

        [EmptyGuidValidation(ErrorMessage = "Layout Id must be set.")]
        public Guid LayoutId { get; set; }

        public Guid? ImageId { get; set; }
        
        public Guid? CategoryId { get; set; }

        public string Description { get; set; }
        
        public string CustomCss { get; set; }
        
        public string CustomJS { get; set; }

        public string MetaTitle { get; set; }
        
        public string MetaDescription { get; set; }
        
        public string MetaKeywords { get; set; }
        
        public bool IsPublic { get; set; }
        
        public bool UseCanonicalUrl { get; set; }
        
        public bool UseNoFollow { get; set; }
        
        public bool UseNoIndex { get; set; }

        public CreatePageRequest()
        {
            Status = PageStatus.Unpublished;
            IsPublic = true;
        }

        public virtual PageProperties ToPageProperties()
        {
            return new PageProperties
                       {
                           PageUrl = PageUrl,
                           Title = Title,
                           Status = Status,
                           Description = Description,
                           CustomCss = CustomCss,
                           CustomJS = CustomJS,
                           MetaTitle = MetaTitle,
                           MetaDescription = MetaDescription,
                           MetaKeywords = MetaKeywords,
                           IsPublic = IsPublic,
                           UseCanonicalUrl = UseCanonicalUrl,
                           UseNoFollow = UseNoFollow,
                           UseNoIndex = UseNoIndex,
                       };
        }
    }
}