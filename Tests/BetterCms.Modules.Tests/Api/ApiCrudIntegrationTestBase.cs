using System;

using BetterCms.Module.Api.Infrastructure;

using NHibernate;

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
        protected abstract TSaveModel GetCreateModel(ISession session);

        protected TCreateRequest GetCreateRequest(TSaveModel model)
        {
            return new TCreateRequest { Data = model };
        }

        protected abstract TGetRequest GetGetRequest(SaveResponseBase saveResponseBase);

        protected abstract TUpdateRequest GetUpdateRequest(TGetResponse getResponse);

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

            // Get
            var getRequest = GetGetRequest(createResponse);
            var getResponse = GetResponse<TGetRequest, TGetResponse, TModel>(getRequest, getFunc);
            OnAfterCreateGet(getResponse, createRequest.Data);

            // Update
            var updateRequest = GetUpdateRequest(getResponse);
            var updateResponse = UpdateResponse<TUpdateRequest, TUpdateResponse, TSaveModel>(updateRequest, updateFunc);

            // Get
            getRequest = GetGetRequest(updateResponse);
            getResponse = GetResponse<TGetRequest, TGetResponse, TModel>(getRequest, getFunc);
            OnAfterUpdateGet(getResponse, updateRequest.Data);

            // Delete
            DeleteResponse(getResponse.Data, deleteFunc);
        }
    }
}
