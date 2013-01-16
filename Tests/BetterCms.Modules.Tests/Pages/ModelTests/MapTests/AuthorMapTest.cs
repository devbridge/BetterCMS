using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;

using NHibernate.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.MapTests
{
    [TestFixture]
    public class AuthorMapTest : DatabaseTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Author_Successfully()
        {
            var content = TestDataProvider.CreateNewAuthor();
            RunEntityMapTestsInTransaction(content);  
        }
    }
}
