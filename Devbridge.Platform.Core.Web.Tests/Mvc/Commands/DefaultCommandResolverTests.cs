using System.Security.Principal;

using Autofac;

using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Web.Dependencies;
using Devbridge.Platform.Core.Web.Mvc;
using Devbridge.Platform.Core.Web.Mvc.Commands;
using Devbridge.Platform.Core.Web.Tests.TestHelpers;
using Devbridge.Platform.Core.Web.Web;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Mvc.Commands
{
    [TestFixture]
    public class DefaultCommandResolverTests : TestBase
    {
        [Test]
        public void ShouldResolve_Commands_Successfully()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType(typeof(CommandTest)).AsSelf();
            ContextScopeProvider.RegisterTypes(containerBuilder);

            var httpContextMoq = new HttpContextMoq();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor
                .Setup(a => a.GetCurrent())
                .Returns(() => httpContextMoq.HttpContextBase);

            var provider = new PerWebRequestContainerProvider(accessor.Object);
            var resolver = new DefaultCommandResolver(provider);

            var commandContext = new CommandContextTest();
            var command = resolver.ResolveCommand<CommandTest>(commandContext);
            Assert.IsNotNull(command);
            Assert.AreEqual(command.Context, commandContext);
        }

        private class CommandTest : ICommand
        {
            public void Execute()
            {
            }

            public ICommandContext Context { get; set; }
        }

        private class CommandContextTest : ICommandContext
        {
            public IMessagesIndicator Messages { get; set; }
            public IPrincipal Principal { get; set; }
        }
    }
}
