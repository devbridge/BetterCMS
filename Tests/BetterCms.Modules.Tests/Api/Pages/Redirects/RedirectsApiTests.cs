using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Redirects
{
    public class RedirectsApiTests : ApiCrudIntegrationTestBase<
        SaveRedirectModel, RedirectModel,
        PostRedirectRequest, PostRedirectResponse,
        GetRedirectRequest, GetRedirectResponse,
        PutRedirectRequest, PutRedirectResponse,
        DeleteRedirectRequest, DeleteRedirectResponse>
    {
        [Test]
        public void Should_CRUD_Redirect_Successfully()
        {
            // Attach to events
            Events.PageEvents.Instance.RedirectCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.RedirectUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.RedirectDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) => 
                Run(session, api.Pages.Redirects.Post, api.Pages.Redirect.Get, api.Pages.Redirect.Put, api.Pages.Redirect.Delete));

            // Detach
            Events.PageEvents.Instance.RedirectCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.RedirectUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.RedirectDeleted -= Instance_EntityDeleted;
        }

        protected override SaveRedirectModel GetCreateModel(ISession session)
        {
            return new SaveRedirectModel
                {
                    PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name)),
                    RedirectUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name))
                };
        }

        protected override GetRedirectRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetRedirectRequest { RedirectId = saveResponseBase.Data.Value };
        }

        protected override PutRedirectRequest GetUpdateRequest(GetRedirectResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name));

            return request;
        }

        protected override void OnAfterGet(GetRedirectResponse getResponse, SaveRedirectModel model)
        {
            Assert.IsNotNull(getResponse.Data.RedirectUrl);
            Assert.IsNotNull(getResponse.Data.PageUrl);

            Assert.AreEqual(getResponse.Data.RedirectUrl, model.RedirectUrl);
            Assert.AreEqual(getResponse.Data.PageUrl, model.PageUrl);
        }
    }
}
