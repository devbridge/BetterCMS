using BetterCms.Core.Models;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Redirects
{
    public class RedirectsApiTests : ApiIntegrationTestBase
    {
        [Test]
        public void Should_CRUD_Redirect_Successfully()
        {
            RunApiActionInTransaction((api, session) =>
                {
                    // Create
                    var createModel = CreateInitialModel();
                    var createResponse = CreateResponse<PostRedirectRequest, 
                        PostRedirectResponse, 
                        SaveRedirectModel>(createModel, api.Pages.Redirects.Post);

                    // Get
                    var getRequest = new GetRedirectRequest { RedirectId = createResponse.Data.Value };
                    var getResponse = GetResponse<GetRedirectRequest, 
                        GetRedirectResponse, 
                        RedirectModel>(getRequest, api.Pages.Redirect.Get);

                    CompareModels(getResponse.Data, createModel);

                    // Update
                    var updateRequest = getResponse.ToPutRequest();
                    updateRequest.Data.PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name));
                    var updateResponse = UpdateResponse<PutRedirectRequest,
                        PutRedirectResponse,
                        SaveRedirectModel>(updateRequest, api.Pages.Redirect.Put);
                    
                    // Get
                    getRequest = new GetRedirectRequest { RedirectId = updateResponse.Data.Value };
                    getResponse = GetResponse<GetRedirectRequest,
                        GetRedirectResponse,
                        RedirectModel>(getRequest, api.Pages.Redirect.Get);

                    CompareModels(getResponse.Data, updateRequest.Data);
                    Assert.AreNotEqual(getResponse.Data.PageUrl, createModel.PageUrl);

                    // Delete
                    DeleteResponse<DeleteRedirectRequest, 
                        DeleteRedirectResponse>(getResponse.Data, api.Pages.Redirect.Delete);
                });
        }

        private SaveRedirectModel CreateInitialModel()
        {
            return new SaveRedirectModel
            {
                PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name)),
                RedirectUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name))
            };
        }

        private void CompareModels(RedirectModel getModel, SaveRedirectModel saveModel)
        {
            Assert.AreEqual(getModel.RedirectUrl, saveModel.RedirectUrl);
            Assert.AreEqual(getModel.PageUrl, saveModel.PageUrl);
        }
    }
}
