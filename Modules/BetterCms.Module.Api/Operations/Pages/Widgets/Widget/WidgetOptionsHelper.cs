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
                        CustomTypeIdentifier = o.CustomOption != null ? o.CustomOption.Identifier : null
                    });
        }
    }
}