using BetterCms.Module.Api.Operations;
using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Filters
{
    public static class CmsApiRequestFilter
    {
        public static void BeforeRequestProcessed(IHttpRequest req, IHttpResponse res, object dto)
        {
            if (dto == null)
            {
                return;
            }

            var requestDto = dto as IRequest;
            if (requestDto != null && req.GetHttpMethodOverride() == "GET")
            {
                var data = req.GetParam("data");
                 
                if (data != null)
                {
                    var requestModelType = dto.GetType().BaseType.GetGenericArguments()[0];
                    requestDto.Data = ServiceStack.Text.JsonSerializer.DeserializeFromString(data, requestModelType);                    
                }
            }

            /*
            var request = dto as ListRequestBase;
            if (request != null)
            {
                var filterJsv = req.GetParam("filter");
                var orderJsv = req.GetParam("order");

                if (!string.IsNullOrWhiteSpace(filterJsv) || !string.IsNullOrWhiteSpace(orderJsv))
                {
                    if (!string.IsNullOrWhiteSpace(filterJsv))
                    {
                        request.Filter = ServiceStack.Text.JsonSerializer.DeserializeFromString<DataFilter>(filterJsv);
                    }

                    if (!string.IsNullOrWhiteSpace(orderJsv))
                    {
                        request.Order = ServiceStack.Text.JsonSerializer.DeserializeFromString<DataOrder>(orderJsv);
                    }
                }
            }
             * */
        }
    }
}