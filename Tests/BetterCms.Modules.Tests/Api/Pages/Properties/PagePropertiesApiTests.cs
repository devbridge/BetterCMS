using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.Models;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.MediaManager.Provider;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;

using NHibernate;

using NUnit.Framework;

using PagePropertiesModel = BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties.PagePropertiesModel;

namespace BetterCms.Test.Module.Api.Pages.PageProperties
{
    public class PagePropertiesApiTests : ApiCrudIntegrationTestBase<
        SavePagePropertiesModel, PagePropertiesModel,
        PostPagePropertiesRequest, PostPagePropertiesResponse,
        GetPagePropertiesRequest, GetPagePropertiesResponse,
        PutPagePropertiesRequest, PutPagePropertiesResponse,
        DeletePagePropertiesRequest, DeletePagePropertiesResponse>
    {
        private Layout layout;
        private Layout defaultLayout;
        private BetterCms.Module.Root.Models.Page masterPage;
        private Region region;
        private IPrincipal principal;
        private int changingPageProepertiesCount;

        [SetUp]
        public void SetUp()
        {
            principal = System.Threading.Thread.CurrentPrincipal;
            SetCurrentPrincipal(RootModuleConstants.UserRoles.AllRoles);
        }

        [Test]
        public void Should_CRUD_PageProperties_WithLayout_Successfully()
        {
            changingPageProepertiesCount = 0;

            // Attach to events
            Events.PageEvents.Instance.PageCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.PagePropertiesChanging += Instance_PagePropertiesChanging;
            Events.PageEvents.Instance.PagePropertiesChanged += Instance_EntityUpdated;
            Events.PageEvents.Instance.PageDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction(
                (api, session) =>
                    {
                        masterPage = null;
                        layout = TestDataProvider.CreateNewLayout();
                        region = TestDataProvider.CreateNewRegion();

                        var layoutRegion = new LayoutRegion { Layout = layout, Region = region };
                        layout.LayoutRegions = new[] { layoutRegion };

                        session.SaveOrUpdate(region);
                        session.SaveOrUpdate(layout);
                        session.SaveOrUpdate(layoutRegion);

                        Run(session, api.Pages.Page.Properties.Post, api.Pages.Page.Properties.Get, api.Pages.Page.Properties.Put, api.Pages.Page.Properties.Delete);
                    });

            Assert.AreEqual(1, changingPageProepertiesCount, "Page properties changing events fired count");

            // Detach from events
            Events.PageEvents.Instance.PageCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.PagePropertiesChanging -= Instance_PagePropertiesChanging;
            Events.PageEvents.Instance.PagePropertiesChanged -= Instance_EntityUpdated;
            Events.PageEvents.Instance.PageDeleted -= Instance_EntityDeleted;
        }

        [Test]
        public void Should_CRUD_PageProperties_WithMasterPage_Successfully()
        {
            changingPageProepertiesCount = 0;

            // Attach to events
            Events.PageEvents.Instance.PageCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.PagePropertiesChanging += Instance_PagePropertiesChanging;
            Events.PageEvents.Instance.PagePropertiesChanged += Instance_EntityUpdated;
            Events.PageEvents.Instance.PageDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction(
                (api, session) =>
                    {
                        masterPage = TestDataProvider.CreateNewPage();
                        masterPage.IsMasterPage = true;
                        layout = null;
                        region = TestDataProvider.CreateNewRegion();
                        session.SaveOrUpdate(region);
                        session.SaveOrUpdate(masterPage);

                        Run(session, api.Pages.Page.Properties.Post, api.Pages.Page.Properties.Get, api.Pages.Page.Properties.Put, api.Pages.Page.Properties.Delete);
                    });

            Assert.AreEqual(1, changingPageProepertiesCount, "Page properties changing events fired count");

            // Detach from events
            Events.PageEvents.Instance.PageCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.PagePropertiesChanging -= Instance_PagePropertiesChanging;
            Events.PageEvents.Instance.PagePropertiesChanged -= Instance_EntityUpdated;
            Events.PageEvents.Instance.PageDeleted -= Instance_EntityDeleted;
        }

        void Instance_PagePropertiesChanging(Events.PagePropertiesChangingEventArgs args)
        {
            changingPageProepertiesCount++;
        }

        protected override SavePagePropertiesModel GetCreateModel(ISession session)
        {
            return new SavePagePropertiesModel
                   {
                       Title = TestDataProvider.ProvideRandomString(MaxLength.Name),
                       Tags = new[] { this.TestDataProvider.ProvideRandomString(MaxLength.Name), this.TestDataProvider.ProvideRandomString(MaxLength.Name) },
                       AccessRules = new[]
                                     {           
                                          new AccessRuleModel
                                                {
                                                    AccessLevel = AccessLevel.ReadWrite, 
                                                    Identity = TestDataProvider.ProvideRandomString(20),
                                                    IsForRole = false
                                                },
                                          new AccessRuleModel
                                                {
                                                    AccessLevel = AccessLevel.Deny, 
                                                    Identity = TestDataProvider.ProvideRandomString(20),
                                                    IsForRole = true
                                                }
                                     },
                       CategoryId = null,
                       CustomCss = TestDataProvider.ProvideRandomString(MaxLength.Text),
                       CustomJavaScript = TestDataProvider.ProvideRandomString(MaxLength.Text),
                       Description = TestDataProvider.ProvideRandomString(MaxLength.Text),
                       FeaturedImageId = null,
                       IsArchived = false,
                       IsMasterPage = false,
                       IsPublished = true,
                       LanguageGroupIdentifier = null,
                       LanguageId = null,
                       LayoutId = layout != null ? layout.Id : (Guid?)null,
                       MasterPageId = masterPage != null ? masterPage.Id : (Guid?)null,
                       SecondaryImageId = null,
                       MainImageId = null,
                       MetaData = new MetadataModel
                                      {
                                          MetaTitle = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                          MetaDescription = TestDataProvider.ProvideRandomString(MaxLength.Text),
                                          MetaKeywords = TestDataProvider.ProvideRandomString(MaxLength.Text)
                                      },
                       PageOptions = new List<OptionValueModel>
                                {
                                    new OptionValueModel
                                    {
                                        DefaultValue = TestDataProvider.ProvideRandomString(100),
                                        Value = TestDataProvider.ProvideRandomString(100),
                                        Key = TestDataProvider.ProvideRandomString(100),
                                        Type =  OptionType.Text,
                                        UseDefaultValue = false
                                    },
                                    new OptionValueModel
                                    {
                                        DefaultValue = Guid.NewGuid().ToString(),
                                        Value = Guid.NewGuid().ToString(),
                                        Key = TestDataProvider.ProvideRandomString(100),
                                        Type = OptionType.Custom,
                                        CustomTypeIdentifier = MediaManagerFolderOptionProvider.Identifier,
                                        UseDefaultValue = false
                                    }
                                },
                       PageUrl = string.Format("{0}/{1}", TestDataProvider.ProvideRandomString(MaxLength.Name), TestDataProvider.ProvideRandomString(MaxLength.Name)),
                       PublishedOn = TestDataProvider.ProvideRandomDateTime(),
                       UseCanonicalUrl = TestDataProvider.ProvideRandomBooleanValue(),
                       UseNoFollow = TestDataProvider.ProvideRandomBooleanValue(),
                       UseNoIndex = TestDataProvider.ProvideRandomBooleanValue()
                   };
        }

        protected override GetPagePropertiesRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetPagePropertiesRequest
                       {
                           PageId = saveResponseBase.Data.Value,
                           Data = new GetPagePropertiesModel
                                      {
                                          IncludeAccessRules = true,
                                          IncludeCategory = true,
                                          IncludeImages = true,
                                          IncludeLanguage = true,
                                          IncludeLayout = true,
                                          IncludeMetaData = true,
                                          IncludePageContents = false, // To not include page content.
                                          IncludePageOptions = true,
                                          IncludePageTranslations = true,
                                          IncludeTags = true
                                      }
                       };
        }

        protected override PutPagePropertiesRequest GetUpdateRequest(GetPagePropertiesResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Title = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetPagePropertiesResponse getResponse, SavePagePropertiesModel model)
        {
            Assert.IsNotNull(getResponse.Data.Title);
            Assert.AreEqual(getResponse.Data.Title, model.Title);
            Assert.AreEqual(getResponse.Tags.Count, model.Tags.Count);

            Assert.IsNotNull(getResponse.PageOptions);
            Assert.AreEqual(getResponse.PageOptions.Count, model.PageOptions.Count);
            Assert.AreEqual(getResponse.PageOptions.Count, 2);

            Assert.IsTrue(getResponse.PageOptions.Where(a1 => !a1.UseDefaultValue).All(a1 => model.PageOptions.Any(a2 => a1.Key == a2.Key
               && a1.CustomTypeIdentifier == a2.CustomTypeIdentifier
               && a1.Value == a2.Value
               && a1.Type == a2.Type)));

            Assert.IsTrue(getResponse.PageOptions.Where(a1 => a1.UseDefaultValue).All(a1 => model.PageOptions.Any(a2 => a1.Key == a2.Key
               && a1.CustomTypeIdentifier == a2.CustomTypeIdentifier
               && a1.Type == a2.Type
               && a1.DefaultValue == a2.DefaultValue)));
        }
    }
}
