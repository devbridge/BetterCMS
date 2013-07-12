using System.Text;

namespace BetterCms.Module.Vimeo.Services.Models.SearchVideo
{
    internal class SearchVideoRequest : VimeoRequestBase
    {
        private readonly string query;

        private readonly string userId;

        public SearchVideoRequest(string query, string userId, VimeoSorting sorting = VimeoSorting.Default, int page = 1, int perPage = 50)
            : base(sorting, page, perPage)
        {
            this.query = query;
            this.userId = userId;
        }

        protected override string MethodName
        {
            get
            {
                return "vimeo.videos.search";
            }
        }

        protected override void AppendParams(StringBuilder sb)
        {
            sb.AppendFormat("&query={0}", query);
            if (!string.IsNullOrEmpty(userId))
            {
                sb.AppendFormat("&user_id={0}", userId);
            }
            sb.AppendFormat("&full_response={0}", 1);
        }
    }
}