// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStyleSheetsToRenderCommandTest.cs" company="Devbridge Group LLC">
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

            var css = model.StyleSheetFiles.FirstOrDefault(ss => ss.EndsWith(RootMinCss, System.StringComparison.Ordinal));
            Assert.IsNotNullOrEmpty(css);

            css = model.StyleSheetFiles.FirstOrDefault(ss => ss.EndsWith(PagesMinCss, System.StringComparison.Ordinal));
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

            var css = model.StyleSheetFiles.FirstOrDefault(ss => ss.EndsWith(RootMinCss, System.StringComparison.Ordinal));
            Assert.IsNotNullOrEmpty(css);

            css = model.StyleSheetFiles.FirstOrDefault(ss => ss.EndsWith(PagesMinCss, System.StringComparison.Ordinal));
            Assert.IsNull(css);
        }
    }
}
