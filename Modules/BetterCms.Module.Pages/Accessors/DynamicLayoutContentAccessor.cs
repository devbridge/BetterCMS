using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class DynamicLayoutContentAccessor : ContentAccessor<DynamicLayoutContent>
    {
        public const string ContentWrapperType = "dynamic-layout-content";

        private readonly RenderPageViewModel childViewModel;

        public DynamicLayoutContentAccessor(DynamicLayoutContent content, IList<IOption> options, RenderPageViewModel childViewModel)
            : base(content, options)
        {
            this.childViewModel = childViewModel;
        }

        public override string GetContentWrapperType()
        {
            return ContentWrapperType;
        }

        public override string GetHtml(HtmlHelper htmlHelper)
        {
            using (var sw = new StringWriter())
            {
                var viewData = new ViewDataDictionary
                {
                    Model = childViewModel
                };

                // Create view
                var context = htmlHelper.ViewContext.Controller.ControllerContext;

                var viewResult = ViewEngines.Engines.FindView(context, "~/Views/Shared/EmptyPage.cshtml", Content.Layout.LayoutPath);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(htmlHelper.ViewContext.Controller.ControllerContext, viewResult.View);

                var html = sw.GetStringBuilder().ToString();
                return html;
            }
        }

        public override string GetCustomStyles(HtmlHelper html)
        {
            return null;
        }

        public override string GetCustomJavaScript(HtmlHelper html)
        {
            return null;
        }
    }
}