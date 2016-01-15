// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplatesJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.Registration
{
    public class TemplatesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetsJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public TemplatesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.template")
        {

            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadSiteSettingsTemplateListUrl", controller => controller.Templates(null)),
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadRegisterTemplateDialogUrl", controller => controller.RegisterTemplate()),
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadEditTemplateDialogUrl", controller => controller.EditTemplate("{0}")),
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "deleteTemplateUrl", controller => controller.DeleteTemplate("{0}", "{1}")),
                            new JavaScriptModuleLinkTo<ContentController>(this, "loadTemplateRegionDialogUrl", controller => controller.PageContentOptions("{0}"))
                        };

            Globalization = new IActionProjection[]
                                {
                                    new JavaScriptModuleGlobalization(this, "createTemplateDialogTitle", () => PagesGlobalization.CreatTemplate_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "editTemplateDialogTitle", () => PagesGlobalization.EditTemplate_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "editTemplateRegionTitle", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                    new JavaScriptModuleGlobalization(this, "deleteTemplateConfirmMessage", () => PagesGlobalization.SiteSettings_Template_DeleteCategoryMessage),
                                    new JavaScriptModuleGlobalization(this, "deleteRegionConfirmMessage", () => PagesGlobalization.DeleteRegion_Confirmation_Message),
                                    new JavaScriptModuleGlobalization(this, "previewImageNotFoundMessage", () => PagesGlobalization.EditTemplate_PreviewImageNotFound_Message),
                                    new JavaScriptModuleGlobalization(this, "deletingMessage", () => RootGlobalization.Message_Deleting),
                                    new JavaScriptModuleGlobalization(this, "templatesTabTitle", () => PagesGlobalization.SiteSettings_Templates_Title)
                                };
        }
    }
}