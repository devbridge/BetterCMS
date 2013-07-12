using Autofac;

using BetterCms.Core.Services;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ApiTests
{
    [TestFixture]
    public class ContentApiTests : ApiTestBase
    {
        [Test]
        public void Should_Create_Preview()
        {
            var securityService = Container.Resolve<ISecurityService>();
            //var contentService = new DefaultContentService(securityService, );
        }
    }
}
