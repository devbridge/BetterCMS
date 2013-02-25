using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.DataServices;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServicesTests
{
    [TestFixture]
    public class DefaultLayoutApiServiceTest : TestBase
    {
        // TODO: Mock-up Fetch()
        /*[Test]
        public void Should_Return_Layouts_List_Successfully()
        {
            BetterCms.Module.Root.Models.Layout layout1 = TestDataProvider.CreateNewLayout();
            BetterCms.Module.Root.Models.Layout layout2 = TestDataProvider.CreateNewLayout();

            layout1.LayoutRegions.Add(TestDataProvider.CreateNewLayoutRegion());

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Layout>())
                .Returns(new[] { layout1, layout2 }.AsQueryable());

            var service = new DefaultLayoutApiService(repositoryMock.Object);
            var layouts = service.GetLayouts();

            Assert.IsNotNull(layouts);
            Assert.AreEqual(layouts.Count, 2);

            var layout = layouts.FirstOrDefault(l => layout1.Id == l.Id);
            Assert.IsNotNull(layout);
            Assert.AreEqual(layout1.Name, layout.Name);
            Assert.IsNotNull(layout.Regions);
            Assert.GreaterOrEqual(layout.Regions.Count(), 2);
        }*/
        
        /*[Test]
        public void Should_Return_Empty_Layouts_List_Successfully()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Layout>())
                .Returns(new BetterCms.Module.Root.Models.Layout[] { }.AsQueryable());

            var service = new DefaultLayoutApiService(repositoryMock.Object);
            var layouts = service.GetLayouts();

            Assert.IsNotNull(layouts);
            Assert.IsEmpty(layouts);
        }*/
    }
}
