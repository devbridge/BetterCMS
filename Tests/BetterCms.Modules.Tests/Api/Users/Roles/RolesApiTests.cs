using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Users.Roles
{
    public class RolesApiTests : ApiCrudIntegrationTestBase<
        SaveRoleModel, RoleModel,
        PostRoleRequest, PostRoleResponse,
        GetRoleRequest, GetRoleResponse,
        PutRoleRequest, PutRoleResponse,
        DeleteRoleRequest, DeleteRoleResponse>
    {
        [Test]
        public void Should_CRUD_Role_Successfully()
        {
            // Attach to events
            Events.UserEvents.Instance.RoleCreated += Instance_EntityCreated;
            Events.UserEvents.Instance.RoleUpdated += Instance_EntityUpdated;
            Events.UserEvents.Instance.RoleDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Users.Roles.Post, api.Users.Role.Get, api.Users.Role.Put, api.Users.Role.Delete));

            // Detach from events
            Events.UserEvents.Instance.RoleCreated -= Instance_EntityCreated;
            Events.UserEvents.Instance.RoleUpdated -= Instance_EntityUpdated;
            Events.UserEvents.Instance.RoleDeleted -= Instance_EntityDeleted;
        }

        protected override SaveRoleModel GetCreateModel(ISession session)
        {
            return new SaveRoleModel
                   {
                       Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                       Description = TestDataProvider.ProvideRandomString(MaxLength.Name)
                   };
        }

        protected override GetRoleRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetRoleRequest { RoleId = saveResponseBase.Data.Value };
        }

        protected override PutRoleRequest GetUpdateRequest(GetRoleResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetRoleResponse getResponse, SaveRoleModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.Description);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
        }
    }
}
