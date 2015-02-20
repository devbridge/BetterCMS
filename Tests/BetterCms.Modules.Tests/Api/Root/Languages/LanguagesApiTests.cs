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
