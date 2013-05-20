using System;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Pages.Models.Api
{
    public class CreatePageDto
    {
        public string PageUrl { get; set; }
        
        public string Title { get; set; }
        
        public PageStatus Status { get; set; }

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

        public CreatePageDto()
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