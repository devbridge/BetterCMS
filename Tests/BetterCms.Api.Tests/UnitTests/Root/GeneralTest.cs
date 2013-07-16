using BetterCms.Core;
using BetterCms.Module.Api.Operations.Root.Version;

using NUnit.Framework;

using ServiceStack.ServiceClient.Web;

namespace BetterCms.Api.Tests.UnitTests.Root
{
    [TestFixture]
    public class GeneralTest
    {
        [Test]
        public void GetCurrentVersion()
        {
            using (var client = new JsonServiceClient("http://localhost:55558/bcms-api/"))
            {
                GetVersionResponse version = client.Get<GetVersionResponse>("current-version");

                Assert.IsNotNull(version);
                Assert.AreEqual(CmsContext.Config.Version, version.Data);
            }
        }
    }
}