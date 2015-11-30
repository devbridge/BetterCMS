// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerControlWidgetAccessor.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

using Common.Logging;

using RazorGenerator.Mvc;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class ServerControlWidgetAccessor : ContentAccessor<ServerControlWidget>
    {
        private static readonly ILog logger = LogManager.GetCurrentClassLogger();

        public const string ContentWrapperType = "server-widget";

        public ServerControlWidgetAccessor(ServerControlWidget content, IList<IOptionValue> options)
            : base(content, options)
        {
        }

        /// <summary>
        /// Gets the contents of the partial view.
        /// </summary>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="html">The HTML helper.</param>
        /// <returns>Contents of the partial view</returns>
        private string GetPartialViewContent(string partialViewName, HtmlHelper html)
        {
            try
            {
                ViewEngineResult partialView = ViewEngines.Engines.FindPartialView(html.ViewContext, partialViewName);
                IView view = partialView.View;

                if (view == null)
                {
                    return GetErrorString(partialViewName, "View not found");
                }

                using (var sw = new StringWriter())
                {
                    var viewData = new ViewDataDictionary();
                    var newViewContext = new ViewContext(html.ViewContext, view, viewData, html.ViewContext.TempData, sw);

                    try
                    {
                        var widgetModel = CreateWidgetViewModel(view);

                        widgetModel.Page = (IRenderPage)html.ViewData.Model;
                        widgetModel.Widget = Content;
                        widgetModel.Options = Options;

                        // Adding to ViewBag (there are old widgets, thay use this)
                        if (Options != null && Options.Count > 0)
                        {
                            foreach (var option in Options)
                            {
                                if (option.Value != null)
                                {
                                    viewData[option.Key] = option.Value;
                                }
                            }
                        }

                        newViewContext.ViewData.Model = widgetModel;
                        view.Render(newViewContext, sw);
                    }
                    catch (InvalidOperationException ex)
                    {
                        logger.Warn(
                            string.Format(
                                "Server widget \"{0}\" rendering failed. Check if widget view model is BetterCms.Module.Root.ViewModels.Cms.WigetViewModel", GetTitle()),
                            ex);
                        newViewContext.ViewData.Model = null;
                        view.Render(newViewContext, sw);
                    }

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception e)
            {
                return GetErrorString(partialViewName, e.Message);
            }
        }

        public override string GetContentWrapperType()
        {
            return ContentWrapperType;
        }

        public override string GetHtml(HtmlHelper html)
        {
            var text = GetPartialViewContent(Content.Url, html);

            if (!string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return "&nbsp;";
        }

        public override string[] GetCustomStyles(HtmlHelper html)
        {
            return null;
        }

        public override string[] GetCustomJavaScript(HtmlHelper html)
        {
            return null;
        }

        public override string[] GetStylesResources(HtmlHelper html)
        {
            if (Options != null)
            {
                return Options.ToStyleResources();
            }

            return null;
        }

        public override string[] GetJavaScriptResources(HtmlHelper html)
        {
            if (Options != null)
            {
                return Options.ToJavaScriptResources();
            }

            return null;
        }

        public override PropertiesPreview GetHtmlPropertiesPreview()
        {
            return new PropertiesPreview
            {
                ViewName = "~/Areas/bcms-pages/Views/History/ServerWidgetPropertiesHistory.cshtml",
                ViewModel = new ServerWidgetHistoryViewModel
                {
                    Name = Content.Name,
                    Url = Content.Url,
                    PreviewUrl = Content.PreviewUrl,
                    // TODO: Categories = Content.Categories
                }
            };
        }

        //
        // TODO: Error view ???
        //
        private static string GetErrorString(string view, string message)
        {
            return string.Format(@"<div class=""bcms-error"">Error rendering view ""{0}"": {1}</div>", view, message);
        }

        private RenderWidgetViewModel CreateWidgetViewModel(IView view)
        {
            Type compiledType = null;

            var razor = view as RazorView;
            if (razor != null)
            {
                compiledType = System.Web.Compilation.BuildManager.GetCompiledType(razor.ViewPath);
            }
            else
            {
                var procompiledView = view as PrecompiledMvcView;
                if (view is PrecompiledMvcView)
                {
                    var typeVariable = typeof(PrecompiledMvcView).GetField("_type", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (typeVariable != null)
                    {
                        compiledType = typeVariable.GetValue(procompiledView) as Type;
                    }
                    else
                    {
                        throw new CmsException("Failed to render server control widget. Failed to retrieve a Model type.");
                    }
                }
            }

            if (compiledType != null)
            {
                var baseType = compiledType.BaseType;
                if (baseType != null && baseType.IsGenericType)
                {
                    var generics = baseType.GetGenericArguments();
                    if (generics.Length > 0 && generics[0].IsSubclassOf(typeof(RenderWidgetViewModel)))
                    {
                        var model = Activator.CreateInstance(generics[0]) as RenderWidgetViewModel;

                        return model;
                    }
                }
            }

            return new RenderWidgetViewModel();
        }
    }
}