using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Web.DynamicHtmlLayout;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class DynamicHtmlLayoutContentAccessor : ContentAccessor<DynamicHtmlLayoutContent>
    {
        public const string ContentWrapperType = "dynamic-html-layout-content";

        private readonly RenderPageViewModel childViewModel;

        public DynamicHtmlLayoutContentAccessor(DynamicHtmlLayoutContent content, IList<IOption> options, RenderPageViewModel childViewModel)
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
                childViewModel.Regions.Add(new PageRegionViewModel { RegionId = new Guid("D5B5CCE3-B0FA-4EB1-B08E-F99ED4C8C979"), RegionIdentifier = "DynamicHtmlSection" });

                var viewData = new ViewDataDictionary
                {
                    Model = childViewModel
                };

                DynamicHtmlLayoutContentsContainer.Push(Content.Id, Content.Html);

                // Create view
                var context = htmlHelper.ViewContext.Controller.ControllerContext;

                var masterVirtualPath = DynamicHtmlLayoutContentsContainer.CreateMasterVirtualPath(Content);

                var viewResult = ViewEngines.Engines.FindView(context, "~/Views/Shared/EmptyPage.cshtml", masterVirtualPath);
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
    /*
    public class MyViewEngine : VirtualPathProviderViewEngine
    {
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new MyView(controllerContext, partialPath, null, false, null);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache = false)
        {
            return new ViewEngineResult(CreateView(controllerContext, viewName, masterName), this);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new MyView(controllerContext, viewPath, null, false, null);
        }
    }

    public class MyView : RazorView
    {
        public MyView(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions)
            : base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions)
        {
        }

        public MyView(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions, IViewPageActivator viewPageActivator)
            : base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions, viewPageActivator)
        {
        }

        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            writer.WriteLine("HIIII");
            // base.RenderView(viewContext, writer, instance);
        }
    }*/
}