using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Api.Operations.Root.Layouts;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

using BetterCms.Module.MediaManager.Provider;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Root.Layouts
{
    public class LayoutsApiTests : ApiCrudIntegrationTestBase<
        SaveLayoutModel, LayoutModel,
        PostLayoutRequest, PostLayoutResponse,
        GetLayoutRequest, GetLayoutResponse,
        PutLayoutRequest, PutLayoutResponse,
        DeleteLayoutRequest, DeleteLayoutResponse>
    {
        [Test]
        public void Should_CRUD_Layout_Successfully()
        {
            // Attach to events
            Events.PageEvents.Instance.LayoutCreated += Instance_EntityCreated;
            Events.PageEvents.Instance.LayoutUpdated += Instance_EntityUpdated;
            Events.PageEvents.Instance.LayoutDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Root.Layouts.Post, api.Root.Layout.Get, api.Root.Layout.Put, api.Root.Layout.Delete));

            // Detach from events
            Events.PageEvents.Instance.LayoutCreated -= Instance_EntityCreated;
            Events.PageEvents.Instance.LayoutUpdated -= Instance_EntityUpdated;
            Events.PageEvents.Instance.LayoutDeleted -= Instance_EntityDeleted;
        }

        protected override SaveLayoutModel GetCreateModel(ISession session)
        {
            var layout = TestDataProvider.CreateNewLayout();

            return new SaveLayoutModel
                   {
                       Name = layout.Name,
                       LayoutPath = layout.LayoutPath,
                       PreviewUrl = layout.PreviewUrl,
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
                       Regions = new List<RegionSaveModel>
                                 {
                                     new RegionSaveModel
                                     {
                                         Description = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                         RegionIdentifier = TestDataProvider.ProvideRandomString(MaxLength.Name)
                                     },
                                     new RegionSaveModel
                                     {
                                         Description = TestDataProvider.ProvideRandomString(MaxLength.Name),
                                         RegionIdentifier = TestDataProvider.ProvideRandomString(MaxLength.Name)
                                     }
                                 }
                   };
        }

        protected override GetLayoutRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request = new GetLayoutRequest { LayoutId = saveResponseBase.Data.Value };
            request.Data.IncludeOptions = true;
            request.Data.IncludeRegions = true;

            return request;
        }

        protected override PutLayoutRequest GetUpdateRequest(GetLayoutResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetLayoutResponse getResponse, SaveLayoutModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.LayoutPath);
            Assert.IsNotNull(getResponse.Data.PreviewUrl);
            Assert.IsNotNull(getResponse.Options);
            Assert.IsNotNull(getResponse.Regions);
            Assert.IsNotEmpty(getResponse.Regions);
            Assert.IsNotEmpty(getResponse.Regions);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.LayoutPath, model.LayoutPath);
            Assert.AreEqual(getResponse.Data.PreviewUrl, model.PreviewUrl);
            
            Assert.AreEqual(getResponse.Options.Count, model.Options.Count);
            Assert.AreEqual(getResponse.Options.Count, 2);
            Assert.IsTrue(getResponse.Options.All(a1 => model.Options.Any(a2 => a1.Key == a2.Key
                   && a1.CustomTypeIdentifier == a2.CustomTypeIdentifier
                   && a1.DefaultValue == a2.DefaultValue
                   && a1.Type == a2.Type)));

            Assert.AreEqual(getResponse.Regions.Count, model.Regions.Count);
            Assert.AreEqual(getResponse.Regions.Count, 2);
            Assert.IsTrue(getResponse.Regions.All(a1 => model.Regions.Any(a2 => a1.RegionIdentifier == a2.RegionIdentifier
                   && a1.Description == a2.Description)));
        }
    }
}
