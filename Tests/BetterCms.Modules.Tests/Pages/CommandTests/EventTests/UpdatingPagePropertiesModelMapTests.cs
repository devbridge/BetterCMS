using System.Linq;

using BetterCms.Module.Pages.Models.Events;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.EventTests
{
    public class UpdatingPagePropertiesModelMapTests : TestBase
    {
        [Test]
        public void ShouldMapModelCorrectly()
        {
            var entity = TestDataProvider.CreateNewPageProperties();
            entity.MasterPage = TestDataProvider.CreateNewPageProperties();
            entity.PublishedOn = TestDataProvider.ProvideRandomDateTime();
            entity.IsInSitemap = TestDataProvider.ProvideRandomBooleanValue();

            var model = new UpdatingPagePropertiesModel(entity);

            Assert.AreEqual(entity.Title, model.Title);
            Assert.AreEqual(entity.PageUrl, model.PageUrl);
            Assert.AreEqual(entity.PageUrlHash, model.PageUrlHash);
            Assert.AreEqual(entity.Description, model.Description);
            Assert.AreEqual(entity.CustomCss, model.CustomCss);
            Assert.AreEqual(entity.CustomJS, model.CustomJS);
            Assert.AreEqual(entity.MetaTitle, model.MetaTitle);
            Assert.AreEqual(entity.MetaKeywords, model.MetaKeywords);
            Assert.AreEqual(entity.MetaDescription, model.MetaDescription);

            Assert.AreEqual(entity.Status, model.Status);
            Assert.AreEqual(entity.PublishedOn, model.PublishedOn);

            Assert.AreEqual(entity.HasSEO, model.HasSEO);
            Assert.AreEqual(entity.UseCanonicalUrl, model.UseCanonicalUrl);
            Assert.AreEqual(entity.UseNoFollow, model.UseNoFollow);
            Assert.AreEqual(entity.UseNoIndex, model.UseNoIndex);
            Assert.AreEqual(entity.IsMasterPage, model.IsMasterPage);
            Assert.AreEqual(entity.IsArchived, model.IsArchived);

            Assert.AreEqual(entity.IsInSitemap, model.IsInSitemap);
            
            Assert.AreEqual(entity.Layout.Id, model.LayoutId);
            Assert.AreEqual(entity.MasterPage.Id, model.MasterPageId);

            foreach (var category in entity.Categories)
            {
                Assert.Contains(category.Id, model.Categories.ToArray());
            }

            
            Assert.AreEqual(entity.Image.Id, model.MainImageId);
            Assert.AreEqual(entity.SecondaryImage.Id, model.SecondaryImageId);
            Assert.AreEqual(entity.FeaturedImage.Id, model.FeaturedImageId);
        }

        [Test]
        public void ShouldMapModelCorrectly_WithNullReferences()
        {
            var entity = TestDataProvider.CreateNewPageProperties();
            entity.Layout = null;
            entity.MasterPage = null;
            entity.Categories = null;
            entity.Image = null;
            entity.SecondaryImage = null;
            entity.FeaturedImage = null;

            var model = new UpdatingPagePropertiesModel(entity);

            Assert.IsNull(model.LayoutId);
            Assert.IsNull(model.MasterPageId);
            Assert.IsEmpty(model.Categories);
            Assert.IsNull(model.MainImageId);
            Assert.IsNull(model.SecondaryImageId);
            Assert.IsNull(model.FeaturedImageId);
        }
    }
}
