using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autofac;

using BetterCms.Core.Services;
using BetterCms.Module.Root.Services;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ServicesTests
{
    [TestFixture]
    public class DefaultContentServiceTest : DatabaseTestBase
    {
        [Test]
        public void Should_Create_Preview()
        {
            var securityService = Container.Resolve<ISecurityService>();
            //var contentService = new DefaultContentService(securityService, );
        }
    }
}
