using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Api.Operations.Users.Users.User;
using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Users.Api.Extensions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Users.User
{
    public class UserService : Service, IUserService
    {
        private readonly IRepository repository;
        
        private readonly IValidateUserService validateUserService;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        private readonly Services.IUserService userService;

        public UserService(IRepository repository, IValidateUserService validateUserService,
            IMediaFileUrlResolver fileUrlResolver, Services.IUserService userService)
        {
            this.repository = repository;
            this.validateUserService = validateUserService;
            this.fileUrlResolver = fileUrlResolver;
            this.userService = userService;
        }

        public GetUserResponse Get(GetUserRequest request)
        {
            var query = repository
                .AsQueryable<Models.User>();

            if (request.UserId.HasValue)
            {
                query = query.Where(user => user.Id == request.UserId);
            }
            else
            {
                query = query.Where(user => user.UserName == request.UserName);
            }
            
            var model = query
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
                        ImageId = user.Image != null && !user.Image.IsDeleted ? user.Image.Id : (System.Guid?)null,
                        ImageCaption = user.Image != null && !user.Image.IsDeleted ? user.Image.Caption : null,
                        ImageThumbnailUrl = user.Image != null && !user.Image.IsDeleted ? fileUrlResolver.EnsureFullPathUrl(user.Image.PublicThumbnailUrl) : null,
                        ImageUrl = user.Image != null && !user.Image.IsDeleted ? fileUrlResolver.EnsureFullPathUrl(user.Image.PublicUrl) : null
                    })
                .FirstOne();

            var response = new GetUserResponse { Data = model };

            if (request.Data.IncludeRoles)
            {
                response.Roles = repository
                    .AsQueryable<Models.UserRole>(userRole => userRole.User.Id == model.Id && !userRole.Role.IsDeleted)
                    .OrderBy(role => role.Role.Name)
                    .Select(role => new RoleModel
                        {
                            Id = role.Role.Id,
                            Version = role.Role.Version,
                            CreatedBy = role.Role.CreatedByUser,
                            CreatedOn = role.Role.CreatedOn,
                            LastModifiedBy = role.Role.ModifiedByUser,
                            LastModifiedOn = role.Role.ModifiedOn,

                            Name = role.Role.Name,
                            Description = role.Role.Description ?? string.Empty,
                            IsSystematic = role.Role.IsSystematic
                        })
                    .ToList();
            }

            return response;
        }

        public DeleteUserResponse Delete(DeleteUserRequest request)
        {
            userService.DeleteUser(request.Id, request.Data.Version);

            return new DeleteUserResponse { Data = true };
        }

        public PutUserResponse Put(PutUserRequest request)
        {
            var model = request.Data.ToServiceModel();
            if (request.Id.HasValue)
            {
                model.Id = request.Id.Value;
            }
            var user = userService.SaveUser(model, false, true);

            return new PutUserResponse { Data = user.Id };
        }

        ValidateUserResponse IUserService.Validate(ValidateUserRequest request)
        {
            return validateUserService.Get(request);
        }
    }
}