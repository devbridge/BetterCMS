using BetterCms.Module.Api.Operations.Users.Users.User;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Api.Extensions
{
    public static class UserModelExtensions
    {
        public static EditUserViewModel ToServiceModel(this SaveUserModel model)
        {
            var serviceModel = new EditUserViewModel
                    {
                        Version = model.Version,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        Image = { ImageId = model.ImageId },
                        Password = model.Password,
                        Roles = model.Roles
                    };

            return serviceModel;
        }
    }
}