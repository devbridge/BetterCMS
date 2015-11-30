// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteMediaCommandTest.cs" company="Devbridge Group LLC">
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
