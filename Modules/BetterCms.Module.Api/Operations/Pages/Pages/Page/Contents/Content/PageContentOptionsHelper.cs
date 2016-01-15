// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageContentOptionsHelper.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    public static class PageContentOptionsHelper
    {
        public static List<OptionValueModel> GetPageContentOptionsList(IRepository repository, Guid pageContentId, IOptionService optionService)
        {
            return GetPageContentOptionsQuery(repository, pageContentId, optionService).ToList();
        }

        public static DataListResponse<OptionValueModel> GetPageContentOptionsResponse(IRepository repository, Guid pageContentId, RequestBase<DataOptions> request, IOptionService optionService)
        {
            return GetPageContentOptionsQuery(repository, pageContentId, optionService).ToDataListResponse(request);
        }

        private static IQueryable<OptionValueModel> GetPageContentOptionsQuery(IRepository repository, Guid pageContentId, IOptionService optionService)
        {
            var pageContent = repository
                .AsQueryable<PageContent>()
                .Where(f => f.Id == pageContentId && !f.IsDeleted && !f.Content.IsDeleted)
                .Fetch(f => f.Page).ThenFetch(f => f.Language)
                .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                .FetchMany(f => f.Options)
                .ToList()
                .FirstOne();

            var langId = pageContent.Page.Language != null ? pageContent.Page.Language.Id.ToString() : "";
            var mergedOptionValues = optionService.GetMergedOptionValuesForEdit(pageContent.Content.ContentOptions, pageContent.Options);

            foreach (var optionValue in mergedOptionValues)
            {
                if (optionValue.Translations != null)
                {
                    var translation = optionValue.Translations.FirstOrDefault(x => x.LanguageId == langId);
                    if (translation != null)
                    {
                        optionValue.OptionValue = optionValue.UseDefaultValue ? translation.OptionValue : optionValue.OptionValue;
                        optionValue.OptionDefaultValue = translation.OptionValue;
                    }
                }
            }

            return mergedOptionValues
                    .Select(o => new OptionValueModel
                        {
                            Key = o.OptionKey,
                            Value = o.OptionValue,
                            DefaultValue = o.OptionDefaultValue,
                            Type = ((Root.OptionType)(int)o.Type),
                            UseDefaultValue = o.UseDefaultValue,
                            CustomTypeIdentifier = o.CustomOption != null ? o.CustomOption.Identifier : null

                        }).AsQueryable();
        }
    }
}