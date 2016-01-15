// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeletePageContentCommandTest.cs" company="Devbridge Group LLC">
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
using Autofac;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Command.Content.DeletePageContent;
using BetterCms.Module.Root.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.ContentTests
{
    [TestFixture]
    public class DeletePageContentCommandTest : IntegrationTestBase
    {
        [Test]
        public void Should_Delete_Page_Content()
        {
            RunActionInTransaction(session =>
            {
                // Create content
                var content = TestDataProvider.CreateNewHtmlContent();
                var pageContent = TestDataProvider.CreateNewPageContent(content);
                
                session.SaveOrUpdate(pageContent);
                session.Flush();
                session.Clear();

                // Delete page content
                var request = new DeletePageContentCommandRequest
                                  {
                                      PageContentId = pageContent.Id,
                                      PageContentVersion = pageContent.Version,
                                      ContentVersion = content.Version
                                  };
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);
                var securityService = new Mock<ISecurityService>().Object;
                var optionService = new Mock<IOptionService>().Object;
                var childContentService = new Mock<IChildContentService>().Object;

                var contentService = new DefaultContentService(securityService, repository, optionService, childContentService);
                var command = new DeletePageContentCommand(contentService);
                command.UnitOfWork = unitOfWork;
                command.Repository = repository;

                var result = command.Execute(request);
                Assert.IsTrue(result);
            });
        }
    }
}
