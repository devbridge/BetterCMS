using Devbridge.Platform.Core.Web.Environment.Application;

namespace Devbridge.Platform.Core.Web
{
    public static class WebApplication
    {
        public static void Initialize()
        {
            WebApplicationEntryPoint.PreApplicationStart();
        }
    }
}