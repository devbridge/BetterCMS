// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetRequestProcessor.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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