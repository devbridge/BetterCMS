using System.Collections.Generic;

using BetterCms.Core;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.Api.Operations.Root.Layouts;
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

        [Test]
        public void GetFilteredSorterList()
        {
            using (var client = new JsonServiceClient("http://localhost:55558/bcms-api/"))
            {
                var layouts =
                    client.Get<GetLayoutsResponse>(
                        new GetLayoutsRequest
                            {
                                Data = 
                                    {
                                        Take = 5,
                                        Skip = 2,
                                        Filter =
                                            new DataFilter
                                                {
                                                    Connector = FilterConnector.Or,
                                                    Where =
                                                        new List<FilterItem>(
                                                        new[] { new FilterItem("test", "val", FilterOperation.Greater), })
                                                }
                                    }
                            });

                Assert.IsNotNull(layouts);
                
            }
        }
    }
}