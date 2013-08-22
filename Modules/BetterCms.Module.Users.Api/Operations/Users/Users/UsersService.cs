using System.Linq;

using BetterCms.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Users.Users;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Users
{
    public class UsersService : Service, IUsersService
    {
        private readonly IRepository repository;

        public UsersService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetUsersResponse Get(GetUsersRequest request)
        {
            request.Data.SetDefaultOrder("UserName");

            var listResponse = repository
                .AsQueryable<Models.User>()
                .Select(user => new UserModel
                    {
                        Id = user.Id,
                        Version = user.Version,
                        CreatedBy = user.CreatedByUser,
                        CreatedOn = user.CreatedOn,
                        LastModifiedBy = user.ModifiedByUser,
                        LastModifiedOn = user.ModifiedOn,

                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        ImageId = user.Image != null ? user.Image.Id : (System.Guid?) null,
                        ImageCaption = user.Image != null ? user.Image.Caption : null,
                        ImageThumbnailUrl = user.Image != null ? user.Image.PublicThumbnailUrl : null,
                        ImageUrl = user.Image != null ? user.Image.PublicUrl : null
                    })
                .ToDataListResponse(request);

            return new GetUsersResponse
                       {
                           Data = listResponse
                       };
        }
    }
}