using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Root.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options
{
    public class ServerControlWidgetOptionsService : Service, IServerControlWidgetOptionsService
    {
        private readonly IRepository repository;

        public ServerControlWidgetOptionsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetServerControlWidgetOptionsResponse Get(GetServerControlWidgetOptionsRequest request)
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

            return new GetServerControlWidgetOptionsResponse { Data = results };
        }
    }
}