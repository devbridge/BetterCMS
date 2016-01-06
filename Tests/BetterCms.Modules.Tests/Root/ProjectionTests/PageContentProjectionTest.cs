using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Services.Caching;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ProjectionTests
{
    [TestFixture]
    public class PageContentProjectionTest : SerializationTestBase
    {
        [Test]
        public void Should_By_Xml_And_Binary_Serializable()
        {
            var pageContent = TestDataProvider.CreateNewPageContent();
            pageContent.Content = TestDataProvider.CreateNewHtmlContent();
            pageContent.Options = new[]
                                                 {
                                                     TestDataProvider.CreateNewPageContentOption(pageContent),
                                                     TestDataProvider.CreateNewPageContentOption(pageContent),
                                                     TestDataProvider.CreateNewPageContentOption(pageContent)
                                                 };

            var cmsConfiguration = new Mock<ICmsConfiguration>();
            var optionService = new DefaultOptionService(null, new HttpRuntimeCacheService(), cmsConfiguration.Object);
            var optionValues = optionService.GetMergedOptionValues(pageContent.Options, null);

            PageContentProjection original = new PageContentProjection(
                pageContent, pageContent.Content, new HtmlContentAccessor((HtmlContent)pageContent.Content, optionValues));

            RunSerializationAndDeserialization(original,
                projection =>
                    {
                        Assert.AreEqual(original.ContentId, projection.ContentId);
                        Assert.AreEqual(original.Order, projection.Order);
                        Assert.AreEqual(original.RegionId, projection.RegionId);
                    });
        }       
    }
}
