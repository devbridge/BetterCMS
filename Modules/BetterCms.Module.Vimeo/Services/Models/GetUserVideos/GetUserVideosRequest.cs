using System.Text;

namespace BetterCms.Module.Vimeo.Services.Models.GetUserVideos
{
    internal class GetUserVideosRequest : VimeoRequestBase
    {
        private readonly string userId;

        public GetUserVideosRequest(string userId, VimeoSorting sorting = VimeoSorting.Default, int page = 1, int perPage = 50)
            : base(sorting, page, perPage)
        {
            this.userId = userId;
        }

        protected override string MethodName
        {
            get
            {
                return "vimeo.videos.getAll";
            }
        }

        protected override void AppendParams(StringBuilder sb)
        {
            sb.AppendFormat("&user_id={0}", userId);
            sb.AppendFormat("&full_response={0}", 1);
        }
    }
}