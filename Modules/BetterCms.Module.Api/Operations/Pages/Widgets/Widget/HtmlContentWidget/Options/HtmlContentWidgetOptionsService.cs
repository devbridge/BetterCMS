using System.Linq;

using BetterCms.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Root.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options
{
    public class HtmlContentWidgetOptionsService : Service, IHtmlContentWidgetOptionsService
    {
        private readonly IRepository repository;

        public HtmlContentWidgetOptionsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetHtmlContentWidgetOptionsResponse Get(GetHtmlContentWidgetOptionsRequest request)
        {
            var results = repository
                .AsQueryable<ContentOption>(o => o.Content.Id == request.WidgetId)
                .Select(o => new OptionModel
                    {
                        Key = o.Key,
                        DefaultValue = o.DefaultValue,
                        Type = (OptionType)(int)o.Type
                    })
                .ToDataListResponse(request);

            return new GetHtmlContentWidgetOptionsResponse { Data = results };
        }
    }
}