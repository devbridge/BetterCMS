using BetterCms.Core.Models;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;

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
            RunApiActionInTransaction((api, session) =>
                Run(session, api.Pages.Widget.ServerControl.Post, api.Pages.Widget.ServerControl.Get, api.Pages.Widget.ServerControl.Put, api.Pages.Widget.ServerControl.Delete));
        }

        protected override SaveServerControlWidgetModel GetCreateModel(ISession session)
        {
            var content = TestDataProvider.CreateNewServerControlWidget();

            session.SaveOrUpdate(content.Category);

            return new SaveServerControlWidgetModel
                {
                    Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                    WidgetUrl = content.Url,
                    PreviewUrl = content.PreviewUrl,
                    IsPublished = true,
                    PublishedOn = content.PublishedOn,
                    PublishedByUser = content.PublishedByUser,
                    CategoryId = content.Category.Id
                };

            // TODO: options with custom options
        }

        protected override GetServerControlWidgetRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request = new GetServerControlWidgetRequest { WidgetId = saveResponseBase.Data.Value };
            request.Data.IncludeOptions = true;

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
            Assert.IsNotNull(getResponse.Data.CategoryId);
            Assert.IsNotNull(getResponse.Data.WidgetUrl);
            Assert.IsNotNull(getResponse.Data.PreviewUrl);
            //Assert.IsNotNull(getResponse.Options);
            //Assert.IsNotEmpty(getResponse.Options);

            // Compare saving entity with retrieved after save entity
            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.IsPublished, model.IsPublished);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Data.PublishedByUser, model.PublishedByUser);
            Assert.AreEqual(getResponse.Data.CategoryId, model.CategoryId);
            Assert.AreEqual(getResponse.Data.PreviewUrl, model.PreviewUrl);
            Assert.AreEqual(getResponse.Data.WidgetUrl, model.WidgetUrl);
            
            //Assert.AreEqual(getResponse.Options.Count, model.Options.Count);
        }
    }
}
