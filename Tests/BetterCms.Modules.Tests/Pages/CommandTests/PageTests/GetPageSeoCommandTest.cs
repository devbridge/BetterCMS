using System;
using System.Linq;

using Autofac;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Pages.Command.Page.GetPageSeo;
using BetterCms.Module.Pages.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.PageTests
{   
    [TestFixture]
    public class GetPageSeoCommandTest : TestBase
    {
        [Ignore] // Fails because of .ToFuture() usage inside GetPageSeoCommand.Execute() method.
        [Test]
        public void Should_Find_Page_And_Return_ViewModel_Successfully()
        {
            PageProperties page1 = this.TestDataProvider.CreateNewPageProperties();
            PageProperties page2 = this.TestDataProvider.CreateNewPageProperties();

            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<PageProperties>())
                .Returns(new[] { page1, page2 }.AsQueryable());

            GetPageSeoCommand command = new GetPageSeoCommand(Container.Resolve<ICmsConfiguration>());
            command.Repository = repositoryMock.Object;

            var model = command.Execute(page1.Id);

            Assert.IsNotNull(model);
            Assert.AreEqual(page1.Id, model.PageId);
            Assert.AreEqual(page1.Version, model.Version);
            Assert.AreEqual(page1.Title, model.PageTitle);
            Assert.AreEqual(page1.PageUrl, model.PageUrlPath);
            Assert.AreEqual(page1.PageUrl, model.ChangedUrlPath);
            Assert.IsTrue(model.CreatePermanentRedirect);
            Assert.AreEqual(page1.MetaTitle, model.MetaTitle);
            Assert.AreEqual(page1.MetaKeywords, model.MetaKeywords);
            Assert.AreEqual(page1.MetaDescription, model.MetaDescription);                 
            Assert.AreEqual(page1.UseCanonicalUrl, model.UseCanonicalUrl);                 
        }

        [Test]
        public void Should_Return_Empty_ViewModel()
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<PageProperties>())
                .Returns(new PageProperties[] { }.AsQueryable());

            GetPageSeoCommand command = new GetPageSeoCommand(Container.Resolve<ICmsConfiguration>());
            command.Repository = repositoryMock.Object;

            var model = command.Execute(Guid.Empty);

            Assert.IsNotNull(model);            
            Assert.AreEqual(Guid.Empty, model.PageId);
        }
    }
}
