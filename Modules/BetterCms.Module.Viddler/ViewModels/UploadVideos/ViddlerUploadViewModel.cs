namespace BetterCms.Module.Viddler.ViewModels.UploadVideos
{
    public class ViddlerUploadViewModel
    {
        public string SessionId { get; set; }

        public string Token { get; set; }

        public string Endpoint { get; set; }

        public string CallbackUrl { get; set; }
    }
}