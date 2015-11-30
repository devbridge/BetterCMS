// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetOptionsHelper.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget
{
    public static class WidgetOptionsHelper
    {
        public static IList<OptionModel> GetWidgetOptionsList(IRepository repository, Guid widgetId)
        {
            return GetWidgetOptionsQuery(repository, widgetId).ToList();
        }

        public static DataListResponse<OptionModel> GetWidgetOptionsResponse(IRepository repository, Guid widgetId, RequestBase<DataOptions> request)
        {
            return GetWidgetOptionsQuery(repository, widgetId).ToDataListResponse(request);
        }

        private static IQueryable<OptionModel> GetWidgetOptionsQuery(IRepository repository, Guid widgetId)
        {
            return repository
                .AsQueryable<ContentOption>(o => o.Content.Id == widgetId)
                .Select(o => new OptionModel
                    {
                        Key = o.Key,
                        DefaultValue = o.DefaultValue,
                        Type = (OptionType)(int)o.Type,
                        CustomTypeIdentifier = o.CustomOption != null ? o.CustomOption.Identifier : null,
                        Translations = o.Translations.Select(x => new OptionTranslationModel
                        {
                            LanguageId = x.Language.Id.ToString(),
                            Value = x.Value
                        }).ToList()
                    });
        }
    }
}