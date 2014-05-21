using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Models;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.MediaManager.Provider;

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
        [Test]
        public void Should_CRUD_HtmlContentWidget_Successfully()
        {
            // Attach to events
            Events.PageEvents.Instance.WidgetCreated += Instance_WidgetCreated;
            Events.PageEvents.Instance.WidgetUpdated += Instance_WidgetUpdated;
            Events.PageEvents.Instance.WidgetDeleted += Instance_WidgetDeleted;
            
            CheckEventsCount(0, 0, 0);

            // Run tests
            RunApiActionInTransaction((api, session) =>
                Run(session, api.Pages.Widget.HtmlContent.Post, api.Pages.Widget.HtmlContent.Get, api.Pages.Widget.HtmlContent.Put, api.Pages.Widget.HtmlContent.Delete));

            CheckEventsCount(1, 1, 1);

            // Detach from events
            Events.PageEvents.Instance.WidgetCreated -= Instance_WidgetCreated;
            Events.PageEvents.Instance.WidgetUpdated -= Instance_WidgetUpdated;
            Events.PageEvents.Instance.WidgetDeleted -= Instance_WidgetDeleted;
        }

        protected override SaveHtmlContentWidgetModel GetCreateModel(ISession session)
        {
            var content = TestDataProvider.CreateNewHtmlContentWidget();

            session.SaveOrUpdate(content.Category);

            return new SaveHtmlContentWidgetModel
                {
                    Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    IsPublished = true,
                    PublishedOn = content.PublishedOn,
                    PublishedByUser = content.PublishedByUser,
                    CategoryId = content.Category.Id,
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
                              }
                };
        }

        protected override GetHtmlContentWidgetRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request = new GetHtmlContentWidgetRequest { WidgetId = saveResponseBase.Data.Value };
            request.Data.IncludeOptions = true;

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
            Assert.IsNotNull(getResponse.Data.CategoryId);
            Assert.IsNotNull(getResponse.Data.CustomCss);
            Assert.IsNotNull(getResponse.Data.Html);
            Assert.IsNotNull(getResponse.Data.CustomJavaScript);
            Assert.IsNotNull(getResponse.Options);
            Assert.IsNotEmpty(getResponse.Options);

            // Compare saving entity with retrieved after save entity
            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.IsPublished, model.IsPublished);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Data.PublishedByUser, model.PublishedByUser);
            Assert.AreEqual(getResponse.Data.CategoryId, model.CategoryId);
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
        }

        private void Instance_WidgetDeleted(Events.SingleItemEventArgs<BetterCms.Module.Root.Models.Widget> args)
        {
            deletedEventCount++;
        }

        private void Instance_WidgetUpdated(Events.SingleItemEventArgs<BetterCms.Module.Root.Models.Widget> args)
        {
            updatedEventCount++;
        }

        private void Instance_WidgetCreated(Events.SingleItemEventArgs<BetterCms.Module.Root.Models.Widget> args)
        {
            createdEventCount++;
        }

        protected override void OnAfterCreate(PostHtmlContentWidgetRequest request, PostHtmlContentWidgetResponse response)
        {
            CheckEventsCount(1, 0, 0);
        }

        protected override void OnAfterUpdate(PutHtmlContentWidgetRequest request, PutHtmlContentWidgetResponse response)
        {
            CheckEventsCount(1, 1, 0);
        }
    }
}
