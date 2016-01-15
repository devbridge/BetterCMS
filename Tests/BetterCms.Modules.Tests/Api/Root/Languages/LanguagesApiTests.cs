// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguagesApiTests.cs" company="Devbridge Group LLC">
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
using System.Globalization;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Root.Languages;
using BetterCms.Module.Api.Operations.Root.Languages.Language;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Root.Languages
{
    public class LanguagesApiTests : ApiCrudIntegrationTestBase<
        SaveLanguageModel, LanguageModel,
        PostLanguageRequest, PostLanguageResponse,
        GetLanguageRequest, GetLanguageResponse,
        PutLanguageRequest, PutLanguageResponse,
        DeleteLanguageRequest, DeleteLanguageResponse>
    {
        [Test]
        public void Should_CRUD_Language_Successfully()
        {
            // Attach to events
            Events.RootEvents.Instance.LanguageCreated += Instance_EntityCreated;
            Events.RootEvents.Instance.LanguageUpdated += Instance_EntityUpdated;
            Events.RootEvents.Instance.LanguageDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Root.Languages.Post, api.Root.Language.Get, api.Root.Language.Put, api.Root.Language.Delete));

            // Detach from events
            Events.RootEvents.Instance.LanguageCreated -= Instance_EntityCreated;
            Events.RootEvents.Instance.LanguageUpdated -= Instance_EntityUpdated;
            Events.RootEvents.Instance.LanguageDeleted -= Instance_EntityDeleted;
        }

        protected override SaveLanguageModel GetCreateModel(ISession session)
        {
            var repository = GetRepository(session);
            var createdCodes = repository.AsQueryable<Language>().Select(l => l.Code).ToList();
            var availableCode = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(c => createdCodes.All(cc => cc != c.Name));

            Assert.IsNotNull(availableCode, "All available languages are created in the database, cannot continue language creation test");

            return new SaveLanguageModel
                   {
                       Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                       Code = availableCode.Name
                   };
        }

        protected override GetLanguageRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetLanguageRequest { LanguageId = saveResponseBase.Data.Value };
        }

        protected override PutLanguageRequest GetUpdateRequest(GetLanguageResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetLanguageResponse getResponse, SaveLanguageModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.Code);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.Code, model.Code);
        }
    }
}
