namespace BetterCms.Module.Root.Services
{
    public class EmptyUserProfileUrlResolver : IUserProfileUrlResolver
    {
        public string GetEditUserProfileUrl()
        {
            return null;
        }
    }
}