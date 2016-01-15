// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetMainJsDataCommandTest.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Module.Root.Commands.GetMainJsData;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Rendering;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.CommandTests
{
    [TestFixture]
    public class GetMainJsDataCommandTest : TestBase
    {
        [Test]
        public void Should_Load_Javascript_Modules_Successfully()
        {
            var command = new GetMainJsDataCommand(Container.Resolve<IRenderingService>(), Container.Resolve<ICmsConfiguration>());
            var model = command.Execute();

            CheckIfJavascriptModulesAreLoaded(model.JavaScriptModules);
        }

        protected void CheckIfJavascriptModulesAreLoaded(IEnumerable<JavaScriptModuleInclude> jsModules)
        {
            // Check if main js modules are loaded
            var jsFiles = new[]
                                     {
                                         // Root
                                         "bcms.modal", 
                                         "bcms", 
                                         "bcms.content", 
                                         "bcms.forms", 

                                         // Pages
                                         "bcms.pages", 
                                         "bcms.pages.content",
                                     };

            foreach (var jsFile in jsFiles)
            {
                var jsModel = jsModules.FirstOrDefault(js => js.Name == jsFile);

                Assert.IsNotNull(jsModel, "A {0} JS module is not found.", jsFile);
            }
        }
    }
}
