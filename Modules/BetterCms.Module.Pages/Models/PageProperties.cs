using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class PageProperties : Page
    {
        public virtual string Description { get; set; }
        public virtual string CustomCss { get; set; }
        public virtual string CustomJS { get; set; }

        public virtual bool UseCanonicalUrl { get; set; }
        public virtual bool UseNoFollow { get; set; }
        public virtual bool UseNoIndex { get; set; }

        public virtual bool IsInSitemap { get; set; }

        public override bool HasSEO
        {
            get
            {
                return base.HasSEO && IsInSitemap;
            }
        }

        public virtual IList<PageTag> PageTags { get; set; }
        
        public virtual Category Category { get; set; }
        public virtual MediaImage Image { get; set; }
        public virtual MediaImage SecondaryImage { get; set; }
        public virtual MediaImage FeaturedImage { get; set; }
        public virtual bool IsArchived { get; set; }

        public PageProperties()
        {
            UseCanonicalUrl = true;
        }

        public virtual PageProperties Duplicate()
        {
            return CopyDataToDuplicate(new PageProperties());
        }

        protected virtual PageProperties CopyDataToDuplicate(PageProperties duplicate)
        {
            duplicate.Language = Language;
            duplicate.LanguageGroupIdentifier = null;
            duplicate.MetaTitle = MetaTitle;
            duplicate.MetaKeywords = MetaKeywords;
            duplicate.MetaDescription = MetaDescription;
            duplicate.UseCanonicalUrl = UseCanonicalUrl;
            duplicate.CustomCss = CustomCss;
            duplicate.CustomJS = CustomJS;
            duplicate.Description = Description;
            duplicate.UseNoFollow = UseNoFollow;
            duplicate.UseNoIndex = UseNoIndex;
            duplicate.Layout = Layout;
            duplicate.MasterPage = MasterPage;
            duplicate.Image = Image;
            duplicate.SecondaryImage = SecondaryImage;
            duplicate.FeaturedImage = FeaturedImage;
            duplicate.Category = Category;
            duplicate.IsArchived = IsArchived;
            duplicate.IsMasterPage = IsMasterPage;

            return duplicate;
        }
    }
}