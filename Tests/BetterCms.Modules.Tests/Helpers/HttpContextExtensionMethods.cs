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