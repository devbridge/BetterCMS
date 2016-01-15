// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectsApiTests.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Redirects
{
    public class RedirectsApiTests : ApiCrudIntegrationTestBase<
        SaveRedirectModel, RedirectModel,
        PostRedirectRequest, PostRedirectResponse,
        GetRedirectRequest, GetRedirectResponse,
        PutRedirectRequest, PutRedirectResponse,
        DeleteRedirectRequest, DeleteRedirectResponse>
    {
        [Test]
        public void Should_CRUD_Redirect_Successfully()
        {
            // Attach to events
            Events.PageEvents.Instance.RedirectCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.RedirectUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.RedirectDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) => 
                Run(session, api.Pages.Redirects.Post, api.Pages.Redirect.Get, api.Pages.Redirect.Put, api.Pages.Redirect.Delete));

            // Detach
            Events.PageEvents.Instance.RedirectCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.RedirectUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.RedirectDeleted -= Instance_EntityDeleted;
        }

        protected override SaveRedirectModel GetCreateModel(ISession session)
        {
            return new SaveRedirectModel
                {
                    PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name)),
                    RedirectUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name))
                };
        }

        protected override GetRedirectRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetRedirectRequest { RedirectId = saveResponseBase.Data.Value };
        }

        protected override PutRedirectRequest GetUpdateRequest(GetRedirectResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(MaxLength.Name));

            return request;
        }

        protected override void OnAfterGet(GetRedirectResponse getResponse, SaveRedirectModel model)
        {
            Assert.IsNotNull(getResponse.Data.RedirectUrl);
            Assert.IsNotNull(getResponse.Data.PageUrl);

            Assert.AreEqual(getResponse.Data.RedirectUrl, model.RedirectUrl);
            Assert.AreEqual(getResponse.Data.PageUrl, model.PageUrl);
        }
    }
}
