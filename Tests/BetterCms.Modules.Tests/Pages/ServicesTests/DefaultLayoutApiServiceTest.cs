using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.DataServices;
using BetterCms.Module.Root.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServicesTests
{
    [TestFixture]
    public class DefaultLayoutApiServiceTest : TestBase
    {
        [Test]
        public void Should_Return_Layouts_List_Successfully()
        {
            Layout layout1 = TestDataProvider.CreateNewLayout();
            Layout layout2 = TestDataProvider.CreateNewLayout();

            layout1.LayoutRegions = new List<LayoutRegion>();
            layout1.LayoutRegions.Add(TestDataProvider.CreateNewLayoutRegion());

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<Layout>())
                .Returns(new[] { layout1, layout2 }.AsQueryable());

            var service = new DefaultLayoutApiService(repositoryMock.Object);
            var layouts = service.GetLayouts();

            Assert.IsNotNull(layouts);
            Assert.AreEqual(layouts.Count, 2);

            var layout = layouts.FirstOrDefault(l => layout1.Id == l.Id);
            Assert.IsNotNull(layout);
            Assert.AreEqual(layout1.Name, layout.Name);
        }
        
        [Test]
        public void Should_Return_Empty_Layouts_List_Successfully()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<Layout>())
                .Returns(new Layout[] { }.AsQueryable());

            var service = new DefaultLayoutApiService(repositoryMock.Object);
            var layouts = service.GetLayouts();

            Assert.IsNotNull(layouts);
            Assert.IsEmpty(layouts);
        }
    }
}
