// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModalJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    public class ModalJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public ModalJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.modal")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "save", () => RootGlobalization.Button_Save), 
                    new JavaScriptModuleGlobalization(this, "cancel", () => RootGlobalization.Button_Cancel), 
                    new JavaScriptModuleGlobalization(this, "ok", () => RootGlobalization.Button_Ok),
                    new JavaScriptModuleGlobalization(this, "saveDraft", () => RootGlobalization.Button_SaveDraft),
                    new JavaScriptModuleGlobalization(this, "saveAndPublish", () => RootGlobalization.Button_SaveAndPublish),
                    new JavaScriptModuleGlobalization(this, "preview", () => RootGlobalization.Button_Preview),
                    new JavaScriptModuleGlobalization(this, "yes", () => RootGlobalization.Button_Yes),
                    new JavaScriptModuleGlobalization(this, "no", () => RootGlobalization.Button_No),
                };
        }
    }
}