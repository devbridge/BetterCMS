using System.Linq;

using Autofac;

using BetterCms.Core.Security;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using BetterModules.Core.Web.Mvc.Commands;
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
                    var accessControlService = new Mock<IAccessControlService>().Object;
                    var commandContext = new Mock<ICommandContext>().Object;
                    var cmsConfiguration = Container.Resolve<ICmsConfiguration>();
                    var httpContextAccessor = new Mock<IHttpContextAccessor>();
                    httpContextAccessor.SetReturnsDefault("http://wwww.bcms.com/uploads/trash");

                    var file = TestDataProvider.CreateNewMediaFileWithAccessRules(3);
                    session.SaveOrUpdate(file);
                    session.Flush();
                    session.Clear();

                    var mediaService = new DefaultMediaService(repository, uow, accessControlService, cmsConfiguration);
                    var command = new DeleteMediaCommand(mediaService);
                    command.Repository = repository;
                    command.UnitOfWork = uow;
                    command.Context = commandContext;

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
