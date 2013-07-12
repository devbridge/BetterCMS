namespace BetterCms.Module.Vimeo.Services.Models
{
    public class OAuth
    {
        public string Token { get; set; }
        public string Permission { get; set; }
        public User User { get; set; }
    }
}