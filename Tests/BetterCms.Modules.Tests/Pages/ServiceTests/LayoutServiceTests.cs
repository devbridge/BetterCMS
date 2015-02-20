using System.Linq;
using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Services;

using Moq;
using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServiceTests
{
    [TestFixture]
    public class LayoutServiceTests : TestBase
    {
        [Test]
        [Ignore] // Fails because of .ToFuture() usage inside service.GetLayouts() method.
        public void Should_Return_Templates_List_Successfully()
        {
            BetterCms.Module.Root.Models.Layout layout1 = TestDataProvider.CreateNewLayout();
            BetterCms.Module.Root.Models.Layout layout2 = TestDataProvider.CreateNewLayout();

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Layout>())
                .Returns(new[] { layout1, layout2 }.AsQueryable());

            var service = new DefaultLayoutService(repositoryMock.Object, 
                new Mock<IOptionService>().Object, 
                new Mock<ICmsConfiguration>().Object, 
                new Mock<IAccessControlService>().Object,
                new Mock<IUnitOfWork>().Object);
            var response = service.GetAvailableLayouts().ToList();

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Title, new[] { layout1, layout2 }.OrderBy(o => o.Name).Select(o => o.Name).First());

            var layout = response.FirstOrDefault(l => layout1.Id == l.TemplateId);
            Assert.IsNotNull(layout);

            Assert.AreEqual(layout1.Name, layout.Title);
        }

        [Test]
        [Ignore] // Fails because of .ToFuture() usage inside service.GetLayouts() method.
        public void Should_Return_Empty_List()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Root.Models.Layout>())
                .Returns(new BetterCms.Module.Root.Models.Layout[] { }.AsQueryable());

            var service = new DefaultLayoutService(repositoryMock.Object, 
                new Mock<IOptionService>().Object, 
                new Mock<ICmsConfiguration>().Object, 
                new Mock<IAccessControlService>().Object,
                new Mock<IUnitOfWork>().Object);
            var response = service.GetAvailableLayouts().ToList();

            Assert.IsNotNull(response);
            Assert.IsEmpty(response);
        }
    }
}
