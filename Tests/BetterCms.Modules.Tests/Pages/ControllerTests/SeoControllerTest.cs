// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeoControllerTest.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            var seoController = new SeoController();
            seoController.ModelState.AddModelError("PageTitle", "Page title required.");

            ActionResult result = seoController.EditSeo(new EditSeoViewModel());

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<JsonResult>(result);

            var viewResult = (JsonResult)result;
            Assert.IsNotNull(viewResult.Data);

            Assert.IsInstanceOf<WireJson>(viewResult.Data);
            var wireJson = (WireJson)viewResult.Data;
            Assert.IsFalse(wireJson.Success);
        }

        [Test]
        public void Should_Save_EditSeoViewModel_And_Return_Success_Flag()
        {
            var redirectService = new Mock<IRedirectService>();
            var pageService = new Mock<IPageService>();
            var sitemapService = new Mock<ISitemapService>();
            var securityService = new Mock<ISecurityService>();
            var urlService = new Mock<IUrlService>();
            var cmsConfiguration = new Mock<ICmsConfiguration>();
            var savePageSeoCommandMock = new Mock<SavePageSeoCommand>(redirectService.Object, pageService.Object, sitemapService.Object, urlService.Object, cmsConfiguration.Object);

            savePageSeoCommandMock.SetupGet(x => x.SecurityService).Returns(securityService.Object);
            savePageSeoCommandMock.Setup(f => f.Execute(It.IsAny<EditSeoViewModel>())).Returns(new EditSeoViewModel()).Verifiable();

            var seoController = new SeoController();
            seoController.CommandResolver = GetMockedCommandResolver(mock =>
                {
                    mock.Setup(resolver => resolver.ResolveCommand<SavePageSeoCommand>(It.IsAny<ICommandContext>())).Returns(() => savePageSeoCommandMock.Object);
                });

            ActionResult result = seoController.EditSeo(new EditSeoViewModel());

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<JsonResult>(result);

            var viewResult = (JsonResult)result;
            Assert.IsNotNull(viewResult.Data);

            Assert.IsInstanceOf<WireJson>(viewResult.Data);
            var wireJson = (WireJson)viewResult.Data;
            Assert.IsTrue(wireJson.Success);

            savePageSeoCommandMock.Verify(f => f.Execute(It.IsAny<EditSeoViewModel>()), Times.Once());
        }
    }
}
