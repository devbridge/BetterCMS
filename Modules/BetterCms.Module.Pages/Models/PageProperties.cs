using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class PageProperties : Page, ICategorized
    {
        public const string CategorizableItemKeyForPages = "Pages";

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


        public virtual MediaImage Image { get; set; }
        public virtual MediaImage SecondaryImage { get; set; }
        public virtual MediaImage FeaturedImage { get; set; }
        public virtual bool IsArchived { get; set; }

        public virtual bool IsReadOnly { get; set; }

        public virtual IList<PageCategory> Categories { get; set; }

        IEnumerable<IEntityCategory> ICategorized.Categories
        {
            get
            {
                return Categories;
            }
        }

        public PageProperties()
        {
            UseCanonicalUrl = true;
        }

        public virtual PageProperties Duplicate()
        {
            return CopyDataToDuplicate(new PageProperties());
        }

        public virtual void AddCategory(IEntityCategory category)
        {
            if (Categories == null)
            {
                Categories = new List<PageCategory>();
            }

            Categories.Add(category as PageCategory);
        }

        public virtual void RemoveCategory(IEntityCategory category)
        {
            if (Categories != null)
            {
                Categories.Remove(category as PageCategory);   
            }           
        }

        public virtual string GetCategorizableItemKey()
        {
            return CategorizableItemKeyForPages;
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
            duplicate.IsArchived = IsArchived;
            duplicate.IsMasterPage = IsMasterPage;
            duplicate.ForceAccessProtocol = ForceAccessProtocol;

            return duplicate;
        }
    }
}