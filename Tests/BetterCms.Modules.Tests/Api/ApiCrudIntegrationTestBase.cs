using System;

using BetterCms.Module.Api.Infrastructure;

using BetterModules.Events;

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
        TDeleteRequest, TDeleteResponse> : 
        
        ApiCrdIntegrationTestBase<TSaveModel, TModel,
            TCreateRequest, TCreateResponse,
            TGetRequest, TGetResponse,
            TDeleteRequest, TDeleteResponse>
        
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
        protected virtual TUpdateRequest GetCreateRequestWithId(TSaveModel model)
        {
            return new TUpdateRequest { Data = model, Id = Guid.NewGuid() };
        }

        protected abstract TUpdateRequest GetUpdateRequest(TGetResponse getResponse);

        protected virtual void OnAfterUpdateGet(TGetResponse getResponse, TSaveModel saveModel)
        {
            OnAfterGet(getResponse, saveModel);
        }

        protected virtual void CheckUpdateEvent()
        {
            CheckEventsCount(1, 1, 0);
        }

        protected override void CheckDeleteEvent()
        {
            CheckEventsCount(1, 1, 1);
        }

        protected void Instance_EntityUpdated<TEntity>(SingleItemEventArgs<TEntity> args)
        {
            updatedEventCount++;
        }

        protected void RunWithIdSpecified(
            ISession session,
            Func<TGetRequest, TGetResponse> getFunc,
            Func<TUpdateRequest, TUpdateResponse> updateFunc,
            Func<TDeleteRequest, TDeleteResponse> deleteFunc)
        {
            createdEventCount = 0;
            deletedEventCount = 0;
            updatedEventCount = 0;

            // Create
            var createModel = GetCreateModel(session);
            var createRequest = GetCreateRequestWithId(createModel);
            var createResponse = UpdateResponse<TUpdateRequest, TUpdateResponse, TSaveModel>(createRequest, updateFunc);
            //session.Flush();
            session.Clear();
            CheckCreateEvent();

            // Get
            var getRequest = GetGetRequest(createResponse);
            var getResponse = GetResponse<TGetRequest, TGetResponse, TModel>(getRequest, getFunc);
            Assert.AreEqual(createRequest.Id, getResponse.Data.Id);
            OnAfterCreateGet(getResponse, createRequest.Data);

            // Update
            var updateRequest = GetUpdateRequest(getResponse);
            var updateResponse = UpdateResponse<TUpdateRequest, TUpdateResponse, TSaveModel>(updateRequest, updateFunc);
            //session.Flush();
            session.Clear();
            CheckUpdateEvent();

            // Get
            getRequest = GetGetRequest(updateResponse);
            getResponse = GetResponse<TGetRequest, TGetResponse, TModel>(getRequest, getFunc);
            OnAfterUpdateGet(getResponse, updateRequest.Data);

            // Delete
            var deleteRequest = GetDeleteRequest(getResponse);
            DeleteResponse(deleteRequest, deleteFunc);
            CheckDeleteEvent();
        }

        protected virtual void Run(
            ISession session,
            Func<TCreateRequest, TCreateResponse> createFunc,
            Func<TGetRequest, TGetResponse> getFunc,
            Func<TUpdateRequest, TUpdateResponse> updateFunc,
            Func<TDeleteRequest, TDeleteResponse> deleteFunc)
        {
            createdEventCount = 0;
            deletedEventCount = 0;
            updatedEventCount = 0;

            // Create
            var createModel = GetCreateModel(session);
            var createRequest = GetCreateRequest(createModel);
            var createResponse = CreateResponse<TCreateRequest, TCreateResponse, TSaveModel>(createRequest, createFunc);
            //session.Flush();
            session.Clear();
            CheckCreateEvent();

            // Get
            var getRequest = GetGetRequest(createResponse);
            var getResponse = GetResponse<TGetRequest, TGetResponse, TModel>(getRequest, getFunc);
            OnAfterCreateGet(getResponse, createRequest.Data);

            // Update
            var updateRequest = GetUpdateRequest(getResponse);
            var updateResponse = UpdateResponse<TUpdateRequest, TUpdateResponse, TSaveModel>(updateRequest, updateFunc);
            //session.Flush();
            session.Clear();
            CheckUpdateEvent();

            // Get
            getRequest = GetGetRequest(updateResponse);
            getResponse = GetResponse<TGetRequest, TGetResponse, TModel>(getRequest, getFunc);
            OnAfterUpdateGet(getResponse, updateRequest.Data);

            // Delete
            var deleteRequest = GetDeleteRequest(getResponse);
            DeleteResponse(deleteRequest, deleteFunc);
            CheckDeleteEvent();
        }
    }
}
