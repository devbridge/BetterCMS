// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutModelExtensions.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Operations.Root.Layouts.Layout;

using BetterCms.Module.Pages.ViewModels.Templates;

namespace BetterCms.Module.Api.Extensions
{
    public static class LayoutModelExtensions
    {
        public static TemplateEditViewModel ToServiceModel(this SaveLayoutModel model)
        {
            var serviceModel = new TemplateEditViewModel();

            serviceModel.Version = model.Version;
            serviceModel.Name = model.Name;
            serviceModel.Url = model.LayoutPath;
            serviceModel.PreviewImageUrl = model.PreviewUrl;

            if (model.Options != null)
            {
                serviceModel.Options = model.Options.ToServiceModel();
            }

            if (model.Regions != null)
            {
                serviceModel.Regions = model
                    .Regions
                    .Select(r => new TemplateRegionItemViewModel
                                 {
                                     Description = r.Description,
                                     Identifier = r.RegionIdentifier
                                 })
                    .ToList();
            }

            return serviceModel;
        }
    }
}