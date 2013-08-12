using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Root.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options
{
    public class LayoutOptionsService : Service, ILayoutOptionsService
    {
        private readonly IRepository repository;

        public LayoutOptionsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetLayoutOptionsResponse Get(GetLayoutOptionsRequest request)
        {
            var results = repository
                .AsQueryable<LayoutOption>(o => o.Layout.Id == request.LayoutId)
                .Select(o => new OptionModel
                    {
                        Key = o.Key,
                        DefaultValue = o.DefaultValue,
                        Type = (OptionType)(int)o.Type
                    })
                .ToDataListResponse(request);

            return new GetLayoutOptionsResponse { Data = results };
        }
    }
}