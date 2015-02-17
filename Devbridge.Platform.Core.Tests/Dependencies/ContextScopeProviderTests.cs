using Autofac;
using Autofac.Core.Registration;

using Devbridge.Platform.Core.Dependencies;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Tests.Dependencies
{
    [TestFixture]
    public class ContextScopeProviderTests : TestBase
    {
        [Test]
        public void ShouldRegisterAndRetrieveService()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                Assert.IsNotNull(container);

                // Service is not registered yet
                bool exceptionWasThrown = false;
                try
                {
                    container.Resolve<ITestInterface>();
                }
                catch (ComponentNotRegisteredException)
                {
                    exceptionWasThrown = true;
                }
                Assert.IsTrue(exceptionWasThrown);
            }

            // Registering service
            var builder = new ContainerBuilder();
            builder.RegisterType<TestInterfaceImplementation>().As<ITestInterface>().SingleInstance();
            ContextScopeProvider.RegisterTypes(builder);

            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var service = container.Resolve<ITestInterface>();

                Assert.IsNotNull(service);
                Assert.IsTrue(service is TestInterfaceImplementation);
            }
        }

        public interface ITestInterface
        {
        }

        public class TestInterfaceImplementation : ITestInterface
        {
        }
    }
}
