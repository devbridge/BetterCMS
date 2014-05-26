using BetterCms.Core.Models;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Contents.HtmlContent
{
    public class HtmlContentApiTests : ApiCrudIntegrationTestBase<
        SaveHtmlContentModel, HtmlContentModel,
        PostHtmlContentRequest, PostHtmlContentResponse,
        GetHtmlContentRequest, GetHtmlContentResponse,
        PutHtmlContentRequest, PutHtmlContentResponse,
        DeleteHtmlContentRequest, DeleteHtmlContentResponse>
    {
        [Test]
        public void Should_CRUD_HtmlContent_Successfully()
        {
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
            var content = TestDataProvider.CreateNewHtmlContent();

            return new SaveHtmlContentModel
                   {
                       Name = content.Name,
                       ActivationDate = content.ActivationDate,
                       ExpirationDate = content.ExpirationDate,
                       Html = content.Html,
                       CustomCss = content.CustomCss,
                       UseCustomCss = true,
                       CustomJavaScript = content.CustomJs,
                       UseCustomJavaScript = true,
                       IsPublished = true,
                       PublishedOn = content.PublishedOn,
                       PublishedByUser = content.PublishedByUser,
                   };
        }

        protected override GetHtmlContentRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetHtmlContentRequest { ContentId = saveResponseBase.Data.Value };
        }

        protected override PutHtmlContentRequest GetUpdateRequest(GetHtmlContentResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetHtmlContentResponse getResponse, SaveHtmlContentModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.ActivationDate);
            Assert.IsNotNull(getResponse.Data.ExpirationDate);
            Assert.IsNotNull(getResponse.Data.Html);
            Assert.IsNotNull(getResponse.Data.CustomCss);
            Assert.IsNotNull(getResponse.Data.CustomJavaScript);
            Assert.IsNotNull(getResponse.Data.PublishedOn);
            Assert.IsNotNull(getResponse.Data.PublishedByUser);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.ActivationDate, model.ActivationDate);
            Assert.AreEqual(getResponse.Data.ExpirationDate, model.ExpirationDate);
            Assert.AreEqual(getResponse.Data.Html, model.Html);
            Assert.AreEqual(getResponse.Data.CustomCss, model.CustomCss);
            Assert.AreEqual(getResponse.Data.UseCustomCss, model.UseCustomCss);
            Assert.AreEqual(getResponse.Data.CustomJavaScript, model.CustomJavaScript);
            Assert.AreEqual(getResponse.Data.UseCustomJavaScript, model.UseCustomJavaScript);
            Assert.AreEqual(getResponse.Data.IsPublished, model.IsPublished);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Data.PublishedByUser, model.PublishedByUser);
        }
    }
}
