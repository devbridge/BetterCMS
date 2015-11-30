// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiCrdIntegrationTestBase.cs" company="Devbridge Group LLC">
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
    public abstract class ApiCrdIntegrationTestBase<
        TSaveModel, TModel,
        TCreateRequest, TCreateResponse,
        TGetRequest, TGetResponse,
        TDeleteRequest, TDeleteResponse> : ApiIntegrationTestBase

        where TGetResponse : ResponseBase<TModel>, new()
        where TGetRequest : IReturn<TGetResponse>, new()

        where TSaveModel : SaveModelBase, new()
        where TModel : ModelBase, new()

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

        protected virtual void OnAfterGet(TGetResponse getResponse, TSaveModel saveModel)
        {
        }

        protected virtual void CheckCreateEvent()
        {
            CheckEventsCount(1, 0, 0);
        }

        protected virtual void CheckDeleteEvent()
        {
            CheckEventsCount(1, 0, 1);
        }

        protected void CheckEventsCount(int expectedCreatedCount, int expectedUpdatedCount, int expectedDeletedCount)
        {
            Assert.AreEqual(expectedCreatedCount, createdEventCount, "Created events fired count");
            Assert.AreEqual(expectedUpdatedCount, updatedEventCount, "Updated events fired count");
            Assert.AreEqual(expectedDeletedCount, deletedEventCount, "Deleted events fired count");
        }

        protected virtual void Run(
            ISession session,
            Func<TCreateRequest, TCreateResponse> createFunc,
            Func<TGetRequest, TGetResponse> getFunc,
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

            // Delete
            var deleteRequest = GetDeleteRequest(getResponse);
            DeleteResponse(deleteRequest, deleteFunc);
            CheckDeleteEvent();
        }

        protected void Instance_EntityDeleted<TEntity>(SingleItemEventArgs<TEntity> args)
        {
            deletedEventCount++;
        }

        protected void Instance_EntityCreated<TEntity>(SingleItemEventArgs<TEntity> args)
        {
            createdEventCount++;
        }
    }
}
