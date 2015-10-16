using System.Linq;

using Autofac;

using BetterCms.Core.Services.Storage;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using BetterModules.Core.Web.Web;

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
                    var storageService = new Mock<IStorageService>().Object;
                    var cmsConfiguration = Container.Resolve<ICmsConfiguration>();
                    var httpContextAccessor = new Mock<IHttpContextAccessor>();
                    httpContextAccessor.SetReturnsDefault("http://wwww.bcms.com/uploads/trash");

                    var file = TestDataProvider.CreateNewMediaFileWithAccessRules(3);
                    session.SaveOrUpdate(file);
                    session.Flush();
                    session.Clear();

                    var mediafileService = new DefaultMediaFileService(storageService, repository, uow, cmsConfiguration, httpContextAccessor.Object, null, null, null);
                    var command = new DeleteMediaCommand(mediafileService);
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
