using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.DataServices;
using BetterCms.Module.MediaManager.DataServices;
using BetterCms.Module.MediaManager.Models;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api
{
    [TestFixture]
    public class ApiTests : DatabaseTestBase
    {
        [Test]
        public void Api_Tests()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);
                var service = new DefaultMediaApiService(repository);

                //var result = service.GetFile(folder.Id);
                






                //var test = result;
            });
        }
    }
}
