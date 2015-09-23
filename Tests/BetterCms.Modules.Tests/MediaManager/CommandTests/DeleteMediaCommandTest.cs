﻿using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using Moq;

using NHibernate.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.MediaManager.CommandTests
{
    [TestFixture]
    public class DeleteMediaCommandTest : IntegrationTestBase
    {
        [Test]
        public void Should_Delete_Media_With_Access_Rules()
        {
            RunActionInTransaction(session =>
                {
                    var uow = new DefaultUnitOfWork(session);
                    var repository = new DefaultRepository(uow);

                    var file = TestDataProvider.CreateNewMediaFileWithAccessRules(3);
                    session.SaveOrUpdate(file);
                    session.Flush();
                    session.Clear();

                    var storageService = new Mock<IMediaFileService>().Object;
                    var command = new DeleteMediaCommand(storageService);
                    command.Repository = repository;
                    command.UnitOfWork = uow;

                    var result = command.Execute(new DeleteMediaCommandRequest
                                        {
                                            Id = file.Id,
                                            Version = file.Version
                                        });

                    Assert.IsTrue(result);

                    session.Clear();

                    var deletedFile = session.Query<MediaFile>().FirstOrDefault(f => f.Id == file.Id && !f.IsDeleted);
                    Assert.IsNull(deletedFile);
                });
        }
    }
}
