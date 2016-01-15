// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorsApiTests.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Api.Operations.Blog.Authors.Author;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Blog.Authors
{
    public class AuthorsApiTests : ApiCrudIntegrationTestBase<
        SaveAuthorModel, AuthorModel,
        PostAuthorRequest, PostAuthorResponse,
        GetAuthorRequest, GetAuthorResponse,
        PutAuthorRequest, PutAuthorResponse,
        DeleteAuthorRequest, DeleteAuthorResponse>
    {
        [Test]
        public void Should_CRUD_Author_Successfully()
        {
            // Attach to events
            Events.BlogEvents.Instance.AuthorCreated += Instance_EntityCreated;
            Events.BlogEvents.Instance.AuthorUpdated += Instance_EntityUpdated;
            Events.BlogEvents.Instance.AuthorDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Blog.Authors.Post, api.Blog.Author.Get, api.Blog.Author.Put, api.Blog.Author.Delete));

            // Detach from events
            Events.BlogEvents.Instance.AuthorCreated -= Instance_EntityCreated;
            Events.BlogEvents.Instance.AuthorUpdated -= Instance_EntityUpdated;
            Events.BlogEvents.Instance.AuthorDeleted -= Instance_EntityDeleted;
        }

        protected override SaveAuthorModel GetCreateModel(ISession session)
        {
            var image = TestDataProvider.CreateNewMediaImage();
            session.SaveOrUpdate(image);

            return new SaveAuthorModel
                {
                    Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    Description = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    ImageId = image.Id
                };
        }

        protected override GetAuthorRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetAuthorRequest { AuthorId = saveResponseBase.Data.Value };
        }

        protected override PutAuthorRequest GetUpdateRequest(GetAuthorResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);
            request.Data.Description = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetAuthorResponse getResponse, SaveAuthorModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.ImageId);
            Assert.IsNotNull(getResponse.Data.Description);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.ImageId, model.ImageId);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
        }
    }
}
