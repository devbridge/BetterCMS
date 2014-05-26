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
        
        protected virtual void CheckCreateEvent()
        {
            CheckEventsCount(1, 0, 0);
        }

        protected virtual void CheckUpdateEvent()
        {
            CheckEventsCount(1, 1, 0);
        }

        protected virtual void CheckDeleteEvent()
        {
            CheckEventsCount(1, 1, 1);
        }

        protected void Run(
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

        protected void CheckEventsCount(int createdCount, int updatedCount, int deletedCount)
        {
            Assert.AreEqual(createdEventCount, createdCount, "Created events fired count");
            Assert.AreEqual(updatedEventCount, updatedCount, "Updated events fired count");
            Assert.AreEqual(deletedEventCount, deletedCount, "Deleted events fired count");
        }

        protected void Instance_EntityDeleted<TEntity>(Events.SingleItemEventArgs<TEntity> args)
        {
            deletedEventCount++;
        }

        protected void Instance_EntityUpdated<TEntity>(Events.SingleItemEventArgs<TEntity> args)
        {
            updatedEventCount++;
        }

        protected void Instance_EntityCreated<TEntity>(Events.SingleItemEventArgs<TEntity> args)
        {
            createdEventCount++;
        }
    }
}
