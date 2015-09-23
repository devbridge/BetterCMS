using System.Linq;

using Autofac;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Commands.GetStyleSheetsToRender;
using BetterCms.Module.Root.Services;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.CommandTests
{
    [TestFixture]
    public class GetStyleSheetsToRenderCommandTest : TestBase
    {
        private const string RootMinCss = "bcms.root.min.css";
        private const string PagesMinCss = "bcms.pages.min.css";

        [Test]
        public void Should_Return_AllModules_StyleSheets_Successfully()
        {
            var request = new GetStyleSheetsToRenderRequest
                              {
                                  RenderPrivateCssIncludes = true,
                                  RenderPublicCssIncludes = true,
                                  ModuleDescriptorType = null
                              };

            var command = new GetStyleSheetsToRenderCommand(Container.Resolve<IRenderingService>());
            var model = command.Execute(request);

            var css = model.StyleSheetFiles.FirstOrDefault(ss => ss.EndsWith(RootMinCss));
            Assert.IsNotNullOrEmpty(css);

            css = model.StyleSheetFiles.FirstOrDefault(ss => ss.EndsWith(PagesMinCss));
            Assert.IsNotNullOrEmpty(css);
        }

        [Test]
        public void Should_Return_RootModule_StyleSheets_Successfully()
        {
            var request = new GetStyleSheetsToRenderRequest
            {
                RenderPrivateCssIncludes = true,
                RenderPublicCssIncludes = true,
                ModuleDescriptorType = typeof(RootModuleDescriptor)
            };

            var command = new GetStyleSheetsToRenderCommand(Container.Resolve<IRenderingService>());
            var model = command.Execute(request);

            var css = model.StyleSheetFiles.FirstOrDefault(ss => ss.EndsWith(RootMinCss));
            Assert.IsNotNullOrEmpty(css);

            css = model.StyleSheetFiles.FirstOrDefault(ss => ss.EndsWith(PagesMinCss));
            Assert.IsNull(css);
        }
    }
}
