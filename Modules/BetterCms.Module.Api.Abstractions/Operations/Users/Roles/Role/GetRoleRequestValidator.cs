using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    public class GetRoleRequestValidator : AbstractValidator<GetRoleRequest>
    {
        public GetRoleRequestValidator()
        {
            RuleFor(request => request.RoleId).Must(RoleNameMustBeNullIfRoleIdProvided).WithMessage("A RoleName field must be null if RoleId is provided.");
            RuleFor(request => request.RoleName).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A RoleId or RoleName should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetRoleRequest getRoleRequest, string roleName)
        {
            return getRoleRequest.RoleId != null || !string.IsNullOrEmpty(getRoleRequest.RoleName);
        }

        private bool RoleNameMustBeNullIfRoleIdProvided(GetRoleRequest getRoleRequest, System.Guid? roleId)
        {
            return roleId != null && string.IsNullOrEmpty(getRoleRequest.RoleName) ||
                   roleId == null && !string.IsNullOrEmpty(getRoleRequest.RoleName);
        }
    }
}
