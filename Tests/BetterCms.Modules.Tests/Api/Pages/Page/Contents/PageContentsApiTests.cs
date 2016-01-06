using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;
using BetterCms.Module.Api.Operations.Root;

using BetterCms.Module.MediaManager.Provider;

using BetterCms.Module.Root.Models;

using BetterModules.Events;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Pages.Page.Contents
{
    public class PageContentsApiTests : ApiCrudIntegrationTestBase<
        SavePageContentModel, PageContentModel,
        PostPageContentRequest, PostPageContentResponse,
        GetPageContentRequest, GetPageContentResponse,
        PutPageContentRequest, PutPageContentResponse,
        DeletePageContentRequest, DeletePageContentResponse>
    {
        private BetterCms.Module.Pages.Models.PageProperties page;

        private bool updateOrder;
        
        private bool setOptions;

        private bool hasParentContent;

        private int contentConfigredCount;

        [Test]
        public void Should_CRUD_PageContent_WithNoSortNoOptions_Successfully()
        {
            updateOrder = false;
            setOptions = false;
            hasParentContent = false;

            RunTest();
        }
        
        [Test]
        public void Should_CRUD_PageContent_WithSortNoOption_Successfully()
        {
            updateOrder = true;
            setOptions = false;
            hasParentContent = false;

            RunTest();
        }
        
        [Test]
        public void Should_CRUD_PageContent_WithSortWithOption_Successfully()
        {
            updateOrder = true;
            setOptions = true;
            hasParentContent = false;

            RunTest();
        }
        
        [Test]
        public void Should_CRUD_PageContent_WithSortWithOption_WithParentContent_Successfully()
        {
            updateOrder = true;
            setOptions = true;
            hasParentContent = true;

            RunTest();
        }

        private void RunTest()
        {
            // Attach to events
            Events.PageEvents.Instance.PageContentInserted += Instance_EntityCreated;
            Events.PageEvents.Instance.PageContentSorted += Instance_EntityUpdated;
            Events.PageEvents.Instance.PageContentConfigured += Instance_PageContentConfigured;
            Events.PageEvents.Instance.PageContentDeleted += Instance_EntityDeleted;
            contentConfigredCount = 0;

            RunApiActionInTransaction(
                (api, session) =>
                {
                    page = TestDataProvider.CreateNewPageProperties();
                    session.SaveOrUpdate(page);

                    Run(session, api.Pages.Page.Content.Post, api.Pages.Page.Content.Get, api.Pages.Page.Content.Put, api.Pages.Page.Content.Delete);
                });

            // Detach from events
            Events.PageEvents.Instance.PageContentInserted -= Instance_EntityCreated;
            Events.PageEvents.Instance.PageContentSorted -= Instance_EntityUpdated;
            Events.PageEvents.Instance.PageContentConfigured -= Instance_PageContentConfigured;
            Events.PageEvents.Instance.PageContentDeleted -= Instance_EntityDeleted;
        }

        protected override PostPageContentRequest GetCreateRequest(SavePageContentModel model)
        {
            var request = base.GetCreateRequest(model);
            request.PageId = page.Id;

            return request;
        }

        protected override SavePageContentModel GetCreateModel(ISession session)
        {
            var parentContent = TestDataProvider.CreateNewHtmlContent(20);
            var parentRegion = TestDataProvider.CreateNewRegion();
            var parentPageContent = TestDataProvider.CreateNewPageContent(parentContent, page, parentRegion);

            var content = TestDataProvider.CreateNewHtmlContent(20);
            var region = TestDataProvider.CreateNewRegion();

            var contentOption = new ContentOption
                                 {
                                     Content = content,
                                     DefaultValue = TestDataProvider.ProvideRandomString(100),
                                     Key = TestDataProvider.ProvideRandomString(100),
                                     Type = BetterCms.Core.DataContracts.Enums.OptionType.Text
                                 };

            session.SaveOrUpdate(parentContent);
            session.SaveOrUpdate(parentRegion);
            session.SaveOrUpdate(parentPageContent);

            session.SaveOrUpdate(content);
            session.SaveOrUpdate(region);
            session.SaveOrUpdate(contentOption);

            var model = new SavePageContentModel
                        {
                            Order = 100, 
                            ContentId = content.Id, 
                            RegionId = region.Id,
                            ParentPageContentId = hasParentContent ? parentPageContent.Id : (Guid?)null
                        };

            if (setOptions)
            {
                model.Options = new List<OptionValueModel>
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
                                    },
                                    new OptionValueModel
                                    {
                                        DefaultValue = contentOption.DefaultValue,
                                        Value = TestDataProvider.ProvideRandomString(100), 
                                        Key = contentOption.Key,
                                        Type = (OptionType)(int)contentOption.Type,
                                        UseDefaultValue = true
                                    },
                                };
            }

            return model;
        }

        protected override GetPageContentRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request = new GetPageContentRequest
                   {
                       PageContentId = saveResponseBase.Data.Value,
                       PageId = page.Id
                   };
            if (setOptions)
            {
                request.Data.IncludeOptions = true;
            }

            return request;
        }

        protected override PutPageContentRequest GetUpdateRequest(GetPageContentResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            if (updateOrder)
            {
                request.Data.Order = 200;
            }

            return request;
        }

        protected override DeletePageContentRequest GetDeleteRequest(GetPageContentResponse getResponse)
        {
            var request = base.GetDeleteRequest(getResponse);
            request.PageId = page.Id;

            return request;
        }

        protected override void OnAfterGet(GetPageContentResponse getResponse, SavePageContentModel model)
        {
            Assert.IsNotNull(getResponse.Data.RegionId);
            Assert.IsNotNull(getResponse.Data.PageId);
            Assert.IsNotNull(getResponse.Data.ContentId);
            if (hasParentContent)
            {
                Assert.IsNotNull(getResponse.Data.ParentPageContentId);
            }
            else
            {
                Assert.IsNull(getResponse.Data.ParentPageContentId);
            }
            Assert.Greater(getResponse.Data.Order, 0);

            Assert.AreEqual(getResponse.Data.ContentId, model.ContentId);
            Assert.AreEqual(getResponse.Data.ParentPageContentId, model.ParentPageContentId);
            Assert.AreEqual(getResponse.Data.RegionId, model.RegionId);
            Assert.AreEqual(getResponse.Data.PageId, page.Id);
            Assert.AreEqual(getResponse.Data.Order, model.Order);

            if (setOptions)
            {
                Assert.IsNotNull(getResponse.Options);

                Assert.AreEqual(getResponse.Options.Count, model.Options.Count);
                Assert.AreEqual(getResponse.Options.Count, 3);

                Assert.IsTrue(getResponse.Options.Where(a1 => !a1.UseDefaultValue).All(a1 => model.Options.Any(a2 => a1.Key == a2.Key
                   && a1.CustomTypeIdentifier == a2.CustomTypeIdentifier
                   && a1.Value == a2.Value
                   && a1.Type == a2.Type)));
                
                Assert.IsTrue(getResponse.Options.Where(a1 => a1.UseDefaultValue).All(a1 => model.Options.Any(a2 => a1.Key == a2.Key
                   && a1.CustomTypeIdentifier == a2.CustomTypeIdentifier
                   && a1.Type == a2.Type
                   && a1.DefaultValue == a2.DefaultValue)));
            }
        }

        void Instance_PageContentConfigured(SingleItemEventArgs<PageContent> args)
        {
            contentConfigredCount ++;
        }

        protected override void CheckCreateEvent()
        {
            CheckEventsCount(1, 0, 0);
            
            Assert.AreEqual(contentConfigredCount, setOptions ? 1 : 0, "Content configured events count");
        }

        protected override void CheckUpdateEvent()
        {
            CheckEventsCount(1, updateOrder ? 1 : 0, 0);

            Assert.AreEqual(contentConfigredCount, setOptions ? 2 : 0, "Content configured events count");
        }

        protected override void CheckDeleteEvent()
        {
            CheckEventsCount(1, updateOrder ? 1 : 0, 1);
        }
    }
}
