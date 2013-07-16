using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataAccess;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ApiTests
{
    [TestFixture]
    public class RedirectApiTests : ApiTestBase
    {
        [Test]
        public void Should_Return_Redirects_List_Successfully()
        {
            BetterCms.Module.Pages.Models.Redirect redirect1 = TestDataProvider.CreateNewRedirect();
            BetterCms.Module.Pages.Models.Redirect redirect2 = TestDataProvider.CreateNewRedirect();

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Pages.Models.Redirect>())
                .Returns(new[] { redirect1, redirect2 }.AsQueryable());

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var redirects = service.GetRedirects();

                Assert.IsNotNull(redirects);
                Assert.AreEqual(redirects.Items.Count, 2);
                Assert.AreEqual(redirects.TotalCount, 2);

                var redirect = redirects.Items.FirstOrDefault(l => redirect1.Id == l.Id);
                Assert.IsNotNull(redirect);
                Assert.AreEqual(redirect1.PageUrl, redirect.PageUrl);
                Assert.AreEqual(redirect1.RedirectUrl, redirect.RedirectUrl);
            }
        }
        
        [Test]
        public void Should_Return_Empty_Redirects_List_Successfully()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<BetterCms.Module.Pages.Models.Redirect>())
                .Returns(new BetterCms.Module.Pages.Models.Redirect[] { }.AsQueryable());

            using (var service = new PagesApiContext(Container.BeginLifetimeScope(), repositoryMock.Object))
            {
                var redirects = service.GetRedirects();

                Assert.IsNotNull(redirects);
                Assert.IsEmpty(redirects.Items);
                Assert.AreEqual(redirects.TotalCount, 0);
            }
        }
    }
}
