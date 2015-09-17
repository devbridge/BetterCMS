using System.Collections.Generic;
using System.Linq;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class RenderPagePropertiesViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPagePropertiesViewModel" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public RenderPagePropertiesViewModel(Models.PageProperties page = null)
        {
            Tags = new List<string>();
            Categories = new List<RenderPageCategoryViewModel>();
            if (page != null)
            {
                Description = page.Description;
                CustomCss = page.CustomCss;
                CustomJS = page.CustomJS;
                UseCanonicalUrl = page.UseCanonicalUrl;
                UseNoFollow = page.UseNoFollow;
                UseNoIndex = page.UseNoIndex;
                IsInSitemap = page.IsInSitemap;
                MetaKeywords = page.MetaKeywords;
                MetaDescription = page.MetaDescription;
                IsArchived = page.IsArchived;
                
                if (page.Image != null)
                {
                    MainImage = new RenderPageImageViewModel(page.Image);
                }
                if (page.FeaturedImage != null)
                {
                    FeaturedImage = new RenderPageImageViewModel(page.FeaturedImage);
                }
                if (page.SecondaryImage != null)
                {
                    SecondaryImage = new RenderPageImageViewModel(page.SecondaryImage);
                }
                if (page.Categories != null)
                {
                    foreach (var category in page.Categories)
                    {
                        Categories.Add(new RenderPageCategoryViewModel(category));
                    }                    
                }
                if (page.PageTags != null)
                {
                    foreach (var tag in page.PageTags.Distinct())
                    {
                        var tagName = tag.Tag.Name;
                        if (!Tags.Contains(tagName))
                        {
                            Tags.Add(tagName);
                        }
                    }
                }
            }
        }

        public string Description { get; set; }

        public string CustomCss { get; set; }

        public string CustomJS { get; set; }

        public bool UseCanonicalUrl { get; set; }

        public bool UseNoFollow { get; set; }

        public bool UseNoIndex { get; set; }

        public bool IsInSitemap { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public bool IsArchived { get; set; }

        public RenderPageImageViewModel MainImage { get; set; }

        public RenderPageImageViewModel FeaturedImage { get; set; }

        public RenderPageImageViewModel SecondaryImage { get; set; }

        public IList<RenderPageCategoryViewModel> Categories { get; set; }

        public IList<string> Tags { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Description: {1}", base.ToString(), Description);
        }
    }
}