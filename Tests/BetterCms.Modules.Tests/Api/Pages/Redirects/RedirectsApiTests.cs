using BetterCms.Core.Models;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;
using BetterCms.Module.Root.Mvc;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Redirects
{
    public class RedirectsApiTests : ApiIntegrationTestBase
    {
        [Test]
        public void Should_CRUD_Redirect_Successfully()
        {
            RunApiActionInTransaction(
                (api, session) =>
                {
                    // Create
                    var createRequest = new PostRedirectRequest();
                    createRequest.Data.PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name));
                    createRequest.Data.RedirectUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name));

                    var createResponse = api.Pages.Redirects.Post(createRequest);
                    Assert.IsNotNull(createResponse);
                    Assert.IsNotNull(createResponse.Data);
                    Assert.IsFalse(createResponse.Data.Value.HasDefaultValue());

                    // Get
                    var getRequest = new GetRedirectRequest();
                    getRequest.RedirectId = createResponse.Data.Value;

                    var getResponse = api.Pages.Redirect.Get(getRequest);
                    Assert.IsNotNull(getResponse);
                    Assert.IsNotNull(getResponse.Data);
                    Assert.AreEqual(getResponse.Data.RedirectUrl, createRequest.Data.RedirectUrl);
                    Assert.AreEqual(getResponse.Data.PageUrl, createRequest.Data.PageUrl);

                    // Update
                    var updateRequest = getResponse.ToPutRequest();
                    updateRequest.Data.PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name));
                    var updateResponse = api.Pages.Redirect.Put(updateRequest);
                    Assert.IsNotNull(updateResponse);
                    Assert.IsNotNull(updateResponse.Data);
                    Assert.AreEqual(updateResponse.Data, createResponse.Data);
                    
                    // Get
                    getRequest = new GetRedirectRequest();
                    getRequest.RedirectId = createResponse.Data.Value;

                    getResponse = api.Pages.Redirect.Get(getRequest);
                    Assert.IsNotNull(getResponse);
                    Assert.IsNotNull(getResponse.Data);
                    Assert.AreEqual(getResponse.Data.RedirectUrl, updateRequest.Data.RedirectUrl);
                    Assert.AreNotEqual(getResponse.Data.PageUrl, createRequest.Data.PageUrl);
                    Assert.AreEqual(getResponse.Data.PageUrl, updateRequest.Data.PageUrl);

                    // Delete
                    var deleteRequest = new DeleteRedirectRequest();
                    deleteRequest.RedirectId = getResponse.Data.Id;
                    deleteRequest.Data.Version = getResponse.Data.Version;

                    var deleteResponse = api.Pages.Redirect.Delete(deleteRequest);
                    Assert.IsNotNull(deleteResponse);
                    Assert.IsTrue(deleteResponse.Data);
                });
        }
    }
}
