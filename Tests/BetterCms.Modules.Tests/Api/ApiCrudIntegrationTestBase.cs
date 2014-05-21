using System;

using BetterCms.Module.Api.Infrastructure;

using NHibernate;

using NUnit.Framework;

using ServiceStack.ServiceHost;

namespace BetterCms.Test.Module.Api
{
    public abstract class ApiCrudIntegrationTestBase<
        TSaveModel, TModel,
        TCreateRequest, TCreateResponse, 
        TGetRequest, TGetResponse, 
        TUpdateRequest, TUpdateResponse,
        TDeleteRequest, TDeleteResponse> : ApiIntegrationTestBase
        
        where TGetResponse : ResponseBase<TModel>, new()
        where TGetRequest : IReturn<TGetResponse>, new()

        where TSaveModel : SaveModelBase, new()
        where TModel : ModelBase, new()

        where TUpdateRequest : PutRequestBase<TSaveModel>, new()
        where TUpdateResponse : SaveResponseBase, new()
        
        where TCreateRequest : RequestBase<TSaveModel>, new()
        where TCreateResponse : SaveResponseBase, new()

        where TDeleteRequest : DeleteRequestBase, new()
        where TDeleteResponse : DeleteResponseBase
    {
        protected int createdEventCount;
        protected int updatedEventCount;
        protected int deletedEventCount;

        protected abstract TSaveModel GetCreateModel(ISession session);

        protected virtual TCreateRequest GetCreateRequest(TSaveModel model)
        {
            return new TCreateRequest { Data = model };
        }

        protected abstract TGetRequest GetGetRequest(SaveResponseBase saveResponseBase);

        protected abstract TUpdateRequest GetUpdateRequest(TGetResponse getResponse);

        protected virtual TDeleteRequest GetDeleteRequest(TGetResponse getResponse)
        {
            var request = new TDeleteRequest
                              {
                                  Id = getResponse.Data.Id,
                                  Data = { Version = getResponse.Data.Version }
                              };
            return request;
        }

        protected virtual void OnAfterCreateGet(TGetResponse getResponse, TSaveModel saveModel)
        {
            OnAfterGet(getResponse, saveModel);
        }

        protected virtual void OnAfterUpdateGet(TGetResponse getResponse, TSaveModel saveModel)
        {
            OnAfterGet(getResponse, saveModel);
        }
        
        protected virtual void OnAfterGet(TGetResponse getResponse, TSaveModel saveModel)
        {
        }
        
        protected virtual void OnAfterCreate(TCreateRequest request, TCreateResponse response)
        {
        }
        
        protected virtual void OnAfterUpdate(TUpdateRequest request, TUpdateResponse response)
        {
        }
        
        protected virtual void OnAfterDelete(TDeleteRequest request, TDeleteResponse response)
        {
        }

        protected void Run(
            ISession session,
            Func<TCreateRequest, TCreateResponse> createFunc,
            Func<TGetRequest, TGetResponse> getFunc,
            Func<TUpdateRequest, TUpdateResponse> updateFunc,
            Func<TDeleteRequest, TDeleteResponse> deleteFunc)
        {
            // Create
            var createModel = GetCreateModel(session);
            var createRequest = GetCreateRequest(createModel);
            var createResponse = CreateResponse<TCreateRequest, TCreateResponse, TSaveModel>(createRequest, createFunc);
            OnAfterCreate(createRequest, createResponse);

            // Get
            var getRequest = GetGetRequest(createResponse);
            var getResponse = GetResponse<TGetRequest, TGetResponse, TModel>(getRequest, getFunc);
            OnAfterCreateGet(getResponse, createRequest.Data);

            // Update
            var updateRequest = GetUpdateRequest(getResponse);
            var updateResponse = UpdateResponse<TUpdateRequest, TUpdateResponse, TSaveModel>(updateRequest, updateFunc);
            OnAfterUpdate(updateRequest, updateResponse);

            // Get
            getRequest = GetGetRequest(updateResponse);
            getResponse = GetResponse<TGetRequest, TGetResponse, TModel>(getRequest, getFunc);
            OnAfterUpdateGet(getResponse, updateRequest.Data);

            // Delete
            var deleteRequest = GetDeleteRequest(getResponse);
            var deleteResponse =DeleteResponse(deleteRequest, deleteFunc);
            OnAfterDelete(deleteRequest, deleteResponse);
        }

        protected void CheckEventsCount(int createdCount, int updatedCount, int deletedCount)
        {
            Assert.AreEqual(createdEventCount, createdCount, "Created events fired count");
            Assert.AreEqual(updatedEventCount, updatedCount, "Updated events fired count");
            Assert.AreEqual(deletedEventCount, deletedCount, "Deleted events fired count");
        }
    }
}
