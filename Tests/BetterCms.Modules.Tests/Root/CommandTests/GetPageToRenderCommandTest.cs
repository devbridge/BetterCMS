using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.CommandTests
{
    [TestFixture]
    public class GetPageToRenderCommandTest : IntegrationTestBase
    {
        [Test]
        public void Should_Return_Page_With_PageContents_Successfully()
        {
            string virtualPath = "/test/" + Guid.NewGuid().ToString().Replace("-", string.Empty) + "/";
            var layout = TestDataProvider.CreateNewLayout();
            var regionA = TestDataProvider.CreateNewRegion();
            var regionB = TestDataProvider.CreateNewRegion();

            var htmlContent = TestDataProvider.CreateNewHtmlContent();
            var serverWidgetContent = TestDataProvider.CreateNewServerControlWidget();
            var htmlContentWidget = TestDataProvider.CreateNewHtmlContentWidget();

            layout.LayoutRegions = new List<LayoutRegion>
                                       {
                                           TestDataProvider.CreateNewLayoutRegion(layout, regionA),
                                           TestDataProvider.CreateNewLayoutRegion(layout, regionB)
                                       };

            var page = TestDataProvider.CreateNewPageProperties(layout);
            page.PageContents = new List<PageContent>
                                    {
                                        TestDataProvider.CreateNewPageContent(htmlContent, page, regionA), 
                                        TestDataProvider.CreateNewPageContent(serverWidgetContent, page, regionB),
                                        TestDataProvider.CreateNewPageContent(htmlContentWidget, page, regionB)
                                    };
            page.PageUrl = virtualPath;

            RunDatabaseActionAndAssertionsInTransaction(
                page, 
                session =>
                    { 
                        session.Save(page);
                        session.Flush();
                    },
                (result, session) =>
                    {
                        Page pageAlias = null;
                        Layout layoutAlias = null;

                        var entity = session.QueryOver(() => pageAlias)
                          .Inner.JoinAlias(() => pageAlias.Layout, () => layoutAlias)
                          .Where(f => f.PageUrl == virtualPath.ToLowerInvariant())
                          .Fetch(f => f.Layout).Eager
                          .Fetch(f => f.Layout.LayoutRegions).Eager
                          .Fetch(f => f.PageContents).Eager
                          .Fetch(f => f.PageContents[0].Content).Eager
                          .SingleOrDefault();

                        Assert.IsNotNull(entity);
                        Assert.AreEqual(page.PageContents.Count(), entity.PageContents.Distinct().Count());
                        Assert.AreEqual(page.Layout.LayoutRegions.Count(), entity.Layout.LayoutRegions.Distinct().Count());
                    });
        }
    }
}
