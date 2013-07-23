using BetterCms.Module.Api.Infrastructure;
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
        }
    }
}