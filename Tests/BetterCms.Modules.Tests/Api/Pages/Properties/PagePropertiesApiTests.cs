using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Root;

using BetterCms.Module.MediaManager.Provider;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Properties
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
        private int changingPageProepertiesCount;

        [SetUp]
        public void SetUp()
        {
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
            var categoryTree = TestDataProvider.CreateNewCategoryTree();
            var category = TestDataProvider.CreateNewCategory(categoryTree);
            var image = TestDataProvider.CreateNewMediaImage();
            session.SaveOrUpdate(category);
            session.SaveOrUpdate(image);
            session.Flush();
            session.Clear();

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
                       CustomCss = TestDataProvider.ProvideRandomString(MaxLength.Text),
                       CustomJavaScript = TestDataProvider.ProvideRandomString(MaxLength.Text),
                       Description = TestDataProvider.ProvideRandomString(MaxLength.Text),
                       Categories = new List<Guid>() { category.Id },
                       FeaturedImageId = image.Id,
                       IsArchived = false,
                       IsMasterPage = false,
                       IsPublished = true,
                       LanguageGroupIdentifier = null,
                       ForceAccessProtocol = ForceProtocolType.ForceHttps,
                       LanguageId = null,
                       LayoutId = layout != null ? layout.Id : (Guid?)null,
                       MasterPageId = masterPage != null ? masterPage.Id : (Guid?)null,
                       SecondaryImageId = image.Id,
                       MainImageId = image.Id,
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
                       PageUrl = string.Format("/{0}/", TestDataProvider.ProvideRandomString(200)),
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
                                          IncludeCategories = true,
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

            Assert.AreEqual(getResponse.Data.CustomCss, model.CustomCss);
            Assert.AreEqual(getResponse.Data.CustomJavaScript, model.CustomJavaScript);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
            foreach (var category in model.Categories)
            {
                Assert.IsNotNull(getResponse.Data.Categories.FirstOrDefault(c => c == category));
            }
            Assert.AreEqual(getResponse.Data.FeaturedImageId, model.FeaturedImageId);
            Assert.AreEqual(getResponse.Data.IsArchived, model.IsArchived);
            Assert.AreEqual(getResponse.Data.IsMasterPage, model.IsMasterPage);
            Assert.AreEqual(getResponse.Data.IsPublished, model.IsPublished);
            Assert.AreEqual(getResponse.Data.LanguageGroupIdentifier, model.LanguageGroupIdentifier);
            Assert.AreEqual(getResponse.Data.LanguageId, model.LanguageId);
            Assert.AreEqual(getResponse.Data.LayoutId, model.LayoutId);
            Assert.AreEqual(getResponse.Data.MasterPageId, model.MasterPageId);
            Assert.AreEqual(getResponse.Data.SecondaryImageId, model.SecondaryImageId);
            Assert.AreEqual(getResponse.Data.MainImageId, model.MainImageId);
            Assert.AreEqual(getResponse.Data.ForceAccessProtocol, ForceProtocolType.ForceHttps);

            Assert.IsNotNull(getResponse.MetaData);
            Assert.AreEqual(getResponse.MetaData.MetaTitle, model.MetaData.MetaTitle);
            Assert.AreEqual(getResponse.MetaData.MetaDescription, model.MetaData.MetaDescription);
            Assert.AreEqual(getResponse.MetaData.MetaKeywords, model.MetaData.MetaKeywords);

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

            Assert.AreEqual(getResponse.Data.PageUrl, model.PageUrl);
            Assert.AreEqual(getResponse.Data.PublishedOn, model.PublishedOn);
            Assert.AreEqual(getResponse.Data.UseCanonicalUrl, model.UseCanonicalUrl);
            Assert.AreEqual(getResponse.Data.UseNoFollow, model.UseNoFollow);
            Assert.AreEqual(getResponse.Data.UseNoIndex, model.UseNoIndex);
        }
    }
}
