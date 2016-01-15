// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiCrudIntegrationTestBase.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
