using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Root;

using BetterCms.Module.MediaManager.Provider;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Widgets
{
    public class HtmlContentWidgetsApiTests : ApiCrudIntegrationTestBase<
        SaveHtmlContentWidgetModel, HtmlContentWidgetModel,
        PostHtmlContentWidgetRequest, PostHtmlContentWidgetResponse,
        GetHtmlContentWidgetRequest, GetHtmlContentWidgetResponse,
        PutHtmlContentWidgetRequest, PutHtmlContentWidgetResponse,
        DeleteHtmlContentWidgetRequest, DeleteHtmlContentWidgetResponse>
    {

        private Category category;

        [Test]
        public void Should_CRUD_HtmlContentWidget_Successfully()
        {
            // Attach to events
            Events.PageEvents.Instance.WidgetCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.WidgetUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.WidgetDeleted += Instance_EntityDeleted;

            // Run tests
            RunApiActionInTransaction(
                (api, session) => Run(
                    session,
                    api.Pages.Widget.HtmlContent.Post,
                    api.Pages.Widget.HtmlContent.Get,
                    api.Pages.Widget.HtmlContent.Put,
                    api.Pages.Widget.HtmlContent.Delete));

            // Detach from events
            Events.PageEvents.Instance.WidgetCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.WidgetUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.WidgetDeleted -= Instance_EntityDeleted;
        }
        
        [Test]
        public void Should_CRUD_HtmlContentWidget_Successfully_WithIdSpecified()
        {
            // Attach to events
            Events.PageEvents.Instance.WidgetCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.WidgetUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.WidgetDeleted += Instance_EntityDeleted;

            // Run tests
            RunApiActionInTransaction(
                (api, session) => RunWithIdSpecified(session, api.Pages.Widget.HtmlContent.Get, api.Pages.Widget.HtmlContent.Put, api.Pages.Widget.HtmlContent.Delete));

            // Detach from events
            Events.PageEvents.Instance.WidgetCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.WidgetUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.WidgetDeleted -= Instance_EntityDeleted;
        }

        protected override SaveHtmlContentWidgetModel GetCreateModel(ISession session)
        {
            var categoryTree = TestDataProvider.CreateNewCategoryTree();
            category = TestDataProvider.CreateNewCategory(categoryTree);
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

            var widget = TestDataProvider.CreateNewHtmlContentWidget();
            session.SaveOrUpdate(widget);

            var content = TestDataProvider.CreateNewHtmlContentWidget();

            var assignmentId1 = Guid.NewGuid();
            var assignmentId2 = Guid.NewGuid();
            content.Html = string.Format("{0}{1}{3}{2}",
                TestDataProvider.ProvideRandomString(50),
                TestDataProvider.CreateChildWidgetAssignment(widget.Id, assignmentId1),
                TestDataProvider.ProvideRandomString(50),
                TestDataProvider.CreateChildWidgetAssignment(widget.Id, assignmentId2));

            session.SaveOrUpdate(content);
            session.Flush();
            return new SaveHtmlContentWidgetModel
                {
                    Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    IsPublished = true,
                    PublishedOn = content.PublishedOn,
                    PublishedByUser = content.PublishedByUser,
                    Categories = new List<Guid>(){category.Id},
                    CustomCss = content.CustomCss,
                    UseCustomCss = true,
                    Html = content.Html,
                    UseHtml = true,
                    CustomJavaScript = content.CustomJs,
                    UseCustomJavaScript = true,
                    Options = new List<OptionModel>
                              {
                                  new OptionModel
                                  {
                                      DefaultValue = "1",
                                      Key = "K1",
                                      Type = OptionType.Text
                                  },

                                  new OptionModel
                                  {
                                      DefaultValue = Guid.NewGuid().ToString(),
                                      Key = "K2",
                                      Type = OptionType.Custom,
                                      CustomTypeIdentifier = MediaManagerFolderOptionProvider.Identifier
                                  }
                              },
                    ChildContentsOptionValues = new List<ChildContentOptionValuesModel>
                                {
                                    new ChildContentOptionValuesModel
                                    {
                                        AssignmentIdentifier = assignmentId1,
                                        OptionValues = new List<OptionValueModel>
                                        {
                                            new OptionValueModel
                                            {
                                                Key = "O1",
                                                Value = "V1",
                                                UseDefaultValue = false,
                                                Type = OptionType.Text
                                            }
                                        }
                                    },
                                    new ChildContentOptionValuesModel
                                    {
                                        AssignmentIdentifier = assignmentId2,
                                        OptionValues = new List<OptionValueModel>
                                        {
                                            new OptionValueModel
                                            {
                                                Key = "O2",
                                                Value = Guid.NewGuid().ToString(),
                                                UseDefaultValue = false,
                                                Type = OptionType.Custom,
                                                CustomTypeIdentifier = "media-images-folder"
                                            },
                                            new OptionValueModel
                                            {
                                                Key = "O3",
                                                Value = Guid.NewGuid().ToString(),
                                                UseDefaultValue = true,
                                                Type = OptionType.Text
                                            }
                                        }
                                    }
                                } 
                };
        }

        protected override GetHtmlContentWidgetRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request = new GetHtmlContentWidgetRequest { WidgetId = saveResponseBase.Data.Value };
            request.Data.IncludeOptions = true;
            request.Data.IncludeChildContentsOptions = true;
            request.Data.IncludeCategories = true;

            return request;
        }

        protected override PutHtmlContentWidgetRequest GetUpdateRequest(GetHtmlContentWidgetResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);
             
            return request;
        }

        protected override void OnAfterGet(GetHtmlContentWidgetResponse getResponse, SaveHtmlContentWidgetModel model)
        {
            // Check if properties are not null
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.PublishedOn);
            Assert.IsNotNull(getResponse.Data.PublishedByUser);
            Assert.IsNotNull(getResponse.Categories);
            Assert.IsNotNull(getResponse.Data.CustomCss);
            Assert.IsNotNull(getResponse.Data.Html);
            Assert.IsNotNull(getResponse.Data.CustomJavaScript);
            Assert.IsNotNull(getResponse.Options);
            Assert.IsNotEmpty(getResponse.Options);
            Assert.IsNotNull(getResponse.ChildContentsOptionValues);
            Assert.IsNotEmpty(getResponse.ChildContentsOptionValues);

            // Compare saving entity with retrieved after save entity
            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.IsPublished, model.IsPublished);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Data.PublishedByUser, model.PublishedByUser);
            foreach (var category in model.Categories)
            {
                Assert.IsTrue(getResponse.Categories.Any(c => c.Id == category));
            }
            Assert.AreEqual(getResponse.Data.CustomCss, model.CustomCss);
            Assert.AreEqual(getResponse.Data.UseCustomCss, model.UseCustomCss);
            Assert.AreEqual(getResponse.Data.Html, model.Html);
            Assert.AreEqual(getResponse.Data.UseHtml, model.UseHtml);
            Assert.AreEqual(getResponse.Data.CustomJavaScript, model.CustomJavaScript);
            Assert.AreEqual(getResponse.Data.UseCustomJavaScript, model.UseCustomJavaScript);

            Assert.AreEqual(getResponse.Options.Count, model.Options.Count);
            Assert.IsTrue(getResponse.Options.All(a1 => model.Options.Any(a2 => a1.Key == a2.Key
                   && a1.CustomTypeIdentifier == a2.CustomTypeIdentifier
                   && a1.DefaultValue == a2.DefaultValue
                   && a1.Type == a2.Type)));

            Assert.AreEqual(getResponse.ChildContentsOptionValues.Count, model.ChildContentsOptionValues.Count);
            model.ChildContentsOptionValues.ToList().ForEach(
                o =>
                {
                    var o1 = getResponse.ChildContentsOptionValues.FirstOrDefault(c => c.AssignmentIdentifier == o.AssignmentIdentifier);
                    Assert.IsNotNull(o1);
                    Assert.IsNotNull(o1.OptionValues);
                    Assert.AreEqual(o1.OptionValues.Count(c => !c.UseDefaultValue), o.OptionValues.Count(c => !c.UseDefaultValue));
                    Assert.IsTrue(o.OptionValues
                        .Where(c => !c.UseDefaultValue)
                        .All(c => o1.OptionValues.All(c1 => c1.Key == c.Key 
                            && c.Value == c1.Value 
                            && c.CustomTypeIdentifier == c1.CustomTypeIdentifier
                            && c.Type == c1.Type)));
                });
        }
    }
}
