using BetterCms.Module.Api;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api
{
    [TestFixture]
    public class UsersModuleServiceValidationTest : TestBase
    {
        [Test]
        public void ShouldCreateAndDisposeApiFacade()
        {
            using (var api = ApiFactory.Create())
            {
                Assert.IsNotNull(api.Users);
                Assert.IsNotNull(api.Users.User);
                Assert.IsNotNull(api.Users.Users);
                Assert.IsNotNull(api.Users.Role);
                Assert.IsNotNull(api.Users.Roles);

                Assert.AreEqual(api.Users.Users.GetType(), System.Type.GetType("BetterCms.Module.Users.Api.Operations.Users.Users.UsersService,BetterCms.Module.Users.Api"));
                Assert.AreEqual(api.Users.User.GetType(), System.Type.GetType("BetterCms.Module.Users.Api.Operations.Users.Users.User.UserService,BetterCms.Module.Users.Api"));
                Assert.AreEqual(api.Users.Role.GetType(), System.Type.GetType("BetterCms.Module.Users.Api.Operations.Users.Roles.Role.RoleService,BetterCms.Module.Users.Api"));
                Assert.AreEqual(api.Users.Roles.GetType(), System.Type.GetType("BetterCms.Module.Users.Api.Operations.Users.Roles.RolesService,BetterCms.Module.Users.Api"));
            }
        }
    }
}
