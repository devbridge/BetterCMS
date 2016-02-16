// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageSeoCommandTest.cs" company="Devbridge Group LLC">
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

            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<PageProperties>())
                .Returns(new[] { page1, page2 }.AsQueryable());

            var command = new GetPageSeoCommand(Container.Resolve<ICmsConfiguration>());
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
            var repositoryMock = new Mock<IRepository>();
            repositoryMock
                .Setup(f => f.AsQueryable<PageProperties>())
                .Returns(new PageProperties[] { }.AsQueryable());

            var command = new GetPageSeoCommand(Container.Resolve<ICmsConfiguration>());
            command.Repository = repositoryMock.Object;

            var model = command.Execute(Guid.Empty);

            Assert.IsNotNull(model);
            Assert.AreEqual(Guid.Empty, model.PageId);
        }
    }
}
