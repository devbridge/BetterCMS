using System.Text;

namespace BetterCms.Module.Vimeo.Services.Models.GetVideo
{
    internal class GetVideoRequest : VimeoRequestBase
    {
        private readonly string videoId;

        public GetVideoRequest(string videoId)
        {
            this.videoId = videoId;
        }

        protected override string MethodName
        {
            get
            {
                return "vimeo.videos.getInfo";
            }
        }

        protected override void AppendParams(StringBuilder sb)
        {
            sb.AppendFormat("&video_id={0}", videoId);
        }
    }
}