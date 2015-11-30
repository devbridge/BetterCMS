// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageHtmlContentCommandTest.cs" company="Devbridge Group LLC">
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
using System;
using System.Security.Principal;

using Autofac;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Services;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Pages.Command.Content.GetPageHtmlContent;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{
    [TestFixture]
    public class GetPageHtmlContentCommandTest : TestBase
    {
        [Test]
        public void Should_Return_Page_Html_Content()
        {
            // Create html content
            var htmlContent = TestDataProvider.CreateNewHtmlContent();
            var pageContent = TestDataProvider.CreateNewPageContent(htmlContent);
            htmlContent.Status = ContentStatus.Published;

            // Mock security service
            var securityMock = new Mock<ISecurityService>();
            securityMock.Setup(s => s.IsAuthorized(It.IsAny<IPrincipal>(), It.IsAny<string>())).Returns(true);

            // Mock content service
            var contentServiceMock = new Mock<IContentService>();
            contentServiceMock
                .Setup(f => f.GetPageContentForEdit(pageContent.Id))
                .Returns(new Tuple<PageContent, Content>(pageContent, htmlContent));

            // Create command
            var command = new GetPageHtmlContentCommand(contentServiceMock.Object, new Mock<IMasterPageService>().Object, Container.Resolve<ICmsConfiguration>());
            command.UnitOfWork = new Mock<IUnitOfWork>().Object;
            command.Repository = new Mock<IRepository>().Object;
            command.Context = new Mock<ICommandContext>().Object;
            command.SecurityService = securityMock.Object;

            // Execute command
            var result = command.Execute(pageContent.Id);                     

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, pageContent.Id);
            Assert.AreEqual(result.ContentId, htmlContent.Id);
            Assert.AreEqual(result.ContentName, htmlContent.Name);
            Assert.AreEqual(result.ContentVersion, htmlContent.Version);
            Assert.AreEqual(result.CurrentStatus, htmlContent.Status);
            Assert.AreEqual(result.PageId, pageContent.Page.Id);
            Assert.AreEqual(result.RegionId, pageContent.Region.Id);
            Assert.AreEqual(result.LiveFrom, htmlContent.ActivationDate);
            Assert.AreEqual(result.LiveTo, htmlContent.ExpirationDate);
        }
    }
}
