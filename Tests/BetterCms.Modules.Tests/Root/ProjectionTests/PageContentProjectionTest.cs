using System.Linq;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Projections;

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

            PageContentProjection original = new PageContentProjection(
                pageContent, pageContent.Content, new HtmlContentAccessor((HtmlContent)pageContent.Content, pageContent.Options.Cast<IOption>().ToList()));

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
