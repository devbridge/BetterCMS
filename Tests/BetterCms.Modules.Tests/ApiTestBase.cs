using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Models;

using Moq;

namespace BetterCms.Test.Module
{
    public class ApiTestBase : TestBase
    {
        protected virtual Mock<IRepository> MockRepository<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            Mock<IRepository> repositoryMock = new Mock<IRepository>();
            
            repositoryMock
                .Setup(f => f.AsQueryable<TEntity>())
                .Returns(entities.AsQueryable());

            repositoryMock.Setup(r => r.First<TEntity>(It.IsAny<Guid>()))
                .Returns(entities.Any() ? entities.First() : null);

            return repositoryMock;
        }
    }
}
