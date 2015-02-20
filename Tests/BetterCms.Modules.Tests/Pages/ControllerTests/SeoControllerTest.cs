using System.Web.Mvc;

using BetterCms.Core.Services;

using BetterCms.Module.Pages.Command.Page.SavePageSeo;
using BetterCms.Module.Pages.Controllers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Web.Models;
using BetterModules.Core.Web.Mvc.Commands;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ControllerTests
{
    /// <summary>
    /// SEO controller tests.
    /// </summary>
    [TestFixture]
    public class SeoControllerTest : ControllerTestBase
    {        
        [Test]
        public void Should_Get_EditSeo_ViewResult_With_EditSeoViewModel_Successfully()
        {
            /* TODO: Solve ControllerContext issue.
            Mock<GetPageSeoCommand> getPageSeoCommandMock = new Mock<GetPageSeoCommand>();            
            getPageSeoCommandMock.Setup(f => f.Execute(It.IsAny<Guid>())).Returns(new EditSeoViewModel());                        

            SeoController seoController = new SeoController();
            seoController.CommandResolver = GetMockedCommandResolver(mock =>
                {
                    mock.Setup(resolver => resolver.ResolveCommand<GetPageSeoCommand>(It.IsAny<ICommandContext>())).Returns(() => getPageSeoCommandMock.Object);
                });

            ActionResult result = seoController.EditSeo(Guid.NewGuid().ToString());
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            ViewResult viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);            
            Assert.IsInstanceOf<EditSeoViewModel>(viewResult.Model);
            getPageSeoCommandMock.Verify(f => f.Execute(It.IsAny<Guid>()), Times.Once());
            */
        }

        [Test]
        public void Should_Try_To_Save_EditSeoViewModel_And_Return_Error_Flag()
        {
            SeoController seoController = new SeoController();
            seoController.ModelState.AddModelError("PageTitle", "Page title required.");

            ActionResult result = seoController.EditSeo(new EditSeoViewModel());            

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<JsonResult>(result);

            JsonResult viewResult = (JsonResult)result;
            Assert.IsNotNull(viewResult.Data);

            Assert.IsInstanceOf<WireJson>(viewResult.Data);
            WireJson wireJson = (WireJson)viewResult.Data;
            Assert.IsFalse(wireJson.Success);
        }

        [Test]
        public void Should_Save_EditSeoViewModel_And_Return_Success_Flag()
        {
            Mock<IRedirectService> redirectService = new Mock<IRedirectService>();
            Mock<IPageService> pageService = new Mock<IPageService>();
            Mock<ISitemapService> sitemapService = new Mock<ISitemapService>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IUrlService> urlService = new Mock<IUrlService>();
            Mock<ICmsConfiguration> cmsConfiguration = new Mock<ICmsConfiguration>();
            Mock<SavePageSeoCommand> savePageSeoCommandMock = new Mock<SavePageSeoCommand>(redirectService.Object, pageService.Object, sitemapService.Object, urlService.Object, cmsConfiguration.Object);

            savePageSeoCommandMock.SetupGet(x => x.SecurityService).Returns(securityService.Object);
            savePageSeoCommandMock.Setup(f => f.Execute(It.IsAny<EditSeoViewModel>())).Returns(new EditSeoViewModel()).Verifiable();

            SeoController seoController = new SeoController();
            seoController.CommandResolver = GetMockedCommandResolver(mock =>
                {
                    mock.Setup(resolver => resolver.ResolveCommand<SavePageSeoCommand>(It.IsAny<ICommandContext>())).Returns(() => savePageSeoCommandMock.Object);
                });

            ActionResult result = seoController.EditSeo(new EditSeoViewModel());

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<JsonResult>(result);

            JsonResult viewResult = (JsonResult)result;
            Assert.IsNotNull(viewResult.Data);

            Assert.IsInstanceOf<WireJson>(viewResult.Data);
            WireJson wireJson = (WireJson)viewResult.Data;
            Assert.IsTrue(wireJson.Success);

            savePageSeoCommandMock.Verify(f => f.Execute(It.IsAny<EditSeoViewModel>()), Times.Once());
        }
    }
}
