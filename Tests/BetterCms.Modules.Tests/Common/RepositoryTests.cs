using System;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Module.Root.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Common
{
    [TestFixture]
    public class RepositoryTests : IntegrationTestBase
    {
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
