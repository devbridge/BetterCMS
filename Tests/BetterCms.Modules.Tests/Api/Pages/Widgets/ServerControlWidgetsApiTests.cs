// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerControlWidgetsApiTests.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;
using BetterCms.Module.Api.Operations.Root;

using BetterCms.Module.MediaManager.Provider;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Widgets
{
    public class ServerControlWidgetsApiTests : ApiCrudIntegrationTestBase<
        SaveServerControlWidgetModel, ServerControlWidgetModel,
        PostServerControlWidgetRequest, PostServerControlWidgetResponse,
        GetServerControlWidgetRequest, GetServerControlWidgetResponse,
        PutServerControlWidgetRequest, PutServerControlWidgetResponse,
        DeleteServerControlWidgetRequest, DeleteServerControlWidgetResponse>
    {
        [Test]
        public void Should_CRUD_ServerControlWidget_Successfully()
        {
            // Attach to events
            Events.PageEvents.Instance.WidgetCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.WidgetUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.WidgetDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Pages.Widget.ServerControl.Post, api.Pages.Widget.ServerControl.Get, api.Pages.Widget.ServerControl.Put, api.Pages.Widget.ServerControl.Delete));

            // Detach from events
            Events.PageEvents.Instance.WidgetCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.WidgetUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.WidgetDeleted -= Instance_EntityDeleted;
        }

        protected override SaveServerControlWidgetModel GetCreateModel(ISession session)
        {
            var categoryTree = TestDataProvider.CreateNewCategoryTree();
            var category = TestDataProvider.CreateNewCategory(categoryTree);
            categoryTree.AvailableFor = new List<CategoryTreeCategorizableItem>
                    {
                        new CategoryTreeCategorizableItem
                        {
                            // See Migration201502101136.cs
                            CategorizableItem = session.Load<CategorizableItem>(new Guid("B2F05159-74AF-4B67-AEB9-36B9CC9EED57")),
                            CategoryTree = categoryTree
                        }
                    };
            session.SaveOrUpdate(categoryTree);
            session.SaveOrUpdate(category);
            session.Flush();

            var content = TestDataProvider.CreateNewServerControlWidget();

            session.SaveOrUpdate(content);
            session.Flush();

            var language1 = TestDataProvider.CreateNewLanguage();
            session.SaveOrUpdate(language1);

            var language2 = TestDataProvider.CreateNewLanguage();
            session.SaveOrUpdate(language2);

            return new SaveServerControlWidgetModel
                {
                    Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    WidgetUrl = content.Url,
                    PreviewUrl = content.PreviewUrl,
                    IsPublished = true,
                    PublishedOn = content.PublishedOn,
                    PublishedByUser = content.PublishedByUser,
                    Categories = new List<Guid>{category.Id},
                    Options = new List<OptionModel>
                              {
                                  new OptionModel
                                  {
                                      DefaultValue = "1",
                                      Key = "K1",
                                      Type = OptionType.Text,
                                      Translations = new List<OptionTranslationModel>
                                                {
                                                    new OptionTranslationModel
                                                    {
                                                        LanguageId = language1.Id.ToString(),
                                                        Value = "translated_lang1"
                                                    },
                                                    new OptionTranslationModel
                                                    {
                                                        LanguageId = language2.Id.ToString(),
                                                        Value = "translated_lang2"
                                                    }
                                                }
                                  },

                                  new OptionModel
                                  {
                                      DefaultValue = Guid.NewGuid().ToString(),
                                      Key = "K2",
                                      Type = OptionType.Custom,
                                      CustomTypeIdentifier = MediaManagerFolderOptionProvider.Identifier
                                  }
                              }
                };
        }

        protected override GetServerControlWidgetRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request = new GetServerControlWidgetRequest { WidgetId = saveResponseBase.Data.Value };
            request.Data.IncludeOptions = true;
            request.Data.IncludeCategories = true;
            return request;
        }

        protected override PutServerControlWidgetRequest GetUpdateRequest(GetServerControlWidgetResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetServerControlWidgetResponse getResponse, SaveServerControlWidgetModel model)
        {
            // Check if properties are not null
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.PublishedOn);
            Assert.IsNotNull(getResponse.Data.PublishedByUser);
            Assert.IsNotNull(getResponse.Categories);
            Assert.IsNotNull(getResponse.Data.WidgetUrl);
            Assert.IsNotNull(getResponse.Data.PreviewUrl);
            Assert.IsNotNull(getResponse.Options);
            Assert.IsNotEmpty(getResponse.Options);

            // Compare saving entity with retrieved after save entity
            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.IsPublished, model.IsPublished);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Data.PublishedByUser, model.PublishedByUser);

            foreach (var category in model.Categories)
            {
                Assert.IsTrue(getResponse.Categories.Any(c => c.Id == category));
            }
            
            Assert.AreEqual(getResponse.Data.PreviewUrl, model.PreviewUrl);
            Assert.AreEqual(getResponse.Data.WidgetUrl, model.WidgetUrl);
            
            Assert.AreEqual(getResponse.Options.Count, model.Options.Count);
            Assert.IsTrue(getResponse.Options.All(a1 => model.Options.Any(a2 => a1.Key == a2.Key
                   && a1.CustomTypeIdentifier == a2.CustomTypeIdentifier
                   && a1.DefaultValue == a2.DefaultValue
                   && a1.Type == a2.Type
                   && a1.Translations.All(t1 => a2.Translations.Any(t2 => t1.LanguageId == t2.LanguageId && t1.Value == t2.Value)))));
        }
    }
}
