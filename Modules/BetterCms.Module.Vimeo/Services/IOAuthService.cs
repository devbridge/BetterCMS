namespace BetterCms.Module.Vimeo.Services
{
    internal interface IOAuthService
    {
        string GetAuthorizationProperty(string url);
    }
}