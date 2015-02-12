using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Pages.Models.Events
{
    public class UpdatingPagePropertiesModel
    {
        public UpdatingPagePropertiesModel(PageProperties pageProperties)
        {
            PageUrl = pageProperties.PageUrl;
            PageUrlHash = pageProperties.PageUrlHash;
            Title = pageProperties.Title;
            Description = pageProperties.Description;
            CustomCss = pageProperties.CustomCss;
            CustomJS = pageProperties.CustomJS;
            MetaTitle = pageProperties.MetaTitle;
            MetaKeywords = pageProperties.MetaKeywords;
            MetaDescription = pageProperties.MetaDescription;

            Status = pageProperties.Status;
            PublishedOn = pageProperties.PublishedOn;

            HasSEO = pageProperties.HasSEO;
            UseCanonicalUrl = pageProperties.UseCanonicalUrl;
            UseNoFollow = pageProperties.UseNoFollow;
            UseNoIndex = pageProperties.UseNoIndex;
            IsMasterPage = pageProperties.IsMasterPage;
            IsArchived = pageProperties.IsArchived;

            IsInSitemap = pageProperties.IsInSitemap;

            LayoutId = pageProperties.Layout != null ? pageProperties.Layout.Id : (Guid?)null;
            MasterPageId = pageProperties.MasterPage != null ? pageProperties.MasterPage.Id : (Guid?)null;

            Categories = pageProperties.Categories != null ? pageProperties.Categories.Select(c => c.Id).ToList() : new List<Guid>();

            MainImageId = pageProperties.Image != null ? pageProperties.Image.Id : (Guid?)null;
            SecondaryImageId = pageProperties.SecondaryImage != null ? pageProperties.SecondaryImage.Id : (Guid?)null;
            FeaturedImageId = pageProperties.FeaturedImage != null ? pageProperties.FeaturedImage.Id : (Guid?)null;
        }

        public string PageUrl { get; private set; }
        public string PageUrlHash { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public string CustomCss { get; set; }
        public string CustomJS { get; set; }
        public string MetaTitle { get; private set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public PageStatus Status { get; private set; }
        public DateTime? PublishedOn { get; private set; }

        public bool HasSEO { get; private set; }
        public bool UseCanonicalUrl { get; private set; }
        public bool UseNoFollow { get; private set; }
        public bool UseNoIndex { get; private set; }
        public bool IsMasterPage { get; private set; }
        public bool IsArchived { get; private set; }

        public bool IsInSitemap { get; private set; }

        public Guid? LayoutId { get; private set; }
        public Guid? MasterPageId { get; private set; }
        public IList<Guid> Categories { get; private set; }
        public Guid? MainImageId { get; private set; }
        public Guid? SecondaryImageId { get; private set; }
        public Guid? FeaturedImageId { get; private set; }

        public override string ToString()
        {
            return string.Format(
                "{0}, Title: {1}, Url: {2}, Status: {3}, HasSEO: {4}, LayoutId: {5}, MasterPageId: {6}",
                base.ToString(),
                Title,
                PageUrl,
                Status,
                HasSEO,
                LayoutId,
                MasterPageId);
        }
    }
}