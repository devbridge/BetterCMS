namespace BetterCms.Module.Users.Services
{
    interface IRegistrationService
    {
        bool IsFirstUserRegistered();

        void NavigateToRegisterFirstUserPage();
    }
}
