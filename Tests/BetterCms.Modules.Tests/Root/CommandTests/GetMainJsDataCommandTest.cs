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
