using BetterCms.Module.Api;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api
{
    [TestFixture]
    public class ApiFacadeTests : TestBase
    {
        [Test]
        public void ShouldCreateAndDisposeApiFasade()
        {            
            IApiFacade apiContainer;

            using (var api = ApiFactory.Create())
            {
                apiContainer = api;

                Assert.IsNotNull(api.Root);
                Assert.IsNotNull(api.Blog);
                Assert.IsNotNull(api.Media);
                Assert.IsNotNull(api.Pages);
            }

            Assert.IsNull(apiContainer.Scope);
        }
    }
}