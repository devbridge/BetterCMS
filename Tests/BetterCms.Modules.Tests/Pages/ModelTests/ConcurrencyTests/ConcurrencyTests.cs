using Autofac;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.ModelTests.ConcurrencyTests
{
    [TestFixture]
    public class ConcurrencyTests : TestBase
    {
        [Test]
        public void TestConcurencySaveWithoutException()
        {
            var sessionFactory = this.Container.Resolve<ISessionFactoryProvider>();
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    // Create layout
                    var layout = this.TestDataProvider.CreateNewLayout();
                    session.Save(layout);
                    session.Flush();

                    // Edit layout
                    layout.Name = "Changed 1";
                    session.Save(layout);
                    session.Flush();

                    // Edit layout
                    layout.Name = "Changed 2";
                    session.Save(layout);
                    session.Flush();
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ConcurrentDataException))]
        public void TestConcurencySaveWithException()
        {
            var sessionFactory = this.Container.Resolve<ISessionFactoryProvider>();
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    // Create layout
                    var layout = this.TestDataProvider.CreateNewLayout();
                    session.Save(layout);
                    session.Flush();

                    // Edit layout
                    layout.Name = "Changed 1";
                    session.Save(layout);
                    session.Flush();

                    // Edit layout
                    layout.Name = "Changed 2";
                    layout.Version = 99999;
                    session.Save(layout);
                    session.Flush();
                }
            }
        }
    }
}
