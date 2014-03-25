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

                var user = req.GetParam("user");
                if (user != null)
                {
                    requestDto.User = ServiceStack.Text.JsonSerializer.DeserializeFromString(user, typeof(ApiIdentity)) as ApiIdentity;
                }
            }
        }

        private static bool IsJson(IHttpRequest req)
        {
            string contentType = req.ContentType;

            if (string.IsNullOrEmpty(contentType))
            {
                // Hack for phantomjs runner (it ignores a regularly provided contentType).
                contentType = req.Headers["X-Content-Type"];
            }
                
            if (!string.IsNullOrEmpty(contentType))
            {
                return contentType.Equals("application/json", StringComparison.OrdinalIgnoreCase) || contentType.StartsWith("application/json;", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}