using System.Text;

namespace BetterCms.Module.Vimeo.Services.Models.CheckAccessToken
{
    internal class CheckAccessTokenRequest : VimeoRequestBase
    {
        protected override string MethodName
        {
            get
            {
                return "vimeo.oauth.checkAccessToken";
            }
        }

        protected override void AppendParams(StringBuilder sb)
        {
            // No parameters are needed.
        }
    }
}