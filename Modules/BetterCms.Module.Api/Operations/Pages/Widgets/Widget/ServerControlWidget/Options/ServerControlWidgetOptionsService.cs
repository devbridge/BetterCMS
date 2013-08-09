using System;

using BetterCms.Core.DataAccess;

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
            throw new NotImplementedException("TODO: implement service");
        }
    }
}