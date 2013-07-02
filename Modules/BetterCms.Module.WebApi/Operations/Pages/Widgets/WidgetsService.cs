using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    public class WidgetsService : Service, IWidgetsService
    {
        public GetWidgetsResponse Get(GetWidgetsRequest request)
        {
            // TODO: need implementation
            return new GetWidgetsResponse
                       {
                           Data =
                               new DataListResponse<WidgetModel>
                                   {
                                       TotalCount = 222,
                                       Items =
                                           new List<WidgetModel>
                                               {
                                                   new WidgetModel(),
                                                   new WidgetModel(),
                                                   new WidgetModel(),
                                               }
                                   }
                       };
        }
    }
}