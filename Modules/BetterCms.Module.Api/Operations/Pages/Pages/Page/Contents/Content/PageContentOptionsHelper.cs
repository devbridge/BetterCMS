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
                .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                .FetchMany(f => f.Options)
                .ToList()
                .FirstOne();

            return optionService
                .GetMergedOptionValuesForEdit(pageContent.Content.ContentOptions, pageContent.Options)
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