using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autofac;

using BetterModules.Core.DataAccess;
using BetterCms.Core.Security;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.PageTests
{
    [TestFixture]
    public class GetPagesListCommandTest : IntegrationTestBase
    {
        [Test]
        public void Should_Do_Not_Return_Denied_Pages()
        {
            var query =
                Container.Resolve<IRepository>()
                         .AsQueryable<BetterCms.Module.Root.Models.Page>()
                         .Select(
                             f => new
                                      {
                                          Id = f.Id,
                                          Rules = f.AccessRules
                                      })
                         .Where(f => f.Rules.Any(b => b.AccessLevel == AccessLevel.Deny));
                            

            var list = query.ToList();

        }
    }
}
