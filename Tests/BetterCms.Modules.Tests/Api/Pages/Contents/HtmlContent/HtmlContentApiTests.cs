using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;
using BetterCms.Module.Api.Operations.Root;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

using ApiContentTextMode = BetterCms.Module.Api.Operations.Pages.ContentTextMode;

namespace BetterCms.Test.Module.Api.Pages.Contents.HtmlContent
{
    public class HtmlContentApiTests : ApiCrudIntegrationTestBase<
        SaveHtmlContentModel, HtmlContentModel,
        PostHtmlContentRequest, PostHtmlContentResponse,
        GetHtmlContentRequest, GetHtmlContentResponse,
        PutHtmlContentRequest, PutHtmlContentResponse,
        DeleteHtmlContentRequest, DeleteHtmlContentResponse>
    {
        private ApiContentTextMode contentTextMode;

        private const string MarkdownOriginalText = "Testing **Bold**";
        private const string MarkdownHtml = "<p>Testing <strong>Bold</strong></p>";

        private const string SimpleOriginalText = "<div>Testing <b>Simple Text</b></div>";
        private const string SimpleTextHtml = "&lt;div&gt;Testing &lt;b&gt;Simple Text&lt;/b&gt;&lt;/div&gt;";

        [Test]
        public void Should_CRUD_HtmlContent_Successfully()
        {
            contentTextMode = ContentTextMode.Html;

            // Attach to events
            Events.PageEvents.Instance.HtmlContentCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.HtmlContentUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.HtmlContentDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Pages.Content.Html.Post, api.Pages.Content.Html.Get, api.Pages.Content.Html.Put, api.Pages.Content.Html.Delete));

            // Detach from events
            Events.PageEvents.Instance.HtmlContentCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.HtmlContentUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.HtmlContentDeleted -= Instance_EntityDeleted;
        }
        
        [Test]
        public void Should_CRUD_HtmlContent_Successfully_WithIdSpecified()
        {
            contentTextMode = ContentTextMode.Html;

            // Attach to events
            Events.PageEvents.Instance.HtmlContentCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.HtmlContentUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.HtmlContentDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                RunWithIdSpecified(session, api.Pages.Content.Html.Get, api.Pages.Content.Html.Put, api.Pages.Content.Html.Delete));

            // Detach from events
            Events.PageEvents.Instance.HtmlContentCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.HtmlContentUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.HtmlContentDeleted -= Instance_EntityDeleted;
        }

        [Test]
        public void Should_CRUD_MarkdownContent_Successfully()
        {
            contentTextMode = ContentTextMode.Markdown;

            // Attach to events
            Events.PageEvents.Instance.HtmlContentCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.HtmlContentUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.HtmlContentDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Pages.Content.Html.Post, api.Pages.Content.Html.Get, api.Pages.Content.Html.Put, api.Pages.Content.Html.Delete));

            // Detach from events
            Events.PageEvents.Instance.HtmlContentCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.HtmlContentUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.HtmlContentDeleted -= Instance_EntityDeleted;
        }
        
        [Test]
        public void Should_CRUD_TextContent_Successfully()
        {
            contentTextMode = ContentTextMode.SimpleText;

            // Attach to events
            Events.PageEvents.Instance.HtmlContentCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.HtmlContentUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.HtmlContentDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Pages.Content.Html.Post, api.Pages.Content.Html.Get, api.Pages.Content.Html.Put, api.Pages.Content.Html.Delete));

            // Detach from events
            Events.PageEvents.Instance.HtmlContentCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.HtmlContentUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.HtmlContentDeleted -= Instance_EntityDeleted;
        }

        protected override SaveHtmlContentModel GetCreateModel(ISession session)
        {
            var widget = TestDataProvider.CreateNewHtmlContentWidget();
            session.SaveOrUpdate(widget);

            var content = TestDataProvider.CreateNewHtmlContent();

            var assignmentId1 = Guid.NewGuid();
            var assignmentId2 = Guid.NewGuid();
            content.Html = string.Format("{0}{1}{3}{2}",
                TestDataProvider.ProvideRandomString(50),
                TestDataProvider.CreateChildWidgetAssignment(widget.Id, assignmentId1),
                TestDataProvider.ProvideRandomString(50),
                TestDataProvider.CreateChildWidgetAssignment(widget.Id, assignmentId2));

            var html = content.Html;
            string originalText = "Just a test";

            if (contentTextMode == ContentTextMode.Markdown)
            {
                html = null;
                originalText = MarkdownOriginalText;
            }
            
            if (contentTextMode == ContentTextMode.SimpleText)
            {
                html = null;
                originalText = SimpleOriginalText;
            }

            return new SaveHtmlContentModel
                   {
                       Name = content.Name,
                       ActivationDate = content.ActivationDate,
                       ExpirationDate = content.ExpirationDate,
                       Html = html,
                       OriginalText = originalText,
                       ContentTextMode = contentTextMode,
                       CustomCss = content.CustomCss,
                       UseCustomCss = true,
                       CustomJavaScript = content.CustomJs,
                       UseCustomJavaScript = true,
                       IsPublished = true,
                       PublishedOn = content.PublishedOn,
                       PublishedByUser = content.PublishedByUser,
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

        protected override GetHtmlContentRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request = new GetHtmlContentRequest { ContentId = saveResponseBase.Data.Value };
            request.Data.IncludeChildContentsOptions = true;

            return request;
        }

        protected override PutHtmlContentRequest GetUpdateRequest(GetHtmlContentResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetHtmlContentResponse getResponse, SaveHtmlContentModel model)
        {
            var html = model.Html;
            string originalText = model.OriginalText;

            if (contentTextMode == ContentTextMode.Markdown)
            {
                html = MarkdownHtml;
                originalText = MarkdownOriginalText;
            }

            if (contentTextMode == ContentTextMode.SimpleText)
            {
                html = SimpleTextHtml;
                originalText = SimpleOriginalText;
            }

            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.ActivationDate);
            Assert.IsNotNull(getResponse.Data.ExpirationDate);
            Assert.IsNotNull(getResponse.Data.Html);
            Assert.IsNotNull(getResponse.Data.CustomCss);
            Assert.IsNotNull(getResponse.Data.CustomJavaScript);
            Assert.IsNotNull(getResponse.Data.PublishedOn);
            Assert.IsNotNull(getResponse.Data.PublishedByUser);
            Assert.IsNotNull(getResponse.ChildContentsOptionValues);

            if (contentTextMode != ContentTextMode.Markdown && contentTextMode != ContentTextMode.SimpleText)
            {
                Assert.IsNotEmpty(getResponse.ChildContentsOptionValues);
            }

            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.ActivationDate, model.ActivationDate);
            Assert.AreEqual(getResponse.Data.ExpirationDate, model.ExpirationDate);
            Assert.AreEqual(getResponse.Data.Html.Trim(), html);
            Assert.AreEqual(getResponse.Data.OriginalText, originalText);
            Assert.AreEqual(getResponse.Data.ContentTextMode, model.ContentTextMode);
            Assert.AreEqual(getResponse.Data.CustomCss, model.CustomCss);
            Assert.AreEqual(getResponse.Data.UseCustomCss, model.UseCustomCss);
            Assert.AreEqual(getResponse.Data.CustomJavaScript, model.CustomJavaScript);
            Assert.AreEqual(getResponse.Data.UseCustomJavaScript, model.UseCustomJavaScript);
            Assert.AreEqual(getResponse.Data.IsPublished, model.IsPublished);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Data.PublishedByUser, model.PublishedByUser);

            if (contentTextMode != ContentTextMode.Markdown && contentTextMode != ContentTextMode.SimpleText)
            {
                Assert.AreEqual(getResponse.ChildContentsOptionValues.Count, model.ChildContentsOptionValues.Count);
                model.ChildContentsOptionValues.ToList().ForEach(
                    o =>
                    {
                        var o1 = getResponse.ChildContentsOptionValues.FirstOrDefault(c => c.AssignmentIdentifier == o.AssignmentIdentifier);
                        Assert.IsNotNull(o1);
                        Assert.IsNotNull(o1.OptionValues);
                        Assert.AreEqual(o1.OptionValues.Count(c => !c.UseDefaultValue), o.OptionValues.Count(c => !c.UseDefaultValue));
                        Assert.IsTrue(
                            o.OptionValues.Where(c => !c.UseDefaultValue)
                                .All(
                                    c =>
                                        o1.OptionValues.All(
                                            c1 => c1.Key == c.Key && c.Value == c1.Value && c.CustomTypeIdentifier == c1.CustomTypeIdentifier && c.Type == c1.Type)));
                    });
            }
        }
    }
}
