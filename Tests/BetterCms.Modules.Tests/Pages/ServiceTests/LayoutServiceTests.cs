// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutServiceTests.cs" company="Devbridge Group LLC">
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

            var repositoryMock = new Mock<IRepository>();
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
            var repositoryMock = new Mock<IRepository>();
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
