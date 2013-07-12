namespace BetterCms.Module.Vimeo.Services.Models
{
    internal class VimeoResponseBase
    {
        public string Generated_In { get; set; }
        public string Stat { get; set; }

        public bool IsOK()
        {
            return Stat.ToLower() == "ok";
        }
    }
}