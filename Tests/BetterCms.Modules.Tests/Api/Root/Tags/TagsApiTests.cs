// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagsApiTests.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Root.Tags
{
    public class TagsApiTests : ApiCrudIntegrationTestBase<
        SaveTagModel, TagModel,
        PostTagRequest, PostTagResponse,
        GetTagRequest, GetTagResponse,
        PutTagRequest, PutTagResponse,
        DeleteTagRequest, DeleteTagResponse>
    {
        [Test]
        public void Should_CRUD_Tag_Successfully()
        {
            // Attach to events
            Events.RootEvents.Instance.TagCreated += Instance_EntityCreated;
            Events.RootEvents.Instance.TagUpdated += Instance_EntityUpdated;
            Events.RootEvents.Instance.TagDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Root.Tags.Post, api.Root.Tag.Get, api.Root.Tag.Put, api.Root.Tag.Delete));

            // Detach from events
            Events.RootEvents.Instance.TagCreated -= Instance_EntityCreated;
            Events.RootEvents.Instance.TagUpdated -= Instance_EntityUpdated;
            Events.RootEvents.Instance.TagDeleted -= Instance_EntityDeleted;
        }

        protected override SaveTagModel GetCreateModel(ISession session)
        {
            return new SaveTagModel
                   {
                       Name = TestDataProvider.ProvideRandomString(MaxLength.Name)
                   };
        }

        protected override GetTagRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetTagRequest { TagId = saveResponseBase.Data.Value };
        }

        protected override PutTagRequest GetUpdateRequest(GetTagResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetTagResponse getResponse, SaveTagModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
        }
    }
}
