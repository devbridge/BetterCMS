namespace BetterCms.Module.Vimeo.Services.Models
{
    internal class OAuth
    {
        public string Token { get; set; }
        public string Permission { get; set; }
        public User User { get; set; }
    }
}