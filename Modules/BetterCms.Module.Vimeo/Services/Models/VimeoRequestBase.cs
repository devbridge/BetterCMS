using System.Text;

namespace BetterCms.Module.Vimeo.Services.Models
{
    internal abstract class VimeoRequestBase
    {
            private const string UrlBase = "http://vimeo.com/api/rest/v2";

            private readonly VimeoSorting sorting;

            private readonly int page;

            private readonly int perPage;

            protected abstract string MethodName { get; }

            protected VimeoRequestBase(VimeoSorting sorting = VimeoSorting.Default, int page = 1, int perPage = 50)
            {
                this.sorting = sorting;
                this.page = page;
                this.perPage = perPage;
            }

            protected abstract void AppendParams(StringBuilder sb);

            private string GetUrlParams()
            {
                var sb = new StringBuilder();

                switch (sorting)
                {
                    case VimeoSorting.Newest:
                        sb.AppendFormat("&sort={0}", "newest");
                        break;
                    case VimeoSorting.Oldest:
                        sb.AppendFormat("&sort={0}", "oldest");
                        break;
                    case VimeoSorting.MostPlayed:
                        sb.AppendFormat("&sort={0}", "most_played");
                        break;
                    case VimeoSorting.MostCommented:
                        sb.AppendFormat("&sort={0}", "most_commented");
                        break;
                    case VimeoSorting.MostLiked:
                        sb.AppendFormat("&sort={0}", "most_liked");
                        break;
                }
                if (page > 1)
                {
                    sb.AppendFormat("&page={0}", page);
                }
                if (perPage > 0 && perPage < 50)
                {
                    sb.AppendFormat("&per_page={0}", perPage);
                }

                AppendParams(sb);

                return sb.ToString();
            }

            public string GetUrl()
            {
                var sb = new StringBuilder();
                sb.Append(UrlBase);
                sb.AppendFormat("?format={0}", "json");
                sb.AppendFormat("&method={0}", MethodName);
                sb.Append(GetUrlParams());
                return sb.ToString();
            }
    }
}