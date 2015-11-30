// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpContextExtensionMethods.cs" company="Devbridge Group LLC">
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
using System.IO;
using System.Web;

namespace BetterCms.Tests.Helpers
{
    public static class HttpContextExtensionMethods
    {
        public static HttpContextBase HttpContext(this HttpContextMoq moqHttpContext)
        {
            return moqHttpContext.HttpContextBase;
        }
        
        public static HttpContextBase RequestWrite(this HttpContextBase httpContextBase, string text)
        {
            httpContextBase.StreamWrite(httpContextBase.Request.InputStream, text);
            return httpContextBase;
        }

        public static string RequestRead(this HttpContextBase httpContextBase)
        {
            return httpContextBase.StreamRead(httpContextBase.Request.InputStream);
        }
        
        public static HttpContextBase ResponseWrite(this HttpContextBase httpContextBase, string text)
        {
            httpContextBase.StreamWrite(httpContextBase.Response.OutputStream, text);
            return httpContextBase;
        }

        public static string ResponseRead(this HttpContextBase httpContextBase)
        {
            return httpContextBase.StreamRead(httpContextBase.Response.OutputStream);
        }
        
        public static HttpContextBase StreamWrite(this HttpContextBase httpContextBase, Stream inputStream, string text)
        {
            using (var streamWriter = new StreamWriter(inputStream))
            {
                inputStream.Position = inputStream.Length;
                streamWriter.Write(text);
                streamWriter.Flush();
                inputStream.Position = 0;
            }
            return httpContextBase;
        }

        public static string StreamRead(this HttpContextBase httpContextBase, Stream inputStream)
        {
            var originalPosition = inputStream.Position;
            using (var streamReader = new StreamReader(inputStream))
            {
                var requestData = streamReader.ReadToEnd();
                inputStream.Position = originalPosition;
                return requestData;
            }
        }
    }
}