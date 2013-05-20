using Autofac;

using BetterCms.Module.Root.Commands.GetProcessorJsData;
using BetterCms.Module.Root.Services;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.CommandTests
{
    [TestFixture]
    public class GetProcessorJsDataCommandTest : GetMainJsDataCommandTest
    {
        [Test]
        public void Should_Load_And_Return_Javascript_Modules_Successfully()
        {
            var command = new GetProcessorJsDataCommand(Container.Resolve<IRenderingService>());
            var model = command.Execute();

            CheckIfJavascriptModulesAreLoaded(model.JavaScriptModules);
        }
    }
}
