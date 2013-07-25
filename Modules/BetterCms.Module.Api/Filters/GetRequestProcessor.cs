using System;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Filters
{
    public static class GetRequestProcessor
    {
        public static void DeserializeJsonFromGet(IHttpRequest req, IHttpResponse res, object dto)
        {
            if (dto == null)
            {
                return;
            }

            var requestDto = dto as IRequest;
            if (requestDto != null && req.GetHttpMethodOverride() == "GET" &&  IsJson(req))
            {
                var data = req.GetParam("data");

                if (data != null)
                {
                    var requestModelType = dto.GetType().BaseType.GetGenericArguments()[0];
                    requestDto.Data = ServiceStack.Text.JsonSerializer.DeserializeFromString(data, requestModelType);
                }
            }
        }

        private static bool IsJson(IHttpRequest req)
        {
            if (!string.IsNullOrEmpty(req.ContentType))
            {
                return req.ContentType.Equals("application/json", StringComparison.OrdinalIgnoreCase) || req.ContentType.StartsWith("application/json;", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}