// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerTestBase.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            var messagesIndicator = new Mock<IMessagesIndicator>();
            return messagesIndicator.Object;
        }

        protected ICommandContext GetMockedCommandContext(IMessagesIndicator messagesIndicator = null)
        {
            var commandContext = new Mock<ICommandContext>();
            commandContext.Setup(f => f.Messages).Returns(() => messagesIndicator ?? GetMockedMessagesIndicator());
            return commandContext.Object;
        }

        protected ICommandResolver GetMockedCommandResolver(Action<Mock<ICommandResolver>> setup)
        {
            var commandResolver = new Mock<ICommandResolver>();
            setup(commandResolver);
            return commandResolver.Object;
        }
    }
}
