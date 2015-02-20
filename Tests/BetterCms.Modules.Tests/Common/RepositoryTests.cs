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
