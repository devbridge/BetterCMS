using System.Collections.Generic;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Api.DataContracts;
using BetterCms.Module.Root.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ApiTests
{
    [TestFixture]
    public class LayoutApiTests : ApiTestBase
    {
        [Test]
        public void Should_Return_Layouts_List_Successfully()
        {
            var fakeLayouts = CreateLayouts().ToList();
            var repositoryMock = MockRepository(fakeLayouts);

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var layouts = service.GetLayouts();

                Assert.IsNotNull(layouts);
                Assert.AreEqual(layouts.Items.Count, fakeLayouts.Count);
                Assert.AreEqual(layouts.TotalCount, fakeLayouts.Count);

                var fakeLayout = fakeLayouts[0];
                var layout = layouts.Items.FirstOrDefault(l => fakeLayout.Id == l.Id);
                Assert.IsNotNull(layout);
                Assert.AreEqual(fakeLayout.Name, layout.Name);
            }
        }

        [Test]
        public void Should_Return_Filtered_Ordered_Layouts_List_Successfully()
        {
            var fakeLayouts = CreateLayouts().ToList();
            var repositoryMock = MockRepository(fakeLayouts);

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var layouts = service.GetLayouts(new GetLayoutsRequest(t => t.Name.Contains("Layout"), null, true));

                Assert.IsNotNull(layouts);
                Assert.AreEqual(layouts.Items.Count, 3);
                Assert.AreEqual(layouts.TotalCount, 3);

                var fakeLayout = fakeLayouts.First(t => t.Name == "Layout3");
                var layout = layouts.Items[0];
                Assert.IsNotNull(layout);
                Assert.AreEqual(fakeLayout.Id, layout.Id);
            }
        }

        [Test]
        public void Should_Return_Filtered_Ordered_Paged_Layouts_List_Successfully()
        {
            var fakeLayouts = CreateLayouts().ToList();
            var repositoryMock = MockRepository(fakeLayouts);

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var request = new GetLayoutsRequest(t => t.Name.Contains("Layout"), null, true);
                request.AddPaging(1, 2);
                var layouts = service.GetLayouts(request);

                Assert.IsNotNull(layouts);
                Assert.AreEqual(layouts.Items.Count, 1);
                Assert.AreEqual(layouts.TotalCount, 3);

                var fakeLayout = fakeLayouts.First(t => t.Name == "Layout2");
                var layout = layouts.Items[0];
                Assert.IsNotNull(layout);
                Assert.AreEqual(fakeLayout.Id, layout.Id);
            }
        }

        [Test]
        public void Should_Return_Empty_Layouts_List_Successfully()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<Layout>())
                .Returns(new Layout[] { }.AsQueryable());

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var layouts = service.GetLayouts();

                Assert.IsNotNull(layouts);
                Assert.IsEmpty(layouts.Items);
                Assert.AreEqual(layouts.TotalCount, 0);
            }
        }

        private IEnumerable<Layout> CreateLayouts()
        {
            Layout layout1 = TestDataProvider.CreateNewLayout();
            Layout layout2 = TestDataProvider.CreateNewLayout();
            Layout layout3 = TestDataProvider.CreateNewLayout();
            Layout layout4 = TestDataProvider.CreateNewLayout();
            Layout layout5 = TestDataProvider.CreateNewLayout();

            layout1.Name = "Layout1";
            layout2.Name = "Layout2";
            layout3.Name = "Layout3";
            layout4.Name = "aaa1";
            layout5.Name = "aaa2";

            return new[] { layout1, layout2, layout3, layout4, layout5 };
        }
    }
}
