using System;

using BetterModules.Core.Web.Mvc;
using BetterModules.Core.Web.Mvc.Commands;

using Moq;

namespace BetterCms.Test.Module
{
    public class ControllerTestBase : TestBase
    {       
        protected IMessagesIndicator GetMockedMessagesIndicator()
        {
            Mock<IMessagesIndicator> messagesIndicator = new Mock<IMessagesIndicator>();            
            return messagesIndicator.Object;            
        }

        protected ICommandContext GetMockedCommandContext(IMessagesIndicator messagesIndicator = null)
        {
            Mock<ICommandContext> commandContext = new Mock<ICommandContext>();
            commandContext.Setup(f => f.Messages).Returns(() => messagesIndicator ?? GetMockedMessagesIndicator());
            return commandContext.Object;
        }

        protected ICommandResolver GetMockedCommandResolver(Action<Mock<ICommandResolver>> setup)
        {
            Mock<ICommandResolver> commandResolver = new Mock<ICommandResolver>();
            setup(commandResolver);
            return commandResolver.Object;
        }
    }
}
