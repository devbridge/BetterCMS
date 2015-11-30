// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryTests.cs" company="Devbridge Group LLC">
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

using Autofac;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Exceptions.DataTier;

using NUnit.Framework;

namespace BetterCms.Test.Module.Common
{
    using System.Collections.Generic;

    [TestFixture]
    public class RepositoryTests : IntegrationTestBase
    {
        [Test]
        public void Saving_New_Entity_With_an_Empty_Id_Should_Generate_Id()
        {
            RunActionInTransaction(session =>
            {
                var layout = TestDataProvider.CreateNewLayout();
                var region = TestDataProvider.CreateNewRegion();

                layout.LayoutRegions = new List<LayoutRegion>
                    {
                        TestDataProvider.CreateNewLayoutRegion(layout, region),
                    };

                var page = TestDataProvider.CreateNewPageProperties(layout);

                page.Id = Guid.Empty;

                Assert.AreEqual(page.Id, Guid.Empty);

                session.SaveOrUpdate(page);
                session.Flush();
                session.Clear();

                Assert.AreNotEqual(page.Id, Guid.Empty);
            });
        }

        [Test]
        public void Saving_New_Entity_With_Predefined_Empty_Id_Should_Not_Change_Id()
        {
            RunActionInTransaction(session =>
            {
                var layout = TestDataProvider.CreateNewLayout();
                var region = TestDataProvider.CreateNewRegion();

                layout.LayoutRegions = new List<LayoutRegion>
                    {
                        TestDataProvider.CreateNewLayoutRegion(layout, region),
                    };

                var page = TestDataProvider.CreateNewPageProperties(layout);

                var pageId = page.Id = Guid.NewGuid();

                Assert.AreEqual(page.Id, pageId);

                session.SaveOrUpdate(page);
                session.Flush();
                session.Clear();

                Assert.AreEqual(page.Id, pageId);
            });
        }

        [Test]
        public void Changing_Saved_Entity_Id_Should_Fail()
        {
            RunActionInTransaction(
                session =>
                    {
                        var layout = TestDataProvider.CreateNewLayout();
                        var region = TestDataProvider.CreateNewRegion();

                        layout.LayoutRegions = new List<LayoutRegion> { TestDataProvider.CreateNewLayoutRegion(layout, region), };

                        var page = TestDataProvider.CreateNewPageProperties(layout);

                        var pageId = page.Id;
                        Assert.AreNotEqual(pageId, Guid.Empty);

                        session.SaveOrUpdate(page);
                        session.Flush();
                        session.Clear();

                        Assert.AreEqual(page.Id, pageId);

                        var originalPage = session.Get<Page>(pageId);
                        Assert.AreEqual(page, originalPage);

                        var newId = Guid.NewGuid();
                        originalPage.Id = newId;

                        Assert.Throws<NHibernate.HibernateException>(
                            () =>
                                {
                                    session.SaveOrUpdate(originalPage);
                                    session.Flush();
                                });
                    });
        }

        [Test]
        public void Saving_Entity_With_Duplicated_Id_Should_Fail()
        {
            RunActionInTransaction(
                session =>
                    {
                        var layout = TestDataProvider.CreateNewLayout();
                        var region = TestDataProvider.CreateNewRegion();

                        layout.LayoutRegions = new List<LayoutRegion> { TestDataProvider.CreateNewLayoutRegion(layout, region), };

                        var page = TestDataProvider.CreateNewPageProperties(layout);

                        var pageId = page.Id;
                        Assert.AreNotEqual(pageId, Guid.Empty);

                        session.SaveOrUpdate(page);
                        session.Flush();
                        session.Clear();

                        Assert.AreEqual(page.Id, pageId);

                        var originalPage = session.Get<Page>(pageId);
                        Assert.AreEqual(page, originalPage);

                        var page2 = TestDataProvider.CreateNewPageProperties(layout);
                        page2.Id = pageId;

                        Assert.Throws<NHibernate.NonUniqueObjectException>(
                            () =>
                                {
                                    session.SaveOrUpdate(page2);
                                    session.Flush();
                                });
                    });
        }

        [Test]
        public void Saving_Entity_With_Duplicated_Id_Should_Fail2()
        {
            RunActionInTransaction(
                session =>
                    {
                        var layout = TestDataProvider.CreateNewLayout();
                        var region = TestDataProvider.CreateNewRegion();

                        layout.LayoutRegions = new List<LayoutRegion> { TestDataProvider.CreateNewLayoutRegion(layout, region), };

                        var page = TestDataProvider.CreateNewPageProperties(layout);

                        var pageId = page.Id;
                        Assert.AreNotEqual(pageId, Guid.Empty);

                        session.SaveOrUpdate(page);
                        session.Flush();
                        session.Clear();

                        Assert.AreEqual(page.Id, pageId);

                        var originalPage = session.Get<Page>(pageId);
                        Assert.AreEqual(page, originalPage);

                        var page2 = TestDataProvider.CreateNewPageProperties(layout);
                        page2.Id = pageId;

                        session.Clear();
                        Assert.Throws<NHibernate.Exceptions.GenericADOException>(
                            () =>
                                {
                                    session.SaveOrUpdate(page2);
                                    session.Flush();
                                });
                    });
        }

        [Test]
        public void First_By_Id_Should_Throw_Exception_If_Entity_Not_Found()
        {
            Assert.Throws<EntityNotFoundException>(
                () =>
                    {
                        var repository = Container.Resolve<IRepository>();
                        repository.First<Page>(Guid.NewGuid());
                    });
        }

        [Test]
        public void First_By_Where_Should_Throw_Exception_If_Entity_Not_Found()
        {
            Assert.Throws<EntityNotFoundException>(
                () =>
                {
                    var repository = Container.Resolve<IRepository>();
                    repository.First<Page>(f => f.Id == Guid.NewGuid());
                });
        }
    }
}
